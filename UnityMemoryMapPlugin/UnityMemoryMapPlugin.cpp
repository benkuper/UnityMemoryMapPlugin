// UnityMemoryMapPlugin.cpp : Defines the exported functions for the DLL application.
//

#define EXPORT_API  __declspec( dllexport )

#include "SharedMemory.h"

FILE * pConsole;
ofxSharedMemory memoryMappedFile;

bool isConnected;

#define NUM_POINTS 10

extern "C" {
	EXPORT_API void openConsole()
	{
		
		AllocConsole();
		freopen_s(&pConsole, "CONOUT$", "wb", stdout);
	}

	EXPORT_API bool setupMemoryShare_C(const char * memoryKey, int memorySize, bool isServer)
	{
		printf("\n** SETUP MEMORY SHARE **\n");
		string memKey = string(memoryKey);
		memoryMappedFile.setup(memKey, memorySize, isServer);
		
		
		printf("Setup is ok !\n");

		return true;
	}

	EXPORT_API bool connectMemoryShare_C()
	{
		if (!isConnected)
		{
			isConnected = memoryMappedFile.connect();
		}

		return isConnected;
	}

	EXPORT_API void * getMemoryData_C(int handle, int maxSize)
	{
		if (!isConnected)
		{
			printf("Not connected, return null");
			return nullptr;
		}

		return memoryMappedFile.getData();
	}

}


