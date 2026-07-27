#ifndef PACKET_H
#define PACKET_H

#include <winsock2.h>

#pragma comment(lib, "Ws2_32.lib")

class Packet
{
public:
	enum class Type
	{
		PositionPacket,
		AckPositionPacket,
		CoinFlipPacket,

		Uninitialized,
	};

public:
	Packet() = delete;
	Packet(Type type) : packetType(type) {}
	Packet(const Packet&) = delete;
	Packet(Packet&&) = delete;
	Packet& operator=(const Packet&) = delete;
	Packet& operator=(Packet&&) = delete;
	virtual ~Packet() = default;

	Type getPacketType() const;

	virtual void serialize(char* buffer);
	virtual void deserialize(const char* buffer);

public:
	Type packetType;

	//static Packet *deserializePacket(const char *buffer);   // long story
};

#endif