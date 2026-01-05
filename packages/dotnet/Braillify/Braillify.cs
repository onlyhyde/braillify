namespace Braillify;

using System;
using System.Runtime.InteropServices;
#if !NET6_0_OR_GREATER
using System.Text;
#endif

/// <summary>
/// 한국어 텍스트를 점자로 변환하는 라이브러리입니다.
/// Library for converting Korean text to braille.
/// </summary>
public static class Braillify
{
#if NETCOREAPP3_0_OR_GREATER
    static Braillify()
    {
        NativeLibraryLoader.EnsureLoaded();
    }
#endif

    /// <summary>
    /// 텍스트를 점자 바이트 배열로 인코딩합니다.
    /// Encodes text to braille byte array.
    /// </summary>
    /// <param name="text">변환할 텍스트 / Text to convert</param>
    /// <returns>점자 바이트 배열 / Braille byte array</returns>
    /// <exception cref="ArgumentNullException">텍스트가 null인 경우 / When text is null</exception>
    /// <exception cref="BraillifyException">인코딩 실패 시 / When encoding fails</exception>
    public static byte[] Encode(string text)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(text);
#else
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }
#endif

#if NET5_0_OR_GREATER
        nint resultPtr = NativeMethods.braillify_encode(text, out nuint length);

        if (resultPtr == 0)
        {
            ThrowLastError();
        }

        try
        {
            var result = new byte[(int)length];
            Marshal.Copy(resultPtr, result, 0, (int)length);
            return result;
        }
        finally
        {
            NativeMethods.braillify_free_bytes(resultPtr, length);
        }
#elif NETCOREAPP3_0_OR_GREATER
        IntPtr resultPtr = NativeMethods.braillify_encode(text, out UIntPtr length);

        if (resultPtr == IntPtr.Zero)
        {
            ThrowLastError();
        }

        try
        {
            var len = (int)length.ToUInt32();
            var result = new byte[len];
            Marshal.Copy(resultPtr, result, 0, len);
            return result;
        }
        finally
        {
            NativeMethods.braillify_free_bytes(resultPtr, length);
        }
#else
        // .NET Standard 2.0: 수동 UTF-8 마샬링
        // .NET Standard 2.0: Manual UTF-8 marshalling
        IntPtr textPtr = StringToUtf8Ptr(text);
        try
        {
            IntPtr resultPtr = NativeMethods.braillify_encode(textPtr, out UIntPtr length);

            if (resultPtr == IntPtr.Zero)
            {
                ThrowLastError();
            }

            try
            {
                var len = (int)length.ToUInt32();
                var result = new byte[len];
                Marshal.Copy(resultPtr, result, 0, len);
                return result;
            }
            finally
            {
                NativeMethods.braillify_free_bytes(resultPtr, length);
            }
        }
        finally
        {
            Marshal.FreeHGlobal(textPtr);
        }
#endif
    }

    /// <summary>
    /// 텍스트를 점자 유니코드 문자열로 인코딩합니다.
    /// Encodes text to braille unicode string.
    /// </summary>
    /// <param name="text">변환할 텍스트 / Text to convert</param>
    /// <returns>점자 유니코드 문자열 / Braille unicode string</returns>
    /// <exception cref="ArgumentNullException">텍스트가 null인 경우 / When text is null</exception>
    /// <exception cref="BraillifyException">인코딩 실패 시 / When encoding fails</exception>
    public static string EncodeToUnicode(string text)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(text);
#else
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }
#endif

#if NET5_0_OR_GREATER
        nint resultPtr = NativeMethods.braillify_encode_to_unicode(text);

        if (resultPtr == 0)
        {
            ThrowLastError();
        }

        try
        {
#if NET6_0_OR_GREATER
            return Marshal.PtrToStringUTF8(resultPtr) ?? string.Empty;
#else
            return PtrToStringUtf8(resultPtr);
#endif
        }
        finally
        {
            NativeMethods.braillify_free_string(resultPtr);
        }
#elif NETCOREAPP3_0_OR_GREATER
        IntPtr resultPtr = NativeMethods.braillify_encode_to_unicode(text);

        if (resultPtr == IntPtr.Zero)
        {
            ThrowLastError();
        }

        try
        {
            return PtrToStringUtf8(resultPtr);
        }
        finally
        {
            NativeMethods.braillify_free_string(resultPtr);
        }
#else
        // .NET Standard 2.0: 수동 UTF-8 마샬링
        // .NET Standard 2.0: Manual UTF-8 marshalling
        IntPtr textPtr = StringToUtf8Ptr(text);
        try
        {
            IntPtr resultPtr = NativeMethods.braillify_encode_to_unicode(textPtr);

            if (resultPtr == IntPtr.Zero)
            {
                ThrowLastError();
            }

            try
            {
                return PtrToStringUtf8(resultPtr);
            }
            finally
            {
                NativeMethods.braillify_free_string(resultPtr);
            }
        }
        finally
        {
            Marshal.FreeHGlobal(textPtr);
        }
