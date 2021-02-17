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

    PDB pdb;

    if (!pdb.LoadPDB(filename))
        return -1;

    auto symbols = pdb.getSymbols();
    auto sources = pdb.getSourceFiles();

    auto objects = pdb.getObjects();

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

            auto source = sources[index];

            char ext[MAX_PATH];
            _splitpath_s(source, NULL, 0, NULL, 0, NULL, 0, ext, MAX_PATH);

            /*bool sourceFile = false;

            if (!strcmp(ext, ".cpp") || !strcmp(ext, ".cc") || !strcmp(ext, ".c"))
                sourceFile = true;*/

            std::cout << "    " << source << std::endl;
        }
    }

    std::cout << "Done" << std::endl;

    return 0;
}