﻿using System;
using System.Text;

namespace Pokemon3D.GameJolt
{
    // This replaces the System.Web.HttpUtility class's UrlEncode method.
    // The .Net framework does not by default contain the System.Web.dll, so instead of importing the whole dll and having people miss this file and ultimately being unable to run this,
    // we implement this function ourselves.
    // All credit goes to Microsoft. Source code directly taken from (and slightly modified) http://referencesource.microsoft.com/#System.Web/httpserverutility.cs,4729f9204762c393.
   
    internal class UrlEncoder
    {
        /// <summary>
        /// Encodes unsafe chars in an Url.
        /// </summary>
        public static string Encode(string s)
        {
            if (s == null)
                return null;

            return Encoding.ASCII.GetString(EncodeToBytes(s, Encoding.UTF8));
        }

        // source taken from: http://referencesource.microsoft.com/#System.Web/Util/HttpEncoder.cs,aa1079312bd28b8c
        private static byte[] EncodeToBytes(string s, Encoding e)
        {
            var bytes = e.GetBytes(s);
            var count = bytes.Length;
            
            var cSpaces = 0;
            var cUnsafe = 0;

            // count them first
            for (var i = 0; i < count; i++)
            {
                var ch = (char)bytes[i];

                if (ch == ' ')
                    cSpaces++;
                else if (!IsUrlSafeChar(ch))
                    cUnsafe++;
            }

            // nothing to expand?
            if (cSpaces == 0 && cUnsafe == 0)
            {
                // DevDiv 912606: respect "offset" and "count"
                if (bytes.Length == count)
                {
                    return bytes;
                }
                var subarray = new byte[count];
                Buffer.BlockCopy(bytes, 0, subarray, 0, count);
                return subarray;
            }

            // expand not 'safe' characters into %XX, spaces to +s
            byte[] expandedBytes = new byte[count + cUnsafe * 2];
            int pos = 0;

            for (int i = 0; i < count; i++)
            {
                byte b = bytes[i];
                char ch = (char)b;

                if (IsUrlSafeChar(ch))
                {
                    expandedBytes[pos++] = b;
                }
                else if (ch == ' ')
                {
                    expandedBytes[pos++] = (byte)'+';
                }
                else {
                    expandedBytes[pos++] = (byte)'%';
                    expandedBytes[pos++] = (byte)IntToHex((b >> 4) & 0xf);
                    expandedBytes[pos++] = (byte)IntToHex(b & 0x0f);
                }
            }

            return expandedBytes;
        }

        // source taken from: http://referencesource.microsoft.com/#System.Web/Util/HttpEncoderUtility.cs,f087b72f303de761
        private static bool IsUrlSafeChar(char ch)
        {
            if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || (ch >= '0' && ch <= '9'))
                return true;

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

        // source taken from: http://referencesource.microsoft.com/#System.Web/Util/HttpEncoderUtility.cs,3a94b289b95ec9f2
        private static char IntToHex(int n)
        {
            return n <= 9 ? (char) (n + '0') : (char) (n - 10 + 'a');
        }
    }
}
