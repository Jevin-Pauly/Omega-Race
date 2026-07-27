#include "PacketMsg.h"
#include <cstring> // for memset, strncpy_s

PacketMsg::PacketMsg()
    : type(MSG_TYPE::DATA), seqNum(0)
{
    std::memset(padding, 0, sizeof(padding));
    std::memset(data, 0, MAX_DATA_SIZE);
}

PacketMsg::PacketMsg(MSG_TYPE msgType, int sequenceNumber, const char *msg)
    : type(msgType), seqNum(sequenceNumber)
{
    std::memset(padding, 0, sizeof(padding));
    std::memset(data, 0, MAX_DATA_SIZE);
    if(msg)
    {
        strncpy_s(data, MAX_DATA_SIZE, msg, MAX_DATA_SIZE - 1);
    }
}
