#include "TCPClient.h"
#include <iostream>
#include <climits> // For INT32_MAX

void TCPClient::run(std::list<int> &nodeList)
{
    if(!socketClient.initialize())
    {
        std::cerr << "[CLIENT] Initialization failed.\n";
        return;
    }

    if(!socketClient.connectToServer("127.0.0.1", 8888))
    {
        std::cerr << "[CLIENT] Could not connect to server.\n";
        return;
    }

    // Send the list to the server
    for(int value : nodeList)
    {
        if(!socketClient.sendInt(value))
        {
            std::cerr << "[CLIENT] Failed to send value: " << value << "\n";
            return;
        }
    }

    // Send end-of-list marker
    if(!socketClient.sendInt(INT32_MAX))
    {
        std::cerr << "[CLIENT] Failed to send end marker.\n";
        return;
    }

    std::cout << "[CLIENT] Sent data to server.\n";

    // Clear the original list
    nodeList.clear();

    // Receive sorted list
    int received = 0;
    while(socketClient.receiveInt(received))
    {
        if(received == INT32_MAX)
            break;

        nodeList.push_back(received);
    }

    std::cout << "[CLIENT] Received sorted data from server.\n";
}
