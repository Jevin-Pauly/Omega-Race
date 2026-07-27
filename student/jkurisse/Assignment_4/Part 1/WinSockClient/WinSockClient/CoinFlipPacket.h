#ifndef COINFLIPPACKET_H
#define COINFLIPPACKET_H

#include "Packet.h"

class CoinFlipPacket : Packet
{
public:
	CoinFlipPacket() : Packet(Type::CoinFlipPacket) { heads = false; }
	CoinFlipPacket(bool _heads) : Packet(Type::CoinFlipPacket), heads(_heads) {}
	~CoinFlipPacket() = default;

	virtual void serialize(char* buffer) override;
	virtual void deserialize(const char* buffer) override;

public:
	bool heads;
};

#endif