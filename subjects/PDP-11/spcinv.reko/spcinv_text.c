// spcinv_text.c
// Generated by decompiling spcinv.sav
// using Reko decompiler version 0.12.1.0.

#include "spcinv.h"

Eq_n t0000 = // 0000
	{
		0x00,
		0x00,
		0x00,
		0x00,
		0x00,
		0x00,
		0x00,
		0x00,
		0x00,
		0x00,
		0x00,
		0x00,
		0x00,
		0x00,
		0x00,
		0x0200,
		0x0200,
		0x00,
		0x00,
		0x12A2,
		0x00,
		0x00,
	};
Eq_n g_t0002 = // 0002
	{
		0x00,
		0x00,
		0x00,
	};
// 0200: void fn0200()
void fn0200()
{
	word16 r5_n;
	byte * r4_n;
	byte * r4_n;
	byte * r4_n;
	struct Eq_n * r2_n;
	struct Eq_n * r3_n;
	word16 r0_n;
	do
	{
		PRINT(&g_b0F9A);
		r2_n = (struct Eq_n *) &t0000.w0002;
		r3_n = null;
		while (TTYIN(out r0_n))
			;
		word16 r0_n;
		while (TTYIN(out r0_n))
			;
		word16 r0_n;
		while (TTYIN(out r0_n))
			;
		if ((byte) r0_n == 66)
		{
			PRINT(&g_b0FDA);
			word16 r0_n;
			while (TTYIN(out r0_n))
				;
			break;
		}
		r2_n = (struct Eq_n *) ((char *) &t0000.w0000 + 1);
		r3_n = (struct Eq_n *) ((char *) &t0000.w0000 + 1);
		if ((byte) r0_n == 0x49)
			break;
		r2_n = null;
		r3_n = (struct Eq_n *) &t0000.w0002;
	} while ((byte) r0_n != 0x45);
	g_ptr0F06 = r2_n;
	g_ptr0F08 = r3_n;
	t0000.w0024 |= 0x1040;
	g_w1166 = 0x1100;
	g_w1168 = 4464;
	FnSubfn(&g_w1166);
	g_w0AB4 = g_w1170;
	g_t0B5E.u1 = (struct Eq_n *) 0x00;
	g_w1166 = 0x0101;
	g_w1168 = 4446;
	if (!FnSubfn(&g_w1166))
	{
		g_w1166 = 0x0801;
		g_w1168 = 0x00;
		g_w116A = 0x0B5E;
		g_w116C = 0x01;
		g_w116E = 0x00;
		FnSubfn(&g_w1166);
		__syscall<word16>(0x88FC);
	}
	fn0BD6();
l02A2:
	byte * r4_n = fn0C20();
	word16 r0_n;
	while (TTYIN(out r0_n) || g_ptr0EFA != null)
	{
		g_w1166 = 0x1100;
		g_w1168 = 4464;
		FnSubfn(&g_w1166);
		Eq_n r0_n = g_w1170 - g_w1172;
		if (r0_n >= 0x00)
		{
			g_w1174 = g_w1170;
			++g_w116E;
			word16 v40_n = ~g_w0EF4;
			g_w0EF6 = v40_n;
			if (v40_n == 0x00)
				g_w0EF8 = ~g_w0EF6;
			if (g_ptr0EFA != null)
			{
				word16 v46_n = g_ptr0EFA - 0x01;
				g_w0EFC = v46_n;
				if (v46_n != 0x00)
					goto l0370;
				if (g_w0F18 == 0x00)
					goto l03AE;
				Eq_n r0_n = fn0486(r0_n, r4_n, out r4_n);
				g_w0F16 = 0x02;
				r4_n = fn0470(r0_n, r4_n);
				g_w0F18 = 0x78;
			}
			if (g_w0EF8 == 0x00)
			{
				if (g_t0F14.u0 >= 0x08)
					goto l0370;
				ci16 v70_n = g_w0F16 - 0x01;
				g_w0F18 = v70_n;
				if (v70_n > 0x00)
					goto l0370;
				g_ptr0EFA = &g_t0456;
			}
			g_ptr0EFA();
l0370:
			byte * r4_n;
			r0_n = fn0998(fn07A6(fn06D6(fn04A0(r4_n)), out r4_n), r4_n, out r4_n);
			if (r4_n != &g_b1178)
				goto l038E;
			if (g_w0F12 != 0x00)
				continue;
			if (g_ptr0F02 != null)
				goto l0392;
			if (g_w0F18 > 0x00)
			{
				g_ptr0F1A = g_w0F18 + 0x01;
				goto l02A2;
			}
l03AE:
			if (g_t0B5A.u1 > g_t0B5A.u1)
			{
				g_t0B5E.u1 = g_t0B5A.u1;
				g_w1166 = 0x0101;
				g_w1168 = 4446;
				if (!FnSubfn(&g_w1166))
				{
					g_w1166 = 0x0201;
					g_w1168 = 4446;
					g_w116A = 0x01;
					r0_n.u0 = 4454;
					if (!FnSubfn(&g_w1166))
						goto l03E6;
				}
				else
				{
l03E6:
					g_w1166 = 0x0901;
					g_w1168 = 0x00;
					g_w116A = 0x0B5E;
					g_w116C = 0x01;
					g_w116E = 0x00;
					FnSubfn(&g_w1166);
					__syscall<word16>(0x88FC);
					r0_n.u0 = 0x0601;
				}
			}
			Eq_n r0_n = fn0486(r0_n, r4_n, out r4_n);
			struct Eq_n * sp_n = (struct Eq_n *) <invalid>;
			sp_n->bFFFFFFFF = 0x01;
			sp_n->b0000 = 0x18;
			Eq_n r0_n = fn0AB6(r0_n, r4_n, out r4_n, out r5_n);
			sp_n->wFFFFFFFD = r5_n;
			word16 r0_n;
			word16 r4_n;
			word16 r5_n;
			fn0AE8(r0_n, r4_n, &g_ptr0420, out r0_n, out r4_n, out r5_n);
			return;
		}
		if (r4_n == &g_b1178)
			continue;
l038E:
		r4_n = fn0AF6(r4_n);
l0392:
	}
	g_b02CB = (byte) r0_n;
	byte * r1_n = &g_b02C6;
	byte * r1_n;
	do
	{
		r1_n = r1_n + 1;
		r1_n = r1_n;
	} while ((byte) r0_n != *r1_n);
	(*((char *) g_a02CC + (r1_n - 711) * 0x02))();
}

