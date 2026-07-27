// TCPServer.cpp

#include "TCPServer.h"
#include <iostream>

void TCPServer::run()
{
    TCPSocket server;
    if(!server.initialize())
    {
        std::cerr << "Server failed to initialize.\n";
        return;
    }

    if(!server.bindAndListen(8888))
    {
        std::cerr << "Server failed to bind and listen.\n";
        return;
    }

    SOCKET clientSocket = server.acceptClient();
    if(clientSocket == INVALID_SOCKET)
    {
        std::cerr << "Server failed to accept client.\n";
        return;
    }

    TCPConnection connection(clientSocket);
    connection.processClient();

    //Trace::out("\nServer: Done\n\n");
}
