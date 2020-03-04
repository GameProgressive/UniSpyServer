using GameSpyLib.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameSpyLib.Encryption
{
    /// <summary>
    /// This class provides an implementation of Encryption type X used in server browser with port 28910.
    /// 
    /// This implementation was converted from Luigi Auriemma's enctypex decoder in C++ to C#
    /// </summary>
    /*
       Copy of the original aluigi program header as follows:
      ------------------------------------------------------

        GS enctypeX servers list decoder/encoder 0.1.3b
        by Luigi Auriemma
        e-mail: aluigi @autistici.org
        web:    aluigi.org

        This is the algorithm used by ANY new and old game which contacts the Gamespy master server.
        It has been written for being used in gslist so there are no explanations or comments here,
        if you want to understand something take a look to gslist.c

            Copyright 2008-2012 Luigi Auriemma

            This program is free software; you can redistribute it and/or modify
            it under the terms of the GNU General Public License as published by
            the Free Software Foundation; either version 2 of the License, or
            (at your option) any later version.

            This program is distributed in the hope that it will be useful,
            but WITHOUT ANY WARRANTY; without even the implied warranty of
            MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
            GNU General Public License for more details.

            You should have received a copy of the GNU General Public License
            along with this program; if not, write to the Free Software
            Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307 USA

            http://www.gnu.org/licenses/gpl-2.0.txt
    */

    public class EnctypeX
    {
        private byte[] _encxkey = new byte[261]; // Static key
        private int _offset = 0; // everything decrypted till now (total)
        private byte[] _clientChallenge = new byte[8];

        public byte[] EncryptData(byte[] secretkey, byte[] clientChallenge, byte[] data, short backendflag)
        {
            List<byte> result = new List<byte>();

            _encxkey.Initialize();
            _clientChallenge.Initialize();

            byte[] secretServerKey = Encoding.ASCII.GetBytes(GameSpyRandom.GenerateRandomString(20, GameSpyRandom.StringType.AlphaNumeric));

            Array.Copy(clientChallenge, 0, _clientChallenge, 0, 8);

            Funcx(secretkey, secretServerKey); // challenge computation

            result.Add(2 ^ 0xEC); // data length

            result.AddRange(BitConverter.GetBytes(backendflag));

            result.Add((byte)(secretServerKey.Length ^ 0xEA)); // secret server key length
            result.AddRange(secretServerKey);

            // Start of encrypted data
            _offset = 0;
            byte[] encData = Encode(data);
            result.AddRange(encData);

            return result.ToArray();
        }

        public List<byte> DecryptData(byte[] secretkey, string gamename, ref byte[] data)
        {
            List<byte> result = new List<byte>();

            _encxkey.Initialize();
            _clientChallenge.Initialize();

            int a, b;

            if (data.Length < 1)
                return result;

            a = (data[0] ^ 0xec) + 2;

            if (data.Length < a)
                return result;

            b = data[a - 1] ^ 0xea;
            if (data.Length < (a + b))
                return result;

            byte[] dataToFunc = new byte[b];
            Array.Copy(data, a, dataToFunc, 0, b);

            for (int i = 0; i < 8; i++)
            {
                if ((i + 1) > gamename.Length)
                    _clientChallenge[i] = 0;
                else
                    _clientChallenge[i] = (byte)gamename[i];
            }

            Funcx(secretkey, dataToFunc); // challenge computation
            a += b;

            _offset = a;

            result.AddRange(Decode(data));
            return result;
        }

        private int Func5(int cnt, byte[] id, ref int n1, ref int n2)
        {
            int i, tmp, mask = 1;

            if (cnt == 0)
                return 0;

            if (cnt > 1)
            {
                do
                {
                    mask = (mask << 1) + 1;
                } while (mask < cnt);
            }

            i = 0;
            do
            {
                n1 = _encxkey[n1 & 0xff] + id[n2];
                n2++;
                if (n2 >= id.Length)
                {
                    n2 = 0;
                    n1 += id.Length;
                }
                tmp = n1 & mask;
                if (++i > 11) tmp %= cnt;
            } while (tmp > cnt);

            return tmp;
        }

        private void Func4(byte[] id)
        {
            int i, n1 = 0, n2 = 0;
            byte t1, t2;

            if (id.Length < 1)
                return;

            for (i = 0; i < 256; i++)
                _encxkey[i] = (byte)i;

            for (i = 255; i >= 0; i--)
            {
                t1 = (byte)Func5(i, id, ref n1, ref n2);
                t2 = _encxkey[i];
                _encxkey[i] = _encxkey[t1];
                _encxkey[t1] = t2;
            }

            _encxkey[256] = _encxkey[1];
            _encxkey[257] = _encxkey[3];
            _encxkey[258] = _encxkey[5];
            _encxkey[259] = _encxkey[7];
            _encxkey[260] = _encxkey[n1 & 0xff];
        }

        private int Func7(byte d)
        {
            byte a, b, c;

            a = _encxkey[256];
            b = _encxkey[257];
            c = _encxkey[a];
            _encxkey[256] = (byte)(a + 1);
            _encxkey[257] = (byte)(b + c);
            a = _encxkey[260];
            b = _encxkey[257];
            b = _encxkey[b];
            c = _encxkey[a];
            _encxkey[a] = b;
            a = _encxkey[259];
            b = _encxkey[257];
            a = _encxkey[a];
            _encxkey[b] = a;
            a = _encxkey[256];
            b = _encxkey[259];
            a = _encxkey[a];
            _encxkey[b] = a;
            a = _encxkey[256];
            _encxkey[a] = c;
            b = _encxkey[258];
            a = _encxkey[c];
            c = _encxkey[259];
            b += a;
            _encxkey[258] = b;
            a = b;
            c = _encxkey[c];
            b = _encxkey[257];
            b = _encxkey[b];
            a = _encxkey[a];
            c += b;
            b = _encxkey[260];
            b = _encxkey[b];
            c += b;
            b = _encxkey[c];
            c = _encxkey[256];
            c = _encxkey[c];
            a += c;
            c = _encxkey[b];
            b = _encxkey[a];
            _encxkey[260] = d;
            c ^= (byte)(b ^ d);
            _encxkey[259] = c;
            return c;
        }

        private int Func7e(byte d)
        {
            byte a, b, c;

            a = _encxkey[256];
            b = _encxkey[257];
            c = _encxkey[a];
            _encxkey[256] = (byte)(a + 1);
            _encxkey[257] = (byte)(b + c);
            a = _encxkey[260];
            b = _encxkey[257];
            b = _encxkey[b];
            c = _encxkey[a];
            _encxkey[a] = b;
            a = _encxkey[259];
            b = _encxkey[257];
            a = _encxkey[a];
            _encxkey[b] = a;
            a = _encxkey[256];
            b = _encxkey[259];
            a = _encxkey[a];
            _encxkey[b] = a;
            a = _encxkey[256];
            _encxkey[a] = c;
            b = _encxkey[258];
            a = _encxkey[c];
            c = _encxkey[259];
            b += a;
            _encxkey[258] = b;
            a = b;
            c = _encxkey[c];
            b = _encxkey[257];
            b = _encxkey[b];
            a = _encxkey[a];
            c += b;
            b = _encxkey[260];
            b = _encxkey[b];
            c += b;
            b = _encxkey[c];
            c = _encxkey[256];
            c = _encxkey[c];
            a += c;
            c = _encxkey[b];
            b = _encxkey[a];
            c ^= (byte)(b ^ d);
            _encxkey[260] = c;   // encrypt
            _encxkey[259] = d;   // encrypt
            return c;
        }

        private int Func6(ref byte[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)Func7(data[i]);
            }
            return data.Length;
        }

        private int Func6e(ref byte[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)Func7e(data[i]);
            }

            return data.Length;
        }

        private void Funcx(byte[] secretkey, byte[] key)
        {
            int seckeylen = secretkey.Length;

            for (int i = 0; i < key.Length; i++)
            {
                _clientChallenge[(secretkey[i % seckeylen] * i) & 7] ^= (byte)(_clientChallenge[i & 7] ^ key[i]);
            }

            Func4(_clientChallenge);
        }

        private byte[] Decode(byte[] data)
        {
            int dataToDecryptSize = data.Length - _offset;
            byte[] dataToDecrypt = new byte[dataToDecryptSize];
            Array.Copy(data, _offset, dataToDecrypt, 0, dataToDecryptSize);

            _offset += Func6(ref dataToDecrypt);

            return dataToDecrypt;
        }

        private byte[] Encode(byte[] data)
        {
            int dataToEncryptSize = data.Length - _offset;
            byte[] dataToEncrypt = new byte[dataToEncryptSize];
            Array.Copy(data, _offset, dataToEncrypt, 0, dataToEncryptSize);

            _offset += Func6e(ref dataToEncrypt);

            return dataToEncrypt;
        }

        public static int GetMasterServerNumber(string gamename)
        {
            int c, server_num = 0;

            if (gamename.Length < 1)
                return 0;

            for (int i = 0; i < gamename.Length; i++)
            {
                c = char.ToLower(gamename[i]);
                server_num = c - (server_num * 0x63306ce7);
            }
            server_num %= 20;

            return server_num;
        }

       
    }
}
