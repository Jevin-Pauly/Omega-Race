#include "UDPClient.h"

void UDPClient::run(std::list<int> &nodeList)
{
    // Initialize UDP socket client
    if(!socketClient.initialize(8888))
    {
        std::cerr << "[CLIENT] Initialization failed.\n";
        return;
    }

    // Send data to server with ACKs
    for(int value : nodeList)
    {
        if(!socketClient.sendInt(value))
        {
            std::cerr << "[CLIENT] Failed to send value: " << value << "\n";
            return;
        }
        std::cout << "[CLIENT] Sent: " << value << "\n";
    }

    // Send end-of-data marker (INT32_MAX)
    if(!socketClient.sendInt(INT32_MAX))
    {
        std::cerr << "[CLIENT] Failed to send end marker.\n";
        return;
    }
    std::cout << "[CLIENT] Sent: END marker\n";

    // Clear the list to receive the sorted data
    nodeList.clear();
    int receivedValue = 0;

    // Receive sorted data
    while(true)
    {
        if(!socketClient.receiveInt(receivedValue))
        {
            std::cerr << "[CLIENT] Failed to receive data.\n";
            break;
        }

        if(receivedValue == INT32_MAX)
        {
            std::cout << "[CLIENT] Received: END signal\n";
            break;
        }

        std::cout << "[CLIENT] Received: " << receivedValue << "\n";
        nodeList.push_back(receivedValue);
    }

    // Clean up the socket client
    socketClient.cleanup();
}
