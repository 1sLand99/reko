// varargs_test_text.c
// Generated by decompiling varargs_test.exe
// using Reko decompiler version 0.12.1.0.

#include "varargs_test.h"

// 0000000140001000: Register word32 fn0000000140001000()
// Called from:
//      Win32CrtStartup
word32 fn0000000140001000()
{
	ptr64 fp;
	ui64 rax_n = g_qw40003000 ^ fp - 200;
	fn0000000140001140(&g_t40002210);
	fn00000001400010D0(&g_t40002228);
	word64 qwLocD0;
	return fn00000001400011B0(rax_n ^ fp - 200, qwLocD0);
}

// 00000001400010C0: Register ptr64 fn00000001400010C0()
// Called from:
//      fn00000001400010D0
//      fn000000014000193C
ptr64 fn00000001400010C0()
{
	return &g_t40003628;
}

// 00000001400010D0: void fn00000001400010D0(Register ptr64 rcx)
// Called from:
//      fn0000000140001000
void fn00000001400010D0(ptr64 rcx)
{
	ptr64 fp;
	_stdio_common_vfscanf(rcx, 0x00, 0x00, 0x00, *fn00000001400010C0(), _acrt_iob_func(0x00, 0x00, 0x00), fp + 0x10);
}

// 0000000140001130: Register ptr64 fn0000000140001130()
// Called from:
//      fn0000000140001140
//      fn000000014000193C
ptr64 fn0000000140001130()
{
	return &g_t40003620;
}

// 0000000140001140: void fn0000000140001140(Register ptr64 rcx)
// Called from:
//      fn0000000140001000
void fn0000000140001140(ptr64 rcx)
{
	ptr64 fp;
	_stdio_common_vfprintf(rcx, 0x00, 0x00, 0x00, *fn0000000140001130(), _acrt_iob_func(0x01, 0x01), fp + 0x10);
}

// 00000001400011B0: Register word32 fn00000001400011B0(Register ui64 rcx, Stack word64 qwArg00)
// Called from:
//      fn0000000140001000
//      fn0000000140001E9C
word32 fn00000001400011B0(ui64 rcx, word64 qwArg00)
{
	if (rcx != g_qw40003000)
		return fn000000014000147C(rcx, qwArg00);
	ui64 rcx_n = __rol<word64,byte>(rcx, 0x10);
	if ((word16) rcx_n == 0x00)
	{
		word64 rax;
		return (word32) rax;
	}
	rcx = __ror<word64,byte>(rcx_n, 0x10);
	return fn000000014000147C(rcx, qwArg00);
}

// 00000001400011D4: void fn00000001400011D4(Register word64 rbx, Stack Eq_n tArg08)
void fn00000001400011D4(word64 rbx, Eq_n tArg08)
{
	set_app_type(0x01, 0x01);
	_set_fmode(fn0000000140001920());
	*__p__commode() = fn0000000140001ABC();
	if (fn000000014000164C(0x01) != 0x00)
	{
		fn0000000140001B5C();
		fn0000000140001854(0x140001BA8);
		word32 eax_n = fn0000000140001918();
		if ((word32) configure_narrow_argv(eax_n, (uint64) eax_n) == 0x00)
		{
			fn0000000140001928();
			if (fn0000000140001958() != 0x00)
				__setusermatherr(&g_t40001ABC);
			fn0000000140001DD0();
			fn0000000140001DD0();
			word32 eax_n = fn0000000140001ABC();
			configthreadlocale(eax_n, (uint64) eax_n);
			if (fn0000000140001938() != 0x00)
				initialize_narrow_environment();
			fn0000000140001ABC();
			return;
		}
	}
	else
	{
		word64 rcx_n;
		fn0000000140001974(0x07, rbx, tArg08, out rcx_n);
		int3();
	}
	word64 rcx_n;
	fn0000000140001974(0x07, rbx, tArg08, out rcx_n);
	int3();
	int3();
	fn0000000140001290();
}

// 0000000140001290: void fn0000000140001290()
// Called from:
//      fn00000001400011D4
void fn0000000140001290()
{
	fn000000014000193C();
}

// 00000001400012A0: void fn00000001400012A0()
void fn00000001400012A0()
{
	fn0000000140001B14();
	word32 eax_n = fn0000000140001ABC();
	set_new_mode(eax_n, (uint64) eax_n);
}

