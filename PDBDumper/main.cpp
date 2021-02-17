#include <iostream>

#include "pdb.h"

int main(int argc, const char** argv)
{
    if (argc < 2)
    {
        std::cout << "Usage: pdbdump <pdbfile>" << std::endl;
        return 1;
    }

    const char* filename = argv[1];

    char errorBuffer[256];
    errorBuffer[0] = '\0';
    PDB* pdb = PDB::LoadPDB(filename, &errorBuffer);

    if (*errorBuffer)
    {
        printf_s("Error: %s\n", errorBuffer);
        return 1;
    }

    auto symbols = pdb->getSymbols();
    auto sources = pdb->getSourceFiles();

    auto objects = pdb->getObjects();

    for (size_t i = 0; i < objects.size(); i++)
    {
        auto object = objects[i];

        std::cout << object->filename << std::endl;

        for (size_t j = 0; j < object->symbolIndices.size(); j++)
        {
            auto index = object->symbolIndices[j];

            std::cout << "  " << symbols[index] << std::endl;
        }

        for (size_t j = 0; j < object->sourceFileIndices.size(); j++)
        {
            auto index = object->sourceFileIndices[j];

            std::cout << "    " << sources[index] << std::endl;
        }
    }

    std::cout << "Done" << std::endl;

    return 0;
}