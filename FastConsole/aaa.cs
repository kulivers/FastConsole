using System.Text;

namespace FastConsole;

public class CustomEncoder
	{
		public string UrlEncode(string str, Encoding e)
		{
			return str == null ? null : Encoding.ASCII.GetString(UrlEncodeToBytes(str, e));
		}

		public string UrlEncode(string str)
		{
			return UrlEncode(str, Encoding.UTF8);
		}

		public string UrlDecode(string value, Encoding encoding)
		{
			if (value == null)
			{
				return null;
			}

			int count = value.Length;
			UrlDecoder helper = new UrlDecoder(count, encoding);

			// go through the string's chars collapsing %XX and %uXXXX and
			// appending each char as char, with exception of %XX constructs
			// that are appended as bytes

			for (int pos = 0; pos < count; pos++)
			{
				char ch = value[pos];

				if (ch == '+')
				{
					ch = ' ';
				}
				else if (ch == '%' && pos < count - 2)
				{
					if (value[pos + 1] == 'u' && pos < count - 5)
					{
						int h1 = FromChar(value[pos + 2]);
						int h2 = FromChar(value[pos + 3]);
						int h3 = FromChar(value[pos + 4]);
						int h4 = FromChar(value[pos + 5]);

						if ((h1 | h2 | h3 | h4) != 0xFF)
						{
							// valid 4 hex chars
							ch = (char)((h1 << 12) | (h2 << 8) | (h3 << 4) | h4);
							pos += 5;

							// only add as char
							helper.AddChar(ch);
							continue;
						}
					}
					else
					{
						int h1 = FromChar(value[pos + 1]);
						int h2 = FromChar(value[pos + 2]);

						if ((h1 | h2) != 0xFF)
						{
							// valid 2 hex chars
							byte b = (byte)((h1 << 4) | h2);
							pos += 2;

							// don't add as char
							helper.AddByte(b);
							continue;
						}
					}
				}

				if ((ch & 0xFF80) == 0)
				{
					helper.AddByte((byte)ch); // 7 bit have to go as bytes because of Unicode
				}
				else
				{
					helper.AddChar(ch);
				}
			}

			return ValidateString(helper.GetString());
		}

		private static string ValidateString(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return input;
			}
			// locate the first surrogate character
			int idxOfFirstSurrogate = -1;
			for (int i = 0; i < input.Length; i++)
			{
				if (char.IsSurrogate(input[i]))
				{
					idxOfFirstSurrogate = i;
					break;
				}
			}

			// fast case: no surrogates = return input string
			if (idxOfFirstSurrogate < 0)
			{
				return input;
			}

			var chars = input.ToCharArray();
			for (int i = idxOfFirstSurrogate; i < chars.Length; i++)
			{
				char thisChar = chars[i];

				// If this character is a low surrogate, then it was not preceded by
				// a high surrogate, so we'll replace it.
				if (char.IsLowSurrogate(thisChar))
				{
					chars[i] = UnicodeReplacementChar;
					continue;
				}

				if (char.IsHighSurrogate(thisChar))
				{
					// If this character is a high surrogate and it is followed by a
					// low surrogate, allow both to remain.
					if (i + 1 < chars.Length && char.IsLowSurrogate(chars[i + 1]))
					{
						i++; // skip the low surrogate also
						continue;
					}

					// If this character is a high surrogate and it is not followed
					// by a low surrogate, replace it.
					chars[i] = UnicodeReplacementChar;
					continue;
				}

				// Otherwise, this is a non-surrogate character and just move to the
				// next character.
			}

			return string.Concat(chars);;
		}

		private const char UnicodeReplacementChar = '\uFFFD';
		public static int FromChar(int c)
		{
			return c >= CharToHexLookup.Length ? 0xFF : CharToHexLookup[c];
		}

		private static byte[] CharToHexLookup => new byte[]
		{
			0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 15
			0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 31
			0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 47
			0x0,  0x1,  0x2,  0x3,  0x4,  0x5,  0x6,  0x7,  0x8,  0x9,  0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 63
			0xFF, 0xA,  0xB,  0xC,  0xD,  0xE,  0xF,  0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 79
			0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 95
			0xFF, 0xa,  0xb,  0xc,  0xd,  0xe,  0xf,  0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 111
			0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 127
			0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 143
			0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 159
			0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 175
			0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 191
			0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 207
			0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 223
			0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 239
			0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF  // 255
		};
		private static char ToCharLower(int value)
		{
			value &= 0xF;
			value += '0';

			if (value > '9')
			{
				value += ('a' - ('9' + 1));
			}

			return (char)value;
		}

		private static byte[] UrlEncodeToBytes(string str, Encoding e)
		{
			if (str == null)
			{
				return null;
			}

			byte[] bytes = e.GetBytes(str);
			return UrlEncode(bytes, 0, bytes.Length, alwaysCreateNewReturnValue: false);
		}

		private static byte[] UrlEncode(byte[] bytes, int offset, int count, bool alwaysCreateNewReturnValue)
		{
			byte[] encoded = UrlEncode(bytes, offset, count);

			return (alwaysCreateNewReturnValue && (encoded != null) && (encoded == bytes))
				? (byte[])encoded.Clone()
				: encoded;
		}

		private static bool IsUrlSafeChar(char ch)
		{
			if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || (ch >= '0' && ch <= '9'))
			{
				return true;
			}

			switch (ch)
			{
				case '-':
				case '_':
				case '.':
				case '!':
				case '*':
				case '(':
				case ')':
					return true;
			}

			return false;
		}

		private static byte[] UrlEncode(byte[] bytes, int offset, int count)
		{
			if (!ValidateUrlEncodingParameters(bytes, offset, count))
			{
				return null;
			}

			int cSpaces = 0;
			int cUnsafe = 0;

			// count them first
			for (int i = 0; i < count; i++)
			{
				char ch = (char)bytes[offset + i];

				if (ch == ' ')
				{
					cSpaces++;
				}
				else if (!IsUrlSafeChar(ch))
				{
					cUnsafe++;
				}
			}

			// nothing to expand?
			if (cSpaces == 0 && cUnsafe == 0)
			{
				// DevDiv 912606: respect "offset" and "count"
				if (0 == offset && bytes.Length == count)
				{
					return bytes;
				}
				else
				{
					byte[] subarray = new byte[count];
					Buffer.BlockCopy(bytes, offset, subarray, 0, count);
					return subarray;
				}
			}

			// expand not 'safe' characters into %XX, spaces to +s
			byte[] expandedBytes = new byte[count + cUnsafe * 2];
			int pos = 0;

			for (int i = 0; i < count; i++)
			{
				byte b = bytes[offset + i];
				char ch = (char)b;

				if (IsUrlSafeChar(ch))
				{
					expandedBytes[pos++] = b;
				}
				else if (ch == ' ')
				{
					expandedBytes[pos++] = (byte)'+';
				}
				else
				{
					expandedBytes[pos++] = (byte)'%';
					expandedBytes[pos++] = (byte)ToCharLower(b >> 4);
					expandedBytes[pos++] = (byte)ToCharLower(b);
				}
			}

			return expandedBytes;
		}

		private static bool ValidateUrlEncodingParameters(byte[] bytes, int offset, int count)
		{
			if (bytes == null && count == 0)
			{
				return false;
			}

			if (bytes == null)
			{
				throw new ArgumentNullException(nameof(bytes));
			}

			if (offset < 0 || offset > bytes.Length)
			{
				throw new ArgumentOutOfRangeException(nameof(offset));
			}

			if (count < 0 || offset + count > bytes.Length)
			{
				throw new ArgumentOutOfRangeException(nameof(count));
			}

			return true;
		}
	}
    sealed class UrlDecoder
    {
	    private readonly int _bufferSize;

	    // Accumulate characters in a special array
	    private int _numChars;
	    private readonly char[] _charBuffer;

	    // Accumulate bytes for decoding into characters in a special array
	    private int _numBytes;
	    private byte[] _byteBuffer;

	    // Encoding to convert chars to bytes
	    private readonly Encoding _encoding;

	    private void FlushBytes()
	    {
		    if (_numBytes > 0)
		    {
			    _numChars += _encoding.GetChars(_byteBuffer, 0, _numBytes, _charBuffer, _numChars);
			    _numBytes = 0;
		    }
	    }

	    internal UrlDecoder(int bufferSize, Encoding encoding)
	    {
		    _bufferSize = bufferSize;
		    _encoding = encoding;

		    _charBuffer = new char[bufferSize];
		    // byte buffer created on demand
	    }

	    internal void AddChar(char ch)
	    {
		    if (_numBytes > 0)
		    {
			    FlushBytes();
		    }

		    _charBuffer[_numChars++] = ch;
	    }

	    internal void AddByte(byte b)
	    {
		    // if there are no pending bytes treat 7 bit bytes as characters
		    // this optimization is temp disable as it doesn't work for some encodings
		    /*
		                    if (_numBytes == 0 && ((b & 0x80) == 0)) {
		                        AddChar((char)b);
		                    }
		                    else
		    */
		    {
			    if (_byteBuffer == null)
			    {
				    _byteBuffer = new byte[_bufferSize];
			    }

			    _byteBuffer[_numBytes++] = b;
		    }
	    }

	    internal string GetString()
	    {
		    if (_numBytes > 0)
		    {
			    FlushBytes();
		    }

		    return _numChars > 0 ? new string(_charBuffer, 0, _numChars) : "";
	    }
    }