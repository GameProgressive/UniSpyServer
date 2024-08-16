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

        if "profileid" not in self.request_key_values:
            raise GPParseException("profileid is missing")

        try:
            self.taget_id = int(self.request_key_values["profileid"])
        except ValueError:
            raise GPParseException("profileid format is incorrect")


class GetProfileRequest(RequestBase):
    profile_id: int
    session_key: str

    def parse(self):
        super().parse()

        if "profileid" not in self.request_key_values:
            raise GPParseException("profileid is missing")

        try:
            self.profile_id = int(self.request_key_values["profileid"])
        except ValueError:
            raise GPParseException("profileid format is incorrect")

        if "sesskey" not in self.request_key_values:
            raise GPParseException("sesskey is missing")

        self.session_key = self.request_key_values["sesskey"]


class NewProfileRequest(RequestBase):
    is_replace_nick_name: bool
    session_key: str
    new_nick: str
    old_nick: str

    def parse(self):
        super().parse()

        if "sesskey" not in self.request_key_values:
            raise GPParseException("sesskey is missing")

        self.session_key = self.request_key_values["sesskey"]

        if "replace" in self.request_key_values:
            if (
                "oldnick" not in self.request_key_values
                and "nick" not in self.request_key_values
            ):
                raise GPParseException("oldnick or nick is missing.")

            if "oldnick" in self.request_key_values:
                self.old_nick = self.request_key_values["oldnick"]
            if "nick" in self.request_key_values:
                self.new_nick = self.request_key_values["nick"]

            self.is_replace_nick_name = True
        else:
            if "nick" not in self.request_key_values:
                raise GPParseException("nick is missing.")

            self.new_nick = self.request_key_values["nick"]
            self.is_replace_nick_name = False


class RegisterCDKeyRequest(RequestBase):
    session_key: str
    cdkey_enc: str

    def parse(self):
        super().parse()

        if "sesskey" not in self.request_key_values:
            raise GPParseException("sesskey is missing")

        self.session_key = self.request_key_values["sesskey"]

        if "cdkeyenc" not in self.request_key_values:
            raise GPParseException("cdkeyenc is missing")

        self.cdkey_enc = self.request_key_values["cdkeyenc"]


class RegisterNickRequest(RequestBase):
    unique_nick: str
    session_key: str
    partner_id: int

    def parse(self):
        super().parse()

        if "sesskey" not in self.request_key_values:
            raise GPParseException("sesskey is missing")

        self.session_key = self.request_key_values["sesskey"]

        if "uniquenick" not in self.request_key_values:
            raise GPParseException("uniquenick is missing")

        self.unique_nick = self.request_key_values["uniquenick"]

        if "partnerid" in self.request_key_values:
            try:
                self.partner_id = int(self.request_key_values["partnerid"])
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

        if "publicmask" in self.request_key_values:
            if not self.request_key_values["publicmask"].isdigit():
                raise GPParseException("publicmask format is incorrect")
            self.has_public_mask_flag = True
            self.public_mask = PublicMasks(int(self.request_key_values["publicmask"]))

        if "sesskey" not in self.request_key_values:
            raise GPParseException("sesskey is missing")
        self.session_key = self.request_key_values["sesskey"]

        if "firstname" in self.request_key_values:
            self.first_name = self.request_key_values["firstname"]
            self.has_first_name_flag = True

        if "lastname" in self.request_key_values:
            self.last_name = self.request_key_values["lastname"]
            self.has_last_name_flag = True

        if "icquin" in self.request_key_values:
            if not self.request_key_values["icquin"].isdigit():
                raise GPParseException("icquin format is incorrect")
            self.has_icq_flag = True
            self.icq_uin = int(self.request_key_values["icquin"])

        # Remaining attribute assignments...
        if "homepage" in self.request_key_values:
            self.home_page = self.request_key_values["homepage"]
            self.has_home_page_flag = True

        if "birthday" in self.request_key_values:
            try:
                date = int(self.request_key_values["birthday"])
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

        if "sex" in self.request_key_values:
            try:
                self.sex = int(self.request_key_values["sex"])
                self.has_sex_flag = True
            except ValueError:
                raise GPParseException("sex format is incorrect")

        if "zipcode" in self.request_key_values:
            self.zip_code = self.request_key_values["zipcode"]
            self.has_zip_code = True

        if "countrycode" in self.request_key_values:
            self.country_code = self.request_key_values["countrycode"]
            self.has_country_code = True

        if "partnerid" in self.request_key_values:
            try:
                self.partner_id = int(self.request_key_values["partnerid"])
            except ValueError:
                raise GPParseException("partnerid is incorrect")

        if "nick" in self.request_key_values:
            self.nick = self.request_key_values["nick"]

        if "uniquenick" in self.request_key_values:
            self.uniquenick = self.request_key_values["uniquenick"]


class UpdateUiRequest(RequestBase):
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

        if "cpubrandid" in self.request_key_values:
            self.cpubrandid = self.request_key_values["cpubrandid"]

        if "cpuspeed" in self.request_key_values:
            self.cpuspeed = self.request_key_values["cpuspeed"]

        if "memory" in self.request_key_values:
            self.memory = self.request_key_values["memory"]

        if "videocard1ram" in self.request_key_values:
            self.videocard1ram = self.request_key_values["videocard1ram"]

        if "videocard2ram" in self.request_key_values:
            self.videocard2ram = self.request_key_values["videocard2ram"]

        if "connectionid" in self.request_key_values:
            self.connectionid = self.request_key_values["connectionid"]

        if "connectionspeed" in self.request_key_values:
            self.connectionspeed = self.request_key_values["connectionspeed"]

        if "hasnetwork" in self.request_key_values:
            self.hasnetwork = self.request_key_values["hasnetwork"]

        if "pic" in self.request_key_values:
            self.pic = self.request_key_values["pic"]
