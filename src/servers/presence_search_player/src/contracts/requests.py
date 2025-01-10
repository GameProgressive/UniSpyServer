from library.src.extentions.gamespy_utils import is_email_format_correct
from library.src.extentions.password_encoder import process_password
from servers.presence_search_player.src.abstractions.contracts import RequestBase
from servers.presence_search_player.src.aggregates.enums import SearchType
from servers.presence_search_player.src.aggregates.exceptions import (
    GPParseException,
)


class CheckRequest(RequestBase):
    # \check\\nick\<nick>\email\<email>\partnerid\0\passenc\<passenc>\gamename\gmtest\final\
    nick: str
    password: str
    email: str
    partner_id: int

    def parse(self):
        super().parse()
        self.password = process_password(self.request_dict)
        if "nick" not in self.request_dict or "email" not in self.request_dict:
            raise GPParseException("check request is incompelete.")

        if not is_email_format_correct(self.request_dict["email"]):
            raise GPParseException(" email format is incorrect.")

        self.nick = self.request_dict["nick"]
        self.email = self.request_dict["email"]

        if "partner_id" in self.request_dict.keys():
            try:
                self.partner_id = int(self.request_dict["partner_id"])
            except:
                raise GPParseException(
                    "no partner id found, check whether need implement the default partnerid")
        else:
            self.partner_id = 0


class NewUserRequest(RequestBase):
    product_id: int
    game_port: int
    cd_key: str
    has_game_name: bool
    has_product_id: bool
    has_cdkey: bool
    has_partner_id: bool
    has_game_port: bool
    nick: str
    email: str
    password: str
    partner_id: int
    game_name: str
    uniquenick: str

    def parse(self):
        super().parse()
        self.password = process_password(self.request_dict)

        if "nick" not in self.request_dict:
            raise GPParseException("nickname is missing.")
        if "email" not in self.request_dict:
            raise GPParseException("email is missing.")
        if not is_email_format_correct(self.request_dict["email"]):
            raise GPParseException("email format is incorrect.")
        self.nick = self.request_dict["nick"]
        self.email = self.request_dict["email"]

        if "uniquenick" in self.request_dict and "namespaceid" in self.request_dict:
            if "namespaceid" in self.request_dict:
                try:
                    self.namespace_id = int(self.request_dict["namespaceid"])
                except ValueError:
                    raise GPParseException("namespaceid is incorrect.")

            self.uniquenick = self.request_dict["uniquenick"]
        self.parse_other_info()

    def parse_other_info(self):
        if "partnerid" in self.request_dict:
            try:
                self.partner_id = int(self.request_dict["partnerid"])
                self.has_partner_id_flag = True
            except ValueError:
                raise GPParseException("partnerid is incorrect.")
        else:
            self.partner_id = 0
        if "productid" in self.request_dict:
            try:
                self.product_id = int(self.request_dict["productid"])
                self.has_product_id_flag = True
            except ValueError:
                raise GPParseException("productid is incorrect.")

        if "gamename" in self.request_dict:
            self.has_game_name_flag = True
            self.game_name = self.request_dict["gamename"]

        if "port" in self.request_dict:
            try:
                self.game_port = int(self.request_dict["port"])
                self.has_game_port_flag = True
            except ValueError:
                raise GPParseException("port is incorrect.")

        if "cdkey" in self.request_dict:
            self.has_cd_key_enc_flag = True
            self.cd_key = self.request_dict["cdkey"]


class NicksRequest(RequestBase):
    password: str
    email: str
    is_require_uniquenicks: bool = True

    def parse(self):
        super().parse()
        self.password = process_password(self.request_dict)
        if "email" not in self.request_dict.keys():
            raise GPParseException("email is missing.")

        self.email = self.request_dict["email"]

        if "pass" in self.request_dict.keys():
            self.is_require_uniquenicks = False


class OthersListRequest(RequestBase):
    profile_ids: list[int] = []
    namespace_id: int = 0

    def parse(self) -> None:
        super().parse()
        if "opids" not in self.request_dict or "namespaceid" not in self.request_dict:
            raise GPParseException("opids or namespaceid is missing.")

        try:
            self.profile_ids = [
                int(opid) for opid in self.request_dict["opids"].strip("|").split("|")
            ]
        except:
            raise GPParseException("opids is incorrect")


