#pragma once

#include <winsock2.h>
#include <ws2tcpip.h>
//#include <cassert>
#include <iostream>

#pragma comment(lib, "ws2_32.lib")

class TCPSocketClient
{
public:
    TCPSocketClient();
    ~TCPSocketClient();

    bool initialize();
    bool connectToServer(const char *ipAddress, unsigned short port);
    bool sendInt(int value);
    bool receiveInt(int &value);
    void cleanup();

private:
    SOCKET clientSocket;
};