// 00000001400012BC: Register word32 fn00000001400012BC(Register (ptr64 (ptr64 code)) rax)
// Called from:
//      Win32CrtStartup
word32 fn00000001400012BC(<anonymous> ** rax)
{
	word64 rsi;
	word56 rsi_56_8_n = SLICE(rsi, word56, 8);
	byte al = (byte) rax;
	word32 ecx;
	word32 edx;
	fn0000000140001600(ecx, edx);
	<anonymous> ** rax_n = rax;
	word64 qwLoc40;
	Eq_n tLoc30;
	if (al == 0x00)
	{
		word64 rcx_n;
		rax_n = fn0000000140001974(0x07, qwLoc40, tLoc30, out rcx_n);
		int3();
	}
	struct Eq_n * gs;
	fn00000001400015C4(gs);
	word32 ecx_n = g_dw400035B0;
	word32 ecx_n = ecx_n;
	<anonymous> ** rax_n = rax_n;
	if (ecx_n == 0x01)
	{
		word64 rcx_n;
		rax_n = fn0000000140001974(0x07, qwLoc40, tLoc30, out rcx_n);
		ecx_n = (word32) rcx_n;
	}
	word32 rax_32_32_n = SLICE(rax_n, word32, 32);
	word64 rsi_n = SEQ(rsi_56_8_n, 0x00);
	uint64 rax_n;
	if (ecx_n == 0x00)
	{
		g_dw400035B0 = 0x01;
		int32 eax_n = _initterm_e(&g_t400021B8, &g_t400021D0);
		rax_n = SEQ(rax_32_32_n, eax_n);
		if (eax_n != 0x00)
		{
			rax_n = 0xFF;
			return (word32) rax_n;
		}
		_initterm(&g_t400021A0, &g_t400021B0);
		g_dw400035B0 = 0x02;
	}
	else
		rsi_n = SEQ(rsi_56_8_n, 0x01);
	byte cl;
	fn00000001400017B4(cl);
	fn0000000140001964();
	byte sil_n = (byte) rsi_n;
	if (*rax_n != null)
	{
		rax_n = fn0000000140001718(rax_n);
		byte al_n = (byte) rax_n;
		word56 rax_56_8_n = SLICE(rax_n, word56, 8);
		if (al_n != 0x00)
		{
			<anonymous> * rbx_n = (<anonymous> *) *rax_n;
			fn0000000140001BF4();
			rbx_n();
		}
	}
	fn000000014000196C();
	if (*rax_n != null && (byte) fn0000000140001718(rax_n) != 0x00)
		register_thread_local_exe_atexit_callback(*rax_n);
	_p___argv();
	_p___argc();
	get_initial_narrow_environment();
	uint64 rax_n = (uint64) fn0000000140001000();
	fn0000000140001AC0();
	int32 eax_n = (word32) rax_n;
	if ((byte) rax_n != 0x00)
	{
		if (sil_n == 0x00)
			cexit();
		byte dl;
		fn00000001400017D8(dl);
		rax_n = (uint64) eax_n;
		return (word32) rax_n;
	}
	else
		exit(eax_n);
}

// 0000000140001434: Register Eq_n Win32CrtStartup()
Eq_n Win32CrtStartup()
{
	Eq_n tLoc18;
	<anonymous> ** rax_n = fn000000014000186C(tLoc18);
	return fn00000001400012BC(rax_n);
}

// 0000000140001448: void fn0000000140001448(Register (ptr64 Eq_n) rcx)
// Called from:
//      fn00000001400011B0
void fn0000000140001448(struct _EXCEPTION_POINTERS * rcx)
{
	word32 rax_32_32_n = SLICE(SetUnhandledExceptionFilter(null), word32, 32);
	UnhandledExceptionFilter(rcx);
	TerminateProcess(SEQ(rax_32_32_n, GetCurrentProcess()), 0xC0000409);
}

// 000000014000147C: Register word32 fn000000014000147C(Register ui64 rcx, Stack word64 qwArg00)
// Called from:
//      fn00000001400011B0
word32 fn000000014000147C(ui64 rcx, word64 qwArg00)
{
	if (IsProcessorFeaturePresent(0x17) == 0x00)
	{
		Eq_n tLoc38;
		fn0000000140001550(&g_t400030E0, tLoc38);
		g_qw400031D8 = qwArg00;
		ptr64 fp;
		g_ptr40003178 = fp + 0x08;
		g_qw40003050 = g_qw400031D8;
		g_qw40003160 = rcx;
		g_dw40003040 = 0xC0000409;
		g_dw40003044 = 0x01;
		g_dw40003058 = 0x01;
		g_qw40003060 = 0x02;
		fn0000000140001448(&g_t40002200);
		word64 rax_n;
		return (word32) rax_n;
	}
	else
		__fastfail(0x02);
}

