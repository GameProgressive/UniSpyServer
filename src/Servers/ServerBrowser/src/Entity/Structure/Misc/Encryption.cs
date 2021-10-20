using System.Collections.Generic;
using UniSpyServer.UniSpyLib.Encryption;
using UniSpyServer.ServerBrowser.Entity.Structure.Misc;
namespace UniSpyServer.ServerBrowser.Entity.Structure.Misc
{
    public class EncryptionParameters
    {
        public byte[] Register = new byte[256];
        public byte Index0;
        public byte Index1;
        public byte Index2;
        public byte Index3;
        public byte Index4;
    }

    public class SBEncryption
    {
        private EncryptionParameters _encParams;
        private byte[] _clientChallenge = new byte[8];
        private byte[] _serverChallenge;
        private byte[] _secretKey;
        public SBEncryption(string secretKey, string clientChallenge, EncryptionParameters encParam)
        {
            // reference private filed to encParams
            _encParams = encParam;
            _clientChallenge = UniSpyEncoding.GetBytes(clientChallenge);
            _serverChallenge = UniSpyEncoding.GetBytes(Constants.ServerChallenge);
            _secretKey = UniSpyEncoding.GetBytes(secretKey);
            InitEncryptionAlgorithm();
        }

        public SBEncryption(EncryptionParameters encParams)
        {
            _encParams = encParams;
        }

        /// <summary>
        /// Initialize the challenge which generates the permutation table
        /// </summary>
        /// <param name="secKey">game secret key</param>
        /// <param name="serverChallenge">the random number challenge that server or client generate</param>
        public void InitEncryptionAlgorithm()
        {
            if (_clientChallenge.Length != 8)
            {
                //we have error client challenge
                throw new System.ArgumentException("Client challenge length not valid!");
            }

            for (int i = 0; i < _serverChallenge.Length; i++)
            {
                int tempIndex0 = i * _secretKey[i % _secretKey.Length] % 8;
                int tempIndex1 = i % 8;
                byte bitwiseResult = (byte)(_clientChallenge[tempIndex1] ^ _serverChallenge[i]);
                _clientChallenge[tempIndex0] ^= (byte)(bitwiseResult & 0xFF);
            }

            InitEncryptionParameters();
        }

        private void InitEncryptionParameters()
        {
            uint i;
            byte toSwap, swapTemp, randomSum;
            byte keyPosition;

            if (_clientChallenge.Length < 1)
            {
                NonChallengeMappingInit();
                return;
            }

            //generate ascii table
            for (i = 0; i < 256; i++)
            {
                _encParams.Register[i] = (byte)i;
            }

            toSwap = 0;
            keyPosition = 0;
            randomSum = 0;
            for (i = 255; i > 0; i--)
            {
                toSwap = IndexPositionGeneration(i, ref randomSum, ref keyPosition);
                swapTemp = _encParams.Register[i];
                _encParams.Register[i] = _encParams.Register[toSwap];
                _encParams.Register[toSwap] = swapTemp;
            }

            // Initialize the indices and data dependencies.
            // Indices are set to different values instead of all 0
            // to reduce what is known about the state of the state.cards
            // when the first byte is emitted.

            _encParams.Index0 = _encParams.Register[1];
            _encParams.Index1 = _encParams.Register[3];
            _encParams.Index2 = _encParams.Register[5];
            _encParams.Index3 = _encParams.Register[7];
            _encParams.Index4 = _encParams.Register[randomSum];
        }

        public byte[] Encrypt(string plainStr)
        {
            byte[] plainText = UniSpyEncoding.GetBytes(plainStr);
            return Encrypt(plainText);
        }

        public byte[] Encrypt(byte[] plainText)
        {
            int i;
            byte[] cipherText = new byte[plainText.Length];
            for (i = 0; i < plainText.Length; i++)
            {
                cipherText[i] = ByteShift(plainText[i]);
            }
            return cipherText;
        }

        private void NonChallengeMappingInit()
        {
            // Initialize the indices and data dependencies.
            byte i, j;
            _encParams.Index0 = 1;
            _encParams.Index1 = 3;
            _encParams.Index2 = 5;
            _encParams.Index3 = 7;
            _encParams.Index4 = 11;

            // Start with state.cards all in inverse order.
            for (i = 0, j = 255; i <= 255; i++, j--)
            {
                _encParams.Register[i] = j;
            }
        }

        private byte ByteShift(byte b)
        {
            byte swapTempStorage;
            _encParams.Index1 = (byte)(_encParams.Index1 + _encParams.Register[_encParams.Index0++]);
            swapTempStorage = _encParams.Register[_encParams.Index4];
            _encParams.Register[_encParams.Index4] = _encParams.Register[_encParams.Index1];
            _encParams.Register[_encParams.Index1] = _encParams.Register[_encParams.Index3];
            _encParams.Register[_encParams.Index3] = _encParams.Register[_encParams.Index0];
            _encParams.Register[_encParams.Index0] = swapTempStorage;
            _encParams.Index2 = (byte)(_encParams.Index2 + _encParams.Register[swapTempStorage]);


            _encParams.Index4 = (byte)(b ^ _encParams.Register[(_encParams.Register[_encParams.Index2] +
                _encParams.Register[_encParams.Index0]) & 0xFF] ^
               _encParams.Register[_encParams.Register[(_encParams.Register[_encParams.Index3] +
               _encParams.Register[_encParams.Index4] +
               _encParams.Register[_encParams.Index1]) & 0xFF]]);
            _encParams.Index3 = b;

            return _encParams.Index4;
        }


        /// <summary>
        /// Generate permutation index position
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="challenge"></param>
        /// <param name="randomSum"></param>
        /// <param name="keyPosition"></param>
        /// <returns></returns>
        private byte IndexPositionGeneration(uint limit, ref byte randomSum, ref byte keyPosition)
        {
            byte swapIndex, retryLimiter, bitMask;

            if (limit == 0)
            {
                return 0;
            }

            retryLimiter = 0;
            bitMask = 1;

            while (bitMask < limit)
            {
                bitMask = (byte)((bitMask << 1) + 1);
            }

            do
            {
                randomSum = (byte)(_encParams.Register[randomSum] + _clientChallenge[keyPosition++]);

                if (keyPosition >= _clientChallenge.Length)
                {
                    keyPosition = 0;
                    randomSum = (byte)(randomSum + _clientChallenge.Length);
                }
                swapIndex = (byte)(bitMask & randomSum);
                if (++retryLimiter > 11)
                {
                    swapIndex %= (byte)limit;
                }
            }
            while (swapIndex > limit);

            return swapIndex;
        }
        public static byte[] BuildCryptHeader()
        {
            // cryptHeader have 14 bytes, when we encrypt data we need skip the first 14 bytes
            var cryptHeader = new List<byte>();
            cryptHeader.Add(2 ^ 0xEC);
            cryptHeader.AddRange(new byte[] { 0, 0 });
            cryptHeader.Add((byte)(Constants.ServerChallenge.Length ^ 0xEA));
            cryptHeader.AddRange(UniSpyEncoding.GetBytes(Constants.ServerChallenge));
            return cryptHeader.ToArray();
        }
    }
}
