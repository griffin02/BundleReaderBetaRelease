#include "stdafx.h"
#include <Windows.h>
#include <stdint.h>
#include <stdio.h>
#include "MurMurHash.h"

const union {
	uint8_t u8[2];
	uint16_t u16;
} EndianMix = { { 1, 0 } };
FORCE_INLINE bool MurMurHash::IsBigEndian()
{
#ifndef NODE_MURMURHASH_TEST_BYTESWAP
	// Constant-folded by the compiler.
	return EndianMix.u16 != 1;
#else
	return true;
#endif // NODE_MURMURHASH_TEST_BYTESWAP
}

FORCE_INLINE uint64_t MurMurHash::BSWAP64(uint64_t u)
{
	return (((u & 0xff00000000000000ULL) >> 56)
		| ((u & 0x00ff000000000000ULL) >> 40)
		| ((u & 0x0000ff0000000000ULL) >> 24)
		| ((u & 0x000000ff00000000ULL) >> 8)
		| ((u & 0x00000000ff000000ULL) << 8)
		| ((u & 0x0000000000ff0000ULL) << 24)
		| ((u & 0x000000000000ff00ULL) << 40)
		| ((u & 0x00000000000000ffULL) << 56));
}

FORCE_INLINE uint64_t MurMurHash::getblock64(const uint64_t * const p, const int i)
{
	if (IsBigEndian()) {
		return BSWAP64(p[i]);
	}
	else {
		return p[i];
	}
}
uint64_t MurMurHash::MurmurHash64A(const void * key, int len, uint64_t seed)
{
	const uint64_t m = BIG_CONSTANT(0xc6a4a7935bd1e995);
	const int r = 47;

	uint64_t h = seed ^ (len * m);

	const uint64_t * data = (const uint64_t *)key;
	const uint64_t * end = data + (len / 8);

	while (data != end)
	{
		uint64_t k = getblock64(data++);

		k *= m;
		k ^= k >> r;
		k *= m;

		h ^= k;
		h *= m;
	}

	const unsigned char * data2 = (const unsigned char*)data;

	switch (len & 7)
	{
	case 7: h ^= uint64_t(data2[6]) << 48;
	case 6: h ^= uint64_t(data2[5]) << 40;
	case 5: h ^= uint64_t(data2[4]) << 32;
	case 4: h ^= uint64_t(data2[3]) << 24;
	case 3: h ^= uint64_t(data2[2]) << 16;
	case 2: h ^= uint64_t(data2[1]) << 8;
	case 1: h ^= uint64_t(data2[0]);
		h *= m;
	};

	h ^= h >> r;
	h *= m;
	h ^= h >> r;

	return h;
}


