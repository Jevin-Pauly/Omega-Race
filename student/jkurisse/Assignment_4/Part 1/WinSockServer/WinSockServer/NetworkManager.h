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

#include <string>
#include "Packet.h"
#include "PositionPacket.h"
#include "AckPositionPacket.h"
#include "Position.h"

class NetworkManager
{
public:
	NetworkManager();
	~NetworkManager();
	NetworkManager(const NetworkManager&) = delete;
	NetworkManager& operator=(const NetworkManager&) = delete;

	void Bind(const SOCKADDR_IN& Target);

	void SendMsg(sockaddr_in target, const char* buffer, int len, int flags = 0);
	void SendMsgWithAck(sockaddr_in target, const char* buffer, int len, int flags = 0);

	void RcvMsg(sockaddr_in& sender, char* buffer, int len, int flags = 0);
	void RcvMsgWithAck(sockaddr_in& sender, char* buffer, int len, int flags = 0);

	void SetDupProbability(uint32_t probability);

	void CleanUp();

private:
	SOCKET mySocket;
	uint32_t dupProbability;
};


#endif _NetworkManager