#include "UDPSocketServer.h"

#define UDP_ACK 777777

UDPSocketServer::UDPSocketServer()
    : serverSocket(INVALID_SOCKET)
{
}

UDPSocketServer::~UDPSocketServer()
{
    if(serverSocket != INVALID_SOCKET)
    {
        closesocket(serverSocket);
    }
    cleanup();
}

bool UDPSocketServer::initialize(unsigned short port)
{
    WSADATA wsaData;
    int result = WSAStartup(MAKEWORD(2, 2), &wsaData);
    if(result != 0)
    {
        std::cerr << "WSAStartup failed: " << result << "\n";
        return false;
    }

    serverSocket = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
    if(serverSocket == INVALID_SOCKET)
    {
        std::cerr << "UDP socket creation failed: " << WSAGetLastError() << "\n";
        return false;
    }

    sockaddr_in service{};
    service.sin_family = AF_INET;
    //service.sin_addr.s_addr = inet_addr("127.0.0.1"); // Localhost
    inet_pton(AF_INET, "127.0.0.1", &service.sin_addr);
    service.sin_port = htons(port);

    if(bind(serverSocket, (sockaddr *)&service, sizeof(service)) == SOCKET_ERROR)
    {
        std::cerr << "Bind failed: " << WSAGetLastError() << "\n";
        return false;
    }

    std::cout << "[SERVER] Listening on port " << port << "\n";
    return true;
}

bool UDPSocketServer::receiveInt(int &value, sockaddr_in &clientAddr, int &addrLen)
{
    int netVal = 0;
    int bytesReceived = recvfrom(serverSocket, (char *)&netVal, sizeof(int), 0, (sockaddr *)&clientAddr, &addrLen);

    if(bytesReceived == SOCKET_ERROR)
    {
        std::cerr << "recvfrom failed: " << WSAGetLastError() << "\n";
        return false;
    }

    value = ntohl(netVal);

    // Send ACK
    int ack = htonl(UDP_ACK);
    sendto(serverSocket, (char *)&ack, sizeof(int), 0, (sockaddr *)&clientAddr, addrLen);

    return true;
}

bool UDPSocketServer::sendInt(int value, const sockaddr_in &clientAddr, int addrLen)
{
    int networkVal = htonl(value);
    int result = sendto(serverSocket, (char *)&networkVal, sizeof(int), 0, (sockaddr *)&clientAddr, addrLen);
    if(result == SOCKET_ERROR)
    {
        std::cerr << "sendto failed: " << WSAGetLastError() << "\n";
        return false;
    }
    // Wait for ACK
    int ack = 0;
    int fromLen = addrLen;
    int bytesReceived;// = recvfrom(serverSocket, (char *)&ack, sizeof(int), 0, (sockaddr *)&clientAddr, &fromLen);
    while((bytesReceived = recvfrom(serverSocket, (char *)&ack, sizeof(int), 0,
        (sockaddr *)&clientAddr, &fromLen)) <= 0);
    if(bytesReceived == SOCKET_ERROR)
        return false;

    ack = ntohl(ack);
    return (ack == UDP_ACK);
}

void UDPSocketServer::cleanup()
{
    WSACleanup();
}
