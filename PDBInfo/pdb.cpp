#include "pdb.h"

#include <iostream>
#include <assert.h>

PDB::PDB()
{
    mFilename = nullptr;
    mDiaDataSource = nullptr;
    mDiaSession = nullptr;
    mGlobalSymbol = nullptr;
    mMachineType = CV_CFL_80386;
}

PDB::~PDB()
{
    // Release DIA objects and CoUninitialize
    if (mGlobalSymbol)
    {
        mGlobalSymbol->Release();
        mGlobalSymbol = nullptr;
    }

    if (mDiaSession)
    {
        mDiaSession->Release();
        mDiaSession = nullptr;
    }

    CoUninitialize();
}

bool PDB::LoadPDB(const char* szFilename)
{
    mFilename = szFilename;

    HRESULT hr = CoInitialize(NULL);

    // Obtain access to the provider
    hr = CoCreateInstance(__uuidof(DiaSource), NULL, CLSCTX_INPROC_SERVER, __uuidof(IDiaDataSource), (void**)&mDiaDataSource);

    if (FAILED(hr))
    {
        printf("CoCreateInstance failed - HRESULT = %08X\n", hr);
        return false;
    }

    char wszExt[MAX_PATH];
    _splitpath_s(szFilename, NULL, 0, NULL, 0, NULL, 0, wszExt, MAX_PATH);

    if (!strcmp(wszExt, ".pdb"))
    {
        size_t numDone;

        // Open and prepare a program database (.pdb) file as a debug data source
        wchar_t wszFilename[_MAX_PATH];
        mbstowcs_s(&numDone, wszFilename, szFilename, sizeof(wszFilename) / sizeof(wszFilename[0]));
        hr = mDiaDataSource->loadDataFromPdb(wszFilename);

        if (FAILED(hr)) {
            printf("loadDataFromPdb failed - HRESULT = %08X\n", hr);

            return false;
        }
    }
    else
    {
        printf("Unsupported file format: %s\n", wszExt);
        return false;
    }

    // Open a session for querying symbols
    hr = mDiaDataSource->openSession(&mDiaSession);

    if (FAILED(hr)) {
        printf("openSession failed - HRESULT = %08X\n", hr);

        return false;
    }

    // Retrieve a reference to the global scope
    hr = mDiaSession->get_globalScope(&mGlobalSymbol);

    if (hr != S_OK) {
        printf("get_globalScope failed\n");

        return false;
    }

    // Set Machine type for getting correct register names
    DWORD dwMachType = 0;
    if (mGlobalSymbol->get_machineType(&dwMachType) == S_OK) {
        switch (dwMachType) {
            case IMAGE_FILE_MACHINE_I386:
                mMachineType = CV_CFL_80386;
                break;
            case IMAGE_FILE_MACHINE_IA64:
                mMachineType = CV_CFL_IA64;
                break;
            case IMAGE_FILE_MACHINE_AMD64:
                mMachineType = CV_CFL_AMD64;
                break;
        }
    }

    return PopulateData();
}

bool PDB::PopulateData()
{
    // Retrieve the compilands first
    IDiaEnumSymbols* pEnumSymbols;
    if (FAILED(mGlobalSymbol->findChildren(SymTagCompiland, NULL, nsNone, &pEnumSymbols)))
        return false;

    IDiaSymbol* pCompiland;
    ULONG celt = 0;

    while (SUCCEEDED(pEnumSymbols->Next(1, &pCompiland, &celt)) && (celt == 1))
    {
        // Retrieve the name of the module

        char* objectfileName = new char[1024];
        BSTR bstrName;

        if (pCompiland->get_name(&bstrName) == S_OK)
        {
            size_t numDone;
            wcstombs_s(&numDone, objectfileName, 1024, bstrName, 1024);

            SysFreeString(bstrName);
        }
        else {
            strcpy_s(objectfileName, 1024, "<no_object_name>");
        }

        ObjectFile* objectfile = nullptr;

        for (size_t i = 0; i < mObjects.size(); i++)
        {
            if (mObjects[i] == nullptr)
                continue;

            if (strcmp(mObjects[i]->filename, objectfileName) == 0)
            {
                objectfile = mObjects[i];
                break;
            }
        }

        if (objectfile == nullptr)
        {
            objectfile = new ObjectFile();
            objectfile->filename = objectfileName;

            mObjects.push_back(objectfile);
        }

        // Find all the symbols defined in this compiland
        IDiaEnumSymbols* pEnumChildren;
        if (SUCCEEDED(pCompiland->findChildren(SymTagNull, NULL, nsNone, &pEnumChildren)))
        {
            IDiaSymbol* pSymbol;
            ULONG celtChildren = 0;

            while (SUCCEEDED(pEnumChildren->Next(1, &pSymbol, &celtChildren)) && (celtChildren == 1))
            {
                BSTR bstrSymbolName;
                if (pSymbol->get_undecoratedName(&bstrSymbolName) == S_OK)
                {
                    char* symbolname = new char[4096];
                    size_t numDone;
                    wcstombs_s(&numDone, symbolname, 4096, bstrSymbolName, 4096);
                    SysFreeString(bstrSymbolName);

                    size_t index = -1;

                    // Check if it already exists
                    for (size_t i = 0; i < mSymbols.size(); i++)
                    {
                        if (mSymbols[i] == nullptr)
                            continue;

                        if (strcmp(mSymbols[i], symbolname) == 0)
                        {
                            index = i;
                            break;
                        }
                    }

                    // If not, add it
                    if (index == -1)
                    {
                        index = mSymbols.size();
                        mSymbols.push_back(symbolname);
                    }

                    objectfile->symbolIndices.push_back(index);
                }

                pSymbol->Release();
            }

            pEnumChildren->Release();
        }


        // Every compiland could contain multiple references to the source files which were used to build it
        // Retrieve all source files by compiland by passing NULL for the name of the source file
        IDiaEnumSourceFiles* pEnumSourceFiles;
        if (SUCCEEDED(mDiaSession->findFile(pCompiland, NULL, nsNone, &pEnumSourceFiles)))
        {
            IDiaSourceFile* pSourceFile;

            while (SUCCEEDED(pEnumSourceFiles->Next(1, &pSourceFile, &celt)) && (celt == 1))
            {
                BSTR bstrSourceName;
                if (pSourceFile->get_fileName(&bstrSourceName) == S_OK)
                {
                    char* filename = new char[MAX_PATH];
                    size_t numDone;
                    wcstombs_s(&numDone, filename, MAX_PATH, bstrSourceName, MAX_PATH);
                    SysFreeString(bstrSourceName);

                    size_t index = -1;

                    // Check if it already exists
                    for (size_t i = 0; i < mSourceFiles.size(); i++)
                    {
                        if (mSourceFiles[i] == nullptr)
                            continue;

                        if (strcmp(mSourceFiles[i], filename) == 0)
                        {
                            index = i;
                            break;
                        }
                    }

                    // If not, add it
                    if (index == -1)
                    {
                        index = mSourceFiles.size();
                        mSourceFiles.push_back(filename);
                    }

                    objectfile->sourceFileIndices.push_back(index);
                }

                pSourceFile->Release();
            }

            pEnumSourceFiles->Release();
        }

        pCompiland->Release();
    }

    pEnumSymbols->Release();

    return true;
}
