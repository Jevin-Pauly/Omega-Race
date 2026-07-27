// Position
// AB 4/21
#include <winsock2.h>

#ifndef _Position
#define _Position

struct Position
{
	int x;
	int y;

	Position()
		: x(0), y(0) {};
	Position(int a, int b)
		: x(a), y(b) {};

	Position operator+(const Position& B) { return Position(this->x + B.x, this->y + B.y); }
	Position operator-(const Position& B) { return Position(this->x - B.x, this->y - B.y); }
	Position operator/(const int& v) { return Position(this->x/v, this->y/v); }

	bool operator==(const Position& B) { return (this->x == B.x) && (this->y == B.y); }

	void deserialize(char* buffer)
	{
		this->x = ntohl(*(int*)buffer);
		this->y = ntohl(*(int*)(buffer + sizeof(int)));
	}

	void serialize(char* buffer) const
	{
		*(int*)buffer = htonl(this->x);
		*(int*)(buffer + sizeof(int)) = htonl(this->y);
	}
};


#endif _Position