using System.Text;

namespace WebAPI.Utilities
{
    public static class Safe64
    {
        static readonly char[] s_base64;
        static readonly int[] s_base64dec;

        static Safe64()
        {
            s_base64 = new char[64];
            s_base64dec = new int[256];
            for (var idx = 0; idx < 256; idx++) s_base64dec[idx] = -1; //Invalid char
            char c;
            int i = 0;
            for (c = 'A'; c <= 'Z'; c++, i++) { s_base64[i] = c; s_base64dec[(int)c] = i; }
            for (c = 'a'; c <= 'z'; c++, i++) { s_base64[i] = c; s_base64dec[(int)c] = i; }
            for (c = '0'; c <= '9'; c++, i++) { s_base64[i] = c; s_base64dec[(int)c] = i; }
            s_base64[i] = '-';
            s_base64dec[(int)'-'] = i;
            s_base64dec[(int)'+'] = i++; //For compatability with 'normal' base64
            s_base64[i] = '_';
            s_base64dec[(int)'_'] = i;
            s_base64dec[(int)'/'] = i++; //For compatability with 'normal' base64
        }

        /// <summary>
        /// Encodes a string of binary data using the URI-Safe Base64 alphabet with no padding.
        /// </summary>
        /// <param name="data">The binary data to encode. If null or zero-length, an empty string is returned.</param>
        /// <returns>The base64 encoded string</returns>
        public static string Encode(byte[] data)
        {
            if (data == null || data.Length == 0) return "";
            var sb = new StringBuilder(4 * ((data.Length / 3) + 1));

            int b1, b2, b3;
            char[] chars = new char[4];
            int len = data.Length;
            int charCount = 4;

            for (var i = 0; i < len; i += 3)
            {
                b1 = data[i];
                if (i + 1 >= len) { b2 = 0; charCount--; } else { b2 = data[i + 1]; }
                if (i + 2 >= len) { b3 = 0; charCount--; } else { b3 = data[i + 2]; }

                chars[0] = s_base64[(b1 & 0xfc) >> 2];
                chars[1] = s_base64[((b1 & 0x03) << 4) | ((b2 & 0xf0) >> 4)];
                chars[2] = s_base64[((b2 & 0x0f) << 2) | ((b3 & 0xc0) >> 6)];
                chars[3] = s_base64[b3 & 0x3f];

                sb.Append(chars, 0, charCount);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Decodes regular or URI-safe Base64 encoded strings. Tolerant of whitespace, line breaks. Supports optional padding characters.
        /// </summary>
        /// <param name="encodedStr">The encoded string. If null or zero-length, a zero-length byte array is returned.</param>
        /// <param name="nullOnError">If true, returns a null result instead of throwing a FormatException. Defaults to false.</param>
        /// <returns>The decoded binary data.</returns>
        /// <exception cref="FormatException">Thrown if invalid data is encountered in the input string.</exception>
        public static byte[] Decode(string encodedStr, bool nullOnError = false)
        {
            if (encodedStr == null || encodedStr.Length == 0) return new byte[0];

            var len = 0;
            var trimPad = false;
            for (var i = 0; i < encodedStr.Length; i++)
            {
                var ch = encodedStr[i];
                if (char.IsWhiteSpace(ch) || ch == 10 || ch == 13) continue;
                if (ch == '=') { trimPad = true; continue; }
                if (ch > 255 || trimPad) { if (nullOnError) return null; else throw new FormatException("Invalid data encountered in string"); }
                len++;
            }

            var remainder = len % 4;
            var byteLen = 3 * (len / 4);
            if (remainder == 1) { if (nullOnError) return null; else throw new FormatException("Invalid data encountered in string"); }
            else if (remainder == 2) byteLen += 1;
            else if (remainder == 3) byteLen += 2;
            var bytes = new byte[byteLen];

            int strIdx = 0;
            int b1, b2, b3;
            int c1, c2, c3, c4;
            int byteIdx = 0;
            var charCount = 0;

            for (var i = 0; i < len; i += 4)
            {
                c1 = c2 = -1;
                c3 = c4 = 0;

                for (charCount = 0; strIdx < encodedStr.Length && charCount < 4; strIdx++)
                {
                    var ch = encodedStr[strIdx];
                    if (char.IsWhiteSpace(ch) || ch == 10 || ch == 13 || ch == '=') continue;

                    charCount++;
                    if (charCount == 1) c1 = s_base64dec[ch];
                    else if (charCount == 2) c2 = s_base64dec[ch];
                    else if (charCount == 3) c3 = s_base64dec[ch];
                    else if (charCount == 4) c4 = s_base64dec[ch];
                }

                if (c1 == -1 || c2 == -1 || c3 == -1 || c4 == -1) { if (nullOnError) return null; else throw new FormatException("Invalid data encountered in string"); }

                b1 = (c1 << 2) | ((c2 & 0x30) >> 4);
                b2 = ((c2 & 0x0f) << 4) | ((c3 & 0x3c) >> 2);
                b3 = ((c3 & 0x03) << 6) | c4;

                bytes[byteIdx++] = (byte)b1;
                if (charCount > 2) bytes[byteIdx++] = (byte)b2;
                if (charCount == 4) bytes[byteIdx++] = (byte)b3;
            }

            return bytes;
        }
    }
}
