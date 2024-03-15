#include "pch-cpp.hpp"

#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif


#include <limits>
#include <stdint.h>


template <typename T1>
struct VirtualActionInvoker1
{
	typedef void (*Action)(void*, T1, const RuntimeMethod*);

	static inline void Invoke (Il2CppMethodSlot slot, RuntimeObject* obj, T1 p1)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_virtual_invoke_data(slot, obj);
		((Action)invokeData.methodPtr)(obj, p1, invokeData.method);
	}
};
template <typename R, typename T1>
struct VirtualFuncInvoker1
{
	typedef R (*Func)(void*, T1, const RuntimeMethod*);

	static inline R Invoke (Il2CppMethodSlot slot, RuntimeObject* obj, T1 p1)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_virtual_invoke_data(slot, obj);
		return ((Func)invokeData.methodPtr)(obj, p1, invokeData.method);
	}
};

struct Dictionary_2_t5B5B38BB06341F50E1C75FB53208A2A66CAE57F7;
struct WeakReference_1_tDC6E83496181D1BAFA3B89CBC00BCD0B64450257;
struct ByteU5BU5D_tA6237BF417AE52AD70CFB4EF24A7A82613DF9031;
struct CharU5BU5D_t799905CF001DD5F13F7DBB310181FC4D8B7D0AAB;
struct StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248;
struct CertificateHandler_t148B524FA5DB39F3ABADB181CD420FC505C33804;
struct DownloadHandler_t1B56C7D3F65D97A1E4B566A14A1E783EA8AE4EBB;
struct ExclusiveReference_t411F04D4CC440EB7399290027E1BBABEF4C28837;
struct Hashtable_tEFC3B6496E6747787D8BB761B51F2AE3A8CFFE2D;
struct Regex_tE773142C2BE45C5D362B0F815AFF831707A51772;
struct RegexCode_tA23175D9DA02AD6A79B073E10EC5D225372ED6C7;
struct RegexRunnerFactory_t72373B672C7D8785F63516DDD88834F286AF41E7;
struct String_t;
struct Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E;
struct UriParser_t920B0868286118827C08B08A15A9456AF6C19D81;
struct Void_t4861ACF8F4594C3437BB48B6E56783494B843915;
struct CachedCodeEntry_tE201C3AD65C234AD9ED7A78C95025824A7A9FF39;
struct UriInfo_t5F91F77A93545DDDA6BB24A609BAF5E232CC1A09;

IL2CPP_EXTERN_C RuntimeClass* Regex_tE773142C2BE45C5D362B0F815AFF831707A51772_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* WebRequestUtils_t23F1FB533DBFDA3BE5624D901D535B4C6EFAD443_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C String_t* _stringLiteral665437E312D2053C5919723E2E7EA49D0A2ADE1F;

struct ByteU5BU5D_tA6237BF417AE52AD70CFB4EF24A7A82613DF9031;

IL2CPP_EXTERN_C_BEGIN
IL2CPP_EXTERN_C_END

#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

struct U3CModuleU3E_t42F165DEA2597BD5AB2C914FCF80349ECF878162 
{
};
struct Il2CppArrayBounds;

struct String_t  : public RuntimeObject
{
	int32_t ____stringLength_4;
	Il2CppChar ____firstChar_5;
};

struct String_t_StaticFields
{
	String_t* ___Empty_6;
};

struct Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E  : public RuntimeObject
{
	String_t* ___m_String_13;
	String_t* ___m_originalUnicodeString_14;
	UriParser_t920B0868286118827C08B08A15A9456AF6C19D81* ___m_Syntax_15;
	String_t* ___m_DnsSafeHost_16;
	uint64_t ___m_Flags_17;
	UriInfo_t5F91F77A93545DDDA6BB24A609BAF5E232CC1A09* ___m_Info_18;
	bool ___m_iriParsing_19;
};

