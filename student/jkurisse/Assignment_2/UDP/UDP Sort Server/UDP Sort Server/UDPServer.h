#pragma once

#include "UDPSocketServer.h"
#include <list>

class UDPServer
{
public:
    void run();

private:
    UDPSocketServer socketServer;
    std::list<int> data;
};
