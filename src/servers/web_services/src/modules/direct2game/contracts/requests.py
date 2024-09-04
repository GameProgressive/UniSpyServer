from servers.web_services.src.exceptions.general import WebExceptions
from servers.web_services.src.modules.direct2game.abstractions.contracts import (
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
        game_id = self._content_element.find(f".//{{{NAMESPACE}}}gameid")
        if game_id is None:
            raise WebExceptions("gameid is missing from the request.")
        self.game_id = int(game_id)

        self.proof = self._content_element.find(f".//{{{NAMESPACE}}}proof")
        if self.proof is None:
            raise WebExceptions("proof is missing from the request.")
        self.access_token = self._content_element.find(
            f".//{{{NAMESPACE}}}access_token"
        )
        if self.access_token is None:
            raise WebExceptions("access_token is missing from the request.")
        self.certificate = self._content_element.find(f".//{{{NAMESPACE}}}certificate")
        if self.certificate is None:
            raise WebExceptions("certificate is missing from the request.")


class GetStoreAvailabilityRequest(RequestBase):
    game_id: int
    version: int
    region: str
    access_token: str

    def parse(self) -> None:
        super().parse()
        game_id = self._content_element.find(f".//{{{NAMESPACE}}}gameid")
        if game_id is None:
            raise WebExceptions("gameid is missing from the request.")
        self.game_id = int(game_id)
        version = self._content_element.find(f".//{{{NAMESPACE}}}version")
        if version is None:
            raise WebExceptions("version is missing from the request.")
        self.version = int(version)
        self.region = self._content_element.find(f".//{{{NAMESPACE}}}region")
        if self.region is None:
            raise WebExceptions("region is missing from the request.")
        self.access_token = self._content_element.find(f".//{{{NAMESPACE}}}accesstoken")
        if self.access_token is None:
            raise WebExceptions("accesstoken is missing from the request.")