#endif
    }

    /// <summary>
    /// 텍스트를 점자 폰트 문자열로 인코딩합니다.
    /// Encodes text to braille font string.
    /// </summary>
    /// <param name="text">변환할 텍스트 / Text to convert</param>
    /// <returns>점자 폰트 문자열 / Braille font string</returns>
    /// <exception cref="ArgumentNullException">텍스트가 null인 경우 / When text is null</exception>
    /// <exception cref="BraillifyException">인코딩 실패 시 / When encoding fails</exception>
    public static string EncodeToBrailleFont(string text)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(text);
#else
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }
#endif

#if NET5_0_OR_GREATER
        nint resultPtr = NativeMethods.braillify_encode_to_braille_font(text);

        if (resultPtr == 0)
        {
            ThrowLastError();
        }

        try
        {
#if NET6_0_OR_GREATER
            return Marshal.PtrToStringUTF8(resultPtr) ?? string.Empty;
#else
            return PtrToStringUtf8(resultPtr);
#endif
        }
        finally
        {
            NativeMethods.braillify_free_string(resultPtr);
        }
#elif NETCOREAPP3_0_OR_GREATER
        IntPtr resultPtr = NativeMethods.braillify_encode_to_braille_font(text);

        if (resultPtr == IntPtr.Zero)
        {
            ThrowLastError();
        }

        try
        {
            return PtrToStringUtf8(resultPtr);
        }
        finally
        {
            NativeMethods.braillify_free_string(resultPtr);
        }
#else
        // .NET Standard 2.0: 수동 UTF-8 마샬링
        // .NET Standard 2.0: Manual UTF-8 marshalling
        IntPtr textPtr = StringToUtf8Ptr(text);
        try
        {
            IntPtr resultPtr = NativeMethods.braillify_encode_to_braille_font(textPtr);

            if (resultPtr == IntPtr.Zero)
            {
                ThrowLastError();
            }

            try
            {
                return PtrToStringUtf8(resultPtr);
            }
            finally
            {
                NativeMethods.braillify_free_string(resultPtr);
            }
        }
        finally
        {
            Marshal.FreeHGlobal(textPtr);
        }
#endif
    }

    private static void ThrowLastError()
    {
#if NET5_0_OR_GREATER
        nint errorPtr = NativeMethods.braillify_get_last_error();

        if (errorPtr == 0)
        {
            throw new BraillifyException("알 수 없는 오류가 발생했습니다. / Unknown error occurred.");
        }

        try
        {
#if NET6_0_OR_GREATER
            var errorMessage = Marshal.PtrToStringUTF8(errorPtr);
#else
            var errorMessage = PtrToStringUtf8(errorPtr);
#endif
            throw new BraillifyException(errorMessage ?? "알 수 없는 오류 / Unknown error");
        }
        finally
        {
            NativeMethods.braillify_free_string(errorPtr);
        }
#else
        IntPtr errorPtr = NativeMethods.braillify_get_last_error();

        if (errorPtr == IntPtr.Zero)
        {
            throw new BraillifyException("알 수 없는 오류가 발생했습니다. / Unknown error occurred.");
        }

        try
        {
            var errorMessage = PtrToStringUtf8(errorPtr);
            throw new BraillifyException(errorMessage);
        }
        finally
        {
            NativeMethods.braillify_free_string(errorPtr);
        }
#endif
    }

#if !NET6_0_OR_GREATER
    private static string PtrToStringUtf8(
#if NET5_0_OR_GREATER
        nint ptr
#else
        IntPtr ptr
#endif
    )
    {
#if NET5_0_OR_GREATER
        if (ptr == 0)
#else
        if (ptr == IntPtr.Zero)
#endif
        {
            return string.Empty;
        }

        // UTF-8 문자열 길이 계산
        // Calculate UTF-8 string length
        var len = 0;
        while (Marshal.ReadByte(ptr, len) != 0)
        {
            len++;
        }

        if (len == 0)
        {
            return string.Empty;
        }

        var buffer = new byte[len];
        Marshal.Copy(ptr, buffer, 0, len);
        return Encoding.UTF8.GetString(buffer);
    }
#endif

#if !NETCOREAPP3_0_OR_GREATER
    private static IntPtr StringToUtf8Ptr(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            // 빈 문자열의 경우 null-terminator만 포함된 버퍼 반환
            // For empty strings, return a buffer with only null-terminator
            IntPtr emptyPtr = Marshal.AllocHGlobal(1);
            Marshal.WriteByte(emptyPtr, 0);
            return emptyPtr;
        }

        byte[] utf8Bytes = Encoding.UTF8.GetBytes(text);
        IntPtr ptr = Marshal.AllocHGlobal(utf8Bytes.Length + 1);
        Marshal.Copy(utf8Bytes, 0, ptr, utf8Bytes.Length);
        Marshal.WriteByte(ptr, utf8Bytes.Length, 0); // null-terminator
        return ptr;
    }
#endif
}
