// LocalStackVariables.h
// Generated by decompiling LocalStackVariables.exe
// using Reko decompiler version 0.12.1.0.

/*
// Equivalence classes ////////////
Eq_1: (struct "Globals"
		(402000 (ptr32 code) __imp__UnhandledExceptionFilter)
		(402004 (ptr32 code) __imp__GetCurrentProcess)
		(402008 (ptr32 code) __imp__TerminateProcess)
		(40200C (ptr32 code) __imp__GetSystemTimeAsFileTime)
		(402010 (ptr32 code) __imp__GetCurrentProcessId)
		(402014 (ptr32 code) __imp__GetCurrentThreadId)
		(402018 (ptr32 code) __imp__GetTickCount)
		(40201C (ptr32 code) __imp__QueryPerformanceCounter)
		(402020 (ptr32 code) __imp__SetUnhandledExceptionFilter)
		(402024 (ptr32 code) __imp__InterlockedCompareExchange)
		(402028 (ptr32 code) __imp__Sleep)
		(40202C (ptr32 code) __imp__InterlockedExchange)
		(402030 (ptr32 code) __imp__IsDebuggerPresent)
		(402038 (ptr32 code) __imp____p__fmode)
		(40203C (ptr32 code) __imp___encode_pointer)
		(402040 (ptr32 code) __imp____set_app_type)
		(402044 (ptr32 code) __imp__?terminate@@YAXXZ)
		(402048 (ptr32 code) __imp___unlock)
		(40204C (ptr32 code) __imp____p__commode)
		(402050 (ptr32 code) __imp___lock)
		(402054 (ptr32 code) __imp___onexit)
		(402058 (ptr32 code) __imp___decode_pointer)
		(40205C (ptr32 code) __imp___except_handler4_common)
		(402060 (ptr32 code) __imp___invoke_watson)
		(402064 (ptr32 code) __imp___controlfp_s)
		(402068 (ptr32 code) __imp___crt_debugger_hook)
		(40206C (ptr32 code) __imp___adjust_fdiv)
		(402070 (ptr32 code) __imp____setusermatherr)
		(402074 (ptr32 code) __imp___configthreadlocale)
		(402078 (ptr32 code) __imp___initterm_e)
		(40207C (ptr32 code) __imp___initterm)
		(402080 (ptr32 code) __imp____initenv)
		(402084 (ptr32 code) __imp__exit)
		(402088 (ptr32 code) __imp___XcptFilter)
		(40208C (ptr32 code) __imp___exit)
		(402090 (ptr32 code) __imp___cexit)
		(402094 (ptr32 code) __imp____getmainargs)
		(402098 (ptr32 code) __imp___amsg_exit)
		(40209C (ptr32 code) __imp____dllonexit)
		(4020A0 (ptr32 code) __imp__printf)
		(4020C8 (str char) str4020C8)
		(4020D8 (str char) str4020D8)
		(4020E0 real64 r4020E0)
		(4020E8 real64 r4020E8)
		(4020F0 real64 r4020F0)
		(4020F8 real64 r4020F8)
		(402200 word32 dw402200)
		(402204 word32 dw402204)
		(402208 word32 dw402208)
		(40220C word32 dw40220C)
		(402210 word32 dw402210)
		(402214 word32 dw402214)
		(402218 word32 dw402218)
		(40221C word32 dw40221C)
		(402220 word32 dw402220)
		(402224 word32 dw402224)
		(402228 word32 dw402228)
		(40222C word32 dw40222C)
		(402230 word32 dw402230)
		(402238 word32 dw402238)
		(40223C word32 dw40223C)
		(402240 word32 dw402240)
		(402244 word32 dw402244)
		(402248 word32 dw402248)
		(40224C word32 dw40224C)
		(402250 word32 dw402250)
		(402254 word32 dw402254)
		(402258 word32 dw402258)
		(40225C word32 dw40225C)
		(402260 word32 dw402260)
		(402264 word32 dw402264)
		(402268 word32 dw402268)
		(40226C word32 dw40226C)
		(402270 word32 dw402270)
		(402274 word32 dw402274)
		(402278 word32 dw402278)
		(40227C word32 dw40227C)
		(402280 word32 dw402280)
		(402284 word32 dw402284)
		(402288 word32 dw402288)
		(40228C word32 dw40228C)
		(402290 word32 dw402290)
		(402294 word32 dw402294)
		(402298 word32 dw402298)
		(40229C word32 dw40229C)
		(4022A0 word32 dw4022A0)
		(403018 (ptr32 Eq_75) ptr403018))
	globals_t (in globals : (ptr32 (struct "Globals")))
Eq_6: (struct "Eq_6" 0010)
	T_6 (in tLoc2C @ 00401006 : Eq_6)
Eq_7: (struct "Eq_7" 0010 (0 word32 dw0000))
	T_7 (in &tLoc2C @ 00401006 : (ptr32 (struct 0010)))
Eq_12: (struct "Eq_12" 0010 (8 real64 r0008))
	T_12 (in &tLoc2C @ 0040100F : (ptr32 (struct 0010)))
Eq_17: (struct "Eq_17" 0010)
	T_17 (in tLoc1C @ 00401012 : Eq_17)
Eq_18: (struct "Eq_18" 0010 (0 word32 dw0000))
	T_18 (in &tLoc1C @ 00401012 : (ptr32 (struct 0010)))
Eq_24: (struct "Eq_24" 0010 (8 real64 r0008))
	T_24 (in &tLoc1C @ 0040101F : (ptr32 (struct 0010)))
Eq_28: (fn (ptr32 Eq_34) ((ptr32 Eq_30), (ptr32 Eq_30)))
	T_28 (in GetMin @ 0040102A : ptr32)
	T_29 (in signature of GetMin @ 004010D0 : void)
Eq_30: (struct "Eq_30" 0010 (0 int32 dw0000) (8 real64 r0008))
	T_30 (in dwArg04 @ 0040102A : (ptr32 Eq_30))
	T_31 (in dwArg08 @ 0040102A : (ptr32 Eq_30))
	T_32 (in &tLoc2C @ 0040102A : (ptr32 (struct 0010)))
	T_33 (in &tLoc1C @ 0040102A : (ptr32 (struct 0010)))
	T_116 (in eax @ 004010CC : (ptr32 Eq_30))
	T_131 (in eax_30 @ 004010DF : (ptr32 Eq_30))
Eq_34: (struct "Eq_34" (0 word32 dw0000) (8 real64 r0008))
	T_34 (in GetMin(&tLoc2C, &tLoc1C) @ 0040102A : word32)
	T_35 (in eax_26 @ 0040102A : (ptr32 Eq_34))
Eq_37: (struct "Eq_37" 0010 (0 word32 dw0000))
	T_37 (in &tLoc2C @ 00401035 : (ptr32 (struct 0010)))
Eq_50: (fn int32 ((ptr32 char), int32, real64, int32, real64))
	T_50 (in printf @ 00401070 : ptr32)
	T_51 (in signature of printf : void)
Eq_58: (struct "Eq_58" 0010 (0 int32 dw0000))
	T_58 (in &tLoc2C @ 00401070 : (ptr32 (struct 0010)))
Eq_62: (struct "Eq_62" 0010 (8 real64 r0008))
	T_62 (in &tLoc2C @ 00401070 : (ptr32 (struct 0010)))
Eq_66: (struct "Eq_66" 0010 (0 int32 dw0000))
	T_66 (in &tLoc1C @ 00401070 : (ptr32 (struct 0010)))
Eq_70: (struct "Eq_70" 0010 (8 real64 r0008))
	T_70 (in &tLoc1C @ 00401070 : (ptr32 (struct 0010)))
Eq_75: (struct "Eq_75" 0010 (0 word32 dw0000) (8 real64 r0008))
	T_75 (in &tLoc1C @ 0040107C : (ptr32 (struct 0010)))
	T_77 (in Mem66[0x00403018<p32>:word32] @ 0040107C : word32)
	T_90 (in Mem70[0x00403018<p32>:word32] @ 00401097 : word32)
	T_96 (in Mem73[0x00403018<p32>:word32] @ 004010A9 : word32)
Eq_79: (struct "Eq_79" 0010 (0 word32 dw0000))
	T_79 (in &tLoc1C @ 00401081 : (ptr32 (struct 0010)))
Eq_85: (struct "Eq_85" 0010 (8 real64 r0008))
	T_85 (in &tLoc1C @ 0040108E : (ptr32 (struct 0010)))
Eq_100: (fn int32 ((ptr32 char), int32, real64))
	T_100 (in printf @ 004010BE : ptr32)
	T_101 (in signature of printf : void)
Eq_106: (struct "Eq_106" 0010 (0 int32 dw0000))
	T_106 (in &tLoc1C @ 004010BE : (ptr32 (struct 0010)))
Eq_110: (struct "Eq_110" 0010 (8 real64 r0008))
	T_110 (in &tLoc1C @ 004010BE : (ptr32 (struct 0010)))
// Type Variables ////////////
globals_t: (in globals : (ptr32 (struct "Globals")))
  Class: Eq_1
  DataType: (ptr32 Eq_1)
  OrigDataType: (ptr32 (struct "Globals"))
T_2: (in eax : int32)
  Class: Eq_2
  DataType: int32
  OrigDataType: int32
T_3: (in argc : int32)
  Class: Eq_3
  DataType: int32
  OrigDataType: int32
T_4: (in argv : (ptr32 (ptr32 char)))
  Class: Eq_4
  DataType: (ptr32 (ptr32 char))
  OrigDataType: (ptr32 (ptr32 char))
T_5: (in 0<32> @ 00401006 : word32)
  Class: Eq_5
  DataType: word32
  OrigDataType: word32
T_6: (in tLoc2C @ 00401006 : Eq_6)
  Class: Eq_6
  DataType: Eq_6
  OrigDataType: (struct 0010)
T_7: (in &tLoc2C @ 00401006 : (ptr32 (struct 0010)))
  Class: Eq_7
  DataType: (ptr32 Eq_7)
  OrigDataType: (ptr32 (struct 0010 (0 T_10 t0000)))
T_8: (in 0<32> @ 00401006 : word32)
  Class: Eq_8
  DataType: word32
  OrigDataType: word32
T_9: (in &tLoc2C + 0<32> @ 00401006 : word32)
  Class: Eq_9
  DataType: ptr32
  OrigDataType: ptr32
T_10: (in Mem10[&tLoc2C + 0<32>:word32] @ 00401006 : word32)
  Class: Eq_5
  DataType: word32
  OrigDataType: word32
T_11: (in 1.0 @ 0040100F : real64)
  Class: Eq_11
  DataType: real64
  OrigDataType: real64
T_12: (in &tLoc2C @ 0040100F : (ptr32 (struct 0010)))
  Class: Eq_12
  DataType: (ptr32 Eq_12)
  OrigDataType: (ptr32 (struct 0010 (8 T_15 t0008)))
T_13: (in 8<i32> @ 0040100F : int32)
  Class: Eq_13
  DataType: int32
  OrigDataType: int32
T_14: (in &tLoc2C + 8<i32> @ 0040100F : word32)
  Class: Eq_14
  DataType: ptr32
  OrigDataType: ptr32
T_15: (in Mem13[&tLoc2C + 8<i32>:real64] @ 0040100F : real64)
  Class: Eq_11
  DataType: real64
  OrigDataType: real64
T_16: (in 0xA<32> @ 00401012 : word32)
  Class: Eq_16
  DataType: word32
  OrigDataType: word32
T_17: (in tLoc1C @ 00401012 : Eq_17)
  Class: Eq_17
  DataType: Eq_17
  OrigDataType: (struct 0010)
T_18: (in &tLoc1C @ 00401012 : (ptr32 (struct 0010)))
  Class: Eq_18
  DataType: (ptr32 Eq_18)
  OrigDataType: (ptr32 (struct 0010 (0 T_21 t0000)))
T_19: (in 0<32> @ 00401012 : word32)
  Class: Eq_19
  DataType: word32
  OrigDataType: word32
T_20: (in &tLoc1C + 0<32> @ 00401012 : word32)
  Class: Eq_20
  DataType: ptr32
  OrigDataType: ptr32
T_21: (in Mem15[&tLoc1C + 0<32>:word32] @ 00401012 : word32)
  Class: Eq_16
  DataType: word32
  OrigDataType: word32
T_22: (in 004020F8 @ 0040101F : ptr32)
  Class: Eq_22
  DataType: (ptr32 real64)
  OrigDataType: (ptr32 (struct (0 T_23 t0000)))
T_23: (in Mem15[0x004020F8<p32>:real64] @ 0040101F : real64)
  Class: Eq_23
  DataType: real64
  OrigDataType: real64
T_24: (in &tLoc1C @ 0040101F : (ptr32 (struct 0010)))
  Class: Eq_24
  DataType: (ptr32 Eq_24)
  OrigDataType: (ptr32 (struct 0010 (8 T_27 t0008)))
T_25: (in 8<i32> @ 0040101F : int32)
  Class: Eq_25
  DataType: int32
  OrigDataType: int32
T_26: (in &tLoc1C + 8<i32> @ 0040101F : word32)
  Class: Eq_26
  DataType: ptr32
  OrigDataType: ptr32
T_27: (in Mem18[&tLoc1C + 8<i32>:real64] @ 0040101F : real64)
  Class: Eq_23
  DataType: real64
  OrigDataType: real64
T_28: (in GetMin @ 0040102A : ptr32)
  Class: Eq_28
  DataType: (ptr32 Eq_28)
  OrigDataType: (ptr32 (fn T_34 (T_32, T_33)))
T_29: (in signature of GetMin @ 004010D0 : void)
  Class: Eq_28
  DataType: (ptr32 Eq_28)
  OrigDataType: 
T_30: (in dwArg04 @ 0040102A : (ptr32 Eq_30))
  Class: Eq_30
  DataType: (ptr32 Eq_30)
  OrigDataType: (ptr32 (struct (0 T_119 t0000) (8 T_137 t0008)))
T_31: (in dwArg08 @ 0040102A : (ptr32 Eq_30))
  Class: Eq_30
  DataType: (ptr32 Eq_30)
  OrigDataType: (ptr32 (struct (0 T_119 t0000) (8 T_134 t0008)))
T_32: (in &tLoc2C @ 0040102A : (ptr32 (struct 0010)))
  Class: Eq_30
  DataType: (ptr32 Eq_30)
  OrigDataType: (ptr32 (struct 0010))
T_33: (in &tLoc1C @ 0040102A : (ptr32 (struct 0010)))
  Class: Eq_30
  DataType: (ptr32 Eq_30)
  OrigDataType: (ptr32 (struct 0010))
T_34: (in GetMin(&tLoc2C, &tLoc1C) @ 0040102A : word32)
  Class: Eq_34
  DataType: (ptr32 Eq_34)
  OrigDataType: word32
T_35: (in eax_26 @ 0040102A : (ptr32 Eq_34))
  Class: Eq_34
  DataType: (ptr32 Eq_34)
  OrigDataType: (ptr32 (struct (0 T_44 t0000) (8 T_49 t0008)))
T_36: (in 0x64<32> @ 00401035 : word32)
  Class: Eq_36
  DataType: word32
  OrigDataType: word32
T_37: (in &tLoc2C @ 00401035 : (ptr32 (struct 0010)))
  Class: Eq_37
  DataType: (ptr32 Eq_37)
  OrigDataType: (ptr32 (struct 0010 (0 T_40 t0000)))
T_38: (in 0<32> @ 00401035 : word32)
  Class: Eq_38
  DataType: word32
  OrigDataType: word32
T_39: (in &tLoc2C + 0<32> @ 00401035 : word32)
  Class: Eq_39
  DataType: ptr32
  OrigDataType: ptr32
T_40: (in Mem37[&tLoc2C + 0<32>:word32] @ 00401035 : word32)
  Class: Eq_36
  DataType: word32
  OrigDataType: word32
T_41: (in 5<32> @ 0040103F : word32)
  Class: Eq_41
  DataType: word32
  OrigDataType: word32
T_42: (in 0<32> @ 0040103F : word32)
  Class: Eq_42
  DataType: word32
  OrigDataType: word32
T_43: (in eax_26 + 0<32> @ 0040103F : word32)
  Class: Eq_43
  DataType: word32
  OrigDataType: word32
T_44: (in Mem39[eax_26 + 0<32>:word32] @ 0040103F : word32)
  Class: Eq_41
  DataType: word32
  OrigDataType: word32
T_45: (in 004020F0 @ 0040104E : ptr32)
  Class: Eq_45
  DataType: (ptr32 real64)
  OrigDataType: (ptr32 (struct (0 T_46 t0000)))
T_46: (in Mem39[0x004020F0<p32>:real64] @ 0040104E : real64)
  Class: Eq_46
  DataType: real64
  OrigDataType: real64
T_47: (in 8<i32> @ 0040104E : int32)
  Class: Eq_47
  DataType: int32
  OrigDataType: int32
T_48: (in eax_26 + 8<i32> @ 0040104E : word32)
  Class: Eq_48
  DataType: ptr32
  OrigDataType: ptr32
T_49: (in Mem43[eax_26 + 8<i32>:real64] @ 0040104E : real64)
  Class: Eq_46
  DataType: real64
  OrigDataType: real64
T_50: (in printf @ 00401070 : ptr32)
  Class: Eq_50
  DataType: (ptr32 Eq_50)
  OrigDataType: (ptr32 (fn T_74 (T_57, T_61, T_65, T_69, T_73)))
T_51: (in signature of printf : void)
  Class: Eq_50
  DataType: (ptr32 Eq_50)
  OrigDataType: 
T_52: (in ptrArg04 @ 00401070 : (ptr32 char))
  Class: Eq_52
  DataType: (ptr32 char)
  OrigDataType: 
T_53: (in  @ 00401070 : int32)
  Class: Eq_53
  DataType: int32
  OrigDataType: 
T_54: (in  @ 00401070 : real64)
  Class: Eq_54
  DataType: real64
  OrigDataType: 
T_55: (in  @ 00401070 : int32)
  Class: Eq_55
  DataType: int32
  OrigDataType: 
T_56: (in  @ 00401070 : real64)
  Class: Eq_56
  DataType: real64
  OrigDataType: 
T_57: (in 0x4020C8<32> @ 00401070 : word32)
  Class: Eq_52
  DataType: (ptr32 char)
  OrigDataType: (ptr32 char)
T_58: (in &tLoc2C @ 00401070 : (ptr32 (struct 0010)))
  Class: Eq_58
  DataType: (ptr32 Eq_58)
  OrigDataType: (ptr32 (struct 0010 (0 T_61 t0000)))
T_59: (in 0<32> @ 00401070 : word32)
  Class: Eq_59
  DataType: word32
  OrigDataType: word32
T_60: (in &tLoc2C + 0<32> @ 00401070 : word32)
  Class: Eq_60
  DataType: ptr32
  OrigDataType: ptr32
T_61: (in Mem56[&tLoc2C + 0<32>:word32] @ 00401070 : word32)
  Class: Eq_53
  DataType: int32
  OrigDataType: int32
T_62: (in &tLoc2C @ 00401070 : (ptr32 (struct 0010)))
  Class: Eq_62
  DataType: (ptr32 Eq_62)
  OrigDataType: (ptr32 (struct 0010 (8 T_65 t0008)))
T_63: (in 8<i32> @ 00401070 : int32)
  Class: Eq_63
  DataType: int32
  OrigDataType: int32
T_64: (in &tLoc2C + 8<i32> @ 00401070 : word32)
  Class: Eq_64
  DataType: ptr32
  OrigDataType: ptr32
T_65: (in Mem52[&tLoc2C + 8<i32>:real64] @ 00401070 : real64)
  Class: Eq_54
  DataType: real64
  OrigDataType: real64
T_66: (in &tLoc1C @ 00401070 : (ptr32 (struct 0010)))
  Class: Eq_66
  DataType: (ptr32 Eq_66)
  OrigDataType: (ptr32 (struct 0010 (0 T_69 t0000)))
T_67: (in 0<32> @ 00401070 : word32)
  Class: Eq_67
  DataType: word32
  OrigDataType: word32
T_68: (in &tLoc1C + 0<32> @ 00401070 : word32)
  Class: Eq_68
  DataType: ptr32
  OrigDataType: ptr32
T_69: (in Mem48[&tLoc1C + 0<32>:word32] @ 00401070 : word32)
  Class: Eq_55
  DataType: int32
  OrigDataType: int32
T_70: (in &tLoc1C @ 00401070 : (ptr32 (struct 0010)))
  Class: Eq_70
  DataType: (ptr32 Eq_70)
  OrigDataType: (ptr32 (struct 0010 (8 T_73 t0008)))
T_71: (in 8<i32> @ 00401070 : int32)
  Class: Eq_71
  DataType: int32
  OrigDataType: int32
T_72: (in &tLoc1C + 8<i32> @ 00401070 : word32)
  Class: Eq_72
  DataType: ptr32
  OrigDataType: ptr32
T_73: (in Mem43[&tLoc1C + 8<i32>:real64] @ 00401070 : real64)
  Class: Eq_56
  DataType: real64
  OrigDataType: real64
T_74: (in printf("%d %f %d %f\n", tLoc2C.dw0000, tLoc2C.r0008, tLoc1C.dw0000, tLoc1C.r0008) @ 00401070 : int32)
  Class: Eq_74
  DataType: int32
  OrigDataType: int32
T_75: (in &tLoc1C @ 0040107C : (ptr32 (struct 0010)))
  Class: Eq_75
  DataType: (ptr32 Eq_75)
  OrigDataType: (ptr32 (struct 0010))
T_76: (in 00403018 @ 0040107C : ptr32)
  Class: Eq_76
  DataType: (ptr32 (ptr32 Eq_75))
  OrigDataType: (ptr32 (struct (0 T_75 t0000)))
T_77: (in Mem66[0x00403018<p32>:word32] @ 0040107C : word32)
  Class: Eq_75
  DataType: (ptr32 Eq_75)
  OrigDataType: word32
T_78: (in 2<32> @ 00401081 : word32)
  Class: Eq_78
  DataType: word32
  OrigDataType: word32
T_79: (in &tLoc1C @ 00401081 : (ptr32 (struct 0010)))
  Class: Eq_79
  DataType: (ptr32 Eq_79)
  OrigDataType: (ptr32 (struct 0010 (0 T_82 t0000)))
T_80: (in 0<32> @ 00401081 : word32)
  Class: Eq_80
  DataType: word32
  OrigDataType: word32
T_81: (in &tLoc1C + 0<32> @ 00401081 : word32)
  Class: Eq_81
  DataType: ptr32
  OrigDataType: ptr32
T_82: (in Mem67[&tLoc1C + 0<32>:word32] @ 00401081 : word32)
  Class: Eq_78
  DataType: word32
  OrigDataType: word32
T_83: (in 004020E8 @ 0040108E : ptr32)
  Class: Eq_83
  DataType: (ptr32 real64)
  OrigDataType: (ptr32 (struct (0 T_84 t0000)))
T_84: (in Mem67[0x004020E8<p32>:real64] @ 0040108E : real64)
  Class: Eq_84
  DataType: real64
  OrigDataType: real64
T_85: (in &tLoc1C @ 0040108E : (ptr32 (struct 0010)))
  Class: Eq_85
  DataType: (ptr32 Eq_85)
  OrigDataType: (ptr32 (struct 0010 (8 T_88 t0008)))
T_86: (in 8<i32> @ 0040108E : int32)
  Class: Eq_86
  DataType: int32
  OrigDataType: int32
T_87: (in &tLoc1C + 8<i32> @ 0040108E : word32)
  Class: Eq_87
  DataType: ptr32
  OrigDataType: ptr32
T_88: (in Mem70[&tLoc1C + 8<i32>:real64] @ 0040108E : real64)
  Class: Eq_84
  DataType: real64
  OrigDataType: real64
T_89: (in 3<32> @ 00401097 : word32)
  Class: Eq_89
  DataType: word32
  OrigDataType: word32
T_90: (in Mem70[0x00403018<p32>:word32] @ 00401097 : word32)
  Class: Eq_75
  DataType: (ptr32 Eq_75)
  OrigDataType: (ptr32 (struct (0 T_93 t0000)))
T_91: (in 0<32> @ 00401097 : word32)
  Class: Eq_91
  DataType: word32
  OrigDataType: word32
T_92: (in Mem70[0x00403018<p32>:word32] + 0<32> @ 00401097 : word32)
  Class: Eq_92
  DataType: word32
  OrigDataType: word32
T_93: (in Mem73[Mem70[0x00403018<p32>:word32] + 0<32>:word32] @ 00401097 : word32)
  Class: Eq_89
  DataType: word32
  OrigDataType: word32
T_94: (in 004020E0 @ 004010A9 : ptr32)
  Class: Eq_94
  DataType: (ptr32 real64)
  OrigDataType: (ptr32 (struct (0 T_95 t0000)))
T_95: (in Mem73[0x004020E0<p32>:real64] @ 004010A9 : real64)
  Class: Eq_95
  DataType: real64
  OrigDataType: real64
T_96: (in Mem73[0x00403018<p32>:word32] @ 004010A9 : word32)
  Class: Eq_75
  DataType: (ptr32 Eq_75)
  OrigDataType: (ptr32 (struct 0010 (8 real64 r0008)))
T_97: (in 8<i32> @ 004010A9 : int32)
  Class: Eq_97
  DataType: int32
  OrigDataType: int32
T_98: (in Mem73[0x00403018<p32>:word32] + 8<i32> @ 004010A9 : word32)
  Class: Eq_98
  DataType: ptr32
  OrigDataType: ptr32
T_99: (in Mem77[Mem73[0x00403018<p32>:word32] + 8<i32>:real64] @ 004010A9 : real64)
  Class: Eq_95
  DataType: real64
  OrigDataType: real64
T_100: (in printf @ 004010BE : ptr32)
  Class: Eq_100
  DataType: (ptr32 Eq_100)
  OrigDataType: (ptr32 (fn T_114 (T_105, T_109, T_113)))
T_101: (in signature of printf : void)
  Class: Eq_100
  DataType: (ptr32 Eq_100)
  OrigDataType: 
T_102: (in ptrArg04 @ 004010BE : (ptr32 char))
  Class: Eq_102
  DataType: (ptr32 char)
  OrigDataType: 
T_103: (in  @ 004010BE : int32)
  Class: Eq_103
  DataType: int32
  OrigDataType: 
T_104: (in  @ 004010BE : real64)
  Class: Eq_104
  DataType: real64
  OrigDataType: 
T_105: (in 0x4020D8<32> @ 004010BE : word32)
  Class: Eq_102
  DataType: (ptr32 char)
  OrigDataType: (ptr32 char)
T_106: (in &tLoc1C @ 004010BE : (ptr32 (struct 0010)))
  Class: Eq_106
  DataType: (ptr32 Eq_106)
  OrigDataType: (ptr32 (struct 0010 (0 T_109 t0000)))
T_107: (in 0<32> @ 004010BE : word32)
  Class: Eq_107
  DataType: word32
  OrigDataType: word32
T_108: (in &tLoc1C + 0<32> @ 004010BE : word32)
  Class: Eq_108
  DataType: ptr32
  OrigDataType: ptr32
T_109: (in Mem82[&tLoc1C + 0<32>:word32] @ 004010BE : word32)
  Class: Eq_103
  DataType: int32
  OrigDataType: int32
T_110: (in &tLoc1C @ 004010BE : (ptr32 (struct 0010)))
  Class: Eq_110
  DataType: (ptr32 Eq_110)
  OrigDataType: (ptr32 (struct 0010 (8 T_113 t0008)))
T_111: (in 8<i32> @ 004010BE : int32)
  Class: Eq_111
  DataType: int32
  OrigDataType: int32
T_112: (in &tLoc1C + 8<i32> @ 004010BE : word32)
  Class: Eq_112
  DataType: ptr32
  OrigDataType: ptr32
T_113: (in Mem77[&tLoc1C + 8<i32>:real64] @ 004010BE : real64)
  Class: Eq_104
  DataType: real64
  OrigDataType: real64
T_114: (in printf("%d %f\n", tLoc1C.dw0000, tLoc1C.r0008) @ 004010BE : int32)
  Class: Eq_114
  DataType: int32
  OrigDataType: int32
T_115: (in 0<32> @ 004010CC : word32)
  Class: Eq_2
  DataType: int32
  OrigDataType: word32
T_116: (in eax @ 004010CC : (ptr32 Eq_30))
  Class: Eq_30
  DataType: (ptr32 Eq_30)
  OrigDataType: word32
T_117: (in 0<32> @ 004010DD : word32)
  Class: Eq_117
  DataType: word32
  OrigDataType: word32
T_118: (in dwArg04 + 0<32> @ 004010DD : word32)
  Class: Eq_118
  DataType: word32
  OrigDataType: word32
T_119: (in Mem6[dwArg04 + 0<32>:word32] @ 004010DD : word32)
  Class: Eq_119
  DataType: int32
  OrigDataType: int32
T_120: (in 0<32> @ 004010DD : word32)
  Class: Eq_120
  DataType: word32
  OrigDataType: word32
T_121: (in dwArg08 + 0<32> @ 004010DD : word32)
  Class: Eq_121
  DataType: word32
  OrigDataType: word32
T_122: (in Mem6[dwArg08 + 0<32>:word32] @ 004010DD : word32)
  Class: Eq_119
  DataType: int32
  OrigDataType: int32
T_123: (in dwArg04->dw0000 >= dwArg08->dw0000 @ 004010DD : bool)
  Class: Eq_123
  DataType: bool
  OrigDataType: bool
T_124: (in 0<32> @ 004010EE : word32)
  Class: Eq_124
  DataType: word32
  OrigDataType: word32
T_125: (in dwArg04 + 0<32> @ 004010EE : word32)
  Class: Eq_125
  DataType: (ptr32 int32)
  OrigDataType: (ptr32 int32)
T_126: (in Mem6[dwArg04 + 0<32>:word32] @ 004010EE : word32)
  Class: Eq_119
  DataType: int32
  OrigDataType: int32
T_127: (in 0<32> @ 004010EE : word32)
  Class: Eq_127
  DataType: word32
  OrigDataType: word32
T_128: (in dwArg08 + 0<32> @ 004010EE : word32)
  Class: Eq_128
  DataType: (ptr32 int32)
  OrigDataType: (ptr32 int32)
T_129: (in Mem6[dwArg08 + 0<32>:word32] @ 004010EE : word32)
  Class: Eq_119
  DataType: int32
  OrigDataType: int32
T_130: (in dwArg04->dw0000 >= dwArg08->dw0000 @ 004010EE : bool)
  Class: Eq_130
  DataType: bool
  OrigDataType: bool
T_131: (in eax_30 @ 004010DF : (ptr32 Eq_30))
  Class: Eq_30
  DataType: (ptr32 Eq_30)
  OrigDataType: (ptr32 (struct (0 T_119 t0000) (8 T_134 t0008)))
T_132: (in 8<i32> @ 00401106 : int32)
  Class: Eq_132
  DataType: int32
  OrigDataType: int32
T_133: (in dwArg08 + 8<i32> @ 00401106 : word32)
  Class: Eq_133
  DataType: ptr32
  OrigDataType: ptr32
T_134: (in Mem6[dwArg08 + 8<i32>:real64] @ 00401106 : real64)
  Class: Eq_134
  DataType: real64
  OrigDataType: real64
T_135: (in 8<i32> @ 00401106 : int32)
  Class: Eq_135
  DataType: int32
  OrigDataType: int32
T_136: (in dwArg04 + 8<i32> @ 00401106 : word32)
  Class: Eq_136
  DataType: ptr32
  OrigDataType: ptr32
T_137: (in Mem6[dwArg04 + 8<i32>:real64] @ 00401106 : real64)
  Class: Eq_134
  DataType: real64
  OrigDataType: real64
T_138: (in dwArg08->r0008 <= dwArg04->r0008 @ 00401106 : bool)
  Class: Eq_138
  DataType: bool
  OrigDataType: bool
T_139:
  Class: Eq_139
  DataType: word32
  OrigDataType: word32
T_140:
  Class: Eq_140
  DataType: word32
  OrigDataType: word32
T_141:
  Class: Eq_141
  DataType: word32
  OrigDataType: word32
T_142:
  Class: Eq_142
  DataType: word32
  OrigDataType: word32
T_143:
  Class: Eq_143
  DataType: word32
  OrigDataType: word32
T_144:
  Class: Eq_144
  DataType: word32
  OrigDataType: word32
T_145:
  Class: Eq_145
  DataType: word32
  OrigDataType: word32
T_146:
  Class: Eq_146
  DataType: word32
  OrigDataType: word32
T_147:
  Class: Eq_147
  DataType: word32
  OrigDataType: word32
T_148:
  Class: Eq_148
  DataType: word32
  OrigDataType: word32
T_149:
  Class: Eq_149
  DataType: word32
  OrigDataType: word32
T_150:
  Class: Eq_150
  DataType: word32
  OrigDataType: word32
T_151:
  Class: Eq_151
  DataType: word32
  OrigDataType: word32
T_152:
  Class: Eq_152
  DataType: word32
  OrigDataType: word32
T_153:
  Class: Eq_153
  DataType: word32
  OrigDataType: word32
T_154:
  Class: Eq_154
  DataType: word32
  OrigDataType: word32
T_155:
  Class: Eq_155
  DataType: word32
  OrigDataType: word32
T_156:
  Class: Eq_156
  DataType: word32
  OrigDataType: word32
T_157:
  Class: Eq_157
  DataType: word32
  OrigDataType: word32
T_158:
  Class: Eq_158
  DataType: word32
  OrigDataType: word32
T_159:
  Class: Eq_159
  DataType: word32
  OrigDataType: word32
T_160:
  Class: Eq_160
  DataType: word32
  OrigDataType: word32
T_161:
  Class: Eq_161
  DataType: word32
  OrigDataType: word32
T_162:
  Class: Eq_162
  DataType: word32
  OrigDataType: word32
T_163:
  Class: Eq_163
  DataType: word32
  OrigDataType: word32
T_164:
  Class: Eq_164
  DataType: word32
  OrigDataType: word32
T_165:
  Class: Eq_165
  DataType: word32
  OrigDataType: word32
T_166:
  Class: Eq_166
  DataType: word32
  OrigDataType: word32
T_167:
  Class: Eq_167
  DataType: word32
  OrigDataType: word32
T_168:
  Class: Eq_168
  DataType: word32
  OrigDataType: word32
T_169:
  Class: Eq_169
  DataType: word32
  OrigDataType: word32
T_170:
  Class: Eq_170
  DataType: word32
  OrigDataType: word32
T_171:
  Class: Eq_171
  DataType: word32
  OrigDataType: word32
T_172:
  Class: Eq_172
  DataType: word32
  OrigDataType: word32
T_173:
  Class: Eq_173
  DataType: word32
  OrigDataType: word32
T_174:
  Class: Eq_174
  DataType: word32
  OrigDataType: word32
T_175:
  Class: Eq_175
  DataType: word32
  OrigDataType: word32
T_176:
  Class: Eq_176
  DataType: word32
  OrigDataType: word32
T_177:
  Class: Eq_177
  DataType: word32
  OrigDataType: word32
T_178:
  Class: Eq_178
  DataType: word32
  OrigDataType: word32
*/
typedef struct Globals {
	<anonymous> * __imp__UnhandledExceptionFilter;	// 402000
	<anonymous> * __imp__GetCurrentProcess;	// 402004
	<anonymous> * __imp__TerminateProcess;	// 402008
	<anonymous> * __imp__GetSystemTimeAsFileTime;	// 40200C
	<anonymous> * __imp__GetCurrentProcessId;	// 402010
	<anonymous> * __imp__GetCurrentThreadId;	// 402014
	<anonymous> * __imp__GetTickCount;	// 402018
	<anonymous> * __imp__QueryPerformanceCounter;	// 40201C
	<anonymous> * __imp__SetUnhandledExceptionFilter;	// 402020
	<anonymous> * __imp__InterlockedCompareExchange;	// 402024
	<anonymous> * __imp__Sleep;	// 402028
	<anonymous> * __imp__InterlockedExchange;	// 40202C
	<anonymous> * __imp__IsDebuggerPresent;	// 402030
	<anonymous> * __imp____p__fmode;	// 402038
	<anonymous> * __imp___encode_pointer;	// 40203C
	<anonymous> * __imp____set_app_type;	// 402040
	<anonymous> * __imp__?terminate@@YAXXZ;	// 402044
	<anonymous> * __imp___unlock;	// 402048
	<anonymous> * __imp____p__commode;	// 40204C
	<anonymous> * __imp___lock;	// 402050
	<anonymous> * __imp___onexit;	// 402054
	<anonymous> * __imp___decode_pointer;	// 402058
	<anonymous> * __imp___except_handler4_common;	// 40205C
	<anonymous> * __imp___invoke_watson;	// 402060
	<anonymous> * __imp___controlfp_s;	// 402064
	<anonymous> * __imp___crt_debugger_hook;	// 402068
	<anonymous> * __imp___adjust_fdiv;	// 40206C
	<anonymous> * __imp____setusermatherr;	// 402070
	<anonymous> * __imp___configthreadlocale;	// 402074
	<anonymous> * __imp___initterm_e;	// 402078
	<anonymous> * __imp___initterm;	// 40207C
	<anonymous> * __imp____initenv;	// 402080
	<anonymous> * __imp__exit;	// 402084
	<anonymous> * __imp___XcptFilter;	// 402088
	<anonymous> * __imp___exit;	// 40208C
	<anonymous> * __imp___cexit;	// 402090
	<anonymous> * __imp____getmainargs;	// 402094
	<anonymous> * __imp___amsg_exit;	// 402098
	<anonymous> * __imp____dllonexit;	// 40209C
	<anonymous> * __imp__printf;	// 4020A0
	char str4020C8[];	// 4020C8
	char str4020D8[];	// 4020D8
	real64 r4020E0;	// 4020E0
	real64 r4020E8;	// 4020E8
	real64 r4020F0;	// 4020F0
	real64 r4020F8;	// 4020F8
	word32 dw402200;	// 402200
	word32 dw402204;	// 402204
	word32 dw402208;	// 402208
	word32 dw40220C;	// 40220C
	word32 dw402210;	// 402210
	word32 dw402214;	// 402214
	word32 dw402218;	// 402218
	word32 dw40221C;	// 40221C
	word32 dw402220;	// 402220
	word32 dw402224;	// 402224
	word32 dw402228;	// 402228
	word32 dw40222C;	// 40222C
	word32 dw402230;	// 402230
	word32 dw402238;	// 402238
	word32 dw40223C;	// 40223C
	word32 dw402240;	// 402240
	word32 dw402244;	// 402244
	word32 dw402248;	// 402248
	word32 dw40224C;	// 40224C
	word32 dw402250;	// 402250
	word32 dw402254;	// 402254
	word32 dw402258;	// 402258
	word32 dw40225C;	// 40225C
	word32 dw402260;	// 402260
	word32 dw402264;	// 402264
	word32 dw402268;	// 402268
	word32 dw40226C;	// 40226C
	word32 dw402270;	// 402270
	word32 dw402274;	// 402274
	word32 dw402278;	// 402278
	word32 dw40227C;	// 40227C
	word32 dw402280;	// 402280
	word32 dw402284;	// 402284
	word32 dw402288;	// 402288
	word32 dw40228C;	// 40228C
	word32 dw402290;	// 402290
	word32 dw402294;	// 402294
	word32 dw402298;	// 402298
	word32 dw40229C;	// 40229C
	word32 dw4022A0;	// 4022A0
	struct Eq_75 * ptr403018;	// 403018
} Eq_1;