// 0000000140001550: void fn0000000140001550(Register Eq_n rcx, Stack Eq_n tArg08)
// Called from:
//      fn00000001400011B0
void fn0000000140001550(Eq_n rcx, Eq_n tArg08)
{
	RtlCaptureContext(rcx);
	Eq_n rsi_n;
	rsi_n.u1 = rcx->t00F8.u1;
	uint64 rdi_n = 0x00;
	do
	{
		word32 edi_n = (word32) rdi_n;
		Eq_n rax_n = RtlLookupFunctionEntry(rsi_n, &tArg08, null);
		if (rax_n == null)
			return;
		ptr64 fp;
		KERNEL32.dll!RtlVirtualUnwind(0x00, 0x00, rsi_n, rax_n, fp + 0x10, fp + 0x18, 0x00, tArg08, rcx, fp + 0x18, fp + 0x10, 0x00);
		rdi_n = (uint64) (edi_n + 0x01);
	} while ((word32) rdi_n < 0x02);
}

// 00000001400015C4: void fn00000001400015C4(Register (ptr32 Eq_n) gs)
// Called from:
//      Win32CrtStartup
void fn00000001400015C4(struct Eq_n * gs)
{
	if (fn0000000140001DC4() != 0x00)
	{
		word64 rcx_n = gs->ptr0030->qw0008;
		word64 rax_n;
		do
		{
			__lock();
			if (!__cmpxchg<word64>(g_qw400035B8, rcx_n, 0x00, out rax_n))
				goto l00000001400015F2;
		} while (rcx_n != rax_n);
	}
	else
	{
l00000001400015F2:
	}
}

// 0000000140001600: void fn0000000140001600(Register word32 ecx, Register word32 edx)
// Called from:
//      Win32CrtStartup
void fn0000000140001600(word32 ecx, word32 edx)
{
	byte al_n = g_b400035F0;
	if (ecx == 0x00)
		al_n = 0x01;
	g_b400035F0 = al_n;
	fn0000000140001BFC(edx, 0x01);
	if (fn0000000140001938() == 0x00)
		return;
	if (fn0000000140001938() != 0x00)
		return;
	fn0000000140001938();
}

// 000000014000164C: Register byte fn000000014000164C(Register up32 ecx)
// Called from:
//      fn00000001400011D4
byte fn000000014000164C(up32 ecx)
{
	if (ecx > 0x01)
	{
		word64 qwLoc50;
		Eq_n tLoc40;
		<anonymous> ** rcx_n;
		fn0000000140001974(0x05, qwLoc50, tLoc40, out rcx_n);
		int3();
		int3();
		int3();
		int3();
		return (byte) fn0000000140001718(rcx_n);
	}
	else
	{
		word64 rax_n;
		if (fn0000000140001DC4() != 0x00 && ecx == 0x00)
		{
			word64 rax_n = initialize_onexit_table(&g_ow400035C0);
			word56 rax_56_8_n = SLICE(rax_n, word56, 8);
			if ((word32) rax_n != 0x00)
				rax_n = SEQ(rax_56_8_n, 0x00);
			else
			{
				word64 rax_n = initialize_onexit_table(&g_ow400035D8);
				rax_n = SEQ(SLICE(rax_n, word56, 8), (byte) ((word32) rax_n == 0x00));
			}
		}
		else
		{
			ui64 rdx_n = g_qw40003000;
			uint64 rax_n = (uint64) ((word32) rdx_n & 0x3F);
			Eq_n r8_n = __ror<word64,byte>(~0x00, 0x40 - (byte) rax_n) ^ rdx_n;
			g_ow400035C0 = SEQ(r8_n, r8_n);
			g_t400035D0.u1 = (real64) r8_n;
			g_ow400035D8 = SEQ(r8_n, r8_n);
			g_t400035E8.u1 = (real64) r8_n;
			rax_n = SEQ(SLICE(rax_n, word56, 8), 0x01);
		}
		return (byte) rax_n;
	}
}

