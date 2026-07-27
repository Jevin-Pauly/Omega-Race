#ifndef POSITIONPACKET_H
#define POSITIONPACKET_H

#include "Packet.h"
#include "Position.h"

class PositionPacket : Packet
{
public:
	PositionPacket() : Packet(Type::PositionPacket) {}
	PositionPacket(const Position& p) : Packet(Type::PositionPacket), pos(p) {}
	~PositionPacket() = default;

	virtual void serialize(char* buffer) override;
	virtual void deserialize(const char* buffer) override;

public:
	Position pos;
};

#endif