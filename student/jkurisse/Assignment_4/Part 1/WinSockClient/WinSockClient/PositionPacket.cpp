#include "PositionPacket.h"

void PositionPacket::serialize(char* buffer)
{
	Packet::serialize(buffer);
	this->pos.serialize(buffer + sizeof(Type));
}

void PositionPacket::deserialize(const char* buffer)
{
	Packet::deserialize(buffer);
	this->pos.deserialize(const_cast<char*>(buffer + sizeof(Type)));
}