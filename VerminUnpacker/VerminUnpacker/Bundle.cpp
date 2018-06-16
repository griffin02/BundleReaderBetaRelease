#include <windows.h>
#include <iostream>
#include <sstream>

#include "Bundle.h"

void Bundle::initData(char** data, const string name, size_t size) {
	*data = (char*)malloc(size);
	
	if(*data == NULL)
		throw BundleException(BundleError::NO_ALLOCATION, {name, toString(size)});
}

void Bundle::Pack(const string &dir, const string &name, unsigned int signature) {
	string new_dir;
	
	//Init
	n_path = dir + "\\unpacked\\" + name;
	new_dir = dir + "\\" + "packed\\";
	
	reader.OpenUnPacked(n_path);
	
	// Allocate space
	initData(&unpacked_data, "unpacked_data", reader.unzipSpace());
	initData(&packed_data, "packed_data", reader.unzipSize() * 2);	
	
	// Unpack the file
	reader.Pack(unpacked_data, packed_data, signature);
	
	// Check if output folder exist
	DWORD dir_typ = GetFileAttributesA(new_dir.c_str());
	if(dir_typ == INVALID_FILE_ATTRIBUTES || ((dir_typ & FILE_ATTRIBUTE_DIRECTORY) == 0))
		throw BundleException(BundleError::DIRECTORY_NO_FOUND, {new_dir});
	
	// Save output
	reader.saveFile(new_dir + name, packed_data, reader.size());
}

void Bundle::Unpack(const string &dir, const string &name) {
	string new_dir;
	
	//Init
	n_path = dir + "\\" + name;
	new_dir = dir + "\\" + "unpacked\\";
	
	reader.OpenPacked(n_path);
	
	// Allocate space
	initData(&packed_data, "packed_data", reader.size() - HEADER_SIZE);	
	initData(&unpacked_data, "unpacked_data", reader.unzipSpace());
	
	// Unpack the file
	reader.Unpack(packed_data, unpacked_data);
	
	// Check if output folder exist
	DWORD dir_typ = GetFileAttributesA(new_dir.c_str());
	if(dir_typ == INVALID_FILE_ATTRIBUTES || ((dir_typ & FILE_ATTRIBUTE_DIRECTORY) == 0))
		throw BundleException(BundleError::DIRECTORY_NO_FOUND, {new_dir});
	
	// Save output
	reader.saveFile(new_dir + name, unpacked_data, reader.unzipSize());
}

void Bundle::UnpackTempHash(const string &dir, const string &name) {
	string new_dir;

	//Init
	n_path = dir + "\\" + name;
	//new_dir = "original\\";

	reader.OpenPacked(n_path);

	// Allocate space
	initData(&packed_data, "packed_data", reader.size() - HEADER_SIZE);
	initData(&unpacked_data, "unpacked_data", reader.unzipSpace());

	reader.UnpackTemp(name.c_str(), packed_data, unpacked_data);

	free(packed_data);
	free(unpacked_data);
	reader.n_file.close();
}

void Bundle::UnpackTempHashSearch(const string &dir, const string &name, const string &searchTerm) {
	string new_dir;

	//Init
	n_path = dir + "\\" + name;
	//new_dir = "original\\";

	reader.OpenPacked(n_path);

	// Allocate space
	initData(&packed_data, "packed_data", reader.size() - HEADER_SIZE);
	initData(&unpacked_data, "unpacked_data", reader.unzipSpace());

	if(reader.signature() == BundleSignature::NEW)
		reader.UnpackTempSearch(name.c_str(), packed_data, unpacked_data, searchTerm.c_str());
	if(reader.signature() == BundleSignature::OLD)
		reader.vt1UnpackTempSearch(name.c_str(), packed_data, unpacked_data, searchTerm.c_str());

	free(packed_data);
	free(unpacked_data);
	reader.n_file.close();
}

void Bundle::vt1UnpackTempHash(const string &dir, const string &name) {
	string new_dir;

	//Init
	n_path = dir + "\\" + name;
	//new_dir = "original\\";

	reader.OpenPacked(n_path);

	// Allocate space
	initData(&packed_data, "packed_data", reader.size() - HEADER_SIZE);
	initData(&unpacked_data, "unpacked_data", reader.unzipSpace());

	reader.vt1UnpackTemp(name.c_str(), packed_data, unpacked_data);
}

void Bundle::getHashes(const string &dir, const string &name) {
	//Init
	n_path = dir + "\\unpacked\\" + name;
	
	reader.OpenUnPacked(n_path);
	
	// Allocate space
	initData(&unpacked_data, "unpacked_data", reader.unzipSize());
	reader.GetHashes(name.c_str(), unpacked_data);
	
}

void Bundle::vt1getHashes(const string &dir, const string &name) {
	//Init
	n_path = dir + "\\unpacked\\" + name;

	reader.OpenUnPacked(n_path);

	// Allocate space
	initData(&unpacked_data, "unpacked_data", reader.unzipSize());
	reader.vt1GetHashes(name.c_str(), unpacked_data);

}

void Bundle::extract(const string &dir, const string &name, const string &type)
{
	//Init
	n_path = dir + "\\" + name;

	reader.OpenUnPacked(n_path);

	// Allocate space
	initData(&unpacked_data, "unpacked_data", reader.unzipSize());
	reader.ExtractFileFromBundle(name.c_str(), unpacked_data, type.c_str());
}

void Bundle::Draw()  {
	cout << "Path: " << n_path << endl;
	cout << "Size: " << reader.size() << endl;
	cout << "Signature: " << toHex(reader.signature()) << endl;
	cout << "Unzip size: " << toString(reader.unzipSize()) << endl;
	cout << "Unzip space: " << toString(reader.unzipSpace()) << endl;
	cout << "Padding: " << toHex(reader.padding()) << endl;
}

void Bundle::LookupHash(const string &input)
{
	uint64_t hash = 0;
	std::istringstream myStream(input.c_str());
	myStream >> std::hex >> hash;
	reader.LookupHashInput(hash);
}