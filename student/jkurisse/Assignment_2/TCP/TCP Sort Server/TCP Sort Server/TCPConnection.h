#pragma once

#include <winsock2.h>
#include <list>

class TCPConnection
{
public:
    TCPConnection(SOCKET clientSocket);
    ~TCPConnection();

    void processClient(); // Handle receive -> sort -> send

private:
    SOCKET clientSocket;
    std::list<int> data;

    bool recvInt(int &value);
    bool sendInt(int value);
};
