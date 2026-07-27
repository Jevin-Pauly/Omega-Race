#include "NetworkManager.h"

#include <fstream>
#include <stdio.h>
#include <iostream>

#include "..\QuoteCollection.h"

NetworkManager::NetworkManager()
{
	WSADATA wsaData;
	int iResult;

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

void NetworkManager::Bind(const SOCKADDR_IN& Target)
{
	bind(this->mySocket, (SOCKADDR*)&Target, sizeof(Target));
}

void NetworkManager::SendMsg(sockaddr_in target, const char* buffer, int len, int flags)
{
	int iResult = sendto(this->mySocket, buffer, len, flags, (SOCKADDR*)&target, sizeof(target));
	if (iResult == SOCKET_ERROR) {
		printf("sendto failed with error: %d\n", WSAGetLastError());
		WSACleanup();
		assert(false);
	}

}

void NetworkManager::RcvMsg(sockaddr_in& sender, char* buffer, int len, int flags)
{
	int SenderAddrSize = sizeof(sender);
	int iResult = recvfrom(this->mySocket, buffer, len, flags, (SOCKADDR*)&sender, &SenderAddrSize);
	if (iResult == SOCKET_ERROR) {
		printf("recvfrom failed with error %d\n", WSAGetLastError());
		WSACleanup();
		assert(false);
	}
}

NetworkManager::~NetworkManager()
{

}

void NetworkManager::CleanUp()
{
	shutdown(this->mySocket, SD_BOTH);
	WSACleanup();
}