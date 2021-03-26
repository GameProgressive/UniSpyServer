using System.Text;
using ServerBrowser.Network;

namespace ServerBrowser.Entity.Structure.Misc
{
    public class SBEncryptionParameters
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
        private SBEncryptionParameters _encParam;
        private byte[] _clientChallenge = new byte[8];
        private byte[] _serverChallenge;
        private byte[] _secretKey;
        public SBEncryption(string secretKey, string clientChallenge, SBEncryptionParameters encParam)
        {
            // reference private filed to encParams
            _encParam = encParam;
            _encParam = new SBEncryptionParameters();            
            _clientChallenge = Encoding.ASCII.GetBytes(clientChallenge);
            _serverChallenge = Encoding.ASCII.GetBytes(SBConstants.ServerChallenge);
            _secretKey = Encoding.ASCII.GetBytes(secretKey);
            InitEncryptionAlgorithm();
        }

        /// <summary>
        /// Initialize permutation table
        /// </summary>
        /// <param name="secretKey">game secret key</param>
        /// <param name="serverChallenge">can be null</param>
        private SBEncryption(string secretKey, string clientChallenge, string serverChallenge)
        {
            _encParam = new SBEncryptionParameters();
            _clientChallenge = Encoding.ASCII.GetBytes(clientChallenge);
            _serverChallenge = Encoding.ASCII.GetBytes(serverChallenge);
            _secretKey = Encoding.ASCII.GetBytes(secretKey);
            InitEncryptionAlgorithm();
        }

        public SBEncryption(SBEncryptionParameters state)
        {
            _encParam = state;
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
            byte toSwap, swaptemp, randomSum;
            byte keyPosition;

            if (_clientChallenge.Length < 1)
            {
                NonChallengeMappingInit();
                return;
            }

            //generate ascii table
            for (i = 0; i < 256; i++)
            {
                _encParam.Register[i] = (byte)i;
            }

            toSwap = 0;
            keyPosition = 0;
            randomSum = 0;
            for (i = 255; i > 0; i--)
            {
                toSwap = IndexPositionGeneration(i, ref randomSum, ref keyPosition);
                swaptemp = _encParam.Register[i];
                _encParam.Register[i] = _encParam.Register[toSwap];
                _encParam.Register[toSwap] = swaptemp;
            }

            // Initialize the indices and data dependencies.
            // Indices are set to different values instead of all 0
            // to reduce what is known about the state of the state.cards
            // when the first byte is emitted.

            _encParam.Index0 = _encParam.Register[1];
            _encParam.Index1 = _encParam.Register[3];
            _encParam.Index2 = _encParam.Register[5];
            _encParam.Index3 = _encParam.Register[7];
            _encParam.Index4 = _encParam.Register[randomSum];
        }

        public byte[] Encrypt(string plainStr)
        {
            byte[] plainText = Encoding.ASCII.GetBytes(plainStr);
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
            _encParam.Index0 = 1;
            _encParam.Index1 = 3;
            _encParam.Index2 = 5;
            _encParam.Index3 = 7;
            _encParam.Index4 = 11;

            // Start with state.cards all in inverse order.
            for (i = 0, j = 255; i <= 255; i++, j--)
            {
                _encParam.Register[i] = j;
            }
        }

        private byte ByteShift(byte b)
        {
            byte swapTempStorage;
            _encParam.Index1 = (byte)(_encParam.Index1 + _encParam.Register[_encParam.Index0++]);
            swapTempStorage = _encParam.Register[_encParam.Index4];
            _encParam.Register[_encParam.Index4] = _encParam.Register[_encParam.Index1];
            _encParam.Register[_encParam.Index1] = _encParam.Register[_encParam.Index3];
            _encParam.Register[_encParam.Index3] = _encParam.Register[_encParam.Index0];
            _encParam.Register[_encParam.Index0] = swapTempStorage;
            _encParam.Index2 = (byte)(_encParam.Index2 + _encParam.Register[swapTempStorage]);


            _encParam.Index4 = (byte)(b ^ _encParam.Register[(_encParam.Register[_encParam.Index2] +
                _encParam.Register[_encParam.Index0]) & 0xFF] ^
               _encParam.Register[_encParam.Register[(_encParam.Register[_encParam.Index3] +
               _encParam.Register[_encParam.Index4] +
               _encParam.Register[_encParam.Index1]) & 0xFF]]);
            _encParam.Index3 = b;

            return _encParam.Index4;
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
                randomSum = (byte)(_encParam.Register[randomSum] + _clientChallenge[keyPosition++]);

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
    }
}
