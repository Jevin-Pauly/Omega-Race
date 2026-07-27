#include "NetworkManager.h"

NetworkManager::NetworkManager()
{
	WSADATA wsaData;
	int iResult;

	// Initialize Winsock
	iResult = WSAStartup(MAKEWORD(2, 2), &wsaData);
	if (iResult != 0) {
		printf("WSAStartup failed: %d\n", iResult);
		assert(false);
	}

	mySocket = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
	if (this->mySocket == INVALID_SOCKET) {
		printf("socket failed with error: %ld\n", WSAGetLastError());
		assert(false);
	}

	printf("Socket ready\n");
}

void NetworkManager::SendMsg(sockaddr_in target, const PacketMsg &msg, int flags)
{
	int size = sizeof(PacketMsg);
	int iResult = sendto(mySocket, (const char *)(&msg), size, flags, (SOCKADDR *)&target, sizeof(target));
	//int iResult = sendto(this->mySocket, buffer, len, flags, (SOCKADDR*)&target, sizeof(target));
	if (iResult == SOCKET_ERROR) {
		printf("sendto failed with error: %d\n", WSAGetLastError());
		WSACleanup();
		assert(false);
	}

}

void NetworkManager::RcvMsg(sockaddr_in& sender, PacketMsg &msg, int flags)
{
	int senderSize = sizeof(sender);
	int size = sizeof(PacketMsg);

	int iResult = recvfrom(mySocket, (char *)(&msg), size, flags, (SOCKADDR *)&sender, &senderSize);
	//int SenderAddrSize = sizeof(sender);
	//int iResult = recvfrom(this->mySocket, buffer, len, flags, (SOCKADDR*)&sender, &SenderAddrSize);
	if (iResult == SOCKET_ERROR) {
		printf("recvfrom failed with error %d\n", WSAGetLastError());
		WSACleanup();
		assert(false);
	}
}

SOCKET NetworkManager::GetSocket() const
{
	return this->mySocket;
}




NetworkManager::~NetworkManager()
{
	
}

void NetworkManager::CleanUp()
{
	closesocket(this->mySocket);
	printf("Socket closed\n");

	WSACleanup();
}