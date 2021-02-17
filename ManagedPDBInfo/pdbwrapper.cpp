#include "pdb.h"

namespace PDBInfo
{
    public ref class PDB
    {
    private:
        ::PDB* mPDB;
    public:
        ref class ObjectFile
        {
        private:
            ::PDB::ObjectFile* mObjectfile;
        public:
            property System::String^ FileName
            {
                System::String^ get()
                {
                    return gcnew System::String(mObjectfile->filename);
                }
            }

            property System::Collections::Generic::List<size_t>^ SymbolIndices
            {
                System::Collections::Generic::List<size_t>^ get()
                {
                    auto ret = gcnew System::Collections::Generic::List<size_t>(mObjectfile->symbolIndices.size());
                    for (auto index : mObjectfile->symbolIndices)
                        ret->Add(index);

                    return ret;
                }
            }

            property System::Collections::Generic::List<size_t>^ SourceFileIndices
            {
                System::Collections::Generic::List<size_t>^ get()
                {
                    auto ret = gcnew System::Collections::Generic::List<size_t>(mObjectfile->sourceFileIndices.size());
                    for (auto index : mObjectfile->sourceFileIndices)
                        ret->Add(index);

                    return ret;
                }
            }
        internal:
            ObjectFile(::PDB::ObjectFile* objectfile)
            {
                mObjectfile = objectfile;
            }
        };

        PDB()
        {
            mPDB = new ::PDB();
        }

        ~PDB()
        {
            delete mPDB;
        }

        property System::String^ FileName
        {
            System::String^ get()
            {
                return gcnew System::String(mPDB->getFilename());
            }
        }

        System::Boolean LoadPDB(System::String^ filename)
        {
            auto buffer = System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(filename);
            bool ret = mPDB->LoadPDB((char*)buffer.ToPointer());
            System::Runtime::InteropServices::Marshal::FreeHGlobal(buffer);

            return ret;
        }

        property System::Collections::Generic::List<ObjectFile^>^ Objects
        {
            System::Collections::Generic::List<ObjectFile^>^ get()
            {
                auto ret = gcnew System::Collections::Generic::List<ObjectFile^>(mPDB->getObjects().size());
                for (auto obj : mPDB->getObjects())
                    ret->Add(gcnew ObjectFile(obj));

                return ret;
            }
        }
        
        property System::Collections::Generic::List<System::String^>^ Symbols
        {
            System::Collections::Generic::List<System::String^>^ get()
            {
                auto ret = gcnew System::Collections::Generic::List<System::String^>(mPDB->getSymbols().size());
                for (auto symbol : mPDB->getSymbols())
                    ret->Add(gcnew System::String(symbol));

                return ret;
            }
        }

        property System::Collections::Generic::List<System::String^>^ SourceFiles
        {
            System::Collections::Generic::List<System::String^>^ get()
            {
                auto ret = gcnew System::Collections::Generic::List<System::String^>(mPDB->getSourceFiles().size());
                for (auto srcfile : mPDB->getSourceFiles())
                    ret->Add(gcnew System::String(srcfile));

                return ret;
            }
        }
    };
}