typedef struct Eq_6 {	// size: 16 10
} Eq_6;

typedef struct Eq_7 {	// size: 16 10
	word32 dw0000;	// 0
} Eq_7;

typedef struct Eq_12 {	// size: 16 10
	real64 r0008;	// 8
} Eq_12;

typedef struct Eq_17 {	// size: 16 10
} Eq_17;

typedef struct Eq_18 {	// size: 16 10
	word32 dw0000;	// 0
} Eq_18;

typedef struct Eq_24 {	// size: 16 10
	real64 r0008;	// 8
} Eq_24;

typedef Eq_34 * (Eq_28)(Eq_30 *, Eq_30 *);

typedef struct Eq_30 {	// size: 16 10
	int32 dw0000;	// 0
	real64 r0008;	// 8
} Eq_30;

typedef struct Eq_34 {
	word32 dw0000;	// 0
	real64 r0008;	// 8
} Eq_34;

typedef struct Eq_37 {	// size: 16 10
	word32 dw0000;	// 0
} Eq_37;

typedef int32 (Eq_50)(char *, int32, real64, int32, real64);

typedef struct Eq_58 {	// size: 16 10
	int32 dw0000;	// 0
} Eq_58;

typedef struct Eq_62 {	// size: 16 10
	real64 r0008;	// 8
} Eq_62;

typedef struct Eq_66 {	// size: 16 10
	int32 dw0000;	// 0
} Eq_66;

typedef struct Eq_70 {	// size: 16 10
	real64 r0008;	// 8
} Eq_70;

typedef struct Eq_75 {	// size: 16 10
	word32 dw0000;	// 0
	real64 r0008;	// 8
} Eq_75;

typedef struct Eq_79 {	// size: 16 10
	word32 dw0000;	// 0
} Eq_79;

typedef struct Eq_85 {	// size: 16 10
	real64 r0008;	// 8
} Eq_85;

typedef int32 (Eq_100)(char *, int32, real64);

typedef struct Eq_106 {	// size: 16 10
	int32 dw0000;	// 0
} Eq_106;

typedef struct Eq_110 {	// size: 16 10
	real64 r0008;	// 8
} Eq_110;

