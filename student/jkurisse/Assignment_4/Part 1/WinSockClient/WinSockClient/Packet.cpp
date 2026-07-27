#include "Packet.h"

Packet::Type Packet::getPacketType() const
{
	return this->packetType;
}

void Packet::serialize(char* buffer)
{
	*(int*)buffer = htonl(static_cast<int>(this->packetType));
}

void Packet::deserialize(const char* buffer)
{
	this->packetType = static_cast<Type>(ntohl(*(int*)buffer));
}