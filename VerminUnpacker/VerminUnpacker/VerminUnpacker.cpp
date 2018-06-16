// VerminUnpacker.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "Bundle.h"
#include <Windows.h>
#include <time.h>
#include <thread>

int _tmain(int argc, char** argv)
{
	try {

		//cout << argv[1] << endl;
		Bundle b;

		//b.Unpack("file\\orginal", "5fdacd98dc8d8ccd");
		//b.Unpack("file\\orginal", "5fdacd98dc8d8ccd.patch_016");
		//b.Unpack("file\\orginal", "5fdacd98dc8d8ccd.patch_017");
		//b.Unpack("file\\orginal", "5fdacd98dc8d8ccd.patch_018");
		//b.Unpack("file\\orginal", "5fdacd98dc8d8ccd.patch_022");

		
		if (strcmp(argv[1], "-u") == 0)
		{
			b.Unpack(argv[2], argv[3]);
		}

		if (strcmp(argv[1], "-p") == 0)
		{
			b.Pack(argv[2], argv[3], BundleSignature::NEW);
		}

		if (strcmp(argv[1], "-hash") == 0)
		{
			b.getHashes(argv[2], argv[3]);
		}

		if (strcmp(argv[1], "-vt1hash") == 0)
		{
			b.vt1getHashes(argv[2], argv[3]);
		}

		if (strcmp(argv[1], "-temphash") == 0)
		{
			b.UnpackTempHash(argv[2], argv[3]);
		}

		if (strcmp(argv[1], "-vt1temphash") == 0)
		{
			b.vt1UnpackTempHash(argv[2], argv[3]);
		}

		if (strcmp(argv[1], "-e") == 0)
		{
			char* origFile = "unpacked";
			b.extract(origFile, argv[2], argv[3]);
		}

		if (strcmp(argv[1], "-lookup") == 0)
		{
			b.LookupHash(argv[2]);
		}

		if (strcmp(argv[1], "-find") == 0)
		{
			int i = 1;
			vector<string> FilesFound;
			char bufferName[0xFFFF];
			namespace fs = std::experimental::filesystem;
			for (auto & p : fs::directory_iterator(argv[2]))
			{
				if (std::experimental::filesystem::is_directory(p.path().c_str()) == false)
				{
					if (wcsstr(p.path().filename().c_str(), L".patch") != 0 || wcsstr(p.path().filename().c_str(), L".") == 0)
					{
						if (wcsstr(p.path().filename().c_str(), L".stream") == 0)
						{
							wcstombs(bufferName, p.path().filename().c_str(), wcslen(p.path().filename().c_str()));
							FilesFound.push_back(bufferName);
							ZeroMemory(bufferName, 0xFFFF);
						}
					}
				}
			}
			
			for (int i = 0; i < FilesFound.size(); i++)
			{
				printf("Searching %i / %i\n", i, FilesFound.size() - 1);
				b.UnpackTempHashSearch(argv[2], FilesFound.at(i).c_str(), argv[3]);
			}
		}
		
		//b.Unpack("file\\orginal", "69cb7fbf3a4e4572");

		//b.Unpack("file\\orginal", "00f1a958e7d2dad9");
		//b.Pack("file\\unpacked", "00f1a958e7d2dad9", BundleSignature::OLD);

		//b.Unpack("file\\orginal", "1f72bcbee0849acb");
		//b.Pack("file\\unpacked", "1f72bcbee0849acb", BundleSignature::OLD);

		//b.Unpack("file\\orginal", "5a86b012ac3ab932");
		//b.Pack("file\\unpacked", "5a86b012ac3ab932", BundleSignature::OLD);

		//b.Unpack("file\\orginal", "4781bad4b7d3a395.patch_012");
		//b.Pack("file\\unpacked", "4781bad4b7d3a395.patch_012", BundleSignature::NEW);

		//b.Unpack("file\\orginal", "bf28271943f6b9c1");
		//b.Pack("file\\unpacked", "bf28271943f6b9c1", BundleSignature::OLD);

		//b.Draw();
	}
	catch (BundleException &be) {
		cout << be.what() << endl;
	}
	catch (exception &e) {
		cout << e.what() << endl;
	}

	//getchar();
	return 0;
}

