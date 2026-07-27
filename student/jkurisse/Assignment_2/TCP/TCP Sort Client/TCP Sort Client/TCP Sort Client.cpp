// TCP Sort Client.cpp 

#include "TestData.h"
#include "TCPClient.h"

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

	//--------------------------------------------------------
	// Get initialize data
	//--------------------------------------------------------
	TestData  data;
	std::list<int>& rNodeList = data.getNodeList();

	//--------------------------------------------------------
	// Print the original data
	//--------------------------------------------------------
	Trace::out("\n\nClient: Original List\n\n");

	int j = 0;
	for (std::list<int>::iterator it = rNodeList.begin(); it != rNodeList.end(); ++it)
	{
		Trace::out("    i:%02d %d\n", j++, *it);
	}

	//--------------------------------------------------------
	// Add your code and hooks here:
	//--------------------------------------------------------

	TCPClient client;
	client.run(rNodeList);




	//--------------------------------------------------------
	// Print the sorted data
	//--------------------------------------------------------
	Trace::out("\n\nClient: Sorted List from the server\n\n");

	int k = 0;
	for (std::list<int>::iterator it = rNodeList.begin(); it != rNodeList.end(); ++it)
	{
		Trace::out("    i:%02d  %d\n", k++, *it);
	}

	Trace::out("\n");
	Trace::out("Client Done\n");
	Trace::out("\n");

	// Uncomment for development
	system("PAUSE");

	return 0;
}
