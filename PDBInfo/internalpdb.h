#pragma once

struct IDiaDataSource;
struct IDiaSession;
struct IDiaSymbol;

class InternalPDB
{
protected:
    IDiaDataSource* mDataSource;
    IDiaSession* mSession;
    IDiaSymbol* mGlobalSymbol;

public:
    ~InternalPDB();

    IDiaDataSource* getDataSource() { return mDataSource; }
    IDiaSession* getSession() { return mSession; }
    IDiaSymbol* getGlobalSymbol() { return mGlobalSymbol; }

    static InternalPDB* LoadPDB(const char* file, char(*errorBuffer)[256] = nullptr);

private:
    InternalPDB();
};
