#include "NetworkManager.h"

#include <stdio.h>
#include <iostream>

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

void NetworkManager::BindAndListen(const SOCKADDR_IN& Target, int backlog)
{
	bind(this->mySocket, (SOCKADDR*)&Target, sizeof(Target));
	listen(this->mySocket, backlog);
}

bool NetworkManager::WaitForRequest()
{
	clientSock = accept(this->mySocket, (SOCKADDR*)&(this->clientAddr), &(this->clientAddrSize) );

	printf("Client connected: %s:%i\n", inet_ntoa(clientAddr.sin_addr), ntohs(clientAddr.sin_port));

	return (clientSock != INVALID_SOCKET);
}

void NetworkManager::SendMsg(const char* buffer, int len, int flags)
{
	if (send(this->clientSock, buffer, len, flags) == SOCKET_ERROR) {
		printf("send failed with error: %d\n", WSAGetLastError());
		assert(false);
	}
}

void NetworkManager::RcvMsg(char* buffer, int len, int flags)
{
	int iResult;

	iResult = recv(this->clientSock, buffer, len, flags); // when the reply is ready
	if (iResult == SOCKET_ERROR)
	{
		printf("recv failed with error: %d\n", WSAGetLastError());
		assert(false);
	}
}

void NetworkManager::CloseRequest()
{
	shutdown(this->clientSock, SD_SEND);
}

NetworkManager::~NetworkManager()
{

}

void NetworkManager::CleanUp()
{
	WSACleanup();
}