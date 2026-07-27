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
}

// Write object to a buffer
void Fish::serialize( char * const buffer ) const
{
	// do your magic here
    UNUSED_VAR(buffer);
}