// 0000000140001718: Register word64 fn0000000140001718(Register (ptr64 (ptr64 code)) rcx)
// Called from:
//      Win32CrtStartup
//      fn000000014000164C
word64 fn0000000140001718(<anonymous> ** rcx)
{
	word56 rax_56_8_n = 0x5A;
	word64 rax_n;
	if (g_w40000000 == 23117)
	{
		int64 rax_n = (int64) g_dw4000003C;
		rax_56_8_n = SLICE(rax_n, word56, 8);
		struct Eq_n * rcx_n = (struct Eq_n *) ((char *) &g_w40000000 + rax_n);
		if (rcx_n->dw0000 == 0x4550)
		{
			rax_56_8_n = 0x02;
			if (rcx_n->w0018 == 0x020B)
			{
				uint64 rax_n = (uint64) rcx_n->w0006;
				struct Eq_n * rdx_n = (struct Eq_n *) ((char *) &rcx_n->w0018 + (uint64) rcx_n->w0014);
				uint64 r8_n = rcx - &g_w40000000;
				word56 rax_56_8_n = SLICE(rax_n, word56, 8);
				struct Eq_n * r9_n = rdx_n + rax_n;
				for (; rdx_n != r9_n; rdx_n += (struct Eq_n *) 0x28)
				{
					word32 ecx_n = rdx_n->dw000C;
					if (r8_n >= (uint64) ecx_n)
					{
						uint64 rax_n = (uint64) (rdx_n->dw0008 + ecx_n);
						rax_56_8_n = SLICE(rax_n, word56, 8);
						if (r8_n < rax_n)
							goto l000000014000178F;
					}
				}
				rdx_n = null;
l000000014000178F:
				if (rdx_n == null)
					rax_n = SEQ(rax_56_8_n, 0x00);
				else if (rdx_n->dw0024 < 0x00)
					rax_n = SEQ(rax_56_8_n, 0x00);
				else
					rax_n = SEQ(rax_56_8_n, 0x01);
				return rax_n;
			}
		}
	}
	rax_n = SEQ(rax_56_8_n, 0x00);
	return rax_n;
}

// 00000001400017B4: void fn00000001400017B4(Register byte cl)
// Called from:
//      Win32CrtStartup
void fn00000001400017B4(byte cl)
{
	if (fn0000000140001DC4() != 0x00 && cl == 0x00)
		g_qw400035B8 = 0x00;
}

// 00000001400017D8: void fn00000001400017D8(Register byte dl)
// Called from:
//      Win32CrtStartup
void fn00000001400017D8(byte dl)
{
	if (g_b400035F0 == 0x00 || dl == 0x00)
	{
		fn0000000140001938();
		fn0000000140001938();
	}
}

// 0000000140001804: Register Eq_n fn0000000140001804(Register Eq_n rcx)
// Called from:
//      fn0000000140001854
Eq_n fn0000000140001804(Eq_n rcx)
{
	ui64 rdx_n = g_qw40003000;
	word32 eax_n;
	if (__ror<word64,byte>(rdx_n ^ g_ow400035C0, (byte) rdx_n & 0x3F) == ~0x00)
		eax_n = (word32) crt_atexit(rcx);
	else
		eax_n = (word32) register_onexit_function(&g_ow400035C0, rcx);
	Eq_n rcx_n;
	rcx_n.u0 = 0x00;
	if (eax_n == 0x00)
		rcx_n = rcx;
	return rcx_n;
}

// 0000000140001854: void fn0000000140001854(Register Eq_n rcx)
// Called from:
//      fn00000001400011D4
void fn0000000140001854(Eq_n rcx)
{
	fn0000000140001804(rcx);
}

// 000000014000186C: Register word64 fn000000014000186C(Stack Eq_n tArg18)
// Called from:
//      Win32CrtStartup
word64 fn000000014000186C(Eq_n tArg18)
{
	ptr64 fp;
	Eq_n tArg10;
	tArg10.dwLowDateTime = (DWORD) 0x00;
	ui64 rax_n = g_qw40003000;
	if (rax_n == 0x2B992DDFA232)
	{
		GetSystemTimeAsFileTime(&tArg10);
		Eq_n v16_n = tArg10.dwLowDateTime ^ (uint64) GetCurrentThreadId() ^ (uint64) GetCurrentProcessId();
		QueryPerformanceCounter(&tArg18);
		ui64 rax_n = (uint64) tArg18.u.LowPart << 0x20 ^ tArg18.QuadPart ^ v16_n ^ fp + 8;
		rax_n = rax_n & 0xFFFFFFFFFFFF;
		if ((rax_n & 0xFFFFFFFFFFFF) == 0x2B992DDFA232)
			rax_n = 0x2B992DDFA233;
		g_qw40003000 = rax_n;
	}
	word64 rax_n = ~rax_n;
	g_qw40003008 = rax_n;
	return rax_n;
}

