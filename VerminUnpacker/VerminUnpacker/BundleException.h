#ifndef H_BUNDLE_EXCEPTION
#define H_BUNDLE_EXCEPTION

#include <iostream>
#include <vector>

using namespace std;

enum BundleError {
	UNKNOWN = 0,
	DIRECTORY_NO_FOUND = 1,
	CAN_NOT_OPEN = 2,
	SIZE_TO_SMALL = 3,
	SIGNATURE = 4,
	READ_TO_MUCH_DATA = 5,
	NO_ALLOCATION = 6,
	INFLATE_ZLIB = 7,
	DEFLATE_ZLIB = 8
};

class BundleException : public exception {
	private:
		enum BundleError n_error;
		vector<string> n_args;
	
	public:
		// Construction and Deconstruction
		BundleException() : n_error(BundleError::UNKNOWN) {};
		BundleException(enum BundleError error_id) : n_error(error_id) {};
		BundleException(enum BundleError error_id, const vector<string> args) : n_error(error_id), n_args(args) {};
		~BundleException() {};
		
		virtual const char* what() const throw();
};

#endif