byte g_b02C6 = 44; // 02C6
byte g_b02CB = 0x00; // 02CB
<anonymous> * g_a02CC[] = // 02CC
	{
	};
byte * g_ptr0420 = &g_b1121; // 0420
<anonymous> g_t0456 = <code>; // 0456
// 0470: Register (ptr16 byte) fn0470(Register Eq_n r0, Register (ptr16 byte) r4)
// Called from:
//      fn0200
byte * fn0470(Eq_n r0, byte * r4)
{
	word16 r5_n;
	byte * r4_n;
	Eq_n r0_n = fn0AB6(r0, r4, out r4_n, out r5_n);
	word16 r0_n;
	byte * r4_n;
	word16 r5_n;
	fn0AE8(r0_n, r4_n, &g_ptr0482, out r0_n, out r4_n, out r5_n);
	return r4_n;
}

byte * g_ptr0482 = &g_b1122; // 0482
// 0486: Register Eq_n fn0486(Register Eq_n r0, Register (ptr16 byte) r4, Register out word16 r4Out)
// Called from:
//      fn0200
Eq_n fn0486(Eq_n r0, byte * r4, word16 & r4Out)
{
	word16 r5_n;
	word16 r4_n;
	word16 r5_n;
	byte * r4_n;
	Eq_n r0_n = fn0A74(fn0AB6(r0, r4, out r4_n, out r5_n), r4_n, out r4_n, out r5_n);
	r4Out = r4_n;
	return r0_n;
}

// 04A0: Register (ptr16 byte) fn04A0(Register (ptr16 byte) r4)
// Called from:
//      fn0200
byte * fn04A0(byte * r4)
{
	struct Eq_n * r5_n;
	byte * r4_n;
	word16 r5_n;
	byte * r4_n;
	ptr16 fp;
	byte * r4_n;
	cui16 v6_n = g_w0EF2 & g_w0EF4;
	g_w0EF2 = v6_n;
	if (v6_n != 0x00)
		return r4;
	union Eq_n * r5_n = g_ptr0F04;
	do
	{
		Eq_n r3_n;
		r3_n.u1 = (int16) (&r5_n->u0)[0x0EF0];
		if (r3_n != 0x00)
		{
			g_t0F0C.u0 = (wchar_t) r3_n;
			Eq_n r0_n = fn0AB6((int16) (&r5_n->u0)[0x0EF3], r4, out r4_n, out r5_n);
			*r4_n = 0x20;
			struct Eq_n * sp_n = fp - 0x01;
			r4 = r4_n + 1;
			Eq_n r0_n = r0_n - 0x01;
			if ((g_ptr0F02 > null || (byte) r0_n > 0x05) && (byte) r0_n != 0x03)
			{
				(&r5_n->u0)[0x0EF3] = (struct Eq_n) ((byte) r0_n - 0x01);
				if ((byte) r0_n >= 0x17)
				{
					sp_n = (struct Eq_n *) <invalid>;
					if (!fn067C(r0_n - 0x01, r3_n, r4_n + 1, out r0_n, out r4, out r5_n))
						goto l04EE;
				}
				else
				{
l04EE:
					word16 r1_n = (word16) ((word32) r0_n + 1);
					Eq_n r0_n;
					if (((byte) r0_n != 0x03 || (g_ptr0F02 <= null || !fn05D4(r0_n, r3_n, r4, out r0_n, out r3_n, out r4, out r5_n))) && !fn064A(r0_n, r1_n, r3_n, r4, out r0_n, out r4, out r5_n))
					{
						struct Eq_n * r2_n = null;
						do
						{
							if (r0_n == r2_n[0x06DC])
							{
l0524:
								r0_n.u1 = r2_n[0x06D6];
								word16 r1_n;
								for (r1_n = 0x08; r1_n != 0x00; --r1_n)
								{
									Eq_n r3_n = r0_n.u1->t0000.u1;
									if (r3_n != 0x00)
									{
										if (r3_n >= 0x00)
										{
											if ((&r5_n->u0)[0x0EF0] <= (byte) r3_n + 0x04)
											{
												if ((&r5_n->u0)[0x0EF0] < ((r0_n.u1)->t0000).u0)
													break;
												Eq_n r1_n = r0_n.u1->t0000.u1;
												r0_n.u1->t0000.u1 = (cui16) (r0_n.u1->t0000.u1 | 0x8000);
												Eq_n r0_n = r0_n - r2_n[0x06D6];
												g_t0F0A.u1 = (struct Eq_n *) r0_n;
												r0_n.u1[0x06E2] = (struct Eq_n) (r0_n.u1[0x06E2] - 0x01);
												struct Eq_n * sp_n = sp_n - 0x01;
												sp_n->b0000 = (byte) r1_n;
												sp_n->t0001.u0 = (byte) r2_n[0x06DC];
												byte * r4_n;
												word16 r5_n;
												byte * r4_n;
												fn0A7C(fn0AB6(r0_n, r4, out r4_n, out r5_n), r4_n, out r4_n);
												g_w0B56 = (word16) r2_n[1770] + g_w0B56;
												r4 = fn0B1A(r4_n, out r5_n);
												Eq_n v75_n = g_w0F12 - 0x01;
												g_t0F14.u0 = (wchar_t) v75_n;
												if (v75_n <= 0x00 && g_ptr0F02 <= null)
												{
													g_ptr0EFA = null;
													g_w0EFC = 0x05;
												}
												goto l0584;
											}
										}
										else
										{
											struct Eq_n * sp_n = sp_n - 0x01;
											sp_n->b0000 = r0_n.u1->t0000.u0;
											sp_n->t0001.u0 = (byte) r2_n[0x06DC];
											Eq_n r0_n = fn0A74(fn0AB6(r0_n, r4, out r4_n, out r5_n), r4_n, out r4_n, out r5_n);
											r0_n.u1->t0000.u1 = 0x00;
											sp_n->bFFFFFFFF = r5_n->b0EF0 + 0x01;
											sp_n->b0000 = r5_n->b0EF3;
											sp_n = sp_n - 0x01;
											r0_n = fn0AB6(r0_n, r4_n, out r4, out r5_n);
										}
									}
									r0_n.u1 = (word32) r0_n + 2;
								}
								break;
							}
							cup16 v46_n = r1_n - r2_n[0x06DC];
							if (v46_n < 0x00)
								break;
							if (v46_n == 0x00)
								goto l0524;
							++r2_n;
						} while (r2_n <= &t0000.w000A);
						sp_n->wFFFFFFFE = r5_n;
						word16 r0_n;
						byte * r4_n;
						word16 r5_n;
						fn0AE8(r0_n, r4, &g_ptr05B8, out r0_n, out r4_n, out r5_n);
						return r4_n;
					}
				}
			}
l0584:
			(&r5_n->u0)[0x0EF0] = (struct Eq_n) 0x00;
			goto l0588;
		}
l0588:
		--r5_n;
	} while (r5_n >= null);
	return r4;
}

