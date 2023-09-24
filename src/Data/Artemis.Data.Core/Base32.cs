// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Security.Cryptography;
using System.Text;

namespace Artemis.Data.Core;

// See http://tools.ietf.org/html/rfc3548#section-5

/// <summary>
/// Base32 encoding/decoding
/// </summary>
public static class Base32
{
    /// <summary>
    /// 若参数为空则抛出异常
    /// </summary>
    /// <param name="argument"></param>
    /// <param name="paramName"></param>
    /// <exception cref="ArgumentNullException"></exception>
    private static void ThrowIfNull(object? argument, string? paramName = null)
    {
        if (argument is null)
        {
            throw new ArgumentNullException(paramName);
        }
    }

    private const string Base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

    /// <summary>
    /// 生成base32串
    /// </summary>
    /// <returns></returns>
    public static string GenerateBase32()
    {
        const int length = 20;
        // base32 takes 5 bytes and converts them into 8 characters, which would be (byte length / 5) * 8
        // except that it also pads ('=') for the last processed chunk if it's less than 5 bytes.
        // So in order to handle the padding we add 1 less than the chunk size to our byte length
        // which will either be removed due to integer division truncation if the length was already a multiple of 5
        // or it will increase the divided length by 1 meaning that a 1-4 byte length chunk will be 1 instead of 0
        // so the padding is now included in our string length calculation
        return string.Create(((length + 4) / 5) * 8, 0, static (buffer, _) =>
        {
            Span<byte> bytes = stackalloc byte[length];
            RandomNumberGenerator.Fill(bytes);

            var index = 0;
            for (var offset = 0; offset < bytes.Length;)
            {
                var numCharsToOutput = GetNextGroup(bytes, ref offset, out var a, out var b, out var c, out var d, out var e, out var f, out var g, out var h);

                buffer[index + 7] = ((numCharsToOutput >= 8) ? Base32Chars[h] : '=');
                buffer[index + 6] = ((numCharsToOutput >= 7) ? Base32Chars[g] : '=');
                buffer[index + 5] = ((numCharsToOutput >= 6) ? Base32Chars[f] : '=');
                buffer[index + 4] = ((numCharsToOutput >= 5) ? Base32Chars[e] : '=');
                buffer[index + 3] = ((numCharsToOutput >= 4) ? Base32Chars[d] : '=');
                buffer[index + 2] = (numCharsToOutput >= 3) ? Base32Chars[c] : '=';
                buffer[index + 1] = (numCharsToOutput >= 2) ? Base32Chars[b] : '=';
                buffer[index] = (numCharsToOutput >= 1) ? Base32Chars[a] : '=';
                index += 8;
            }
        });
    }

    /// <summary>
    /// 转为Base32
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string ToBase32(byte[] input)
    {
        ThrowIfNull(input);

        var sb = new StringBuilder();
        for (var offset = 0; offset < input.Length;)
        {
            var numCharsToOutput = GetNextGroup(input, ref offset, out var a, out var b, out var c, out var d, out var e, out var f, out var g, out var h);

            sb.Append((numCharsToOutput >= 1) ? Base32Chars[a] : '=');
            sb.Append((numCharsToOutput >= 2) ? Base32Chars[b] : '=');
            sb.Append((numCharsToOutput >= 3) ? Base32Chars[c] : '=');
            sb.Append((numCharsToOutput >= 4) ? Base32Chars[d] : '=');
            sb.Append((numCharsToOutput >= 5) ? Base32Chars[e] : '=');
            sb.Append((numCharsToOutput >= 6) ? Base32Chars[f] : '=');
            sb.Append((numCharsToOutput >= 7) ? Base32Chars[g] : '=');
            sb.Append((numCharsToOutput >= 8) ? Base32Chars[h] : '=');
        }

        return sb.ToString();
    }

    /// <summary>
    /// 解析Base32
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    public static byte[] FromBase32(string input)
    {
        ThrowIfNull(input);

        var trimmedInput = input.AsSpan().TrimEnd('=');
        if (trimmedInput.Length == 0)
        {
            return Array.Empty<byte>();
        }

        var output = new byte[trimmedInput.Length * 5 / 8];
        var bitIndex = 0;
        var inputIndex = 0;
        var outputBits = 0;
        var outputIndex = 0;
        while (outputIndex < output.Length)
        {
            var byteIndex = Base32Chars.IndexOf(char.ToUpperInvariant(trimmedInput[inputIndex]));
            if (byteIndex < 0)
            {
                throw new FormatException();
            }

            var bits = Math.Min(5 - bitIndex, 8 - outputBits);
            output[outputIndex] <<= bits;
            output[outputIndex] |= (byte)(byteIndex >> (5 - (bitIndex + bits)));

            bitIndex += bits;
            if (bitIndex >= 5)
            {
                inputIndex++;
                bitIndex = 0;
            }

            outputBits += bits;
            if (outputBits < 8) 
                continue;
            outputIndex++;
            outputBits = 0;
        }
        return output;
    }

    // returns the number of bytes that were output
    private static int GetNextGroup(Span<byte> input, ref int offset, out byte a, out byte b, out byte c, out byte d, out byte e, out byte f, out byte g, out byte h)
    {
        var retVal = (input.Length - offset) switch
        {
            1 => 2,
            2 => 4,
            3 => 5,
            4 => 7,
            _ => 8
        };

        var b1 = (offset < input.Length) ? input[offset++] : 0U;
        var b2 = (offset < input.Length) ? input[offset++] : 0U;
        var b3 = (offset < input.Length) ? input[offset++] : 0U;
        var b4 = (offset < input.Length) ? input[offset++] : 0U;
        var b5 = (offset < input.Length) ? input[offset++] : 0U;

        a = (byte)(b1 >> 3);
        b = (byte)(((b1 & 0x07) << 2) | (b2 >> 6));
        c = (byte)((b2 >> 1) & 0x1f);
        d = (byte)(((b2 & 0x01) << 4) | (b3 >> 4));
        e = (byte)(((b3 & 0x0f) << 1) | (b4 >> 7));
        f = (byte)((b4 >> 2) & 0x1f);
        g = (byte)(((b4 & 0x3) << 3) | (b5 >> 5));
        h = (byte)(b5 & 0x1f);

        return retVal;
    }
}
