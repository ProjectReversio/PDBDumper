#pragma once

#include <Dia2.h>
#include <vector>

class PDB
{
protected:

    struct ObjectFile
    {
        const char* filename;
        std::vector<const char*> symbols;
        std::vector<size_t> sourceFileIndices;
    };

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