byte * g_ptr05B8 = &g_b1116; // 05B8
// 05D4: FlagGroup bool fn05D4(Register Eq_n r0, Register Eq_n r3, Register (ptr16 byte) r4, Register out Eq_n r0Out, Register out Eq_n r3Out, Register out (ptr16 byte) r4Out, Register out (ptr16 Eq_n) r5Out)
// Called from:
//      fn04A0
bool fn05D4(Eq_n r0, Eq_n r3, byte * r4, union Eq_n & r0Out, union Eq_n & r3Out, byte & r4Out, struct Eq_n & r5Out)
{
	struct Eq_n * r5_n;
	byte * r4_n;
	Eq_n r0_n;
	word16 r5_n;
	byte * r4_n;
	cup16 v9_n = r3 - g_ptr0F02;
	Eq_n NZVC_n;
	NZVC_n.u1 = cond(v9_n - 0x04);
	if (v9_n > 0x04)
	{
		r0Out = r0;
		r3Out = r3;
		r4Out = r4;
		struct Eq_n * r5;
		r5Out = r5;
		return (NZVC_n & 0x04) != 0x00;
	}
	else
	{
		byte * r4_n;
		word16 r5_n;
		byte * r4_n;
		fn0A7C(fn0AB6(r0, r4, out r4_n, out r5_n), r4_n, out r4_n);
		uint16 r0_n = fn0A94();
		ui16 r3_n = __rcl<word16,byte>(__rcl<word16,byte>(0x00, 0x01, cond(r0_n << 1) & 0x01), 0x01, cond(r0_n << 2) & 0x01);
		Eq_n r0_n;
		r0_n.u1 = g_a0F2A[r3_n];
		Eq_n v28_n = g_t0F0E.u0 >> 1;
		g_t0F0E.u0 = (int16) v28_n;
		if (v28_n >= 0x00)
			r0_n.u1 = (word32) r0_n + 200;
		g_w0B58 = &r0_n.u1->t0000.u0 + g_w0B58;
		Eq_n r0_n = fn0AB6(r0_n, r4_n, out r4_n, out r5_n);
		Eq_n Z_n = fn0AE8(r0_n, r4_n, &g_ptr0624, out r0_n, out r4_n, out r5_n);
		r0Out = r0_n;
		r3Out = r3_n << 1;
		r4Out = r4_n;
		r5Out = r5_n;
		return Z_n != 0x00;
	}
}

byte * g_ptr0624 = &g_b1121; // 0624
// 064A: FlagGroup bool fn064A(Register Eq_n r0, Register word16 r1, Register Eq_n r3, Register (ptr16 byte) r4, Register out Eq_n r0Out, Register out (ptr16 byte) r4Out, Register out (ptr16 Eq_n) r5Out)
// Called from:
//      fn04A0
bool fn064A(Eq_n r0, word16 r1, Eq_n r3, byte * r4, union Eq_n & r0Out, byte & r4Out, struct Eq_n & r5Out)
{
	struct Eq_n * r5_n;
	byte * r4_n;
	Eq_n r0_n;
	struct Eq_n * r2_n = g_ptr0F06;
	cui16 Z_n;
	do
	{
		if ((byte) r3 == r2_n[0x0EE6] && r2_n[0x0EEC] >= 0x00)
		{
			if ((byte) r1 == r2_n[0x0EE9])
			{
				r2_n[0x0EE6] = (struct Eq_n) 0x00;
				Z_n = 0x04;
				break;
			}
			if ((byte) r0 == r2_n[0x0EE9])
			{
				Eq_n Z_n = fn0AE8(r0, r4, &g_ptr066A, out r0_n, out r4_n, out r5_n);
				r0Out = r0_n;
				r4Out = r4_n;
				r5Out = r5_n;
				return Z_n != 0x00;
			}
		}
		--r2_n;
		Z_n = cond(r2_n) & 0x04;
	} while (r2_n >= null);
	r0Out = r0;
	r4Out = r4;
	struct Eq_n * r5;
	r5Out = r5;
	return Z_n != 0x00;
}

byte * g_ptr066A = &g_b1116; // 066A
// 067C: FlagGroup bool fn067C(Register Eq_n r0, Register Eq_n r3, Register (ptr16 byte) r4, Register out Eq_n r0Out, Register out (ptr16 byte) r4Out, Register out (ptr16 Eq_n) r5Out)
// Called from:
//      fn04A0
//      fn06D6
bool fn067C(Eq_n r0, Eq_n r3, byte * r4, union Eq_n & r0Out, byte & r4Out, struct Eq_n & r5Out)
{
	struct Eq_n * r1_n = (struct Eq_n *) (r0.u1 + (r3 - 0x01));
	byte v12_n = r1_n->b0E2A;
	cui16 Z_n = cond(v12_n) & 0x04;
	struct Eq_n * r5;
	if (v12_n != 0x00)
	{
		--r1_n->b0E2A;
		struct Eq_n * r1_n = (int16) r1_n->b0E2A;
		byte * r4_n;
		r0 = fn0AB6(r0, r4, out r4_n, out r5);
		byte v24_n = r1_n->b0EE0;
		*r4_n = v24_n;
		r4 = r4_n + 1;
		Z_n = cond(v24_n) & 0x04;
	}
	r0Out = r0;
	r4Out = r4;
	r5Out = r5;
	return Z_n != 0x00;
}

