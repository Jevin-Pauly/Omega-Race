#pragma once

#include "UDPSocketClient.h"
#include <list>

class UDPClient
{
public:
    void run(std::list<int> &nodeList);

private:
    UDPSocketClient socketClient;
};
