#ifndef H_BUNDLE
#define H_BUNDLE

#include <string>

#include "Convert.h"
#include "BundleReader.h"
#include "BundleException.h"

using namespace std;

enum BundleSignature {
	OLD = 0xf0000004,
	NEW = 0xf0000005
};

class Bundle {
	private:
		string n_path;
		
		char* packed_data;
		char* unpacked_data;
		
		BundleReader reader;
		
		void initData(char** data, const string name, size_t size);
		
	public:
		// Construction and Deconstruction
		Bundle() {};
		~Bundle() {};
		
		void Pack(const string &dir, const string &name, unsigned int signature);
		void Unpack(const string &dir, const string &name);
		void UnpackTempHash(const string &dir, const string &name);
		void UnpackTempHashSearch(const string &dir, const string &name, const string &searchTerm);
		void vt1UnpackTempHash(const string &dir, const string &name);
		void getHashes(const string &dir, const string &name);
		void vt1getHashes(const string &dir, const string &name);
		void extract(const string &dir, const string &name, const string &type);
		void LookupHash(const string &input);
		
		// Debug
		void Draw();
};

#endif