// 06A2: void fn06A2()
void fn06A2()
{
	if (g_t0F0A.u1 != 0x00)
	{
		union Eq_n * r5_n = g_ptr0F04;
		do
		{
			if ((&r5_n->u0)[0x0EF0] == 0x00)
			{
				Eq_n r0_n;
				r0_n.u0 = g_t0F14.u0;
				if (r0_n >= 0x08)
				{
					(&r5_n->u0)[0x0EF0] = (struct Eq_n) ((byte) r0_n + 0x02);
					(&r5_n->u0)[0x0EF3] = (struct Eq_n) 0x18;
					g_t0F0C.u0 = 0x00;
				}
				return;
			}
			--r5_n;
		} while (r5_n >= null);
	}
}

// 06D6: Register (ptr16 byte) fn06D6(Register (ptr16 byte) r4)
// Called from:
//      fn0200
byte * fn06D6(byte * r4)
{
	byte * r4_n;
	word16 r5_n;
	byte * r4_n;
	ptr16 fp;
	struct Eq_n * r2_n = g_ptr0F06;
	do
	{
		Eq_n r3_n;
		r3_n.u1 = (int16) r2_n[0x0EE6];
		if (r3_n != 0x00)
		{
			cui16 v13_n = g_w0EF2 & g_w0EF4;
			g_w0EF2 = v13_n;
			if (v13_n == 0x00 && (r2_n[0x0EEC] <= 0x00 || g_w0EF4 == g_w0EF4))
				goto l07A0;
			Eq_n r0_n;
			r0_n.u1 = (int16) r2_n[0x0EE9];
			Eq_n r0_n;
			word16 r5_n;
			if (r2_n[0x0EEC] >= 0x00)
			{
				byte * r4_n;
				r0_n = fn0AB6(r0_n, r4, out r4_n, out r5_n);
				*r4_n = 0x20;
				r4 = r4_n + 1;
			}
			else
			{
				r0_n = fn0AB6(r0_n, r4, out r4, out r5_n);
				r2_n[0x0EEC] = (struct Eq_n) (r2_n[0x0EEC] & 0x7F);
			}
			r2_n[0x0EE9] = (struct Eq_n) ((byte) r2_n[0x0EE9] + 1);
			struct Eq_n * sp_n = fp - 0x01;
			Eq_n r0_n;
			r0_n.u1 = (word32) r0_n + 1;
			if (r0_n != 0x19)
			{
				if (r0_n != 0x18)
				{
					if (r0_n < 22)
						goto l078A;
					sp_n = (struct Eq_n *) <invalid>;
					if (!fn067C(r0_n, r3_n, r4, out r0_n, out r4, out r5_n))
						goto l0784;
				}
				else if (g_ptr0EFA == null && (r3_n >= g_t0F14.u0 && r3_n - 0x04 <= g_t0F14.u0))
				{
					Eq_n r0_n = fn0A7C(fn0AB6(r0_n, r4, out r4_n, out r5_n), r4_n, out r4_n);
					g_ptr0EFA = null;
					g_w0EFC = 0x05;
					g_ptr0F1A = g_w0F18 - 0x01;
					word16 r0_n;
					byte * r4_n;
					word16 r5_n;
					fn0AE8(r0_n, r4_n, &g_ptr077E, out r0_n, out r4_n, out r5_n);
					return r4_n;
				}
l078A:
				sp_n->wFFFFFFFE = r5_n;
				word16 r0_n;
				byte * r4_n;
				word16 r5_n;
				fn0AE8(r0_n, r4, &g_ptr078E, out r0_n, out r4_n, out r5_n);
				return r4_n;
			}
l0784:
			r2_n[0x0EE6] = (struct Eq_n) 0x00;
		}
l07A0:
		--r2_n;
	} while (r2_n >= null);
	return r4;
}

