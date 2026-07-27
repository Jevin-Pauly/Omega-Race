#pragma once

#include <WinSock2.h>
#include <WS2tcpip.h>
#include <iostream>

#pragma comment(lib, "ws2_32.lib")

#define UDP_ACK 777777

class UDPSocketClient
{
public:
    UDPSocketClient();
    ~UDPSocketClient();

    bool initialize(unsigned short serverPort);
    bool sendInt(int value);
    bool receiveInt(int &value);
    void cleanup();

private:
    SOCKET clientSocket;
    sockaddr_in serverAddr;
};
