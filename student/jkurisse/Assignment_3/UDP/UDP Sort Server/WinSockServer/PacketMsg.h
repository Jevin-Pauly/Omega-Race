#pragma once
#include <cstdint>

const int MAX_DATA_SIZE = 512;

enum class MSG_TYPE : uint8_t // 1 byte
{
    DATA = 0,
    ACK = 1
};

struct PacketMsg
{
    MSG_TYPE type;        // 1 byte
    uint8_t padding[3];   // 3 bytes to align seqNum on 4-byte boundary
    int seqNum;           // 4 bytes
    char data[MAX_DATA_SIZE]; // 512 bytes

    // Default constructor
    PacketMsg();

    // Param constructor
    PacketMsg(MSG_TYPE msgType, int sequenceNumber, const char *msg = nullptr);
};
