#include "UDPSocketClient.h"
#include <cstring>

UDPSocketClient::UDPSocketClient()
    : clientSocket(INVALID_SOCKET)
{
}

UDPSocketClient::~UDPSocketClient()
{
    if(clientSocket != INVALID_SOCKET)
    {
        closesocket(clientSocket);
    }
    cleanup();
}

bool UDPSocketClient::initialize(unsigned short serverPort)
{
    WSADATA wsaData;
    int result = WSAStartup(MAKEWORD(2, 2), &wsaData);
    if(result != 0)
    {
        std::cerr << "WSAStartup failed: " << result << "\n";
        return false;
    }

    clientSocket = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
    if(clientSocket == INVALID_SOCKET)
    {
        std::cerr << "UDP socket creation failed: " << WSAGetLastError() << "\n";
        return false;
    }

    memset(&serverAddr, 0, sizeof(serverAddr));
    serverAddr.sin_family = AF_INET;
    //serverAddr.sin_addr.s_addr = inet_addr(serverIP.c_str());
    inet_pton(AF_INET, "127.0.0.1", &serverAddr.sin_addr);
    serverAddr.sin_port = htons(serverPort);

    return true;
}

bool UDPSocketClient::sendInt(int value)
{
    int networkVal = htonl(value);
    int result = sendto(clientSocket, (char *)&networkVal, sizeof(int), 0, (sockaddr *)&serverAddr, sizeof(serverAddr));
    if(result == SOCKET_ERROR)
    {
        std::cerr << "sendto failed: " << WSAGetLastError() << "\n";
        return false;
    }

    // Wait for ACK
    int ack = 0;
    int bytesReceived = recvfrom(clientSocket, (char *)&ack, sizeof(int), 0, NULL, NULL);
    if(bytesReceived == SOCKET_ERROR)
        return false;

    ack = ntohl(ack);
    return (ack == UDP_ACK);
}

bool UDPSocketClient::receiveInt(int &value)
{
    int networkVal = 0;
    int bytesReceived = recvfrom(clientSocket, (char *)&networkVal, sizeof(int), 0, NULL, NULL);
    if(bytesReceived == SOCKET_ERROR)
    {
        std::cerr << "recvfrom failed: " << WSAGetLastError() << "\n";
        return false;
    }

    value = ntohl(networkVal);

    // Send ACK back to the sender (the server)
    int ack = htonl(UDP_ACK);
    int sent = sendto(clientSocket, (char *)&ack, sizeof(int), 0, (sockaddr *)&serverAddr, sizeof(serverAddr));
    if(sent == SOCKET_ERROR)
    {
        std::cerr << "sendto (ACK) failed: " << WSAGetLastError() << "\n";
        return false;
    }

    return true;
}

void UDPSocketClient::cleanup()
{
    WSACleanup();
}
