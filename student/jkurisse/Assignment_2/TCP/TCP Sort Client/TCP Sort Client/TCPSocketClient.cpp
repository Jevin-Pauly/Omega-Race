#include "TCPSocketClient.h"

TCPSocketClient::TCPSocketClient()
    : clientSocket(INVALID_SOCKET)
{
}

TCPSocketClient::~TCPSocketClient()
{
    if(clientSocket != INVALID_SOCKET)
    {
        closesocket(clientSocket);
        clientSocket = INVALID_SOCKET;
    }
    cleanup();
}

bool TCPSocketClient::initialize()
{
    WSADATA wsaData;
    int result = WSAStartup(MAKEWORD(2, 2), &wsaData);
    assert(result == 0);

    clientSocket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
    if(clientSocket == INVALID_SOCKET)
    {
        std::cerr << "[CLIENT] Socket creation failed: " << WSAGetLastError() << "\n";
        return false;
    }

    return true;
}

bool TCPSocketClient::connectToServer(const char *ipAddress, unsigned short port)
{
    sockaddr_in serverAddr{};
    serverAddr.sin_family = AF_INET;
    serverAddr.sin_port = htons(port);
    inet_pton(AF_INET, ipAddress, &serverAddr.sin_addr);

    int result = connect(clientSocket, (sockaddr *)&serverAddr, sizeof(serverAddr));
    if(result == SOCKET_ERROR)
    {
        std::cerr << "[CLIENT] Connection failed: " << WSAGetLastError() << "\n";
        return false;
    }

    std::cout << "[CLIENT] Connected to server at " << ipAddress << ":" << port << "\n";
    return true;
}

bool TCPSocketClient::sendInt(int value)
{
    int networkVal = htonl(value);
    int result = send(clientSocket, (char *)(&networkVal), sizeof(int), 0);
    return result != SOCKET_ERROR;
}

bool TCPSocketClient::receiveInt(int &value)
{
    int networkVal = 0;
    int result = recv(clientSocket, (char *)(&networkVal), sizeof(int), 0);
    if(result == SOCKET_ERROR || result == 0)
    {
        return false;
    }

    value = ntohl(networkVal);
    return true;
}

void TCPSocketClient::cleanup()
{
    WSACleanup();
}