// 0000000140001918: Register word32 fn0000000140001918()
// Called from:
//      fn00000001400011D4
word32 fn0000000140001918()
{
	return 0x01;
}

// 0000000140001920: Register word32 fn0000000140001920()
// Called from:
//      fn00000001400011D4
word32 fn0000000140001920()
{
	return 0x4000;
}

// 0000000140001928: void fn0000000140001928()
// Called from:
//      fn00000001400011D4
void fn0000000140001928()
{
	InitializeSListHead(&g_u40003600);
}

// 0000000140001938: Register byte fn0000000140001938()
// Called from:
//      fn00000001400011D4
//      fn0000000140001600
//      fn00000001400017D8
byte fn0000000140001938()
{
	return 0x01;
}

// 000000014000193C: void fn000000014000193C()
// Called from:
//      fn0000000140001290
void fn000000014000193C()
{
	ui64 * rax_n = fn0000000140001130();
	*rax_n |= 0x04;
	ui64 * rax_n = fn00000001400010C0();
	*rax_n |= 0x02;
}

// 0000000140001958: Register word32 fn0000000140001958()
// Called from:
//      fn00000001400011D4
word32 fn0000000140001958()
{
	return (word32) (g_dw40003014 == 0x00);
}

// 0000000140001964: void fn0000000140001964()
// Called from:
//      Win32CrtStartup
void fn0000000140001964()
{
}

// 000000014000196C: void fn000000014000196C()
// Called from:
//      Win32CrtStartup
void fn000000014000196C()
{
}

// 0000000140001974: Register uint64 fn0000000140001974(Register word32 ecx, Stack word64 qwArg00, Stack Eq_n tArg10, Register out (ptr64 Eq_n) rcxOut)
// Called from:
//      fn00000001400011D4
//      Win32CrtStartup
//      fn000000014000164C
uint64 fn0000000140001974(word32 ecx, word64 qwArg00, Eq_n tArg10, struct _EXCEPTION_POINTERS & rcxOut)
{
	if (IsProcessorFeaturePresent(0x17) == 0x00)
	{
		g_dw40003610 = 0x00;
		Eq_n tLoc04D8;
		memset(&tLoc04D8, 0, 0x04D0);
		RtlCaptureContext(&tLoc04D8);
		Eq_n rbx_n;
		rbx_n.u1 = tLoc04D8.t00F8.u1;
		Eq_n rax_n = RtlLookupFunctionEntry(rbx_n, &tArg10, null);
		ptr64 fp;
		if (rax_n != null)
			KERNEL32.dll!RtlVirtualUnwind(0x00, 0x00, rbx_n, rax_n, fp + 24, fp + 32, &tLoc04D8, 0x00, tArg10, &tLoc04D8, fp + 32, fp + 24, 0x00);
		tLoc04D8.qw00F8 = qwArg00;
		tLoc04D8.SegEs = (DWORD) (fp + 0x08);
		word64 qwLoc0578;
		memset(&qwLoc0578, 0, 0x98);
		qwLoc0578 = (word64) 0x40000015;
		qwLoc0578.dw0004 = 0x01;
		Eq_n eax_n = IsDebuggerPresent();
		Eq_n tLoc0588;
		tLoc0588.ExceptionRecord = (PEXCEPTION_RECORD) &qwLoc0578;
		tLoc0588.ptr0008 = &tLoc04D8;
		word32 rax_32_32_n = SLICE(SetUnhandledExceptionFilter(null), word32, 32);
		Eq_n eax_n = UnhandledExceptionFilter(&tLoc0588);
		byte bl_n = (byte) (eax_n == 0x01);
		struct _EXCEPTION_POINTERS * rcx_n = &tLoc0588;
		uint64 rax_n = SEQ(rax_32_32_n, eax_n);
		if (eax_n == 0x00)
		{
			ui32 eax_n = 0x00 - (bl_n != 0x00);
			g_dw40003610 &= eax_n;
			rax_n = (uint64) eax_n;
		}
		rcxOut = rcx_n;
		return rax_n;
	}
	else
		__fastfail(ecx);
}

