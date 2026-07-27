//-----------------------------------------------------------------------------
//
// Real-Time Networking
//----------------------------------------------------------------------------- 

#ifndef FISH_H
#define FISH_H

struct apple
{
	// big four
	apple() = default;
	apple(const apple &) = default;
	apple & operator = (const apple &) = default;
	~apple() { a = 0; b = 0; c = 0; }

	int a; // 4
	int b; // 4
	int c; // 4
};

struct orange
{
	// big four
	orange() = default;
	orange(const orange &) = default;
	orange & operator = (const orange &) = default;
	~orange() { a = 0; b = 0; x = 0; d = 0; }

	int		a; // 4
	float	x; // 4
	char	b; // 1
	char	d; // 1
};

class Fish
{
public:
	
	// constructors
	Fish();
	Fish( const apple &inApple, const orange &inOrange, int inX, char inA, float inY);
	Fish(const Fish &) = default;

	// destructor
	~Fish();

	// assignment
	Fish & operator = (const Fish &) = delete;

	// accessors 
	float getY() const;
	int   getX() const;
	char  getA() const ;
	const apple & getApple() const;
	const orange & getOrange() const;

	void clear();

	// Read from a buffer
	void deserialize( const char * const buffer );

	// Write object to a buffer
	void serialize( char * const buffer ) const;

private:
	// data to serialize
	apple	*pApple; // 8 (pointer size)
	orange   *pOrange; // 8
	int      x; // 4
	float    y; //4
	char     a; // 1
	char	pad1;
	char	pad2;
	char	pad3;

};

#endif