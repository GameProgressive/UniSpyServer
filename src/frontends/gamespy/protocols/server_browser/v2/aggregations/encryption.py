from frontends.gamespy.library.abstractions.enctypt_base import Byte, EncryptBase

SERVER_CHALLENGE = "0000000000"


class EncryptionParameters:
    register: list[Byte]
    index_0: Byte
    index_1: Byte
    index_2: Byte
    index_3: Byte
    index_4: Byte

    def __init__(self):
        self.register = [Byte(i) for i in range(256)]
        self.index_0 = Byte(0)
        self.index_1 = Byte(0)
        self.index_2 = Byte(0)
        self.index_3 = Byte(0)
        self.index_4 = Byte(0)


def clamp(value: int) -> int:
    return value % 256


class EnctypeX(EncryptBase):
    _enc_params: EncryptionParameters
    _clientChallenge: list[Byte]
    _serverChallenge: list[Byte]
    _secretKey: list[Byte]

    def __init__(self, secretKey: str, clientChallenge: str):
        assert isinstance(secretKey, str)
        assert isinstance(clientChallenge, str)
        self._enc_params = EncryptionParameters()
        self._clientChallenge = Byte.from_bytes(
            clientChallenge.encode("ascii"))
        self._serverChallenge = Byte.from_bytes(
            SERVER_CHALLENGE.encode("ascii"))
        self._secretKey = Byte.from_bytes(secretKey.encode("ascii"))
        self.init_encryption_algorithm()

    def init_encryption_algorithm(self):
        if len(self._clientChallenge) != 8:
            raise ValueError("Client challenge length not valid!")

        for i in range(len(self._serverChallenge)):
            temp_index_0 = Byte(
                i) * self._secretKey[i % len(self._secretKey)] % Byte(8)
            temp_index1 = Byte(i % 8)
            bitwise_result = self._clientChallenge[temp_index1.value] ^ self._serverChallenge[i]
            self._clientChallenge[temp_index_0.value] ^= bitwise_result

        self.init_encryption_parameters()

    def init_encryption_parameters(self):
        if len(self._clientChallenge) < 1:
            self.non_challenge_mapping_init()
            return

        to_swap = Byte(0)
        key_position = Byte(0)
        random_sum = Byte(0)
        for i in range(255, 0, -1):
            random_sum, key_position, to_swap = self.index_position_generation(
                Byte(i), random_sum, key_position)
            swap_temp = self._enc_params.register[i]
            self._enc_params.register[i] = self._enc_params.register[to_swap.value]
            self._enc_params.register[to_swap.value] = swap_temp

        self._enc_params.index_0 = self._enc_params.register[1]
        self._enc_params.index_1 = self._enc_params.register[3]
        self._enc_params.index_2 = self._enc_params.register[5]
        self._enc_params.index_3 = self._enc_params.register[7]
        self._enc_params.index_4 = self._enc_params.register[random_sum.value]

    def non_challenge_mapping_init(self):
        self._enc_params.index_0 = Byte(1)
        self._enc_params.index_1 = Byte(3)
        self._enc_params.index_2 = Byte(5)
        self._enc_params.index_3 = Byte(7)
        self._enc_params.index_4 = Byte(11)
        self._enc_params.register = [Byte(i) for i in range(256, 0, -1)]

    def byte_shift(self, b: Byte) -> Byte:
        self._enc_params.index_1 += self._enc_params.register[self._enc_params.index_0.value]
        self._enc_params.index_0 += Byte(1)
        swap_temp_storage = self._enc_params.register[self._enc_params.index_4.value]
        self._enc_params.register[self._enc_params.index_4.value] = self._enc_params.register[
            self._enc_params.index_1.value
        ]
        self._enc_params.register[self._enc_params.index_1.value] = self._enc_params.register[
            self._enc_params.index_3.value
        ]
        self._enc_params.register[self._enc_params.index_3.value] = self._enc_params.register[
            self._enc_params.index_0.value
        ]
        self._enc_params.register[self._enc_params.index_0.value] = swap_temp_storage
        self._enc_params.index_2 += self._enc_params.register[swap_temp_storage.value]

        part1 = (
            self._enc_params.register[self._enc_params.index_2.value]
            + self._enc_params.register[self._enc_params.index_0.value]
        ).value % 256
        part2 = (self._enc_params.register[self._enc_params.index_3.value].value
                 + self._enc_params.register[self._enc_params.index_4.value].value
                 + self._enc_params.register[self._enc_params.index_1.value].value) % 256
        part3 = self._enc_params.register[part2].value
        self._enc_params.index_4 = Byte(
            b.value ^ self._enc_params.register[part1].value
            ^ self._enc_params.register[part3].value
        )
        self._enc_params.index_3 = b

        return self._enc_params.index_4

    def index_position_generation(self, limit: Byte, random_sum: Byte, key_position: Byte) -> tuple[Byte, Byte, Byte]:
        swap_index, retry_limiter, bit_mask = Byte(0), Byte(0), Byte(1)
        if limit == Byte(0):
            return random_sum, key_position, Byte(0)

        while bit_mask < limit:
            bit_mask = (bit_mask << Byte(1)) + Byte(1)

        while True:
            random_sum = (
                self._enc_params.register[random_sum.value] +
                self._clientChallenge[key_position.value]
            )
            key_position += Byte(1)

            if key_position >= Byte(len(self._clientChallenge)):
                key_position = Byte(0)
                random_sum += Byte(len(self._clientChallenge))

            swap_index = bit_mask & random_sum

            if retry_limiter > Byte(11):
                swap_index %= limit
            retry_limiter += Byte(1)

            if swap_index <= limit:
                break

        return random_sum, key_position, swap_index

    def encrypt(self, plain_text: bytes):
        # skip first 14 bytes
        head_buffer = plain_text[:14]
        body_buffer = plain_text[14:]
        cipher_body = bytearray(body_buffer)
        for i in range(len(body_buffer)):
            c = self.byte_shift(Byte(body_buffer[i]))
            cipher_body[i] = c.value
        cipher_body = bytes(cipher_body)
        cipher = head_buffer+cipher_body
        return cipher