// 0000000140001ABC: Register word32 fn0000000140001ABC()
// Called from:
//      fn00000001400011D4
//      fn00000001400012A0
word32 fn0000000140001ABC()
{
	return 0x00;
}

// 0000000140001AC0: void fn0000000140001AC0()
// Called from:
//      Win32CrtStartup
void fn0000000140001AC0()
{
	Eq_n rax_n = GetModuleHandleW(null);
	if (rax_n == null || rax_n->unused != 23117)
		return;
	struct Eq_n * rax_n = (struct Eq_n *) ((char *) &rax_n->unused + (int64) (&rax_n->unused)[0x0F]);
	if (rax_n->dw0000 != 0x4550 || (rax_n->w0018 != 0x020B || rax_n->dw0084 <= 0x0E))
		;
}

// 0000000140001B14: void fn0000000140001B14()
// Called from:
//      fn00000001400012A0
void fn0000000140001B14()
{
	SetUnhandledExceptionFilter(&g_t40001B24);
}

// 0000000140001B24: void fn0000000140001B24(Register (ptr64 (ptr64 Eq_n)) rcx)
void fn0000000140001B24(struct Eq_n ** rcx)
{
	struct Eq_n * rax_n = (struct Eq_n *) *rcx;
	if (rax_n->dw0000 != ~0x1F928C9C || rax_n->dw0018 != 0x04)
		return;
	up32 ecx_n = rax_n->dw0020;
	if (ecx_n > 0x19930522 && ecx_n != 0x01994000)
		return;
	api-ms-win-crt-runtime-l1-1-0.dll!terminate();
	int3();
	fn0000000140001B5C();
}

// 0000000140001B5C: void fn0000000140001B5C()
// Called from:
//      fn00000001400011D4
//      fn0000000140001B24
void fn0000000140001B5C()
{
	word64 * rbx_n;
	for (rbx_n = &g_qw40002680; rbx_n < &g_qw40002680; ++rbx_n)
	{
		if (*rbx_n != 0x00)
		{
			fn0000000140001BF4();
			fn0000000000000000();
		}
	}
}

// 0000000140001BA8: void fn0000000140001BA8()
void fn0000000140001BA8()
{
	word64 * rbx_n;
	for (rbx_n = &g_qw40002690; rbx_n < &g_qw40002690; ++rbx_n)
	{
		if (*rbx_n != 0x00)
		{
			fn0000000140001BF4();
			fn0000000000000000();
		}
	}
}

// 0000000140001BF4: void fn0000000140001BF4()
// Called from:
//      Win32CrtStartup
//      fn0000000140001B5C
//      fn0000000140001BA8
void fn0000000140001BF4()
{
	g_ptr40002190();
}

