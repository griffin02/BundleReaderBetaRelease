#include "Bundle.h"

const char* BundleException::what() const throw() {
	string error;
	
	// Init
	error = "[ERROR] ";
	
	switch(n_error) {
		// Unknown error
		case BundleError::UNKNOWN:
			error += "Unknown";
		break;
		case BundleError::DIRECTORY_NO_FOUND:
			if(n_args.size() == 1)
				error += "Can not find directory: " + n_args[0];
		break;
		case BundleError::CAN_NOT_OPEN:
			error += "Can not open file path.";
			
			// Add file size to exception
			if(n_args.size() == 1)
				error += " File path: " + n_args[0];
		break;
		case BundleError::SIZE_TO_SMALL:
			error += "The file size of the bundle file is to small.";
			
			// Add file size to exception
			if(n_args.size() == 1)
				error += " Found Size: " + n_args[0];
		break;
		case BundleError::SIGNATURE:
			error += "Not a bundle file signature.";
			
			// Add signature to exception
			if(n_args.size() == 1)
				error += " Found Signature: " + n_args[0];
		break;
		case BundleError::READ_TO_MUCH_DATA:
			error += "To much data try to get read from bundle file.";
			
			if(n_args.size() == 3)
				error += "Try to read adress " + n_args[0] + "-" + n_args[1] + " but only till adress " + n_args[2] + " is available.";
		break;
		case BundleError::NO_ALLOCATION:
			error += "Can not allocate space.";
			
			if(n_args.size() == 2)
				error += " This happends in '" + n_args[0] + "' when it tried to allocate " + n_args[1] + " space";
		break;
		case BundleError::INFLATE_ZLIB:
			error += "Inflate zlib data went wrong.";
			
			if(n_args.size() == 1)
				error += " Error code: " + n_args[0];
		break;
		case BundleError::DEFLATE_ZLIB:
			error += "Deflate zlib data went wrong.";
		break;
		default:
			error += "Unknown error code.";
		break;
	}
	
	return toConstChar(error);
}