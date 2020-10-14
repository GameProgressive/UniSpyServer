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
        public GOACryptState State { get; protected set; }
        byte[] _clientChallenge = new byte[8];
        byte[] _serverChallenge;
        /// <summary>
        /// Initialize permutation table
        /// </summary>
        /// <param name="secKey">game secret key</param>
        /// <param name="serverChallenge">can be null</param>
        public GOAEncryption(byte[] secKey, byte[] clientChallenge, byte[] serverChallenge)
        {
            State = new GOACryptState();
            _clientChallenge = clientChallenge;
            _serverChallenge = serverChallenge;
            InitCryptKey(secKey);
        }

        public GOAEncryption(string secKey, string clientChallenge, string serverChallenge)
        {
            State = new GOACryptState();
            _clientChallenge = Encoding.ASCII.GetBytes(clientChallenge);
            _serverChallenge = Encoding.ASCII.GetBytes(serverChallenge);
            InitCryptKey(Encoding.ASCII.GetBytes(secKey));
        }

        public GOAEncryption(GOACryptState state)
        {
            State = state;
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
                State.cards[i] = (byte)i;
            }

            toswap = 0;
            keypos = 0;
            rsum = 0;
            for (i = 255; i > 0; i--)
            {
                toswap = KeyRand(i, ref rsum, ref keypos);
                swaptemp = State.cards[i];
                State.cards[i] = State.cards[toswap];
                State.cards[toswap] = swaptemp;
            }

            // Initialize the indices and data dependencies.
            // Indices are set to different values instead of all 0
            // to reduce what is known about the state of the state.cards
            // when the first byte is emitted.

            State.rotor = State.cards[1];
            State.ratchet = State.cards[3];
            State.avalanche = State.cards[5];
            State.last_plain = State.cards[7];
            State.last_cipher = State.cards[rsum];

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
                cipherText[i] = GOAEncryptByteShift(plainText[i]);
            }
            return cipherText;
        }

        private void GOANonChallengeHashInit()
        {

            // Initialize the indices and data dependencies.
            byte i, j;
            State.rotor = 1;
            State.ratchet = 3;
            State.avalanche = 5;
            State.last_plain = 7;
            State.last_cipher = 11;

            // Start with state.cards all in inverse order.
            for (i = 0, j = 255; i <= 255; i++, j--)
            {
                State.cards[i] = j;
            }

        }

        private byte GOAEncryptByteShift(byte b)
        {
            byte swaptemp;
            State.ratchet = (byte)(State.ratchet + State.cards[State.rotor++]);
            swaptemp = State.cards[State.last_cipher];
            State.cards[State.last_cipher] = State.cards[State.ratchet];
            State.cards[State.ratchet] = State.cards[State.last_plain];
            State.cards[State.last_plain] = State.cards[State.rotor];
            State.cards[State.rotor] = swaptemp;
            State.avalanche = (byte)(State.avalanche + State.cards[swaptemp]);


            State.last_cipher =
                (byte)(b ^ State.cards[(State.cards[State.avalanche] + State.cards[State.rotor]) & 0xFF] ^
       State.cards[State.cards[(State.cards[State.last_plain] +
       State.cards[State.last_cipher] +
       State.cards[State.ratchet]) & 0xFF]]);
            State.last_plain = b;

            return State.last_cipher;
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
                rsum = (byte)(State.cards[rsum] + _clientChallenge[keypos++]);

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
