#pragma once

#include <WinSock2.h>
#include <WS2tcpip.h>
#include <iostream>

#pragma comment(lib, "ws2_32.lib")

class UDPSocketServer
{
public:
    UDPSocketServer();
    ~UDPSocketServer();

    bool initialize(unsigned short port);
    bool receiveInt(int &value, sockaddr_in &clientAddr, int &addrLen);
    bool sendInt(int value, const sockaddr_in &clientAddr, int addrLen);
    void cleanup();

private:
    SOCKET serverSocket;
};
