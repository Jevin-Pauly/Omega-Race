#include <iostream>

struct A
{
	char r;	// 1 bytes
	char g;	// 1 bytes
	char b;	// 1 bytes
};

struct B
{
	int x;	// 4 bytes
	int y;	// 4 bytes
};

struct C
{
	int x;	// 4 bytes
	char b;	// 1 bytes
};

struct D
{
	void* p;  // 4 bytes
	char  a;  // 1 bytes
	float x;  // 4 bytes
	char  b;  // 1 bytes
	int   y;  // 4 bytes
	char  c;  // 1 bytes
};

struct D_Expl
{
	void* p;  // 4 bytes
	char  a;  // 1 bytes
	char  pad1;
	char  pad2;
	char  pad3;
	float x;  // 4 bytes
	char  b;  // 1 bytes
	char  pad4;
	char  pad5;
	char  pad6;
	int   y;  // 4 bytes
	char  c;  // 1 bytes
	char  pad7;
	char  pad8;
	char  pad9;
};

struct D_Better
{
	void* p;  // 4 bytes
	float x;  // 4 bytes
	int   y;  // 4 bytes
	char  a;  // 1 bytes
	char  b;  // 1 bytes
	char  c;  // 1 bytes
	char  pad1;
};

struct E
{
	char a;		// 1 byte
	double t;	// 8 bytes
	float s;	// 4 bytes
};

struct E_Better
{
	double t;	// 8 bytes
	float s;	// 4 bytes
	char a;		// 1 byte
	char  pad1;
	char  pad2;
	char  pad3;
};


int main()
{
	printf("Size of bool: %i bytes\n", sizeof(bool));
	printf("Size of A: %i bytes\n", sizeof(A));
	printf("Size of B: %i bytes\n", sizeof(B));
	printf("Size of C: %i bytes\n", sizeof(C));

	D_Better testD;
	//D testD;
	testD.p = &testD;
	testD.a = 'A';
	testD.x = 1;
	testD.b = 'B';
	testD.y = 2;
	testD.c = 'C';

	C array_C[3];

	array_C[0].x = 1;		// known data
	array_C[0].b = 'A';
	array_C[1].x = 2;
	array_C[1].b = 'B';
	array_C[2].x = 3;
	array_C[2].b = 'C';

	printf("Size of D: %i bytes\n", sizeof(D));
	printf("Size of D_Expl: %i bytes\n", sizeof(D_Expl));
	printf("Size of D_Better: %i bytes\n", sizeof(D_Better));

	printf("Size of E: %i bytes\n", sizeof(E));
	printf("Size of E_Better: %i bytes\n", sizeof(E_Better));


	printf("\n");
	system("pause");
}
