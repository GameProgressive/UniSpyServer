from frontends.gamespy.library.abstractions.contracts import RequestBase


def add_headers(request: RequestBase) -> dict:
    request.parse()
    if isinstance(request.raw_request, bytes):
        request.raw_request = request.raw_request.decode(
            "ascii", "backslashreplace")
    data = request.to_dict()
    data["client_ip"] = "192.168.0.1"
    data["server_id"] = "950b7638-a90d-469b-ac1f-861e63c8c613"
    data["client_port"] = 1234
    return data
