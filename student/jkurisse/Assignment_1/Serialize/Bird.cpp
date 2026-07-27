//-----------------------------------------------------------------------------
// Real-Time Networking
//----------------------------------------------------------------------------- 

#include <stdio.h>
#include <string.h>
#include "Unused.h"
#include "Bird.h"

// constructor
Bird::Bird()
: x(0), y(0), s(0)
{
}

// destructor
Bird::~Bird()
{
	//printf("Bird destructor\n");
	delete[] this->s;
}


// specialized constructor
Bird::Bird( int _x, short _y)
	: x(_x), y(_y)
{
	this->s = new char[55]; // do NOT assume fixed size value...
 	const char *refText = "This is a test to have a very long string to Serialize";
	strcpy_s( this->s, 55, refText ); // do NOT assume fixed size value...
}

// accessor
int Bird::getX() const
{
	return this->x;
};

short Bird::getY() const
{
	return this->y;
};

// return a const read pointer to the string
const char *Bird::getS() const
{
	return this->s;
};

void Bird::clear()
{
	memset(s, 0, strlen(s));
	delete[] this->s;
	this->s = nullptr;
	this->x = 0;
	this->y = 0;
}

// Read from a buffer
void Bird::deserialize( const char * const buffer ) 
{
	// do your magic here
	UNUSED_VAR(buffer);
	const char *p = buffer;

	// Read x
	memcpy(&this->x, p, sizeof(this->x));
	p += sizeof(this->x);

	// Read y
	memcpy(&this->y, p, sizeof(this->y));
	p += sizeof(this->y);

	// Read length
	size_t len = 0;
	memcpy(&len, p, sizeof(len)); // len now holds the size of the string incoming
	p += sizeof(len);

	// Allocate memory and copy string
	if(this->s == nullptr) // this might be problem upon further inspection
		this->s = new char[len];
	memcpy(this->s, p, len);
}

// Write object to a buffer
void Bird::serialize( char * const buffer ) const
{
	// do your magic here
	UNUSED_VAR(buffer);
	char *p = buffer;

	// Write x
	memcpy(p, &this->x, sizeof(this->x));
	p += sizeof(this->x);

	// Write y
	memcpy(p, &this->y, sizeof(this->y));
	p += sizeof(this->y);

	// Write length of string
	size_t len = strlen(this->s) + 1; // include null terminator
	memcpy(p, &len, sizeof(len)); // on deserialization we know how much space we need to allocate for the string (including null)
	p += sizeof(len);

	// Write the string itself
	memcpy(p, this->s, len);
}

