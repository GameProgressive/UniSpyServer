using System;
using System.Text;

namespace GameSpyLib.Encryption
{
    public class GOACryptState
    {
        public byte[] cards = new byte[256];
        public byte rotor;
        public byte ratchet;
        public byte avalanche;
        public byte last_plain;
        public byte last_cipher;
    }


    public class GOAEncryption
    {
        GOACryptState state = new GOACryptState();
        byte[] _clientChallenge = new byte[8];
        byte[] _serverChallenge;
        /// <summary>
        /// Initialize permutation table
        /// </summary>
        /// <param name="secKey">game secret key</param>
        /// <param name="serverChallenge">can be null</param>
        public GOAEncryption(byte[] secKey,byte[] clientChallenge ,byte[] serverChallenge)
        {
            _clientChallenge = clientChallenge;
            _serverChallenge = serverChallenge;
            InitCryptKey(secKey);
        }

        public GOAEncryption(string secKey,string clientChallenge, string serverChallenge)
        {
            byte[] secretKey = Encoding.ASCII.GetBytes(secKey);
            _clientChallenge = Encoding.ASCII.GetBytes(clientChallenge);
            _serverChallenge = Encoding.ASCII.GetBytes(serverChallenge);
            InitCryptKey(secretKey);
        }

        /// <summary>
        /// Initialize the challenge which generates the permutation table
        /// </summary>
        /// <param name="secKey">game secret key</param>
        /// <param name="serverChallenge">the random number challenge that server or client generate</param>
        private void InitCryptKey(byte[] secKey)
        {
            if (_clientChallenge.Length != 8)
            {
                //we have error client challenge
                return;
            }

            for (int i = 0; i < _serverChallenge.Length; i++)
            {
                _clientChallenge[i * secKey[i % secKey.Length] % 8] ^= (byte)((_clientChallenge[i % 8] ^ _serverChallenge[i]) & 0xFF);
            }

            GOACryptInit();
        }



        private void GOACryptInit()
        {
            uint i;
            byte toswap, swaptemp, rsum;
            byte keypos;

            if (_clientChallenge.Length < 1)
            {
                GOANonChallengeHashInit();
                return;
            }

            //generate ascii table
            for (i = 0; i < 256; i++)
            {
                state.cards[i] = (byte)i;
            }

            toswap = 0;
            keypos = 0;
            rsum = 0;
            for (i = 255; i > 0; i--)
            {
                toswap = KeyRand(i, ref rsum, ref keypos);
                swaptemp = state.cards[i];
                state.cards[i] = state.cards[toswap];
                state.cards[toswap] = swaptemp;
            }

            // Initialize the indices and data dependencies.
            // Indices are set to different values instead of all 0
            // to reduce what is known about the state of the state.cards
            // when the first byte is emitted.

            state.rotor = state.cards[1];
            state.ratchet = state.cards[3];
            state.avalanche = state.cards[5];
            state.last_plain = state.cards[7];
            state.last_cipher = state.cards[rsum];

        }

        private void GOANonChallengeHashInit()
        {

            // Initialize the indices and data dependencies.
            byte i, j;
            state.rotor = 1;
            state.ratchet = 3;
            state.avalanche = 5;
            state.last_plain = 7;
            state.last_cipher = 11;

            // Start with state.cards all in inverse order.
            for (i = 0, j = 255; i <= 255; i++, j--)
            {
                state.cards[i] = j;
            }

        }
        byte swaptemp;
        private byte GOAEncryptEachByte(byte b)
        {
            
            state.ratchet = (byte)(state.ratchet + state.cards[state.rotor++]);
            swaptemp = state.cards[state.last_cipher];
            state.cards[state.last_cipher] = state.cards[state.ratchet];
            state.cards[state.ratchet] = state.cards[state.last_plain];
            state.cards[state.last_plain] = state.cards[state.rotor];
            state.cards[state.rotor] = swaptemp;
            state.avalanche = (byte)(state.avalanche + state.cards[swaptemp]);


            state.last_cipher =
                (byte)(b ^ state.cards[(state.cards[state.avalanche] + state.cards[state.rotor]) & 0xFF] ^
       state.cards[state.cards[(state.cards[state.last_plain] +
       state.cards[state.last_cipher] +
       state.cards[state.ratchet]) & 0xFF]]);
            state.last_plain = b;

            return state.last_cipher;
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
                cipherText[i] = GOAEncryptEachByte(plainText[i]);
            }
            return cipherText;
        }

        /// <summary>
        /// Generate permutation index position
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="challenge"></param>
        /// <param name="rsum"></param>
        /// <param name="keypos"></param>
        /// <returns></returns>
        private byte KeyRand(uint limit, ref byte rsum, ref byte keypos)
        {
            byte u, retryLimiter, mask;

            if (limit == 0)
            {
                return 0;
            }

            retryLimiter = 0;
            mask = 1;

            while (mask < limit)
            {
                mask = (byte)((mask << 1) + 1);
            }

            do
            {
                rsum = (byte)(state.cards[rsum] + _clientChallenge[keypos++]);

                if (keypos >= _clientChallenge.Length)
                {
                    keypos = 0;
                    rsum = (byte)(rsum + _clientChallenge.Length);
                }
                u = (byte)(mask & rsum);
                if (++retryLimiter > 11)
                {
                    u %= (byte)limit;
                }
            }
            while (u > limit);

            return u;
        }

    }
}
