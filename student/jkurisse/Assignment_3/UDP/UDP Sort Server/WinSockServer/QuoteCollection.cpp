#include "QuoteCollection.h"

QuoteCollection::QuoteCollection(std::string* pquotes, int count)
	: quoteCol(pquotes), total(count), index(0)
{

}

std::string& QuoteCollection::NextQuote()
{
	index = (index + 1) % total;

	return quoteCol[index];
}