byte * g_ptr077E = &g_b1121; // 077E
byte * g_ptr078E = &g_b111B; // 078E
// 07A6: Register Eq_n fn07A6(Register (ptr16 byte) r4, Register out (ptr16 byte) r4Out)
// Called from:
//      fn0200
Eq_n fn07A6(byte * r4, byte & r4Out)
{
	word16 r5_n;
	byte * r4_n;
	union Eq_n * fp;
	union Eq_n * sp_n = fp;
	ci16 v6_n = g_w0F1C - 0x01;
	g_w0F1E = v6_n;
	Eq_n r0;
	if (v6_n == 0x00)
	{
		g_w0F1E = g_w0F26;
		word16 r5_n = 0x00;
		do
		{
			struct Eq_n * r2_n = g_ptr0F1A;
			r0.u1 = r2_n->t0DB8.u1;
			if (r0 != 0x00)
			{
				if (g_w0EFC != 0x00)
				{
					struct Eq_n * r1_n = r2_n->ptr0DAC;
					word16 r3_n;
					for (r3_n = 0x08; r3_n != 0x00; --r3_n)
					{
						if (r1_n->t0000.u0 != 0x00)
						{
							--sp_n;
							sp_n->u0.b0000 = r1_n->t0000.u1;
							(&sp_n->u0)[1] = (struct Eq_n) (byte) r0;
							byte * r4_n;
							word16 r5_n;
							r0 = fn0A74(fn0AB6(r0, r4, out r4_n, out r5_n), r4_n, out r4, out r5_n);
							if (r1_n->t0000.u0 <= 0x00)
								r1_n->t0000.u0 = 0x00;
						}
						++r1_n;
					}
					r2_n->t0DB8.u1 = (struct Eq_n *) ((char *) &r2_n->t0DB8.u1->t0000 + 1);
					r0.u1 = (word32) r0 + 1;
				}
				struct Eq_n * r1_n = r2_n->ptr0DAC;
				cui16 r3_n;
				for (r3_n = 0x08; r3_n != 0x00; --r3_n)
				{
					if (r1_n->t0000.u0 > 0x00)
					{
						r1_n->t0000.u0 += g_w0F20;
						if (r1_n->t0000.u0 <= 0x08 || (r1_n->t0000).u0 >= 0x48)
							g_ptr0F00 = sp_n;
						if (r0 == 0x17)
							g_ptr0F02 = sp_n;
						if (r0 >= 22)
							fn093C(r0, r1_n);
						struct Eq_n * sp_n = sp_n - 0x01;
						sp_n->t0000.u1 = r1_n->t0000.u1;
						sp_n->b0001 = (byte) r0;
						Eq_n r0_n = fn0AB6(r0, r4, out r4_n, out r5_n);
						fn096A(r3_n);
						sp_n->wFFFFFFFE = r5_n;
						Eq_n r0_n;
						byte * r4_n;
						word16 r5_n;
						fn0AE8(r0_n, r4_n, &g_ptr083C, out r0_n, out r4_n, out r5_n);
						r4Out = r4_n;
						return r0_n;
					}
					if (r1_n->t0000.u0 < 0x00 && g_w0EFC == 0x00)
					{
						--sp_n;
						sp_n->u0.b0000 = r1_n->t0000.u1;
						(&sp_n->u0)[1] = (struct Eq_n) (byte) r0;
						byte * r4_n;
						word16 r5_n;
						r0 = fn0A74(fn0AB6(r0, r4, out r4_n, out r5_n), r4_n, out r4, out r5_n);
						r1_n->t0000.u0 = 0x00;
					}
					++r1_n;
				}
				if (r5_n == 0x00)
					r2_n->t0DB8.u1 = (struct Eq_n *) 0x00;
			}
			ci16 v81_n = g_w0F16 - 0x02;
			g_w0F16 = v81_n;
			if (v81_n < 0x00)
			{
				g_w0F1C = 0x0A;
				g_w0DAA = ~g_w0DA8;
				if (g_w0EFC != 0x00)
					g_w0EFE = 0x00;
				else if (g_w0EFE != 0x00)
				{
					g_w0F22 = -g_w0F20;
					if (g_ptr0F00 == null)
						g_w0EFE = 0x01;
				}
				g_ptr0F00 = null;
				g_ptr0F02 = null;
				goto l08B2;
			}
		} while (r5_n == 0x00);
		g_w0F28 = r5_n;
	}
l08B2:
	if (g_w0F12 != 0x00)
	{
		cui16 v38_n = g_w0EF2 & g_w0EF4;
		g_w0EF2 = v38_n;
		if (v38_n != 0x00)
		{
			struct Eq_n * r2_n = g_ptr0F06;
			do
			{
				if (r2_n[0x0EE6] == 0x00)
				{
					Eq_n r0_n;
					struct Eq_n * r1_n;
					ui16 r1_n;
					do
					{
						r0_n = fn0A94() & g_t0F0E.u0;
						if (r0_n != 0x00)
							goto l093A;
						uint16 r0_n = fn0A94();
						r1_n = g_ptr0F08;
						if (r1_n >= null && r1_n->w0DC4 > 0x00)
						{
							r0_n <<= 1;
							if (r0_n << 1 < 0x00)
								break;
						}
						ui32 v118_n = (uint32) r0_n << 0x01;
						cui16 r0_n = (word16) v118_n;
						r1_n = __rcl<word16,byte>(__rcl<word16,byte>(SLICE(v118_n, word16, 16), 0x01, cond(r0_n << 1) & 0x01), 0x01, cond(r0_n << 2) & 0x01);
						r1_n = r1_n << 1;
					} while (g_a0DC4[r1_n] <= 0x00);
					r0_n.u0 = 0x0DB8;
					ci16 * r3_n;
					do
					{
						r0_n -= 0x02;
						r3_n = (ci16 *) ((char *) r1_n + ((r0_n.u1)->t0000).u1);
					} while (*r3_n <= 0x00);
					r2_n[0x0EE6] = (struct Eq_n) (*r3_n + 0x02);
					r2_n[0x0EE9] = (struct Eq_n) r0_n.u1[6];
					r2_n[0x0EEC] = (struct Eq_n) 0x80;
					if (r0_n != 3500)
					{
						r0_n = fn0A94() & g_t0F0C.u0;
						if (r0_n == 0x00)
							goto l0936;
					}
					else
					{
l0936:
						r2_n[0x0EEC] = (struct Eq_n) ((byte) r2_n[0x0EEC] + 1);
					}
l093A:
					r4Out = r4;
					return r0_n;
				}
				--r2_n;
			} while (r2_n >= null);
		}
	}
	r4Out = r4;
	return r0;
}

byte * g_ptr083C = &g_b112B; // 083C
// 093C: void fn093C(Register Eq_n r0, Register (ptr16 Eq_n) r1)
// Called from:
//      fn07A6
void fn093C(Eq_n r0, struct Eq_n * r1)
{
	word16 r3_n = r1->t0000.u0;
	cui16 r3_n = r3_n - 0x01;
	if (g_w0F20 >= 0x00)
		r3_n = r3_n - 0x02;
	struct Eq_n * r3_n = r0.u1 + r3_n - 22 + 0x0E40;
	word16 wLoc04_n = 0x07;
	word16 v19_n;
	do
	{
		r3_n->b0000 = 0x00;
		v19_n = wLoc04_n - 0x01;
		++r3_n;
		wLoc04_n = v19_n;
	} while (v19_n != 0x00);
}

// 096A: void fn096A(Register cui16 r3)
// Called from:
//      fn07A6
void fn096A(cui16 r3)
{
	if ((r3 & 0x01) == 0x00)
	{
		if (g_w0DA8 == 0x00)
		{
l0976:
			g_b112D = 0x2F;
			g_b1131 = 0x5C;
			return;
		}
	}
	else if (g_w0DA8 != 0x00)
		goto l0976;
	g_b112D = 0x5C;
	g_b1131 = 0x2F;
}

