#include "CoinFlipPacket.h"

void CoinFlipPacket::serialize(char* buffer)
{
	Packet::serialize(buffer);
	*(bool*)(buffer + sizeof(Type)) = heads;
}

void CoinFlipPacket::deserialize(const char* buffer)
{
	Packet::deserialize(buffer);
	heads = *(bool*)(buffer + sizeof(Type));
}