struct Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E_StaticFields
{
	String_t* ___UriSchemeFile_0;
	String_t* ___UriSchemeFtp_1;
	String_t* ___UriSchemeGopher_2;
	String_t* ___UriSchemeHttp_3;
	String_t* ___UriSchemeHttps_4;
	String_t* ___UriSchemeWs_5;
	String_t* ___UriSchemeWss_6;
	String_t* ___UriSchemeMailto_7;
	String_t* ___UriSchemeNews_8;
	String_t* ___UriSchemeNntp_9;
	String_t* ___UriSchemeNetTcp_10;
	String_t* ___UriSchemeNetPipe_11;
	String_t* ___SchemeDelimiter_12;
	bool ___s_ConfigInitialized_20;
	bool ___s_ConfigInitializing_21;
	int32_t ___s_IdnScope_22;
	bool ___s_IriParsing_23;
	bool ___useDotNetRelativeOrAbsolute_24;
	bool ___IsWindowsFileSystem_25;
	RuntimeObject* ___s_initLock_26;
	CharU5BU5D_t799905CF001DD5F13F7DBB310181FC4D8B7D0AAB* ___HexLowerChars_27;
	CharU5BU5D_t799905CF001DD5F13F7DBB310181FC4D8B7D0AAB* ____WSchars_28;
};

struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F  : public RuntimeObject
{
};
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F_marshaled_pinvoke
{
};
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F_marshaled_com
{
};

struct WebRequestUtils_t23F1FB533DBFDA3BE5624D901D535B4C6EFAD443  : public RuntimeObject
{
};

struct WebRequestUtils_t23F1FB533DBFDA3BE5624D901D535B4C6EFAD443_StaticFields
{
	Regex_tE773142C2BE45C5D362B0F815AFF831707A51772* ___domainRegex_0;
};

struct Boolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22 
{
	bool ___m_value_0;
};

struct Boolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_StaticFields
{
	String_t* ___TrueString_5;
	String_t* ___FalseString_6;
};

struct Byte_t94D9231AC217BE4D2E004C4CD32DF6D099EA41A3 
{
	uint8_t ___m_value_0;
};

struct Char_t521A6F19B456D956AF452D926C32709DC03D6B17 
{
	Il2CppChar ___m_value_0;
};

struct Char_t521A6F19B456D956AF452D926C32709DC03D6B17_StaticFields
{
	ByteU5BU5D_tA6237BF417AE52AD70CFB4EF24A7A82613DF9031* ___s_categoryForLatin1_3;
};

struct Int32_t680FF22E76F6EFAD4375103CBBFFA0421349384C 
{
	int32_t ___m_value_0;
};

struct IntPtr_t 
{
	void* ___m_value_0;
};

struct IntPtr_t_StaticFields
{
	intptr_t ___Zero_1;
};

struct TimeSpan_t8195C5B013A2C532FEBDF0B64B6911982E750F5A 
{
	int64_t ____ticks_22;
};

struct TimeSpan_t8195C5B013A2C532FEBDF0B64B6911982E750F5A_StaticFields
{
	TimeSpan_t8195C5B013A2C532FEBDF0B64B6911982E750F5A ___Zero_19;
	TimeSpan_t8195C5B013A2C532FEBDF0B64B6911982E750F5A ___MaxValue_20;
	TimeSpan_t8195C5B013A2C532FEBDF0B64B6911982E750F5A ___MinValue_21;
};

struct UInt64_t8F12534CC8FC4B5860F2A2CD1EE79D322E7A41AF 
{
	uint64_t ___m_value_0;
};

struct Void_t4861ACF8F4594C3437BB48B6E56783494B843915 
{
	union
	{
		struct
		{
		};
		uint8_t Void_t4861ACF8F4594C3437BB48B6E56783494B843915__padding[1];
	};
};

struct CertificateHandler_t148B524FA5DB39F3ABADB181CD420FC505C33804  : public RuntimeObject
{
	intptr_t ___m_Ptr_0;
};
struct CertificateHandler_t148B524FA5DB39F3ABADB181CD420FC505C33804_marshaled_pinvoke
{
	intptr_t ___m_Ptr_0;
};
struct CertificateHandler_t148B524FA5DB39F3ABADB181CD420FC505C33804_marshaled_com
{
	intptr_t ___m_Ptr_0;
};

struct DownloadHandler_t1B56C7D3F65D97A1E4B566A14A1E783EA8AE4EBB  : public RuntimeObject
{
	intptr_t ___m_Ptr_0;
};
struct DownloadHandler_t1B56C7D3F65D97A1E4B566A14A1E783EA8AE4EBB_marshaled_pinvoke
{
	intptr_t ___m_Ptr_0;
};
struct DownloadHandler_t1B56C7D3F65D97A1E4B566A14A1E783EA8AE4EBB_marshaled_com
{
	intptr_t ___m_Ptr_0;
};

