// QOTD Client
// AB Aug 2020

#define _WINSOCK_DEPRECATED_NO_WARNINGS	// I know, we shouldn't use deprecating functions, but in this case, it's a bit messy...

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
#include <string>

void AdjustWindow()
{
	HWND hwndFound = GetConsoleWindow();
	RECT rc;
	GetWindowRect(GetDesktopWindow(), &rc);  // screen dimension in right and bottom
	MoveWindow(hwndFound, rc.right / 2, 0, rc.right / 2, rc.bottom / 2, true);
}

int main()
{
	AdjustWindow(); // Positioning console window for convenience

	int iResult;

	// Collection of quotes
	std::string Quotes[] = {
		"\"Any fool can write code that a computer can understand.\nGood programmers write code that humans can understand.\" \n- Martin Fowler",
		"\"First, solve the problem. Then, write the code.\" \n- John Johnson", 
		"\"Experience is the name everyone gives to their mistakes.\" \n- Oscar Wilde",
		"\"In order to be irreplaceable, one must always be different\" \n- Coco Chanel",
		"\"Sometimes it pays to stay in bed on Monday, rather than \nspending the rest of the week debugging Monday’s code.\" \n- Dan Salomon",
		"\"Perfection is achieved not when there is nothing more to add, but rather \nwhen there is nothing more to take away.\" \n- Antoine de Saint \n- Exupery",
		"\"Code is like humor.When you have to explain it, it’s bad.\" \n- Cory House",
		"\"Fix the cause, not the symptom.\" \n- Steve Maguire",
		"\"Optimism is an occupational hazard of programming: feedback is the treatment.\" Kent Beck",
		"\"When to use iterative development? You should use iterative development \nonly on projects that you want to succeed.\" \n- Martin Fowler",
		"\"Simplicity is the soul of efficiency.\" \n- Austin Freeman",
		"\"Before software can be reusable it first has to be usable.\" \n- Ralph Johnson",
		"\"Talk is cheap.Show me the code.\" \n- Linus Torvalds",
		"\"Always code as if the guy who ends up maintaining your code \nwill be a violent psychopath who knows where you live\" \n- John Woods",
		"\"I'm not a great programmer; I'm just a good programmer with great habits.\" \n- Kent Beck",
		"\"Give a man a program, frustrate him for a day. \nTeach a man to program, frustrate him for a lifetime.\" \n- Muhammad Waseem"
	};
	int index = 0;

	// Initialize Winsock 2 DLL
	WSADATA wsaData;
	iResult = WSAStartup(MAKEWORD(2, 2), &wsaData);
	if (iResult != 0) {
		printf("WSAStartup failed: %d\n", iResult);
		return EXIT_FAILURE;
	}

	// Get a socket
	SOCKET mySocket;
	mySocket = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
	if (mySocket == INVALID_SOCKET) {
		printf("Socket creation error: %d\n", WSAGetLastError());
		return EXIT_FAILURE;
	}

	// Establishes that the socket is attached to the address and port
	// Note: unlike sento, call to recvfrom do _not_ have implicit calls to bind

	// Our host address
	sockaddr_in OurAddr;
	OurAddr.sin_family = AF_INET;
	OurAddr.sin_addr.s_addr = INADDR_ANY;	// let's the OS determine our current IP address https://docs.microsoft.com/en-us/windows/win32/api/winsock/nf-winsock-bind#remarks:~:text=If%20an%20application%20does%20not%20care,than%20one%20network%20interface%20and%20address).
	OurAddr.sin_port = htons(8888);			// We specify the port we want our server to use (avoid the common ports)
											
	if (bind(mySocket, (SOCKADDR*)& OurAddr, sizeof(OurAddr)) == SOCKET_ERROR)
	{
		printf("Bind failed with error code : %d", WSAGetLastError());  // https://docs.microsoft.com/en-us/windows/win32/winsock/windows-sockets-error-codes-2
		exit(EXIT_FAILURE);
	}

	// Now we can *do* something with the socket

	const int BufLen = 1024;
	char MsgBuf[BufLen] = { 0 };	// What we send

	sockaddr_in SenderAddr;			// used for recvFrom parameters...
	int SenderAddrSize = sizeof(SenderAddr);

	while (true) // Maybe try shutting down based on a received message?
	{
		printf("Waiting for a request...\n");

		// Note: The difference between 'recv' and 'recvfrom' is whether we get the address of the sender
		// As a server is expected to send UDP replies, we need this address, so recvfrom it is.
		iResult = recvfrom(mySocket, MsgBuf, BufLen, 0, (SOCKADDR*)& SenderAddr, &SenderAddrSize);
		if (iResult == SOCKET_ERROR) {
			printf("recvfrom failed with error %d\n", WSAGetLastError());
			WSACleanup();
			return 1;
		}

		printf("Request received from %s:%i\n", inet_ntoa(SenderAddr.sin_addr), ntohs(SenderAddr.sin_port)  );
		printf("\tmsg is: %s\n\n", MsgBuf);
		memset(MsgBuf, 0, sizeof(MsgBuf));  // clear buffer

		// Slowing things down to demo/study
		//Sleep(3000);

		printf("Sending a quote to the sender...\n");

		iResult = sendto(mySocket, Quotes[index].c_str(), Quotes[index].length() + 1, 0, (SOCKADDR*)& SenderAddr, SenderAddrSize);
		if (iResult == SOCKET_ERROR) {
			printf("sendto failed with error: %d\n", WSAGetLastError());
			WSACleanup();
			return 1;
		}	

		index = (index + 1) % 16; // cycle through all the quotes one at a time
	}

	// When the application is finished sending, close the socket.
	printf("Finished sending. Closing socket.\n");
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
