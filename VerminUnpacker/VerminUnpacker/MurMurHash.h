#pragma once
#define BIG_CONSTANT(x) (x)
#define FORCE_INLINE  __forceinline

class MurMurHash
{
public:
	FORCE_INLINE bool IsBigEndian();
	FORCE_INLINE uint64_t BSWAP64(uint64_t u);
	FORCE_INLINE uint64_t getblock64(const uint64_t * const p, const int i = 0L);
	uint64_t MurmurHash64A(const void * key, int len, uint64_t seed);
};

extern MurMurHash Hasher;