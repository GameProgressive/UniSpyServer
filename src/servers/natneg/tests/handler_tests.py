import unittest

from library.src.unispy_server_config import CONFIG
from library.tests.mock_objects.general import ConnectionMock, LogMock, RequestHandlerMock
from servers.natneg.src.contracts.requests import InitRequest
from servers.natneg.src.contracts.results import InitResult
from servers.natneg.src.handlers.handlers import InitHandler
import responses

from servers.natneg.tests.mock_objects import ClientMock


def create_client():
    handler = RequestHandlerMock()
    logger = LogMock()
    conn = ConnectionMock(
        handler=handler,
        config=CONFIG.servers["NatNegotiation"], t_client=ClientMock,
        logger=logger)

    return conn._client


class HandlerTests(unittest.TestCase):
    @responses.activate
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
        url = f"{
            CONFIG.backend.url}/{handler._client.server_config.server_name}/{handler._backend_url}/"
        responses.add(responses.POST, url, json={"message": "ok"}, status=200)

        handler.handle()


if __name__ == "__main__":
    test = HandlerTests()
    test.init_test()
