#pragma once

#include <Dia2.h>
#include <vector>

#ifndef PDB_DYNAMICLIB
#define DLL_EXPORT
#else
#ifdef PDB_DLL
#define DLL_EXPORT __declspec(dllexport)
#else
#define DLL_EXPORT __declspec(dllimport)
#endif
#endif

class DLL_EXPORT PDB
{
public:
    struct ObjectFile
    {
        const char* filename;
        std::vector<const char*> symbols;
        std::vector<size_t> sourceFileIndices;
    };

protected:
    const char* mFilename;
    IDiaDataSource* mDiaDataSource;
    IDiaSession* mDiaSession;
    IDiaSymbol* mGlobalSymbol;
    DWORD mMachineType;

    std::vector<ObjectFile*> mObjects;
    std::vector<const char*> mSourceFiles;

public:
    PDB();
    ~PDB();

    const char* getFilename() { return mFilename; }

    bool LoadPDB(const char* szFilename);

    std::vector<ObjectFile*> getObjects() { return mObjects; }
    std::vector<const char*> getSourceFiles() { return mSourceFiles; }

private:
    bool PopulateData();
};