#pragma once

#include <list>
#include "TCPSocketClient.h"

class TCPClient
{
public:
    void run(std::list<int> &nodeList);

private:
    TCPSocketClient socketClient;
};
