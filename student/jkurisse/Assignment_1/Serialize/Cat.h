//-----------------------------------------------------------------------------
//
// Real-Time Networking
//----------------------------------------------------------------------------- 

#ifndef CAT_H
#define CAT_H

class Cat
{
public:
	// big 4
	Cat(float _x, char _a, char _b, float _y, int _i, char _c, double _d);
	Cat() = default;
	Cat & operator = (const Cat &) = default;
	~Cat() = default;

	// Read from a buffer
	void deserialize( const char * const buffer );

	// Write object to a buffer
	void serialize( char * const buffer ) const;

	// accessors
	float	getX() const;
	char	getA() const;
	char	getB() const;
	float	getY() const;
	int		getI() const;
	char	getC() const;
	double	getD() const;

	void clear();

private:

	// data to serialize
	double	d; // 8
	float	x; // 4
	float	y; // 4
	int	    i; // 4
	char	c; // 1
	char	a; // 1
	char	b; // 1
	char	pad1; // 1

};



#endif