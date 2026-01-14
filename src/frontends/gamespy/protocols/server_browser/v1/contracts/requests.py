from frontends.gamespy.protocols.query_report.aggregates.exceptions import QRException
from frontends.gamespy.protocols.server_browser.v1.abstractions.contracts import RequestBase
from frontends.gamespy.protocols.server_browser.v1.aggregations.enums import Modifier


class ValidateRequest(RequestBase):
    game_name: str
    game_version: str
    validate: str
    enctype: str
    query_id: str

    def parse(self) -> None:
        super().parse()
        game_name = self._request_dict.get("gamename")
        if game_name is None:
            raise QRException("gamename is missing")
        self.game_name = game_name

        version = self._request_dict.get("gamever")
        if version is None:
            raise QRException("version is missing")
        self.game_version = version
        validate = self._request_dict.get("gamever")
        if validate is None:
            raise QRException("validate is missing")
        self.validate = validate

        enctype = self._request_dict.get("gamever")
        if enctype is None:
            raise QRException("enctype is missing")
        self.enctype = enctype

        query_id = self._request_dict.get("queryid")
        if query_id is None:
            raise QRException("query_id is missing")
        self.query_id = query_id


class ServerListRequest(RequestBase):
    modifier: Modifier
    filter: str | None
    game_name: str

    def parse(self) -> None:
        super().parse()
        modifier = self._request_dict.get("list")
        if modifier is None:
            raise QRException("modifier is missing")
        self.modifier = Modifier(modifier)
        
        self.filter = self._request_dict.get("where")

        game_name = self._request_dict.get("gamename")
        if game_name is None:
            raise QRException("gamename is missing")
        self.game_name = game_name
