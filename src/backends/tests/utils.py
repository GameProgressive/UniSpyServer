from uuid import UUID
from frontends.gamespy.library.abstractions.contracts import RequestBase


def add_headers(data: dict) -> dict:
    info = {}
    info["client_ip"] = "192.168.0.1"
    info["server_id"] = "950b7638-a90d-469b-ac1f-861e63c8c613"
    info["client_port"] = 1234
    request = {}
    request["info"] = info
    request["data"] = data
    return request
