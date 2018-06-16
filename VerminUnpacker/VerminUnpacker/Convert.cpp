#include <sstream>
#include <stdint.h>
#include <stdio.h>

#include "Convert.h"

unsigned int toUInteger(char* data) {
	return ((unsigned int)data[0] & 0xff) +
			(((unsigned int)data[1] & 0xff) << 8) +
			(((unsigned int)data[2] & 0xff) << 16) +
			(((unsigned int)data[3] & 0xff) << 24);
} 

unsigned long long toULongInteger(char* data) {
	return ((unsigned long long)data[0] & 0xff) +
		(((unsigned long long)data[1] & 0xff) << 8) +
		(((unsigned long long)data[2] & 0xff) << 16) +
		(((unsigned long long)data[3] & 0xff) << 24) +
		(((unsigned long long)data[4] & 0xff) << 32) + 
		(((unsigned long long)data[5] & 0xff) << 40) + 
		(((unsigned long long)data[6] & 0xff) << 48) + 
		(((unsigned long long)data[7] & 0xff) << 56);
}

string toString(int input) {
	ostringstream stream;
	
	// Convert integer to hex string
	stream << input;
	
	return stream.str();
}

string toHex(unsigned int input) {
	stringstream stream;
	
	// Convert integer to hex string
	stream << hex << input;
	
	return "0x" + stream.str();
}

string toHex64(unsigned long long input) {
	stringstream stream;

	// Convert integer to hex string
	stream << hex << input;

	return "0x" + stream.str();
}

const char* toConstChar(string str) {
	char* data;
	
	// Allocate space
	data = new char[str.size() + 1];
	
	// Copy string data
	std::copy(str.begin(), str.end(), data);
	
	// Add a end of string code
	data[str.size()] = '\0';
	
	return data;
}

void toData(char* data, unsigned int n) {
	data[0] = n & 0xff;
	data[1] = (n >> 8) & 0xff;
	data[2] = (n >> 16) & 0xff;
	data[3] = (n >> 24) & 0xff;
}