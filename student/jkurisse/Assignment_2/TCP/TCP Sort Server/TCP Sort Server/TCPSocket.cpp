#include "TCPSocket.h"
#include <iostream>

TCPSocket::TCPSocket()
    : serverSocket(INVALID_SOCKET)
{
}

TCPSocket::~TCPSocket()
{
    if(serverSocket != INVALID_SOCKET)
    {
        closesocket(serverSocket);
        serverSocket = INVALID_SOCKET;
    }
}

bool TCPSocket::initialize()
{
    WSADATA wsaData;
    int result = WSAStartup(MAKEWORD(2, 2), &wsaData);
    assert(result == 0);

    serverSocket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
    if(serverSocket == INVALID_SOCKET)
    {
        std::cerr << "Socket creation failed: " << WSAGetLastError() << "\n";
        return false;
    }

    return true;
}

bool TCPSocket::bindAndListen(unsigned short port)
{
    sockaddr_in service{};
    service.sin_family = AF_INET;
    //service.sin_addr.s_addr = inet_addr("127.0.0.1"); // Loopback
    inet_pton(AF_INET, "127.0.0.1", &service.sin_addr);
    service.sin_port = htons(port);

    if(bind(serverSocket, (SOCKADDR *)&service, sizeof(service)) == SOCKET_ERROR)
    {
        std::cerr << "Bind failed: " << WSAGetLastError() << "\n";
        return false;
    }

    if(listen(serverSocket, SOMAXCONN) == SOCKET_ERROR)
    {
        std::cerr << "Listen failed: " << WSAGetLastError() << "\n";
        return false;
    }

    std::cout << "Server is listening on port " << port << "\n";
    return true;
}

SOCKET TCPSocket::acceptClient()
{
    SOCKET clientSocket = accept(serverSocket, nullptr, nullptr);
    if(clientSocket == INVALID_SOCKET)
    {
        std::cerr << "Accept failed: " << WSAGetLastError() << "\n";
    }
    return clientSocket;
}

void TCPSocket::cleanup()
{
    WSACleanup();
}
