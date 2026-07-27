// QuoteCollection
// AB 4/21

#ifndef _QuoteCollection
#define _QuoteCollection

#include <string>

class QuoteCollection
{
public:
	QuoteCollection() = delete;
	~QuoteCollection() = default;
	QuoteCollection(const QuoteCollection&) = delete;
	QuoteCollection& operator=(const QuoteCollection&) = delete;

	QuoteCollection(std::string* pquotes, int count);
	std::string& NextQuote();

private:
	std::string* quoteCol;
	int total;
	int index;
};

#endif _QuoteCollection
