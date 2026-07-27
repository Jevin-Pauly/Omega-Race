//-----------------------------------------------------------------------------
//
// Real-Time Networking
//----------------------------------------------------------------------------- 

#include <stdio.h>
#include <string.h>
#include "Unused.h"
#include "Snake.h"

Medusa::Medusa()
: head(0)
{
}

Medusa::~Medusa()
{
	Snake *pSnake = (Snake *)this->head;

	// delete every snake associated with Medusa
	while( pSnake != 0 )
	{
		// squirrel away for delete
		Snake *pTmp = pSnake;

		// advanced to next snake
		pSnake = (Snake *)pSnake->next;

		// goodbye old snake
		delete(pTmp);
	}
}

void Medusa::clear()
{
	this->head = nullptr;
}

void Medusa::insertSnake( Snake &inSnake )
{
	// fix pointers
	if( this->head != 0 )
	{
		this->head->prev = &inSnake;
		inSnake.next = this->head;
	}
	// push to the front of head
	this->head = &inSnake;
}

Egg::Egg( char inA, double inB, int inX )
: a(inA),b(inB),x(inX)
{
}

SnakeLink::SnakeLink()
: next(0), prev(0)
{
}

Snake::Snake(unsigned int _key, char inA, double inB, int inX)
: key(_key)
{
	this->pEgg = new Egg( inA, inB, inX);
}

Snake::~Snake()
{
	delete pEgg;
}

// Accessors
const SnakeLink *Medusa::getHeadSnake( ) const
{
	return head;
}

const unsigned int Snake::getKey() const
{
	return this->key;
}

const Egg &Snake::getEgg() const
{
	return *this->pEgg;
}


// Read from a buffer
void Snake::deserialize( const char * const buffer )
{
	// do your magic here
    UNUSED_VAR(buffer);
	const char *p = buffer;

	// Read key
	memcpy(&this->key, p, sizeof(this->key));
	p += sizeof(this->key); // 4 bytes

	// Replace current egg
	if(this->pEgg)
		delete this->pEgg;

	this->pEgg = new Egg();

	// Read Egg data
	memcpy(&this->pEgg->b, p, sizeof(this->pEgg->b));
	p += sizeof(this->pEgg->b); // 8 bytes

	memcpy(&this->pEgg->x, p, sizeof(this->pEgg->x));
	p += sizeof(this->pEgg->x); // 4 bytes

	memcpy(&this->pEgg->a, p, sizeof(this->pEgg->a));
	p += sizeof(this->pEgg->a); // 1 byte
}

// Write object to a buffer
void Snake::serialize( char * const buffer ) const
{
	// do your magic here
    UNUSED_VAR(buffer);
	char *p = buffer;

	// Write key
	memcpy(p, &this->key, sizeof(this->key));
	p += sizeof(this->key); // 4 bytes

	// Write Egg data
	memcpy(p, &this->pEgg->b, sizeof(this->pEgg->b));
	p += sizeof(this->pEgg->b); // 8 bytes

	memcpy(p, &this->pEgg->x, sizeof(this->pEgg->x));
	p += sizeof(this->pEgg->x); // 4 bytes

	memcpy(p, &this->pEgg->a, sizeof(this->pEgg->a));
	p += sizeof(this->pEgg->a); // 1 byte
}

// Read from a buffer
void Medusa::deserialize( const char * const buffer )
{
	// do your magic here
    UNUSED_VAR(buffer);
	const char *p = buffer;

	// Clear old data
	Snake *node = (Snake *)this->head;
	while(node)
	{
		Snake *tmp = node;
		node = (Snake *)node->next;
		delete tmp;
	}
	this->head = nullptr;

	// Read count
	int count = 0;
	memcpy(&count, p, sizeof(count));
	p += sizeof(count); // 4 bytes

	// Allocate temporary array of snakes
	Snake **tempSnakes = new Snake * [count];

	// Deserialize each into array
	for(int i = 0; i < count; ++i)
	{
		tempSnakes[i] = new Snake();
		tempSnakes[i]->deserialize(p);
		p += tempSnakes[i]->getSize();
	}

	// Insert into Medusa in reverse order
	for(int i = count - 1; i >= 0; --i)
	{
		this->insertSnake(*tempSnakes[i]);
	}

	// Clean up temp array (not snakes)
	delete[] tempSnakes;
}

// Write object to a buffer
void Medusa::serialize( char * const buffer ) const
{
	// do your magic here
    UNUSED_VAR(buffer);
	char *p = buffer;

	// Count how many snakes
	int count = 0;
	const SnakeLink *node = this->head;
	while(node)
	{
		count++;
		node = node->next;
	}

	// Write count (for allocation in serialize)
	memcpy(p, &count, sizeof(count));
	p += sizeof(count); // 4 bytes

	// Write each Snake
	node = this->head;
	while(node)
	{
		const Snake *snake = (Snake *)(node);
		snake->serialize(p);
		p += snake->getSize();; // 4 + 8 + 4 + 1 bytes
		node = node->next;
	}
}

// Call for the byte skip
size_t Snake::getSize() const
{
	return sizeof(this->key) + sizeof(this->pEgg->a) + sizeof(this->pEgg->b) + sizeof(this->pEgg->x);
}
