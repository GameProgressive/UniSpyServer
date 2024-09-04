from typing import Optional
from library.src.extentions.gamespy_utils import is_valid_date
from servers.presence_connection_manager.src.abstractions.contracts import RequestBase
from servers.presence_connection_manager.src.enums.general import PublicMasks
from servers.presence_search_player.src.exceptions.general import (
    GPParseException,
)


class AddBlockRequest(RequestBase):
    taget_id: int

    def parse(self):
        super().parse()

        if "profileid" not in self.request_dict:
            raise GPParseException("profileid is missing")

        try:
            self.taget_id = int(self.request_dict["profileid"])
        except ValueError:
            raise GPParseException("profileid format is incorrect")


class GetProfileRequest(RequestBase):
    profile_id: int
    session_key: str

    def parse(self):
        super().parse()

        if "profileid" not in self.request_dict:
            raise GPParseException("profileid is missing")

        try:
            self.profile_id = int(self.request_dict["profileid"])
        except ValueError:
            raise GPParseException("profileid format is incorrect")

        if "sesskey" not in self.request_dict:
            raise GPParseException("sesskey is missing")

        self.session_key = self.request_dict["sesskey"]


class NewProfileRequest(RequestBase):
    is_replace_nick_name: bool
    session_key: str
    new_nick: str
    old_nick: str

    def parse(self):
        super().parse()

        if "sesskey" not in self.request_dict:
            raise GPParseException("sesskey is missing")

        self.session_key = self.request_dict["sesskey"]

        if "replace" in self.request_dict:
            if (
                "oldnick" not in self.request_dict
                and "nick" not in self.request_dict
            ):
                raise GPParseException("oldnick or nick is missing.")

            if "oldnick" in self.request_dict:
                self.old_nick = self.request_dict["oldnick"]
            if "nick" in self.request_dict:
                self.new_nick = self.request_dict["nick"]

            self.is_replace_nick_name = True
        else:
            if "nick" not in self.request_dict:
                raise GPParseException("nick is missing.")

            self.new_nick = self.request_dict["nick"]
            self.is_replace_nick_name = False


class RegisterCDKeyRequest(RequestBase):
    session_key: str
    cdkey_enc: str

    def parse(self):
        super().parse()

        if "sesskey" not in self.request_dict:
            raise GPParseException("sesskey is missing")

        self.session_key = self.request_dict["sesskey"]

        if "cdkeyenc" not in self.request_dict:
            raise GPParseException("cdkeyenc is missing")

        self.cdkey_enc = self.request_dict["cdkeyenc"]


class RegisterNickRequest(RequestBase):
    unique_nick: str
    session_key: str
    partner_id: int

    def parse(self):
        super().parse()

        if "sesskey" not in self.request_dict:
            raise GPParseException("sesskey is missing")

        self.session_key = self.request_dict["sesskey"]

        if "uniquenick" not in self.request_dict:
            raise GPParseException("uniquenick is missing")

        self.unique_nick = self.request_dict["uniquenick"]

        if "partnerid" in self.request_dict:
            try:
                self.partner_id = int(self.request_dict["partnerid"])
            except ValueError:
                raise GPParseException("partnerid is missing")


