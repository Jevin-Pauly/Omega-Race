//-----------------------------------------------------------------------------
//
// Real-Time Networking
//----------------------------------------------------------------------------- 

#include <stdio.h>
#include <string.h>
#include "Unused.h"
#include "Fish.h"

// constructor
Fish::Fish()
: x(0),a(0),y(0.0f),pApple(0),pOrange(0)
{

}

Fish::Fish( const apple &inApple, const orange &inOrange, int inX, char inA, float inY)
{
	this->pApple = new apple(inApple);
	this->pOrange = new orange(inOrange);
	this->x = inX;
	this->a = inA;
	this->y = inY;
}

void Fish::clear()
{
	delete this->pApple;
	delete this->pOrange;

	this->pApple = nullptr;
	this->pOrange = nullptr;
	this->x = 0;
	this->a = 0;
	this->y = 0;
}


// destructor
Fish::~Fish()
{
	//printf("fish destructor\n");
	delete this->pApple;
	delete this->pOrange;
}


float Fish::getY() const
{
	return this->y;
}

int Fish::getX() const
{
	return this->x;
}

char Fish::getA() const 
{
	return this->a;
}

const apple &Fish::getApple() const 
{
	return *(this->pApple);
}

const orange &Fish::getOrange() const
{
	return *(this->pOrange);
}

// Read from a buffer
void Fish::deserialize( const char * const buffer ) 
{
	// do your magic here
	UNUSED_VAR(buffer);
	const char *p = buffer;

	// Read x
	memcpy(&this->x, p, sizeof(this->x));
	p += sizeof(this->x);

	// Read a
	memcpy(&this->a, p, sizeof(this->a));
	p += sizeof(this->a);

	// Read y
	memcpy(&this->y, p, sizeof(this->y));
	p += sizeof(this->y);

	// Allocate and read apple
	if(this->pApple == nullptr)
		this->pApple = new apple();
	memcpy(this->pApple, p, sizeof(apple));
	p += sizeof(apple);

	// Allocate and read orange
	if(this->pOrange == nullptr)
		this->pOrange = new orange();
	memcpy(this->pOrange, p, sizeof(orange));
	p += sizeof(orange);
}

// Write object to a buffer
void Fish::serialize( char * const buffer ) const
{
	// do your magic here
	UNUSED_VAR(buffer);
	char *p = buffer;

	// Write x
	memcpy(p, &this->x, sizeof(this->x));
	p += sizeof(this->x);

	// Write a
	memcpy(p, &this->a, sizeof(this->a));
	p += sizeof(this->a);

	// Write y
	memcpy(p, &this->y, sizeof(this->y));
	p += sizeof(this->y);

	// Write apple (not the pointer, the actual object)
	memcpy(p, this->pApple, sizeof(apple));
	p += sizeof(apple);

	// Write orange (not the pointer, the actual object)
	memcpy(p, this->pOrange, sizeof(orange));
	p += sizeof(orange);
}

