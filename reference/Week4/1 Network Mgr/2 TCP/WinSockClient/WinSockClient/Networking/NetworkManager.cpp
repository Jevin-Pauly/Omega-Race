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

	mySocket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (this->mySocket == INVALID_SOCKET) {
		printf("socket failed with error: %ld\n", WSAGetLastError());
		assert(false);
	}

	printf("Socket ready\n");
}

void NetworkManager::ConnectTo(const SOCKADDR_IN& Target)
{
	int iResult;

	if (connect(this->mySocket, (SOCKADDR*)&Target, sizeof(Target)) == SOCKET_ERROR)
	{
		printf("connect function failed with error: %ld\n", WSAGetLastError());
		iResult = closesocket(this->mySocket);
		if (iResult == SOCKET_ERROR) {
			printf("closesocket function failed with error: %ld\n", WSAGetLastError());
			assert(false);
		}
	}

	printf("Connected to server: %s:%i\n", inet_ntoa(Target.sin_addr), ntohs(Target.sin_port));
}

void NetworkManager::SendMsg(const char* buffer, int len, int flags)
{
	if (send(this->mySocket, buffer, len, flags) == SOCKET_ERROR) {
		printf("send failed with error: %d\n", WSAGetLastError());
		assert(false);
	}

	shutdown(this->mySocket, SD_SEND);
}

int NetworkManager::RcvMsg(char* buffer, int len, int flags )
{
	int iResult;

	

	iResult = recv(this->mySocket, buffer, len, flags); // when the reply is ready
	if (iResult == SOCKET_ERROR)
	{
		printf("recv failed with error: %d\n", WSAGetLastError());
		assert(false);
	}

	return iResult;
}




NetworkManager::~NetworkManager()
{
	
}

void NetworkManager::CleanUp()
{
	shutdown(this->mySocket, SD_BOTH);
	printf("Socket closed\n");

	WSACleanup();
}