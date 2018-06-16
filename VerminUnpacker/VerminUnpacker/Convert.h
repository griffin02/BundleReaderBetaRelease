#ifndef H_CONVERT
#define H_CONVERT

#include <string>

using namespace std;

unsigned int toUInteger(char* data);
unsigned long long toULongInteger(char* data);
string toString(int input);
string toHex(unsigned int input);
string toHex64(unsigned long long input);
const char* toConstChar(string str);
void toData(char* data, unsigned int n);

#endif