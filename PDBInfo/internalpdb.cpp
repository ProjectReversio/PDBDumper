#include "internalpdb.h"

#include <Dia2.h>

#include <iostream>

#define DBGAPI_ERROR(fmt, ...) \
{ \
    if (errorBuffer != nullptr) \
        sprintf_s((char*)*errorBuffer, 256, fmt, __VA_ARGS__); \
    delete api; \
    return nullptr; \
}

InternalPDB::InternalPDB() : mDataSource(nullptr), mSession(nullptr), mGlobalSymbol(nullptr)
{
    auto _ = CoInitialize(NULL);
}

InternalPDB::~InternalPDB()
{
    // Release DIA objects and CoUninitialize
    if (mGlobalSymbol)
    {
        mGlobalSymbol->Release();
        mGlobalSymbol = nullptr;
    }

    if (mSession)
    {
        mSession->Release();
        mSession = nullptr;
    }

    if (mDataSource)
    {
        mDataSource->Release();
        mDataSource = nullptr;
    }

    CoUninitialize();
}

InternalPDB* InternalPDB::LoadPDB(const char* file, char(*errorBuffer)[256])
{
    InternalPDB* api = new InternalPDB();

    HRESULT hr;

    // Obtain access to the provider
    hr = CoCreateInstance(__uuidof(DiaSource), NULL, CLSCTX_INPROC_SERVER, __uuidof(IDiaDataSource), (void**)&api->mDataSource);

    if (FAILED(hr))
        DBGAPI_ERROR("CoCreateInstance failed - HRESULT = %08X", hr);

    char wszExt[MAX_PATH];
    _splitpath_s(file, NULL, 0, NULL, 0, NULL, 0, wszExt, MAX_PATH);

    if (!strcmp(wszExt, ".pdb"))
    {
        // Open and prepare a program database (.pdb) file as a debug data source
        size_t numDone;
        wchar_t wszFilename[_MAX_PATH];
        mbstowcs_s(&numDone, wszFilename, file, sizeof(wszFilename) / sizeof(wszFilename[0]));
        HRESULT hr = api->mDataSource->loadDataFromPdb(wszFilename);

        if (FAILED(hr))
            DBGAPI_ERROR("loadDataFromPdb failed - HRESULT = %08X", hr);
    }
    else
        DBGAPI_ERROR("Unsupported file format: %s\n", wszExt);

    // Open a session for querying symbols
    hr = api->mDataSource->openSession(&api->mSession);

    if (FAILED(hr))
        DBGAPI_ERROR("openSession failed - HRESULT = %08X", hr);

    // Retrieve a reference to the global scope
    hr = api->mSession->get_globalScope(&api->mGlobalSymbol);

    if (hr != S_OK)
        DBGAPI_ERROR("get_globalScope failed\n");

    return api;
}