// 0998: Register Eq_n fn0998(Register Eq_n r0, Register (ptr16 byte) r4, Register out (ptr16 byte) r4Out)
// Called from:
//      fn0200
Eq_n fn0998(Eq_n r0, byte * r4, byte & r4Out)
{
	union Eq_n * r2_n = g_ptr0F02;
	if (r2_n <= null)
	{
		ci16 v11_n = g_w0F1E - 0x01;
		g_w0F20 = v11_n;
		if (v11_n != 0x00)
		{
			if (r2_n == null || g_w0F1E > 0x28)
				goto l0A5E;
			goto l0A2A;
		}
		g_w0F20 = 100;
		g_w0F12 = 0x01;
		g_w0A6A = 4404;
		uint16 r0_n = fn0A94();
		r2_n = (union Eq_n *) ((char *) &t0000.w0000 + 1);
		word16 r1_n = 0x00;
		r0 = r0_n << 1;
		if (r0_n << 1 < 0x00)
		{
			r0 = r0_n << 2;
			if (r0_n << 2 < 0x00)
			{
				g_w0F12 = g_w0F10 + 0x01;
				g_w0A6A = 0x113D;
				r1_n = 0x0A;
			}
		}
		if (r0 <= 0x00)
		{
			g_w0F12 = -g_w0F10;
			g_w0A66 += r1_n;
			r2_n = (union Eq_n *) (&t0000.w0036 + 0x0A);
		}
	}
	if (g_w0EF4 != 0x00 || g_w0EF6 != 0x00)
	{
l0A18:
		g_ptr0F04 = r2_n;
		r4Out = r4;
		return r0;
	}
	if (g_w0F10 >= 0x00)
	{
		if ((byte) r2_n != 0x49)
		{
l0A04:
			r2_n = &r2_n->u0 + g_w0F10;
			byte * r4_n;
			word16 r5_n;
			r0 = fn0A60(fn0AB6(r0, r4, out r4_n, out r5_n), r4_n, out r4);
			goto l0A18;
		}
	}
	else if ((byte) r2_n != 0x02)
		goto l0A04;
l0A2A:
	byte * r4_n;
	word16 r5_n;
	byte * r4_n;
	word16 r5_n;
	byte * r4_n;
	word16 r5_n;
	word16 r5_n;
	r0 = fn0A74(fn0AB6(fn0A74(fn0AB6(r0, r4, out r4_n, out r5_n), r4_n, out r4_n, out r5_n), r4_n, out r4_n, out r5_n), r4_n, out r4, out r5_n);
	g_ptr0F04 = null;
	if (g_w0F12 <= 0x00)
	{
		g_ptr0EFA = null;
		g_w0EFC = 0x05;
	}
l0A5E:
	r4Out = r4;
	return r0;
}

// 0A60: Register Eq_n fn0A60(Register Eq_n r0, Register (ptr16 byte) r4, Register out word16 r4Out)
// Called from:
//      fn0998
Eq_n fn0A60(Eq_n r0, byte * r4, word16 & r4Out)
{
	Eq_n r0_n;
	word16 r4_n;
	word16 r5_n;
	fn0AE8(r0, r4, &g_ptr0A64, out r0_n, out r4_n, out r5_n);
	r4Out = r4_n;
	return r0_n;
}

byte * g_ptr0A64 = &g_b1121; // 0A64
word16 g_w0A66 = 0x0977; // 0A66
word16 g_w0A6A = 0x00; // 0A6A
// 0A74: Register Eq_n fn0A74(Register Eq_n r0, Register (ptr16 byte) r4, Register out word16 r4Out, Register out word16 r5Out)
// Called from:
//      fn0486
//      fn04A0
//      fn07A6
//      fn0998
Eq_n fn0A74(Eq_n r0, byte * r4, word16 & r4Out, word16 & r5Out)
{
	Eq_n r0_n;
	word16 r4_n;
	word16 r5_n;
	fn0AE8(r0, r4, &g_ptr0A78, out r0_n, out r4_n, out r5_n);
	r4Out = r4_n;
	r5Out = r5_n;
	return r0_n;
}

byte * g_ptr0A78 = &g_b1155; // 0A78
// 0A7C: Register Eq_n fn0A7C(Register Eq_n r0, Register (ptr16 byte) r4, Register out word16 r4Out)
// Called from:
//      fn04A0
//      fn05D4
//      fn06D6
Eq_n fn0A7C(Eq_n r0, byte * r4, word16 & r4Out)
{
	*r4 = g_b0F24;
	Eq_n r0_n;
	word16 r4_n;
	word16 r5_n;
	fn0AE8(r0, r4 + 1, &g_ptr0A84, out r0_n, out r4_n, out r5_n);
	r4Out = r4_n;
	return r0_n;
}

byte * g_ptr0A84 = &g_b1121; // 0A84
// 0A94: Register ui16 fn0A94()
// Called from:
//      fn05D4
//      fn07A6
//      fn0998
ui16 fn0A94()
{
	ui16 r0_n = ((SEQ(SLICE(__swab(g_w0AB2), byte, 8), 0x00) << 1) + g_w0AB2 << 2) + g_w0AB2;
	g_w0AB4 = r0_n + 0x3619;
	return r0_n + 0x3619;
}

word16 g_w0AB2 = 0x87; // 0AB2
ui16 g_w0AB4 = 0x00; // 0AB4
// 0AB6: Register Eq_n fn0AB6(Register Eq_n r0, Register (ptr16 byte) r4, Register out ptr16 r4Out, Register out (ptr16 Eq_n) r5Out)
// Called from:
//      fn0200
//      fn0470
//      fn0486
//      fn04A0
//      fn05D4
//      fn067C
//      fn06D6
//      fn07A6
//      fn0998
//      fn0AF6
//      fn0B1A
Eq_n fn0AB6(Eq_n r0, byte * r4, ptr16 & r4Out, struct Eq_n & r5Out)
{
	Eq_n r0_n;
	ptr16 r4_n;
	struct Eq_n * r5_n;
	fn0AE8(r0, r4, &g_ptr0ABA, out r0_n, out r4_n, out r5_n);
	r4Out = r4_n;
	r5Out = r5_n;
	return r0_n;
}

