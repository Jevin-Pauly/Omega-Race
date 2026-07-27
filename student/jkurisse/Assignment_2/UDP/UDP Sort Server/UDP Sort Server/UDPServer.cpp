#include "UDPServer.h"
#include <climits>

void UDPServer::run()
{
    if(!socketServer.initialize(8888))
    {
        std::cerr << "[SERVER] Initialization failed.\n";
        return;
    }

    sockaddr_in clientAddr{};
    int addrLen = sizeof(clientAddr);
    int received = 0;

    std::cout << "[SERVER] Waiting for data...\n";

    // Receive data until INT32_MAX is received
    while(true)
    {
        if(!socketServer.receiveInt(received, clientAddr, addrLen))
        {
            std::cerr << "[SERVER] Failed to receive.\n";
            return;
        }

        if(received == INT32_MAX)
            break;

        data.push_back(received);
        std::cout << "[SERVER] Received: " << received << "\n";
    }

    std::cout << "[SERVER] Done receiving. Sorting...\n";
    data.sort();

    for(int val : data)
    {
        if(!socketServer.sendInt(val, clientAddr, addrLen))
        {
            std::cerr << "[SERVER] Failed to send sorted data.\n";
            return;
        }
        std::cout << "[SERVER] Sent: " << val << "\n";
    }

    // Send end-of-list marker
    socketServer.sendInt(INT32_MAX, clientAddr, addrLen);
    std::cout << "[SERVER] Done sending sorted data.\n";
}
