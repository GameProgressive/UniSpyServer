from frontends.gamespy.protocols.web_services.aggregations.exceptions import WebException
from frontends.gamespy.protocols.web_services.modules.direct2game.abstractions.contracts import (
    NAMESPACE,
    RequestBase,
)


class GetPurchaseHistoryRequest(RequestBase):
    game_id: int
    access_token: str
    proof: str
    certificate: str

    def parse(self) -> None:
        super().parse()
        self.game_id = self._get_int("gameid")
        self.proof = self._get_str("proof")
        self.access_token = self._get_str("access_token")
        self.certificate = self._get_str("certificate")


class GetStoreAvailabilityRequest(RequestBase):
    game_id: int
    version: int
    region: str
    access_token: str

    def parse(self) -> None:
        super().parse()
        self.game_id = self._get_int("gameid")
        self.version = self._get_int("version")
        self.region = self._get_str("region")
        self.access_token = self._get_str("accesstoken")
