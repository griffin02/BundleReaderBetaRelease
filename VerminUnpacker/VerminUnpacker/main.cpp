#include <iostream>

#include "Bundle.h"

using namespace std;

/*
	http://www.winimage.com/zLibDll/index.html
*/

int main() {
	try {
		Bundle b;
		
		//b.Unpack("file\\orginal", "5fdacd98dc8d8ccd");
		//b.Unpack("file\\orginal", "5fdacd98dc8d8ccd.patch_016");
		//b.Unpack("file\\orginal", "5fdacd98dc8d8ccd.patch_017");
		//b.Unpack("file\\orginal", "5fdacd98dc8d8ccd.patch_018");
		//b.Unpack("file\\orginal", "5fdacd98dc8d8ccd.patch_022");
		b.Unpack("file\\orginal", "5fdacd98dc8d8ccd.patch_023");
		
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
		
		b.Draw();
	}
	catch(BundleException &be) {
		cout << be.what() << endl;
	}
	catch(exception &e) {
		cout << e.what() << endl;
	}
	
	return 0;
}