struct Regex_tE773142C2BE45C5D362B0F815AFF831707A51772  : public RuntimeObject
{
	TimeSpan_t8195C5B013A2C532FEBDF0B64B6911982E750F5A ___internalMatchTimeout_10;
	String_t* ___pattern_12;
	int32_t ___roptions_13;
	RegexRunnerFactory_t72373B672C7D8785F63516DDD88834F286AF41E7* ___factory_14;
	Hashtable_tEFC3B6496E6747787D8BB761B51F2AE3A8CFFE2D* ___caps_15;
	Hashtable_tEFC3B6496E6747787D8BB761B51F2AE3A8CFFE2D* ___capnames_16;
	StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* ___capslist_17;
	int32_t ___capsize_18;
	ExclusiveReference_t411F04D4CC440EB7399290027E1BBABEF4C28837* ____runnerref_19;
	WeakReference_1_tDC6E83496181D1BAFA3B89CBC00BCD0B64450257* ____replref_20;
	RegexCode_tA23175D9DA02AD6A79B073E10EC5D225372ED6C7* ____code_21;
	bool ____refsInitialized_22;
};

struct Regex_tE773142C2BE45C5D362B0F815AFF831707A51772_StaticFields
{
	int32_t ___s_cacheSize_1;
	Dictionary_2_t5B5B38BB06341F50E1C75FB53208A2A66CAE57F7* ___s_cache_2;
	int32_t ___s_cacheCount_3;
	CachedCodeEntry_tE201C3AD65C234AD9ED7A78C95025824A7A9FF39* ___s_cacheFirst_4;
	CachedCodeEntry_tE201C3AD65C234AD9ED7A78C95025824A7A9FF39* ___s_cacheLast_5;
	TimeSpan_t8195C5B013A2C532FEBDF0B64B6911982E750F5A ___s_maximumMatchTimeout_6;
	TimeSpan_t8195C5B013A2C532FEBDF0B64B6911982E750F5A ___s_defaultMatchTimeout_8;
	TimeSpan_t8195C5B013A2C532FEBDF0B64B6911982E750F5A ___InfiniteMatchTimeout_9;
};
#ifdef __clang__
#pragma clang diagnostic pop
#endif
struct ByteU5BU5D_tA6237BF417AE52AD70CFB4EF24A7A82613DF9031  : public RuntimeArray
{
	ALIGN_FIELD (8) uint8_t m_Items[1];

	inline uint8_t GetAt(il2cpp_array_size_t index) const
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items[index];
	}
	inline uint8_t* GetAddressAt(il2cpp_array_size_t index)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items + index;
	}
	inline void SetAt(il2cpp_array_size_t index, uint8_t value)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		m_Items[index] = value;
	}
	inline uint8_t GetAtUnchecked(il2cpp_array_size_t index) const
	{
		return m_Items[index];
	}
	inline uint8_t* GetAddressAtUnchecked(il2cpp_array_size_t index)
	{
		return m_Items + index;
	}
	inline void SetAtUnchecked(il2cpp_array_size_t index, uint8_t value)
	{
		m_Items[index] = value;
	}
};



IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Il2CppChar String_get_Chars_mC49DF0CD2D3BE7BE97B3AD9C995BE3094F8E36D3 (String_t* __this, int32_t ___index0, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Uri__ctor_m24549041BC5661EAC10BA8CB35B60AD6512AF69B (Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* __this, String_t* ___uriString0, int32_t ___uriKind1, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Uri_get_IsAbsoluteUri_m2F9B759B85D295639D7959A616E1FBC203D756DF (Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* Uri_get_AbsoluteUri_m080934F4F2E2160EBEABDF00F8B6D59888EA63AE (Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Uri__ctor_m5AA91BCDE21E8AD62B9A48948CF3661B2F00E38F (Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* __this, Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* ___baseUri0, Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* ___relativeUri1, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Regex__ctor_m082970AA73B8236360F0CA651FA24A8D1EBF89CD (Regex_tE773142C2BE45C5D362B0F815AFF831707A51772* __this, String_t* ___pattern0, const RuntimeMethod* method) ;
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* WebRequestUtils_RedirectTo_m63855E63B6B6EAFD4BCE65395BB744AAF5BD6AB4 (String_t* ___baseUri0, String_t* ___redirectUri1, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* V_0 = NULL;
	Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* V_1 = NULL;
	Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* V_2 = NULL;
	bool V_3 = false;
	bool V_4 = false;
	String_t* V_5 = NULL;
	{
		String_t* L_0 = ___redirectUri1;
		NullCheck(L_0);
		Il2CppChar L_1;
		L_1 = String_get_Chars_mC49DF0CD2D3BE7BE97B3AD9C995BE3094F8E36D3(L_0, 0, NULL);
		V_3 = (bool)((((int32_t)L_1) == ((int32_t)((int32_t)47)))? 1 : 0);
		bool L_2 = V_3;
		if (!L_2)
		{
			goto IL_001a;
		}
	}
	{
		String_t* L_3 = ___redirectUri1;
		Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* L_4 = (Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E*)il2cpp_codegen_object_new(Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E_il2cpp_TypeInfo_var);
		Uri__ctor_m24549041BC5661EAC10BA8CB35B60AD6512AF69B(L_4, L_3, 2, NULL);
		V_0 = L_4;
		goto IL_0022;
	}

IL_001a:
	{
		String_t* L_5 = ___redirectUri1;
		Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* L_6 = (Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E*)il2cpp_codegen_object_new(Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E_il2cpp_TypeInfo_var);
		Uri__ctor_m24549041BC5661EAC10BA8CB35B60AD6512AF69B(L_6, L_5, 0, NULL);
		V_0 = L_6;
	}

IL_0022:
	{
		Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* L_7 = V_0;
		NullCheck(L_7);
		bool L_8;
		L_8 = Uri_get_IsAbsoluteUri_m2F9B759B85D295639D7959A616E1FBC203D756DF(L_7, NULL);
		V_4 = L_8;
		bool L_9 = V_4;
		if (!L_9)
		{
			goto IL_0038;
		}
	}
	{
		Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* L_10 = V_0;
		NullCheck(L_10);
		String_t* L_11;
		L_11 = Uri_get_AbsoluteUri_m080934F4F2E2160EBEABDF00F8B6D59888EA63AE(L_10, NULL);
		V_5 = L_11;
		goto IL_0052;
	}

IL_0038:
	{
		String_t* L_12 = ___baseUri0;
		Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* L_13 = (Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E*)il2cpp_codegen_object_new(Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E_il2cpp_TypeInfo_var);
		Uri__ctor_m24549041BC5661EAC10BA8CB35B60AD6512AF69B(L_13, L_12, 1, NULL);
		V_1 = L_13;
		Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* L_14 = V_1;
		Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* L_15 = V_0;
		Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* L_16 = (Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E*)il2cpp_codegen_object_new(Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E_il2cpp_TypeInfo_var);
		Uri__ctor_m5AA91BCDE21E8AD62B9A48948CF3661B2F00E38F(L_16, L_14, L_15, NULL);
		V_2 = L_16;
		Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* L_17 = V_2;
		NullCheck(L_17);
		String_t* L_18;
		L_18 = Uri_get_AbsoluteUri_m080934F4F2E2160EBEABDF00F8B6D59888EA63AE(L_17, NULL);
		V_5 = L_18;
		goto IL_0052;
	}

IL_0052:
	{
		String_t* L_19 = V_5;
		return L_19;
	}
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WebRequestUtils__cctor_m31ADEB75E00368144C8F223647D81A3B5ADD3D69 (const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Regex_tE773142C2BE45C5D362B0F815AFF831707A51772_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&WebRequestUtils_t23F1FB533DBFDA3BE5624D901D535B4C6EFAD443_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral665437E312D2053C5919723E2E7EA49D0A2ADE1F);
		s_Il2CppMethodInitialized = true;
	}
	{
		Regex_tE773142C2BE45C5D362B0F815AFF831707A51772* L_0 = (Regex_tE773142C2BE45C5D362B0F815AFF831707A51772*)il2cpp_codegen_object_new(Regex_tE773142C2BE45C5D362B0F815AFF831707A51772_il2cpp_TypeInfo_var);
		Regex__ctor_m082970AA73B8236360F0CA651FA24A8D1EBF89CD(L_0, _stringLiteral665437E312D2053C5919723E2E7EA49D0A2ADE1F, NULL);
		((WebRequestUtils_t23F1FB533DBFDA3BE5624D901D535B4C6EFAD443_StaticFields*)il2cpp_codegen_static_fields_for(WebRequestUtils_t23F1FB533DBFDA3BE5624D901D535B4C6EFAD443_il2cpp_TypeInfo_var))->___domainRegex_0 = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&((WebRequestUtils_t23F1FB533DBFDA3BE5624D901D535B4C6EFAD443_StaticFields*)il2cpp_codegen_static_fields_for(WebRequestUtils_t23F1FB533DBFDA3BE5624D901D535B4C6EFAD443_il2cpp_TypeInfo_var))->___domainRegex_0), (void*)L_0);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
IL2CPP_EXTERN_C void CertificateHandler_t148B524FA5DB39F3ABADB181CD420FC505C33804_marshal_pinvoke(const CertificateHandler_t148B524FA5DB39F3ABADB181CD420FC505C33804& unmarshaled, CertificateHandler_t148B524FA5DB39F3ABADB181CD420FC505C33804_marshaled_pinvoke& marshaled)
{
	marshaled.___m_Ptr_0 = unmarshaled.___m_Ptr_0;
}
IL2CPP_EXTERN_C void CertificateHandler_t148B524FA5DB39F3ABADB181CD420FC505C33804_marshal_pinvoke_back(const CertificateHandler_t148B524FA5DB39F3ABADB181CD420FC505C33804_marshaled_pinvoke& marshaled, CertificateHandler_t148B524FA5DB39F3ABADB181CD420FC505C33804& unmarshaled)
{
	intptr_t unmarshaledm_Ptr_temp_0;
	memset((&unmarshaledm_Ptr_temp_0), 0, sizeof(unmarshaledm_Ptr_temp_0));
	unmarshaledm_Ptr_temp_0 = marshaled.___m_Ptr_0;
	unmarshaled.___m_Ptr_0 = unmarshaledm_Ptr_temp_0;
}
IL2CPP_EXTERN_C void CertificateHandler_t148B524FA5DB39F3ABADB181CD420FC505C33804_marshal_pinvoke_cleanup(CertificateHandler_t148B524FA5DB39F3ABADB181CD420FC505C33804_marshaled_pinvoke& marshaled)
{
}
IL2CPP_EXTERN_C void CertificateHandler_t148B524FA5DB39F3ABADB181CD420FC505C33804_marshal_com(const CertificateHandler_t148B524FA5DB39F3ABADB181CD420FC505C33804& unmarshaled, CertificateHandler_t148B524FA5DB39F3ABADB181CD420FC505C33804_marshaled_com& marshaled)
{
	marshaled.___m_Ptr_0 = unmarshaled.___m_Ptr_0;
}
IL2CPP_EXTERN_C void CertificateHandler_t148B524FA5DB39F3ABADB181CD420FC505C33804_marshal_com_back(const CertificateHandler_t148B524FA5DB39F3ABADB181CD420FC505C33804_marshaled_com& marshaled, CertificateHandler_t148B524FA5DB39F3ABADB181CD420FC505C33804& unmarshaled)
{
	intptr_t unmarshaledm_Ptr_temp_0;
	memset((&unmarshaledm_Ptr_temp_0), 0, sizeof(unmarshaledm_Ptr_temp_0));
	unmarshaledm_Ptr_temp_0 = marshaled.___m_Ptr_0;
	unmarshaled.___m_Ptr_0 = unmarshaledm_Ptr_temp_0;
}
IL2CPP_EXTERN_C void CertificateHandler_t148B524FA5DB39F3ABADB181CD420FC505C33804_marshal_com_cleanup(CertificateHandler_t148B524FA5DB39F3ABADB181CD420FC505C33804_marshaled_com& marshaled)
{
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool CertificateHandler_ValidateCertificate_m81E194684391CA512FFC7C3A9172B56D6A1E2752 (CertificateHandler_t148B524FA5DB39F3ABADB181CD420FC505C33804* __this, ByteU5BU5D_tA6237BF417AE52AD70CFB4EF24A7A82613DF9031* ___certificateData0, const RuntimeMethod* method) 
{
	bool V_0 = false;
	{
		V_0 = (bool)0;
		goto IL_0005;
	}

IL_0005:
	{
		bool L_0 = V_0;
		return L_0;
	}
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool CertificateHandler_ValidateCertificateNative_m73467B32D9B40F6897951F71F80CDDA92F5FD785 (CertificateHandler_t148B524FA5DB39F3ABADB181CD420FC505C33804* __this, ByteU5BU5D_tA6237BF417AE52AD70CFB4EF24A7A82613DF9031* ___certificateData0, const RuntimeMethod* method) 
{
	bool V_0 = false;
	{
		ByteU5BU5D_tA6237BF417AE52AD70CFB4EF24A7A82613DF9031* L_0 = ___certificateData0;
		bool L_1;
		L_1 = VirtualFuncInvoker1< bool, ByteU5BU5D_tA6237BF417AE52AD70CFB4EF24A7A82613DF9031* >::Invoke(4, __this, L_0);
		V_0 = L_1;
		goto IL_000b;
	}

IL_000b:
	{
		bool L_2 = V_0;
		return L_2;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
IL2CPP_EXTERN_C void DownloadHandler_t1B56C7D3F65D97A1E4B566A14A1E783EA8AE4EBB_marshal_pinvoke(const DownloadHandler_t1B56C7D3F65D97A1E4B566A14A1E783EA8AE4EBB& unmarshaled, DownloadHandler_t1B56C7D3F65D97A1E4B566A14A1E783EA8AE4EBB_marshaled_pinvoke& marshaled)
{
	marshaled.___m_Ptr_0 = unmarshaled.___m_Ptr_0;
}
IL2CPP_EXTERN_C void DownloadHandler_t1B56C7D3F65D97A1E4B566A14A1E783EA8AE4EBB_marshal_pinvoke_back(const DownloadHandler_t1B56C7D3F65D97A1E4B566A14A1E783EA8AE4EBB_marshaled_pinvoke& marshaled, DownloadHandler_t1B56C7D3F65D97A1E4B566A14A1E783EA8AE4EBB& unmarshaled)
{
	intptr_t unmarshaledm_Ptr_temp_0;
	memset((&unmarshaledm_Ptr_temp_0), 0, sizeof(unmarshaledm_Ptr_temp_0));
	unmarshaledm_Ptr_temp_0 = marshaled.___m_Ptr_0;
	unmarshaled.___m_Ptr_0 = unmarshaledm_Ptr_temp_0;
}
IL2CPP_EXTERN_C void DownloadHandler_t1B56C7D3F65D97A1E4B566A14A1E783EA8AE4EBB_marshal_pinvoke_cleanup(DownloadHandler_t1B56C7D3F65D97A1E4B566A14A1E783EA8AE4EBB_marshaled_pinvoke& marshaled)
{
}
IL2CPP_EXTERN_C void DownloadHandler_t1B56C7D3F65D97A1E4B566A14A1E783EA8AE4EBB_marshal_com(const DownloadHandler_t1B56C7D3F65D97A1E4B566A14A1E783EA8AE4EBB& unmarshaled, DownloadHandler_t1B56C7D3F65D97A1E4B566A14A1E783EA8AE4EBB_marshaled_com& marshaled)
{
	marshaled.___m_Ptr_0 = unmarshaled.___m_Ptr_0;
}
IL2CPP_EXTERN_C void DownloadHandler_t1B56C7D3F65D97A1E4B566A14A1E783EA8AE4EBB_marshal_com_back(const DownloadHandler_t1B56C7D3F65D97A1E4B566A14A1E783EA8AE4EBB_marshaled_com& marshaled, DownloadHandler_t1B56C7D3F65D97A1E4B566A14A1E783EA8AE4EBB& unmarshaled)
{
	intptr_t unmarshaledm_Ptr_temp_0;
	memset((&unmarshaledm_Ptr_temp_0), 0, sizeof(unmarshaledm_Ptr_temp_0));
	unmarshaledm_Ptr_temp_0 = marshaled.___m_Ptr_0;
	unmarshaled.___m_Ptr_0 = unmarshaledm_Ptr_temp_0;
}
IL2CPP_EXTERN_C void DownloadHandler_t1B56C7D3F65D97A1E4B566A14A1E783EA8AE4EBB_marshal_com_cleanup(DownloadHandler_t1B56C7D3F65D97A1E4B566A14A1E783EA8AE4EBB_marshaled_com& marshaled)
{
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void DownloadHandler_ReceiveContentLengthHeader_m545073C4FB5AB9BB6640D24FC2F32070E5FE3F17 (DownloadHandler_t1B56C7D3F65D97A1E4B566A14A1E783EA8AE4EBB* __this, uint64_t ___contentLength0, const RuntimeMethod* method) 
{
	{
		uint64_t L_0 = ___contentLength0;
		VirtualActionInvoker1< int32_t >::Invoke(5, __this, ((int32_t)L_0));
		return;
	}
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void DownloadHandler_ReceiveContentLength_m058DE0B56C3EBE8ECE3F9570852024EE2A84DE4F (DownloadHandler_t1B56C7D3F65D97A1E4B566A14A1E783EA8AE4EBB* __this, int32_t ___contentLength0, const RuntimeMethod* method) 
{
	{
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
