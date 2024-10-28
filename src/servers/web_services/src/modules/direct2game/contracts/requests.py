from servers.web_services.src.aggregations.exceptions import WebException
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
        if game_id is None or game_id.text is None:
            raise WebException("gameid is missing from the request.")
        self.game_id = int(game_id.text)

        proof = self._content_element.find(f".//{{{NAMESPACE}}}proof")
        if proof is None or proof.text is None:
            raise WebException("proof is missing from the request.")
        self.proof = proof.text

        access_token = self._content_element.find(
            f".//{{{NAMESPACE}}}access_token"
        )

        if access_token is None or access_token.text is None:
            raise WebException("access_token is missing from the request.")
        self.access_token = access_token.text

        certificate = self._content_element.find(
            f".//{{{NAMESPACE}}}certificate")
        if certificate is None or certificate.text is None:
            raise WebException("certificate is missing from the request.")
        self.certificate = certificate.text


class GetStoreAvailabilityRequest(RequestBase):
    game_id: int
    version: int
    region: str
    access_token: str

    def parse(self) -> None:
        super().parse()
        game_id = self._content_element.find(f".//{{{NAMESPACE}}}gameid")
        if game_id is None or game_id.text is None:
            raise WebException("gameid is missing from the request.")
        self.game_id = int(game_id.text)
        version = self._content_element.find(f".//{{{NAMESPACE}}}version")
        if version is None or version.text is None:
            raise WebException("version is missing from the request.")
        self.version = int(version.text)
        region = self._content_element.find(f".//{{{NAMESPACE}}}region")
        if region is None or region.text is None:
            raise WebException("region is missing from the request.")
        self.region = region.text

        access_token = self._content_element.find(
            f".//{{{NAMESPACE}}}accesstoken")

        if access_token is None or access_token.text is None:
            raise WebException("accesstoken is missing from the request.")
        self.access_token = access_token.text
