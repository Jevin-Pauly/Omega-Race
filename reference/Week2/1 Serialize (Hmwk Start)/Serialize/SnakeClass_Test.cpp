//-----------------------------------------------------------------------------
//
// Real-Time Networking
//----------------------------------------------------------------------------- 

//---------------------------------------------------------------------------
// HEADER FILES:
//---------------------------------------------------------------------------
#include "Snake.h"


//---------------------------------------------------------------------------
// TESTS:
//---------------------------------------------------------------------------
TEST(SnakeClass_0, Serialize_tests, false)
{
	// Create and load a Medusa with several snakes

	// Data
	unsigned int Key[] = { 0xBCCDBACD, 0x1C34FEC1, 0xCCFE0220, 0xFC7C4313 };
	char A[] = { 'r','#', '2', '?' };
	double B[] = { 77.44, 66.77, -23456.99, 9234.87 };
	int X[] = { 55, 10201110, 0xcb, 94 };

	Medusa Gorgon;

	// check medusa
	CHECK(Gorgon.getHeadSnake() == 0);

	// Create several snakes
	Snake* s0 = new Snake(Key[0], A[0], B[0], X[0]);
	Snake* s1 = new Snake(Key[1], A[1], B[1], X[1]);
	Snake* s2 = new Snake(Key[2], A[2], B[2], X[2]);
	Snake* s3 = new Snake(Key[3], A[3], B[3], X[3]);

	Gorgon.insertSnake(*s0);
	Gorgon.insertSnake(*s1);
	Gorgon.insertSnake(*s2);
	Gorgon.insertSnake(*s3);

	// validate the data in the medusa

	Egg tmpEgg;
	Snake* pSnake = (Snake*)Gorgon.getHeadSnake();
	CHECK(pSnake != 0);

	tmpEgg = pSnake->getEgg();

	CHECK(pSnake->getKey() == Key[3]);
	CHECK(tmpEgg.a == A[3]);
	CHECK(tmpEgg.b == B[3]);
	CHECK(tmpEgg.x == X[3]);

	pSnake = (Snake*)pSnake->next;
	CHECK(pSnake != 0);

	tmpEgg = pSnake->getEgg();

	CHECK(pSnake->getKey() == Key[2]);
	CHECK(tmpEgg.a == A[2]);
	CHECK(tmpEgg.b == B[2]);
	CHECK(tmpEgg.x == X[2]);

	pSnake = (Snake*)pSnake->next;
	CHECK(pSnake != 0);

	tmpEgg = pSnake->getEgg();

	CHECK(pSnake->getKey() == Key[1]);
	CHECK(tmpEgg.a == A[1]);
	CHECK(tmpEgg.b == B[1]);
	CHECK(tmpEgg.x == X[1]);

	pSnake = (Snake*)pSnake->next;
	CHECK(pSnake != 0);

	tmpEgg = pSnake->getEgg();

	CHECK(pSnake->getKey() == Key[0]);
	CHECK(tmpEgg.a == A[0]);
	CHECK(tmpEgg.b == B[0]);
	CHECK(tmpEgg.x == X[0]);

	pSnake = (Snake*)pSnake->next;
	CHECK(pSnake == 0);

	// ensure that structure is small and efficient
	CHECK(sizeof(Egg) == 16);

	// Serialize the medusa

		// create a local buffer
	char buff[1024];

	// serialize the data
	Gorgon.serialize(buff);
	Gorgon.clear();
	delete s0;
	delete s1;
	delete s2;
	delete s3;

	// Recreate a medusa from the serialized data

		// Create a new
	Medusa newGorgon;

	// deserialize the data
	newGorgon.deserialize(buff);

	// validate the data in the new_medusa


		// Testing head snake
	pSnake = (Snake*)newGorgon.getHeadSnake();
	CHECK(pSnake != 0);
	tmpEgg = pSnake->getEgg();
	CHECK(pSnake->getKey() == Key[3]);
	CHECK(tmpEgg.a == A[3]);
	CHECK(tmpEgg.b == B[3]);
	CHECK(tmpEgg.x == X[3]);

	// Testing 2nd snake
	pSnake = (Snake*)pSnake->next;
	CHECK(pSnake != 0);
	tmpEgg = pSnake->getEgg();
	CHECK(pSnake->getKey() == Key[2]);
	CHECK(tmpEgg.a == A[2]);
	CHECK(tmpEgg.b == B[2]);
	CHECK(tmpEgg.x == X[2]);

	// Testing 3rd snake
	pSnake = (Snake*)pSnake->next;
	CHECK(pSnake != 0);
	tmpEgg = pSnake->getEgg();
	CHECK(pSnake->getKey() == Key[1]);
	CHECK(tmpEgg.a == A[1]);
	CHECK(tmpEgg.b == B[1]);
	CHECK(tmpEgg.x == X[1]);

	// Testing last snake
	pSnake = (Snake*)pSnake->next;
	CHECK(pSnake != 0);
	tmpEgg = pSnake->getEgg();
	CHECK(pSnake->getKey() == Key[0]);
	CHECK(tmpEgg.a == A[0]);
	CHECK(tmpEgg.b == B[0]);
	CHECK(tmpEgg.x == X[0]);

	// Test that the list ends properly
	Snake* ptmpsnake = pSnake;
	pSnake = (Snake*)pSnake->next;
	CHECK(pSnake == 0);

	// Test the prev links
	CHECK(ptmpsnake->prev->prev->prev == (Snake*)newGorgon.getHeadSnake());
}
TEST_END