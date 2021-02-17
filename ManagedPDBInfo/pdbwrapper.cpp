#include "pdb.h"

namespace PDBInfo
{
    public ref struct PDBException : public System::Exception
    {
        public:
            PDBException() : System::Exception()
            {

            }

            PDBException(System::String^ message) : System::Exception(message)
            {
                
            }

            PDBException(System::String^ message, System::Exception^ innerException) : System::Exception(message, innerException)
            {

            }

            PDBException(System::Runtime::Serialization::SerializationInfo^ info, System::Runtime::Serialization::StreamingContext context) : System::Exception(info, context)
            {

            }
    };

    public ref class PDB
    {
    private:
        ::PDB* mPDB;

        PDB(::PDB* pdb)
        {
            mPDB = pdb;
        }
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

            property System::Collections::Generic::List<System::Int32>^ SymbolIndices
            {
                System::Collections::Generic::List<System::Int32>^ get()
                {
                    auto ret = gcnew System::Collections::Generic::List<System::Int32>((System::Int32)mObjectfile->symbolIndices.size());
                    for (auto index : mObjectfile->symbolIndices)
                        ret->Add((System::Int32)index);

                    return ret;
                }
            }

            property System::Collections::Generic::List<System::Int32>^ SourceFileIndices
            {
                System::Collections::Generic::List<System::Int32>^ get()
                {
                    auto ret = gcnew System::Collections::Generic::List<System::Int32>((System::Int32)mObjectfile->sourceFileIndices.size());
                    for (auto index : mObjectfile->sourceFileIndices)
                        ret->Add((System::Int32)index);

                    return ret;
                }
            }
        internal:
            ObjectFile(::PDB::ObjectFile* objectfile)
            {
                mObjectfile = objectfile;
            }
        };

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

        static PDB^ LoadPDB(System::String^ filename)
        {
            auto buffer = System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(filename);

            char errorBuffer[256];
            errorBuffer[0] = '\0';
            ::PDB* pdb = ::PDB::LoadPDB((char*)buffer.ToPointer(), &errorBuffer);
            System::Runtime::InteropServices::Marshal::FreeHGlobal(buffer);

            if (pdb == nullptr)
            {
                if (*errorBuffer)
                    throw gcnew PDBException(gcnew System::String(errorBuffer));
                else
                    throw gcnew PDBException(gcnew System::String("Unknown Error"));
            }

            return gcnew PDB(pdb);
        }

        property System::Collections::Generic::List<ObjectFile^>^ Objects
        {
            System::Collections::Generic::List<ObjectFile^>^ get()
            {
                auto ret = gcnew System::Collections::Generic::List<ObjectFile^>((System::Int32)mPDB->getObjects().size());
                for (auto obj : mPDB->getObjects())
                    ret->Add(gcnew ObjectFile(obj));

                return ret;
            }
        }

        property System::Collections::Generic::List<System::String^>^ Symbols
        {
            System::Collections::Generic::List<System::String^>^ get()
            {
                auto ret = gcnew System::Collections::Generic::List<System::String^>((System::Int32)mPDB->getSymbols().size());
                for (auto symbol : mPDB->getSymbols())
                    ret->Add(gcnew System::String(symbol));

                return ret;
            }
        }

        property System::Collections::Generic::List<System::String^>^ SourceFiles
        {
            System::Collections::Generic::List<System::String^>^ get()
            {
                auto ret = gcnew System::Collections::Generic::List<System::String^>((System::Int32)mPDB->getSourceFiles().size());
                for (auto srcfile : mPDB->getSourceFiles())
                    ret->Add(gcnew System::String(srcfile));

                return ret;
            }
        }
    };
}