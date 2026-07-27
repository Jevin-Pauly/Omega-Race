#include "NetworkManager.h"

#include <fstream>
#include <stdio.h>
#include <iostream>

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
	dupProbability = 0; // Set to 0 for no duplicates

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

	if (dupProbability > 0) {
		unsigned int randomValue = rand() % 100;
		if (randomValue < dupProbability) {
			printf("[Client] Sending Duplicate Message\n");
			iResult = sendto(this->mySocket, buffer, len, flags, (SOCKADDR*)&target, sizeof(target));
			if (iResult == SOCKET_ERROR) {
				printf("sendto failed with error: %d\n", WSAGetLastError());
				WSACleanup();
				assert(false);
			}
		}
	}
}

void NetworkManager::SendMsgWithAck(sockaddr_in target, const char* buffer, int len, int flags)
{
	SendMsg(target, buffer, len, flags);

	// Wait for ack
	char ackBuffer[1024];
	sockaddr_in sender;

	RcvMsg(sender, ackBuffer, sizeof(ackBuffer), 0);

	// Check if the acknowledgment is valid
	Packet ackPacket(Packet::Type::Uninitialized);
	ackPacket.deserialize(ackBuffer);
	
	while (ackPacket.getPacketType() != Packet::Type::AckPositionPacket)
	{
		// Get next
		RcvMsg(sender, ackBuffer, sizeof(ackBuffer), 0);
		ackPacket.deserialize(ackBuffer);
	}

	assert(ackPacket.getPacketType() == Packet::Type::AckPositionPacket);
	AckPositionPacket ackPosPacket(Position(0,0));
	ackPosPacket.deserialize(ackBuffer);
	Trace::out("[Client] Received acknowledgment for message\n");
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

void NetworkManager::RcvMsgWithAck(sockaddr_in& sender, char* buffer, int len, int flags)
{
	RcvMsg(sender, buffer, len, flags);

	Packet packet(Packet::Type::Uninitialized);
	packet.deserialize(buffer);
	while (packet.getPacketType() == Packet::Type::AckPositionPacket)
	{
		// Get next
		RcvMsg(sender, buffer, len, flags);
		packet.deserialize(buffer);
	}

	char ackBuffer[1024];

	AckPositionPacket ackPosPacket(Position(0, 0));
	ackPosPacket.serialize(ackBuffer);
	SendMsg(sender, ackBuffer, sizeof(ackBuffer), 0);
}

void NetworkManager::SetDupProbability(uint32_t probability)
{
	dupProbability = probability;
}


NetworkManager::~NetworkManager()
{

}

void NetworkManager::CleanUp()
{
	shutdown(this->mySocket, SD_BOTH);
	WSACleanup();
}