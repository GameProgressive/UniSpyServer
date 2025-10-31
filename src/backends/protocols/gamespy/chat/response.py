from backends.library.abstractions.contracts import DataResponse
from frontends.gamespy.protocols.chat.contracts.results import AtmResult, CryptResult, GetCKeyResult, GetChannelKeyResult, GetKeyResult, JoinResult, KickResult, ListResult, ModeResult, NamesResult, NickResult, NoticeResult, PartResult, PingResult, PrivateResult, SetCKeyResult, SetChannelKeyResult, TopicResult, UtmResult, WhoIsResult, WhoResult


class PingResponse(DataResponse):
    result: PingResult


class CryptResponse(DataResponse):
    result: CryptResult


class GetKeyResponse(DataResponse):
    result: GetKeyResult


class ListResponse(DataResponse):
    result: ListResult


class NicksResponse(DataResponse):
    result: NickResult


class WhoResponse(DataResponse):
    result: WhoResult


class WhoIsResponse(DataResponse):
    result: WhoIsResult


class JoinResponse(DataResponse):
    result: JoinResult


class GetChannelKeyResponse(DataResponse):
    result: GetChannelKeyResult


class GetCkeyResponse(DataResponse):
    result: GetCKeyResult


class KickResponse(DataResponse):
    result: KickResult


class ModeResponse(DataResponse):
    result: ModeResult


class NamesResponse(DataResponse):
    result: NamesResult


class PartResponse(DataResponse):
    result: PartResult


class SetChannelKeyResponse(DataResponse):
    result: SetChannelKeyResult


class SetCKeyResponse(DataResponse):
    result: SetCKeyResult


class TopicResponse(DataResponse):
    result: TopicResult


class AtmResponse(DataResponse):
    result: AtmResult


class UtmResponse(DataResponse):
    result: UtmResult


class NoticeResponse(DataResponse):
    result: NoticeResult


class PrivateResponse(DataResponse):
    result: PrivateResult