// 0000000140001BFC: void fn0000000140001BFC(Register word32 edx, Register word32 ebx)
// Called from:
//      fn0000000140001600
void fn0000000140001BFC(word32 edx, word32 ebx)
{
	word64 rbx;
	word32 ebx_n = (word32) rbx;
	g_dw4000301C = 0x02;
	__cpuid(0x00, 0x00, &0x00, &ebx_n, &0x00, &edx);
	g_dw40003018 = 0x01;
	ui32 r8d_n = g_dw40003614;
	__cpuid(0x01, 0x00, &0x01, &ebx_n, &0x00, &(ebx_n ^ 1970169159));
	byte bLoc20_n = 0x00;
	ui32 r8d_n = r8d_n;
	ui32 r11d_n = ebx_n ^ 1752462657 | edx ^ 0x69746E65 | 0x444D4163;
	word32 ebx_n = ebx_n;
	if ((edx ^ 0x49656E69 | 1818588270 | ebx_n ^ 1970169159) != 0x00)
	{
l0000000140001CE9:
		if (r11d_n == 0x00 && false)
		{
			ui32 r8d_n = r8d_n | 0x04;
			g_dw40003614 = r8d_n;
			r8d_n = r8d_n;
		}
		if (false)
		{
			__cpuid(0x07, 0x00, &0x07, &ebx_n, &0x00, &(ebx_n ^ 1970169159));
			bLoc20_n = (byte) ebx_n;
			if (!__bt<word32>(ebx_n, 0x09))
				g_dw40003614 = r8d_n | 0x02;
		}
		if (!__bt<word32>(0x00, 0x14))
		{
			g_dw40003018 = 0x02;
			g_dw4000301C = 0x06;
			if (!__bt<word32>(0x00, 0x1B) && !__bt<word32>(0x00, 0x1C))
			{
				word64 edx_eax_n = __xgetbv(0x00);
				if (((SLICE(edx_eax_n, byte, 32) << 0x20 | (byte) edx_eax_n) & 0x06) == 0x06)
				{
					ui32 eax_n = g_dw4000301C | 0x08;
					g_dw40003018 = 0x03;
					g_dw4000301C = eax_n;
					if ((bLoc20_n & 0x20) != 0x00)
					{
						g_dw40003018 = 0x05;
						g_dw4000301C = eax_n | 0x20;
					}
				}
			}
		}
	}
	else
	{
		g_qw40003020 = ~0x00;
		r8d_n = r8d_n | 0x04;
		g_dw40003614 = r8d_n | 0x04;
		ebx_n = ebx_n;
		if (true)
		{
			ebx_n = ebx_n;
			if (false)
				goto l0000000140001CDE;
			ebx_n = ebx_n;
			if (true)
			{
				ebx_n = ebx_n;
				if (true)
				{
					ebx_n = 0x00010001;
					if (!__bt<word64>(0x100010001, 0xFFFCF9B0))
						goto l0000000140001CDE;
				}
				goto l0000000140001CE9;
			}
		}
l0000000140001CDE:
		r8d_n = r8d_n | 0x04 | 0x01;
		g_dw40003614 = r8d_n;
		goto l0000000140001CE9;
	}
}

// 0000000140001DC4: Register word32 fn0000000140001DC4()
// Called from:
//      fn00000001400015C4
//      fn000000014000164C
//      fn00000001400017B4
word32 fn0000000140001DC4()
{
	return (word32) (g_dw40003030 != 0x00);
}

// 0000000140001DD0: void fn0000000140001DD0()
// Called from:
//      fn00000001400011D4
void fn0000000140001DD0()
{
}

// 0000000140001E7C: void fn0000000140001E7C(Register Eq_n rdx, Register (ptr64 Eq_n) r9)
void fn0000000140001E7C(Eq_n rdx, struct Eq_n * r9)
{
	word64 qwLoc30;
	fn0000000140001E9C(rdx, r9, r9->ptr0038, qwLoc30);
}

// 0000000140001E9C: void fn0000000140001E9C(Register Eq_n rcx, Register (ptr64 Eq_n) rdx, Register (ptr64 Eq_n) r8, Stack word64 qwArg00)
// Called from:
//      fn0000000140001E7C
void fn0000000140001E9C(Eq_n rcx, struct Eq_n * rdx, struct Eq_n * r8, word64 qwArg00)
{
	ui32 r11d_n = r8->t0000.u0 & ~0x07;
	Eq_n r9_n = rcx;
	Eq_n r10_n = rcx;
	if ((r8->t0000.u1 & 0x04) != 0x00)
		r10_n = (word64) rcx + (int64) r8->dw0004 & (int64) (-r8->dw0008);
	word64 rdx_n = (word64) *((word64) r10_n + (int64) r11d_n);
	struct Eq_n * rcx_n = (uint64) rdx->ptr0010->dw0008 + rdx->qw0008;
	if ((rcx_n->b0003 & 0x0F) != 0x00)
		r9_n = (word64) rcx + (uint64) ((word32) rcx_n->b0003 & ~0x0F);
	ui64 r9_n = r9_n ^ rdx_n;
	fn00000001400011B0(r9_n, qwArg00);
}

// 0000000140001F10: void fn0000000140001F10(Register (ptr64 code) rax)
void fn0000000140001F10(<anonymous> * rax)
{
	rax();
}

// 0000000140001F12: void fn0000000140001F12(Register (ptr64 (ptr64 word32)) rcx)
void fn0000000140001F12(word32 ** rcx)
{
	seh_filter_exe((uint64) **rcx, rcx);
}

// 0000000140001F30: void fn0000000140001F30()
void fn0000000140001F30()
{
}

