#ifndef ACKPOSITION_H
#define ACKPOSITION_H

#include "Packet.h"
#include "Position.h"

class AckPositionPacket : Packet
{
public:
	AckPositionPacket() : Packet(Type::AckPositionPacket) {}
	AckPositionPacket(const Position& p) : Packet(Type::AckPositionPacket), pos(p) {}
	~AckPositionPacket() = default;

	virtual void serialize(char* buffer) override;
	virtual void deserialize(const char* buffer) override;

public:
	Position pos;
};

#endif