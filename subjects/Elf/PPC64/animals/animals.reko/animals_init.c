// animals_init.c
// Generated by decompiling animals
// using Reko decompiler version 0.12.1.0.

#include "animals.h"

// 0000000000002760: void fn0000000000002760(Register (ptr64 Eq_n) r2)
// Called from:
//      fn0000000000002780
void fn0000000000002760(struct Eq_n * r2)
{
	<anonymous> * r12_n = r2->ptrFFFF8280;
	r12_n();
}

// 0000000000002780: void fn0000000000002780(Register (ptr64 Eq_n) r2, Register word64 lr, Register word64 xer)
void fn0000000000002780(struct Eq_n * r2, word64 lr, word64 xer)
{
	if (r2->qwFFFF8008 != 0x00)
	{
		fn0000000000002760(r2);
		struct Eq_n * qwLoc48;
		r2 = qwLoc48;
	}
	fn0000000000002BD0(r2, lr, xer);
	fn0000000000005820(r2);
}

