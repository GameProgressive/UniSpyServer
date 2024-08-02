from library.src.abstractions.enctypt_base import EncryptBase

SERVER_CHALLENGE = "0000000000"


class EncryptionParameters:
    def __init__(self):
        self.Register = bytearray(256)
        self.Index0 = 0
        self.Index1 = 0
        self.Index2 = 0
        self.Index3 = 0
        self.Index4 = 0


class EnctypeX(EncryptBase):
    def __init__(self, secretKey: str, clientChallenge: str):
        assert isinstance(secretKey, str)
        assert isinstance(clientChallenge, str)
        self._encParams = EncryptionParameters()
        self._clientChallenge = bytearray(clientChallenge.encode("ascii"))
        self._serverChallenge = bytearray(SERVER_CHALLENGE.encode("ascii"))
        self._secretKey = bytearray(secretKey.encode("ascii"))
        self.init_encryption_algorithm()

    def init_encryption_algorithm(self):
        if len(self._clientChallenge) != 8:
            raise ValueError("Client challenge length not valid!")

        for i in range(len(self._serverChallenge)):
            tempIndex0 = i * self._secretKey[i % len(self._secretKey)] % 8
            tempIndex1 = i % 8
            bitwiseResult = self._clientChallenge[tempIndex1] ^ self._serverChallenge[i]
            self._clientChallenge[tempIndex0] ^= bitwiseResult & 0xFF

        self.init_encryption_parameters()

    def init_encryption_parameters(self):
        if len(self._clientChallenge) < 1:
            self.non_challenge_mapping_init()
            return

        self._encParams.Register = bytearray(range(256))
        toSwap = 0
        keyPosition = 0
        randomSum = 0
        for i in range(255, 0, -1):
            toSwap = self.index_position_generation(i, randomSum, keyPosition)
            swapTemp = self._encParams.Register[i]
            self._encParams.Register[i] = self._encParams.Register[toSwap]
            self._encParams.Register[toSwap] = swapTemp

        self._encParams.Index0 = self._encParams.Register[1]
        self._encParams.Index1 = self._encParams.Register[3]
        self._encParams.Index2 = self._encParams.Register[5]
        self._encParams.Index3 = self._encParams.Register[7]
        self._encParams.Index4 = self._encParams.Register[randomSum]

    def non_challenge_mapping_init(self):
        self._encParams.Index0 = 1
        self._encParams.Index1 = 3
        self._encParams.Index2 = 5
        self._encParams.Index3 = 7
        self._encParams.Index4 = 11
        self._encParams.Register = bytearray(range(255, -1, -1))

    def byte_shift(self, b):
        self._encParams.Index1 += self._encParams.Register[self._encParams.Index0]
        swapTempStorage = self._encParams.Register[self._encParams.Index4]
        self._encParams.Register[self._encParams.Index4] = self._encParams.Register[
            self._encParams.Index1
        ]
        self._encParams.Register[self._encParams.Index1] = self._encParams.Register[
            self._encParams.Index3
        ]
        self._encParams.Register[self._encParams.Index3] = self._encParams.Register[
            self._encParams.Index0
        ]
        self._encParams.Register[self._encParams.Index0] = swapTempStorage
        self._encParams.Index2 += self._encParams.Register[swapTempStorage]

        self._encParams.Index4 = (
            b
            ^ self._encParams.Register[
                (
                    self._encParams.Register[self._encParams.Index2]
                    + self._encParams.Register[self._encParams.Index0]
                )
                & 0xFF
            ]
            ^ self._encParams.Register[
                self._encParams.Register[
                    (
                        self._encParams.Register[self._encParams.Index3]
                        + self._encParams.Register[self._encParams.Index4]
                        + self._encParams.Register[self._encParams.Index1]
                    )
                    & 0xFF
                ]
            ]
        )
        self._encParams.Index3 = b

        return self._encParams.Index4

    def index_position_generation(self, limit, randomSum, keyPosition):
        swapIndex, retryLimiter, bitMask = 0, 0, 1
        if limit == 0:
            return 0

        while bitMask < limit:
            bitMask = (bitMask << 1) + 1

        while True:
            randomSum = (
                self._encParams.Register[randomSum] + self._clientChallenge[keyPosition]
            )

            if keyPosition >= len(self._clientChallenge):
                keyPosition = 0
                randomSum += len(self._clientChallenge)

            swapIndex = bitMask & randomSum
            retryLimiter += 1
            if retryLimiter > 11:
                swapIndex %= limit

            if swapIndex <= limit:
                break

        return swapIndex

    def encrypt(self, plainText):
        return plainText
