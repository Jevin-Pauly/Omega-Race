#include "AckPositionPacket.h"

void AckPositionPacket::serialize(char* buffer)
{
	Packet::serialize(buffer);
	this->pos.serialize(buffer + sizeof(Type));
}

void AckPositionPacket::deserialize(const char* buffer)
{
	Packet::deserialize(buffer);
	this->pos.deserialize(const_cast<char*>(buffer + sizeof(Type)));
}