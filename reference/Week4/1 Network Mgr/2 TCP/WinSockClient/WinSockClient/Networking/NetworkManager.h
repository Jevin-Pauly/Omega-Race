// NetworkManager
// AB 4/21

#ifndef _NetworkManager
#define _NetworkManager

#define _WINSOCK_DEPRECATED_NO_WARNINGS

#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include <windows.h>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <iphlpapi.h>

#pragma comment(lib, "Ws2_32.lib")

class NetworkManager
{
public:
	NetworkManager();
	~NetworkManager();
	NetworkManager(const NetworkManager&) = delete;
	NetworkManager& operator=(const NetworkManager&) = delete;

	void ConnectTo(const SOCKADDR_IN& Target);

	void SendMsg(const char* buffer, int len, int flags = 0);
	int RcvMsg(char* buffer, int len, int flags = 0);

	void CleanUp();

private:
	SOCKET mySocket;	
};


#endif _NetworkManager