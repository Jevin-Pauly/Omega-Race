// QOTD Client
// AB Aug 2020

#define _WINSOCK_DEPRECATED_NO_WARNINGS	// I know, we shouldn't use deprecating functions, 
										// but in this case, it's a bit messy...
										// See "Version 2" below.

#ifndef WIN32_LEAN_AND_MEAN			// another wonderful MS hack to keep their headers under control...
#define WIN32_LEAN_AND_MEAN
#endif
#include <windows.h>

#include <winsock2.h>	// both needed for using Winsock
#include <ws2tcpip.h>	//
#include <iphlpapi.h>	// IP Helper API

#pragma comment(lib, "Ws2_32.lib")	// https://docs.microsoft.com/en-us/windows/win32/winsock/creating-a-basic-winsock-application
									// because of course MS...

#include <stdio.h>
#include <iostream>

void AdjustWindow()
{
	HWND hwndFound = GetConsoleWindow();   
	RECT rc;
	GetWindowRect(GetDesktopWindow(), &rc);  // screen dimension in right and bottom
	MoveWindow(hwndFound, 0, 0, rc.right / 2, rc.bottom / 2, true);
}

int main()
{
	AdjustWindow(); // Positioning console window for convenience

	int iResult;

	// Initialize Winsock2 DLL
	WSADATA wsaData;
	iResult = WSAStartup(MAKEWORD(2, 2), &wsaData);		
	if (iResult != 0) {
		printf("WSAStartup failed: %d\n", iResult);
		return EXIT_FAILURE;
	}
	printf("Wisock2 Initialization\n");

	// Get a UDP socket
	SOCKET mySocket;
	mySocket = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
	if (mySocket == INVALID_SOCKET) {
		printf("Socket creation error: %d\n", WSAGetLastError());
		return EXIT_FAILURE;
	}
	printf("UDP socket created\n\n");


	// target host: the QOTD server
	sockaddr_in TargetAddr;

	//* Version 1: Old school and deprecated
	TargetAddr.sin_family = AF_INET;
	TargetAddr.sin_addr.s_addr = inet_addr("104.9.242.101");  // Technically we should also test for invalid address errors
	TargetAddr.sin_port = htons(17);	// See all the other 'host-to-network' (hton) converters
										// https://docs.microsoft.com/en-us/windows/win32/api/winsock/nf-winsock-htons
	//*/

	/* Version 2: proper winsock2 way.
	TargetAddr.sin_family = AF_INET;
	TargetAddr.sin_port = htons(17);
	iResult = InetPton(TargetAddr.sin_family, "104.9.242.101", &(TargetAddr.sin_addr));
	if (iResult == 0)
	{
		printf("Invalid address string...");
		exit(EXIT_FAILURE);
	}
	else if ( iResult == SOCKET_ERROR)
	{
		printf("Address binary convertion failure: %d", WSAGetLastError() );
		exit(EXIT_FAILURE);
	};
	//*/

	/* Version 3a: (deprecated but easier) Using DNS Query to get IP addr of QOTD server
	hostent* RemoteHost;
	RemoteHost = gethostbyname("djxmmx.net");			// multiple DNS hits... https://mxtoolbox.com/SuperTool.aspx?action=a%3adjxmmx.net&run=toolpage
	//RemoteHost = gethostbyname("104-9-242-101.lightspeed.bcvloh.sbcglobal.net");
	char* IPaddr;
	IPaddr = inet_ntoa(*(struct in_addr*)*RemoteHost->h_addr_list);
	printf("DNS says server's IP address is %s\n", IPaddr);

	TargetAddr.sin_family = AF_INET;
	TargetAddr.sin_addr.s_addr = inet_addr(IPaddr);  // Technically we should also test for invalid address errors
	TargetAddr.sin_port = htons(17);
	//*/

	/* Version 3b: Using DNS Query to get IP addr of QOTD server (including 
	struct addrinfo hints;
	ZeroMemory(&hints, sizeof(hints));
	hints.ai_family = AF_INET;
	hints.ai_socktype = SOCK_DGRAM;
	hints.ai_protocol = IPPROTO_UDP;

	struct addrinfo* pAddrList;
	iResult = GetAddrInfo("djxmmx.net", "17", &hints, &pAddrList);

	// loop over all returned results and do inverse lookup 
	struct addrinfo* res;
	int ind = 0;
	for (res = pAddrList; res != nullptr; res = res->ai_next) {
		char hostname[NI_MAXHOST];
		iResult = getnameinfo(res->ai_addr, res->ai_addrlen, hostname, NI_MAXHOST, NULL, 0, 0);
		if (iResult != 0) {
			fprintf(stderr, "error in getnameinfo: %s\n", gai_strerror(iResult));
			continue;
		}
		if (*hostname != '\0')
		{
			ind++;
			printf("hostname #%i: %s\n", ind, hostname);
		}
	}

	// In reality, use failover: on fail/timeout, try next one...
	int selection;
	printf("Select host #: ");
	std::cin >> selection;
	res = pAddrList;
	selection--;
	while (selection > 0)
	{
		res = res->ai_next;
		selection--;
	}

	TargetAddr.sin_family = AF_INET;
	TargetAddr.sin_addr = ((struct sockaddr_in*)res->ai_addr)->sin_addr; //inet_addr(IPaddr);  // Technically we should also test for invalid address errors
	TargetAddr.sin_port = htons(17);
	//*/

	/* Version 4: Loopback for server demo
	TargetAddr.sin_family = AF_INET;
	TargetAddr.sin_addr.s_addr = inet_addr("127.0.0.1");  // This computer see localhost https://en.wikipedia.org/wiki/Localhost
	TargetAddr.sin_port = htons(8888);
	//*/
	
	// NOTE: No calls to 'bind' needed since 'sendto' performs implicit bind
	// https://docs.microsoft.com/en-us/windows/win32/api/winsock/nf-winsock-sendto

	// Now we can *do* something with the socket

	const int BufLen = 1024;
	char MsgBuf[BufLen] = { 0 };	// What we send
	char ReplyBuf[BufLen];			// Replies we get

	sockaddr_in SenderAddr;			// used for recvFrom parameters...
	int SenderAddrSize = sizeof(SenderAddr);

	while (MsgBuf[0] != '!')
	{
		// User input (QOTD protocol accept any string)
		printf("Message to send: ");
		std::cin >> MsgBuf;

		if (MsgBuf[0] != '!') // lazy user control
		{
			printf("Sending a datagram to the server...\n");
			iResult = sendto(mySocket, MsgBuf, strlen(MsgBuf) + 1, 0, (SOCKADDR*)& TargetAddr, sizeof(TargetAddr));
			if (iResult == SOCKET_ERROR) {
				printf("sendto failed with error: %d\n", WSAGetLastError());
				WSACleanup();
				return 1;
			}

			// Let's find out which port number we got assigned..
			sockaddr_in myaddr;
			socklen_t myaddrlen = sizeof(myaddr);
			iResult = getsockname(mySocket, (struct sockaddr*)&myaddr, &myaddrlen);
			if (iResult != 0) {
				printf("getsockname failed: %d\n", WSAGetLastError());
				return EXIT_FAILURE;
			}
			printf("(Socket now locally bound to port #%i\n\n", ntohs(myaddr.sin_port));

			printf("Waiting for a reply...\n");
			iResult = recvfrom(mySocket, ReplyBuf, BufLen, 0, (SOCKADDR*)& SenderAddr, &SenderAddrSize);
			if (iResult == SOCKET_ERROR) {
				printf("recvfrom failed with error %d\n", WSAGetLastError());
				WSACleanup();
				return 1;
			}

			printf("Datagram received from %s:%i\n\n", inet_ntoa(SenderAddr.sin_addr), ntohs(SenderAddr.sin_port));
			printf("Quote:\n%s\n\n", ReplyBuf);
			memset(ReplyBuf, 0, sizeof(ReplyBuf));  // clear reply buffer
		}
	}

	// When the application is finished sending, close the socket.

	printf("Informing server we're done.\n");
	iResult = shutdown(mySocket, SD_BOTH);
	if (iResult == SOCKET_ERROR) 
	{
		printf("shutdown failed: %d\n", WSAGetLastError());
		closesocket(mySocket);
		WSACleanup();
		return 1;
	}

	printf("Closing socket.\n");
	iResult = closesocket(mySocket);
	if (iResult == SOCKET_ERROR) {
		printf("closesocket failed with error: %d\n", WSAGetLastError());
		WSACleanup();
		return 1;
	}

	WSACleanup();

	system("pause");  // preventing the console from closing

	return 0;
}
