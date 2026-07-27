//-----------------------------------------------------------------------------
//
// Real-Time Networking
//----------------------------------------------------------------------------- 

#ifndef DOG_H
#define DOG_H

class Dog
{
public:
	// big four
	Dog(int _i, char _a, float _y, int _j);
	Dog() = default;
	Dog & operator = (const Dog &) = default;
	~Dog() = default;

	// Read from a buffer
	void deserialize( const char * const buffer );

	// Write object to a buffer
	void serialize( char * const buffer ) const;

	// accessors
	int		getI() const;
	char	getA() const;
	float	getY() const;
	int		getJ() const;

	void clear();

private:
	// data to serialize
	int		i; // 4
	int		j; // 4
	float	y; // 4

	char	a; // 1
	char	pad1; // 1
	char	pad2; // 1
	char	pad3; // 1

};



#endif