class OthersRequest(RequestBase):
    profile_id: int
    game_name: str

    def parse(self):
        super().parse()

        if "gamename" not in self.request_dict:
            raise GPParseException("gamename is missing.")

        self.game_name = self.request_dict["gamename"]

        if (
            "profileid" not in self.request_dict
            or "namespaceid" not in self.request_dict
        ):
            raise GPParseException("profileid or namespaceid is missing.")

        if "profileid" not in self.request_dict:
            raise GPParseException("profileid is incorrect.")

        try:
            self.profile_id = int(self.request_dict["profileid"])
        except ValueError:
            raise GPParseException("profileid is incorrect.")


class SearchRequest(RequestBase):
    skip_num: int
    request_type: SearchType
    game_name: str
    profile_id: int
    partner_id: int
    email: str
    nick: str
    uniquenick: str
    session_key: str
    firstname: str
    lastname: str
    icquin: str

    def __init__(self, raw_request: str) -> None:
        super().__init__(raw_request)
        self.skip_num = 0

    def parse(self) -> None:
        super().parse()

        if (
            "profileid" not in self.request_dict
            and "nick" not in self.request_dict
            and "email" not in self.request_dict
            and "namespaceid" not in self.request_dict
            and "gamename" not in self.request_dict
            and "sesskey" not in self.request_dict
        ):
            raise GPParseException("Search request is incomplete.")

        self.session_key = self.request_dict["sesskey"]

        if "firstname" in self.request_dict:
            self.firstname = self.request_dict["firstname"]

        if "lastname" in self.request_dict:
            self.lastname = self.request_dict["lastname"]

        if "icquin" in self.request_dict:
            self.icquin = self.request_dict["icquin"]

        if "gamename" in self.request_dict:
            self.game_name = self.request_dict["gamename"]

        if "profileid" in self.request_dict:
            try:
                self.profile_id = int(self.request_dict["profileid"])
            except ValueError:
                raise GPParseException("profileid is incorrect.")

        if "partnerid" in self.request_dict:
            try:
                self.partner_id = int(self.request_dict["partnerid"])
            except ValueError:
                raise GPParseException("partnerid is incorrect.")

        if "skip" in self.request_dict:
            try:
                self.skip_num = int(self.request_dict["skip"])
            except ValueError:
                raise GPParseException("skip number is incorrect.")

        if "uniquenick" in self.request_dict and "namespaceid" in self.request_dict:
            self.request_type = SearchType.UNIQUENICK_NAMESPACEID_SEARCH
            self.uniquenick = self.request_dict["uniquenick"]
        elif "nick" in self.request_dict and "email" in self.request_dict:
            self.request_type = SearchType.NICK_EMAIL_SEARCH
            self.nick = self.request_dict["nick"]
            self.email = self.request_dict["email"]
        elif "nick" in self.request_dict:
            self.request_type = SearchType.NICK_SEARCH
            self.nick = self.request_dict["nick"]
        elif "email" in self.request_dict:
            self.request_type = SearchType.EMAIL_SEARCH
            self.email = self.request_dict["email"]
        else:
            raise GPParseException("unknown search type.")


class SearchUniqueRequest(RequestBase):
    uniquenick: str
    namespace_ids: list[int] = []

    def parse(self):
        super().parse()

        if (
            "uniquenick" not in self.request_dict
            or "namespaces" not in self.request_dict
        ):
            raise GPParseException("searchunique request is incomplete.")

        try:
            self.uniquenick = self.request_dict["uniquenick"]
        except KeyError:
            raise GPParseException("uniquenick is missing.")

        try:
            self.namespace_ids = [
                int(namespace_id)
                for namespace_id in self.request_dict["namespaces"]
                .lstrip(",")
                .split(",")
            ]
        except ValueError:
            raise GPParseException("namespaces is incorrect.")


class UniqueSearchRequest(RequestBase):
    preferred_nick: str
    game_name: str

    def parse(self):
        super().parse()

        if "preferrednick" not in self.request_dict:
            raise GPParseException("preferrednick is missing.")

        self.preferred_nick = self.request_dict["preferrednick"]

        if "gamename" not in self.request_dict:
            raise GPParseException("gamename is missing.")

        self.game_name = self.request_dict["gamename"]

        if "namespaceid" not in self.request_dict:
            raise GPParseException("namespaceid is missing.")

        try:
            self.namespace_id = int(self.request_dict["namespaceid"])
        except ValueError:
            raise GPParseException("namespaceid is incorrect.")


class ValidRequest(RequestBase):
    email: str

    def parse(self):
        super().parse()

        if "email" not in self.request_dict or not is_email_format_correct(
            self.request_dict["email"]
        ):
            raise GPParseException("valid request is incomplete.")

        self.email = self.request_dict["email"]
