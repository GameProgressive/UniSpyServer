import unittest

from library.src.unispy_server_config import CONFIG
from library.tests.mock_objects.general import ClientMock, ConnectionMock, LogMock, RequestHandlerMock
from servers.natneg.src.contracts.requests import InitRequest
from servers.natneg.src.contracts.results import InitResult
from servers.natneg.src.handlers.handlers import InitHandler


def create_client():
    handler = RequestHandlerMock()
    logger = LogMock()
    conn = ConnectionMock(
        handler=handler,
        config=CONFIG.servers["NatNegotiation"], t_client=ClientMock,
        logger=logger)
    return conn._client


class HandlerTests(unittest.TestCase):

    def init_test(self):
        raw = bytes(
            [
                0xfd, 0xfc, 0x1e, 0x66, 0x6a, 0xb2, 0x03,
                0x00,
                0x00, 0x00, 0x03, 0x09, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            ]
        )  # fmt: skip
        req = InitRequest(raw)
        client = create_client()
        handler = InitHandler(client, req)
        # change function pointer
        handler._data_operate = lambda: None
        handler.handle()
        handler._result = InitResult()


if __name__ == "__main__":
    test = HandlerTests()
    test.init_test()
