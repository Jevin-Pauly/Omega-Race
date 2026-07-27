#include "TCPConnection.h"
#include <iostream>
#include <algorithm>
#include <cassert>

TCPConnection::TCPConnection(SOCKET socket)
    : clientSocket(socket)
{
    assert(clientSocket != INVALID_SOCKET);
}

TCPConnection::~TCPConnection()
{
    if(clientSocket != INVALID_SOCKET)
    {
        closesocket(clientSocket);
    }
}

bool TCPConnection::recvInt(int &value)
{
    int networkVal;
    int result = recv(clientSocket, (char *)(&networkVal), sizeof(int), 0);
    if(result == SOCKET_ERROR || result == 0)
    {
        return false;
    }
    value = ntohl(networkVal);
    return true;
}

bool TCPConnection::sendInt(int value)
{
    int networkVal = htonl(value);
    int result = send(clientSocket, (char *)(&networkVal), sizeof(int), 0);
    return result != SOCKET_ERROR;
}

void TCPConnection::processClient()
{
    // Receive integers until client sends (end of list marker)
    int received = 0;
    while(recvInt(received))
    {
        if(received == INT32_MAX)
            break;

        data.push_back(received);
    }

    std::cout << "Server: Received list of size: " << data.size() << "\n";

    // Sort the list
    data.sort();

    // Send back the sorted list one-by-one
    for(int value : data)
    {
        if(!sendInt(value))
        {
            std::cerr << "Failed to send value to client.\n";
            break;
        }
    }

    // Send end-of-list marker
    //sendInt(-1);

    std::cout << "Server: Sent sorted list.\n";
}