class UpdateProfileRequest(RequestBase):
    has_public_mask_flag: Optional[bool] = None
    public_mask: Optional[PublicMasks] = None
    session_key: Optional[str] = None
    partner_id: Optional[int] = None
    nick: Optional[str] = None
    uniquenick: Optional[str] = None
    has_first_name_flag: Optional[bool] = None
    first_name: Optional[str] = None
    has_last_name_flag: Optional[bool] = None
    last_name: Optional[str] = None
    has_icq_flag: Optional[bool] = None
    icq_uin: Optional[int] = None
    has_home_page_flag: Optional[bool] = None
    home_page: Optional[str] = None
    has_birthday_flag: bool = False
    birth_day: Optional[int] = None
    birth_month: Optional[int] = None
    birth_year: Optional[int] = None
    has_sex_flag: bool = False
    sex: Optional[int] = None
    has_zip_code: bool = False
    zip_code: Optional[str] = None
    has_country_code: bool = False
    country_code: Optional[str] = None

    def parse(self):
        super().parse()

        if "publicmask" in self.request_dict:
            if not self.request_dict["publicmask"].isdigit():
                raise GPParseException("publicmask format is incorrect")
            self.has_public_mask_flag = True
            self.public_mask = PublicMasks(
                int(self.request_dict["publicmask"]))

        if "sesskey" not in self.request_dict:
            raise GPParseException("sesskey is missing")
        self.session_key = self.request_dict["sesskey"]

        if "firstname" in self.request_dict:
            self.first_name = self.request_dict["firstname"]
            self.has_first_name_flag = True

        if "lastname" in self.request_dict:
            self.last_name = self.request_dict["lastname"]
            self.has_last_name_flag = True

        if "icquin" in self.request_dict:
            if not self.request_dict["icquin"].isdigit():
                raise GPParseException("icquin format is incorrect")
            self.has_icq_flag = True
            self.icq_uin = int(self.request_dict["icquin"])

        # Remaining attribute assignments...
        if "homepage" in self.request_dict:
            self.home_page = self.request_dict["homepage"]
            self.has_home_page_flag = True

        if "birthday" in self.request_dict:
            try:
                date = int(self.request_dict["birthday"])
                d = (date >> 24) & 0xFF
                m = (date >> 16) & 0xFF
                y = date & 0xFFFF
                if is_valid_date(d, m, y):
                    self.birth_day = d
                    self.birth_month = m
                    self.birth_year = y
                    self.has_birthday_flag = True
            except ValueError:
                pass

        if "sex" in self.request_dict:
            try:
                self.sex = int(self.request_dict["sex"])
                self.has_sex_flag = True
            except ValueError:
                raise GPParseException("sex format is incorrect")

        if "zipcode" in self.request_dict:
            self.zip_code = self.request_dict["zipcode"]
            self.has_zip_code = True

        if "countrycode" in self.request_dict:
            self.country_code = self.request_dict["countrycode"]
            self.has_country_code = True

        if "partnerid" in self.request_dict:
            try:
                self.partner_id = int(self.request_dict["partnerid"])
            except ValueError:
                raise GPParseException("partnerid is incorrect")

        if "nick" in self.request_dict:
            self.nick = self.request_dict["nick"]

        if "uniquenick" in self.request_dict:
            self.uniquenick = self.request_dict["uniquenick"]


class UpdateUserInfoRequest(RequestBase):
    cpubrandid: Optional[str] = None
    cpuspeed: Optional[str] = None
    memory: Optional[str] = None
    videocard1ram: Optional[str] = None
    videocard2ram: Optional[str] = None
    connectionid: Optional[str] = None
    connectionspeed: Optional[str] = None
    hasnetwork: Optional[str] = None
    pic: Optional[str] = None

    def parse(self):
        super().parse()

        if "cpubrandid" in self.request_dict:
            self.cpubrandid = self.request_dict["cpubrandid"]

        if "cpuspeed" in self.request_dict:
            self.cpuspeed = self.request_dict["cpuspeed"]

        if "memory" in self.request_dict:
            self.memory = self.request_dict["memory"]

        if "videocard1ram" in self.request_dict:
            self.videocard1ram = self.request_dict["videocard1ram"]

        if "videocard2ram" in self.request_dict:
            self.videocard2ram = self.request_dict["videocard2ram"]

        if "connectionid" in self.request_dict:
            self.connectionid = self.request_dict["connectionid"]

        if "connectionspeed" in self.request_dict:
            self.connectionspeed = self.request_dict["connectionspeed"]

        if "hasnetwork" in self.request_dict:
            self.hasnetwork = self.request_dict["hasnetwork"]

        if "pic" in self.request_dict:
            self.pic = self.request_dict["pic"]
