#ifndef H_BUNDLE_READER
#define H_BUNDLE_READER

#include <string>
#include <vector>
#include <fstream>
#include <json_dto\pub.hpp>

#define HEADER_SIZE  0xC
#define ZLIB_CHUNK   0x10000

using namespace std;

class HashList
{
public:
	vector<string> hashFileList{};
	vector<string> hashTypeList{};
	//vector<string> realTypeList{};
};

class UnkHashList
{
public:
	vector<unsigned long long> hashTypeList{};
};

class FileHashList
{
public:
	unsigned long long hashFileName;
	unsigned long long hashFileType;
};


namespace json_dto
{

	template < typename JSON_IO >
	void
		json_io(JSON_IO & io, HashList &obj)
	{
		io
			& json_dto::mandatory("fileNameHash", obj.hashFileList)
			& json_dto::mandatory("fileTypeHash", obj.hashTypeList);
			//& json_dto::mandatory("realFileType", obj.realTypeList);
	}

	template < typename JSON_IO >
	void
		json_io(JSON_IO & io, UnkHashList &obj)
	{
		io
			& json_dto::mandatory("fileTypeHash", obj.hashTypeList);
	}

}

//template <typename JSON_IO> void json_io(JSON_IO &io, HashList &obj)
//{
//	io && json_dto::mandatory("hashList", obj.hashList);
//}

class BundleReader {
	private:
		
		
		size_t n_size;
		unsigned int n_signature;
		size_t n_unzip_size;
		int n_padding;
		int n_total_zlib;
		int n_total_files;
	public:
		// Construction and Deconstruction
		BundleReader() : n_size(0), n_unzip_size(0), n_padding(0), n_total_zlib(0), n_total_files(0) {};
		~BundleReader() {};
		ifstream n_file;
		const size_t size() const { return n_size; };
		const unsigned int signature() const { return n_signature; };
		const size_t unzipSize() const { return n_unzip_size; };
		const size_t unzipSpace() const { return ((n_unzip_size / ZLIB_CHUNK) + 1) * ZLIB_CHUNK; };
		const int padding() const { return n_padding; };
		const int totalZlibFiles() const { return n_total_zlib; };
		const int totalFiles() const { return n_total_files; }
		
		void OpenPacked(const string file_path);
		void OpenUnPacked(const string file_path);
		
		void Pack(char* unpacked_data, char* packed_data, unsigned int signature);
		void Unpack(char* packed_data, char* unpacked_data);
		void UnpackTemp(const char* name, char* packed_data, char* unpacked_data);
		void UnpackTempSearch(const char* name, char* packed_data, char* unpacked_data, const char* searchTerm);
		void vt1UnpackTemp(const char* name, char* packed_data, char* unpacked_data);
		void vt1UnpackTempSearch(const char* name, char* packed_data, char* unpacked_data, const char* searchTerm);
		
		void saveFile(const string file_path, char* data, int size);
		void saveZlib(int nr, char* data, int size);
		
		// Zlib compress/decompress
		unsigned int deflateZlib(char* source, char* dest);
		unsigned int inflateZlib(char* source, char* dest);

		void GetHashes(const char* name, char* unpacked_data);
		void vt1GetHashes(const char* name, char* unpacked_data);
		void GetHashesTemp(const char* name, char* unpacked_data, int size);
		void GetHashesTempSearch(const char* name, char* unpacked_data, int size, const char* searchTerm);
		void vt1GetHashesTempSearch(const char* name, char* unpacked_data, int size, const char* searchTerm);
		void vt1GetHashesTemp(const char* name, char* unpacked_data, int size);
		void ExtractFileFromBundle(const char* name, char* unpacked_data, const char* fileType);
		void LookupHashInput(uint64_t inputHash);
		void LoadAdditionalDictionary();
};

#endif