byte * g_ptr0ABA = &g_b111E; // 0ABA
// 0AE8: FlagGroup bool fn0AE8(Register Eq_n r0, Register (ptr16 byte) r4, Register (ptr16 (ptr16 byte)) r5, Register out word16 r0Out, Register out ptr16 r4Out, Register out word16 r5Out)
// Called from:
//      fn0200
//      fn0470
//      fn04A0
//      fn05D4
//      fn064A
//      fn06D6
//      fn07A6
//      fn0A60
//      fn0A74
//      fn0A7C
//      fn0AB6
//      fn0B1A
//      fn0C20
bool fn0AE8(Eq_n r0, byte * r4, byte ** r5, word16 & r0Out, ptr16 & r4Out, word16 & r5Out)
{
	byte * r0_n = (byte *) *r5;
	byte v9_n;
	do
	{
		v9_n = *r0_n;
		*r4 = v9_n;
		++r0_n;
		++r4;
	} while (v9_n != 0x00);
	(*((char *) r5 + 2))();
	word16 r0_n;
	r0Out = r0_n;
	r4Out = r4 - 0x01;
	word16 wArg00;
	r5Out = wArg00;
	cui16 NZV_n;
	return (NZV_n & 0x04) != 0x00;
}

// 0AF6: Register (ptr16 byte) fn0AF6(Register (ptr16 byte) r4)
// Called from:
//      fn0200
byte * fn0AF6(byte * r4)
{
	byte * r4_n;
	word16 r5_n;
	fn0AB6(g_t0F14.u0 + 0x02, r4, out r4_n, out r5_n);
	*r4_n = 0x80;
	PRINT(&g_b1178);
	return &g_b1178;
}

// 0B1A: Register (ptr16 byte) fn0B1A(Register (ptr16 byte) r4, Register out (ptr16 Eq_n) r5Out)
// Called from:
//      fn04A0
byte * fn0B1A(byte * r4, struct Eq_n & r5Out)
{
	word16 r5_n;
	byte * r4_n;
	Eq_n r0_n = fn0AB6(g_t0B5A.u1, r4, out r4_n, out r5_n);
	word16 r0_n;
	byte * r4_n;
	struct Eq_n * r5_n;
	fn0AE8(r0_n, r4_n, &g_ptr0B34, out r0_n, out r4_n, out r5_n);
	r5Out = r5_n;
	return r4_n;
}

byte * g_ptr0B34 = &g_b1121; // 0B34
word16 g_w0B56 = ~0x6F; // 0B56
word16 g_w0B58 = 0x1121; // 0B58
Eq_n g_t0B5A = // 0B5A
	{
		0x87
	};
word16 g_w0B5C = 0x00; // 0B5C
Eq_n g_t0B5E = // 0B5E
	{
		0x00
	};
// 0B60: void fn0B60(Register cup16 r0, Register ci16 r3, Register (ptr16 byte) r4)
void fn0B60(cup16 r0, ci16 r3, byte * r4)
{
	word16 wLoc08_n = 0x00;
	if (r3 >= 0x00)
		g_w0BC8 = 0x20;
	else
	{
		g_w0BC8 = 0x30;
		r3 = -r3;
	}
	if (r3 != 0x00)
	{
		word16 * r2_n = 0x0BD4 - (r3 << 1);
		do
		{
			word16 r5_n = 0x30;
			cup16 v17_n = *r2_n;
			++r2_n;
			if (v17_n == 0x00)
				return;
			while (true)
			{
				r0 -= v17_n;
				if (r0 < 0x00)
					break;
				++r5_n;
			}
			r0 += v17_n;
			int16 r5_n;
			if (wLoc08_n == 0x00)
			{
				if (r5_n != ~0x2F)
				{
					++wLoc08_n;
					goto l0BB4;
				}
				if (r3 == 0x01 || *r2_n == 0x00)
					goto l0BB4;
				r5_n = (int16) g_b0BC6;
			}
			else
			{
l0BB4:
				r5_n = r5_n + 0x00;
			}
			*r4 = (byte) r5_n;
			++r4;
			--r3;
		} while (r3 != 0x00);
	}
}

byte g_b0BC6 = 0x87; // 0BC6
word16 g_w0BC8 = 0x20; // 0BC8
// 0BD6: void fn0BD6()
// Called from:
//      fn0200
void fn0BD6()
{
	g_ptr0F1A = (struct Eq_n *) ((char *) &t0000.w0002 + 1);
	g_w0B5C = 0x00;
	g_t0F0E.u0 = 0xF800;
	g_w0F10 = 0xE000;
	struct Eq_n * r1_n = &g_t0E56;
	word16 r0_n;
	for (r0_n = 0x06; r0_n != 0x00; --r0_n)
	{
		word16 r2_n;
		for (r2_n = 0x0A; r2_n != 0x00; --r2_n)
		{
			r1_n->b0000 = 0x04;
			r1_n = &r1_n->b0000 + 1;
		}
		++r1_n;
	}
	g_w1166 = 0x1100;
	g_w1168 = 4464;
	FnSubfn(&g_w1166);
	g_w1174 = g_w1170;
}

// 0C20: Register word16 fn0C20()
// Called from:
//      fn0200
word16 fn0C20()
{
	word16 * r1_n = (word16 *) g_a0DB8;
	word16 r0_n;
	Eq_n r2_n;
	r2_n.u0 = 0x05;
	for (r0_n = 0x06; r0_n != 0x00; --r0_n)
	{
		*r1_n = (word16) r2_n;
		++r1_n;
		r2_n.u1 = (word32) r2_n + 2;
	}
	struct Eq_n * r1_n = null;
	word16 r0_n;
	word16 r2_n = 11;
	for (r0_n = 0x08; r0_n != 0x00; --r0_n)
	{
		r1_n[1776] = (struct Eq_n) r2_n;
		r1_n[0x06F8] = (struct Eq_n) r2_n;
		r1_n[0x0700] = (struct Eq_n) r2_n;
		r1_n[1800] = (struct Eq_n) r2_n;
		r1_n[1808] = (struct Eq_n) r2_n;
		r1_n[1816] = (struct Eq_n) r2_n;
		r1_n[0x06E2] = (struct Eq_n) 0x06;
		++r1_n;
		r2_n += 0x08;
	}
	word16 * r1_n = (word16 *) g_a0EE6;
	Eq_n r0_n;
	r0_n.u0 = 0x10;
	do
	{
		*r1_n = 0x00;
		++r1_n;
		--r0_n;
	} while (r0_n != 0x00);
	g_t0F0A.u1 = (struct Eq_n *) ~0x00;
	g_t0F14.u0 = 0x30;
	g_w0F16 = 0x02;
	g_w0F18 = 0x78;
	g_w0F1C = 0x0A;
	g_w0F1E = 0x04;
	g_w0F28 = 0x04;
	g_w0F20 = 100;
	g_w0F22 = 0x01;
	g_t0F0A.u1 = (struct Eq_n *) (g_t0F0A.u1 << 1);
	Eq_n v18_n = g_t0F0C.u0 << 1;
	g_t0F0C.u0 = (wchar_t) v18_n;
	if (v18_n == 0x00)
		g_t0F0C.u0 = (wchar_t) (g_t0F0C.u0 >> 1);
	word16 r0_n;
	word16 r4_n;
	word16 r5_n;
	fn0AE8(r0_n, &g_b1178, &g_ptr0CC2, out r0_n, out r4_n, out r5_n);
	return r4_n;
}

