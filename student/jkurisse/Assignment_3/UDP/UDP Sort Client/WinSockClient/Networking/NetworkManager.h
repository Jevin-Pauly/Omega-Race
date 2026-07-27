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

#include "../PacketMsg.h"

#pragma comment(lib, "Ws2_32.lib")


class NetworkManager
{
public:
	NetworkManager();
	~NetworkManager();
	NetworkManager(const NetworkManager&) = delete;
	NetworkManager& operator=(const NetworkManager&) = delete;

	//void SendMsg(sockaddr_in target, const char* buffer, int len, int flags = 0);
	//void RcvMsg(sockaddr_in& sender, char* buffer, int len, int flags = 0);
	void SendMsg(sockaddr_in target, const PacketMsg &msg, int flags = 0);
	void RcvMsg(sockaddr_in &sender, PacketMsg &msg, int flags = 0);

	SOCKET GetSocket() const;


	void CleanUp();

private:
	SOCKET mySocket;	
};


#endif _NetworkManager