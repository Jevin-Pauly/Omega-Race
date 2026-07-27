#pragma once

#include <winsock2.h>
#include <ws2tcpip.h>
//#include <cassert>

#pragma comment(lib, "ws2_32.lib")

class TCPSocket
{
public:
    TCPSocket();
    ~TCPSocket();

    bool initialize();                        // WSAStartup + socket
    bool bindAndListen(unsigned short port); // Bind + Listen
    SOCKET acceptClient();                   // Accept a client
    void cleanup();                          // WSACleanup

private:
    SOCKET serverSocket;
};
