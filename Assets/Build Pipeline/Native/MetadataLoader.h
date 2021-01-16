#pragma once
#include "il2cpp-config.h"
#include "StaticMetadata.h"

namespace il2cpp
{
namespace vm
{
    class LIBIL2CPP_CODEGEN_API MetadataLoader
    {
    public:
        static void* LoadMetadataFile(const char* fileName);
        static void UnloadMetadataFile(void* fileBuffer);
    };
} // namespace vm
} // namespace il2cpp
