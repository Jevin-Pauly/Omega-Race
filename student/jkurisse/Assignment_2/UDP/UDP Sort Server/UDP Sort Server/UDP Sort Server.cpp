// UDP Sort Server.cpp 
#include "UDPServer.h"

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

	//----------------------------------------------------
	// Add your magic here
	//----------------------------------------------------

	UDPServer server;
	server.run();

	//----------------------------------------------------
	// print and exit
	//----------------------------------------------------

	Trace::out("\n");
	Trace::out("Server: Done\n");
	Trace::out("\n");

	// Uncomment for development
	system("PAUSE");
	return 0;
}