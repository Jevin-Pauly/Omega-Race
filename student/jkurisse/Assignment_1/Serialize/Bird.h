//-----------------------------------------------------------------------------
//
// Real-Time Networking
//----------------------------------------------------------------------------- 

#ifndef BIRD_H
#define BIRD_H

class Bird
{
public:
	// constructors
	Bird();
	Bird( int _x, short _y);

	// destructor
	~Bird();

	// assignment
	Bird & operator = (const Bird &) = delete;

	// accessors
	int getX() const;
	short getY() const;
	const char *getS() const;

	void clear();

	// Read from a buffer
	void deserialize( const char * const buffer );

	// Write object to a buffer
	void serialize( char * const buffer ) const;

private:
	// data to serialize
	int     x; // 4
	char	*s; // 8
	short	y; // 2
};

#endif