byte * g_ptr0CC2 = &g_b1111; // 0CC2
// 0D98: void fn0D98(Register (ptr16 Eq_n) r0, Register (ptr16 byte) r4)
void fn0D98(struct Eq_n * r0, byte * r4)
{
	word16 r1_n;
	for (r1_n = 0x46; r1_n != 0x00; --r1_n)
	{
		*r4 = g_a0EE0[(int16) r0->b0000];
		++r0;
		++r4;
	}
}

word16 g_w0DA8 = 0x87; // 0DA8
word16 g_w0DAA = 0x00; // 0DAA
Eq_n g_a0DAC[6] = // 0DAC
	{
		
		{
			
			{
				0xE0
			},
		},
		
		{
			
			{
				0xF0
			},
		},
		
		{
			
			{
				0x00
			},
		},
		
		{
			
			{
				0x10
			},
		},
		
		{
			
			{
				0x20
			},
		},
		
		{
			
			{
				0x30
			},
		},
	};
Eq_n g_a0DB8[6] = // 0DB8
	{
		
		{
			
			{
				0x00
			},
		},
		
		{
			
			{
				0x00
			},
		},
		
		{
			
			{
				0x00
			},
		},
		
		{
			
			{
				0x00
			},
		},
		
		{
			
			{
				0x00
			},
		},
		
		{
			
			{
				0x00
			},
		},
	};
word16 g_a0DC4[] = // 0DC4
	{
	};
word16 g_a0DD4[6] = // 0DD4
	{
		0x1E,
		0x19,
		0x14,
		0x0F,
		0x0A,
		0x05,
	};
word16 g_a0DE0[] = // 0DE0
	{
	};
word16 g_a0DF0[] = // 0DF0
	{
	};
word16 g_a0E00[] = // 0E00
	{
	};
word16 g_a0E10[] = // 0E10
	{
	};
word16 g_a0E20[] = // 0E20
	{
	};
word16 g_a0E30[] = // 0E30
	{
	};
Eq_n g_t0E56 = // 0E56
	{
		0x00,
	};
byte g_a0EE0[] = // 0EE0
	{
	};
Eq_n g_a0EE6[] = // 0EE6
	{
	};
Eq_n g_a0EE9[] = // 0EE9
	{
	};
ci8 g_a0EEC[] = // 0EEC
	{
	};
byte g_a0EF0[] = // 0EF0
	{
	};
cui16 g_w0EF2 = 0x00; // 0EF2
byte g_a0EF3[] = // 0EF3
	{
	};
cui16 g_w0EF4 = 0x00; // 0EF4
word16 g_w0EF6 = 0x00; // 0EF6
word16 g_w0EF8 = 0x00; // 0EF8
<anonymous> * g_ptr0EFA = null; // 0EFA
word16 g_w0EFC = 0x00; // 0EFC
word16 g_w0EFE = 0x00; // 0EFE
union Eq_n * g_ptr0F00 = null; // 0F00
union Eq_n * g_ptr0F02 = null; // 0F02
union Eq_n * g_ptr0F04 = null; // 0F04
struct Eq_n * g_ptr0F06 = &g_t0002; // 0F06
struct Eq_n * g_ptr0F08 = null; // 0F08
Eq_n g_t0F0A = // 0F0A
	{
		~0x00
	};
Eq_n g_t0F0C = // 0F0C
	{
		L'\x01'
	};
Eq_n g_t0F0E = // 0F0E
	{
		0
	};
ci16 g_w0F10 = 0x00; // 0F10
ci16 g_w0F12 = 0x00; // 0F12
Eq_n g_t0F14 = // 0F14
	{
		L'\0'
	};
ci16 g_w0F16 = 0x00; // 0F16
ci16 g_w0F18 = 0x00; // 0F18
struct Eq_n * g_ptr0F1A = null; // 0F1A
word16 g_w0F1C = 0x00; // 0F1C
ci16 g_w0F1E = 0x00; // 0F1E
ci16 g_w0F20 = 0x00; // 0F20
ci16 g_w0F22 = 0x00; // 0F22
byte g_b0F24 = 0x00; // 0F24
ci16 g_w0F26 = 0x0407; // 0F26
word16 g_w0F28 = 0x04; // 0F28
Eq_n g_a0F2A[] = // 0F2A
	{
	};
char g_b0F9A = '\r'; // 0F9A
char g_b0FDA = '\r'; // 0FDA
byte g_b1111 = 0x1B; // 1111
byte g_b1116 = 0x1B; // 1116
byte g_b111B = 0x0A; // 111B
byte g_b111E = 0x1B; // 111E
byte g_b1121 = 0x00; // 1121
byte g_b1122 = 0x08; // 1122
byte g_b112B = 0x08; // 112B
byte g_b112D = 0x2F; // 112D
byte g_b1131 = 0x5C; // 1131
byte g_b1155 = 0x20; // 1155
word16 g_w1166 = 0x00; // 1166
word16 g_w1168 = 0x00; // 1168
word16 g_w116A = 0x00; // 116A
word16 g_w116C = 0x00; // 116C
word16 g_w116E = 0x00; // 116E
ui16 g_w1170 = 0x00; // 1170
word16 g_w1172 = 0x00; // 1172
ui16 g_w1174 = 0x00; // 1174
byte g_b1178 = 0x00; // 1178
