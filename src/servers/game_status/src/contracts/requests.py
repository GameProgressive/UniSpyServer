from library.src.extentions.gamespy_utils import convert_to_key_value
from servers.game_status.src.abstractions.contracts import RequestBase
from servers.game_status.src.enums.general import AuthMethod, PersistStorageType
from servers.game_status.src.exceptions.general import GSException


class AuthGameRequest(RequestBase):
    game_name: str

    def parse(self) -> None:
        super().parse()
        if "lid" not in self.request_dict and "id" not in self.request_dict:
            raise GSException("localid is missing")

        if "gamename" not in self.request_dict:
            raise GSException("gamename is missing")
        self.game_name = self.request_dict["gamename"]

        if "response" not in self.request_dict:
            raise GSException("response is missing")
        self.response = self.request_dict["response"]

        if "port" in self.request_dict:
            try:
                self.port = int(self.request_dict["port"])
            except:
                raise GSException("port format is incorrect")


class AuthPlayerRequest(RequestBase):
    auth_type: AuthMethod
    profile_id: int

    auth_token: str
    response: str
    cdkey_hash: str
    nick: str

    def parse(self) -> None:
        super().parse()
        if "lid" not in self.request_dict and "id" not in self.request_dict:
            raise GSException("localid is missing from auth game request")
        if "pid" in self.request_dict and "resp" in self.request_dict:
            try:
                self.profile_id = int(self.request_dict["profile_id"])
            except:
                raise GSException("profile id format is incorrect")
            self.auth_type = AuthMethod.PROFILE_ID_AUTH
        elif "authtoken" in self.request_dict and "response" in self.request_dict:
            self.auth_token = self.request_dict["authtoken"]
            self.response = self.request_dict["response"]
            self.auth_type = AuthMethod.PARTNER_ID_AUTH
        elif "keyhash" in self.request_dict and "nick" in self.request_dict:
            self.cdkey_hash = self.request_dict["keyhash"]
            self.nick = self.request_dict["nick"]

        else:
            raise GSException("unknown authp request type")


class GetPlayerDataRequest(RequestBase):
    profile_id: str
    storage_type: PersistStorageType
    data_index: int
    is_get_all_data: bool = False
    keys: list[str] = []

    def parse(self) -> None:
        super().parse()

        if "lid" not in self.request_dict and "id" not in self.request_dict:
            raise GSException("localid is missing from auth game request")

        if "pid" in self.request_dict:
            try:
                self.profile_id = int(self.request_dict["profile_id"])
            except:
                raise GSException("pid format is incorrect")

        if "ptype" in self.request_dict:
            try:
                self.storage_type = PersistStorageType(
                    int(self.request_dict["ptype"]))
            except:
                raise GSException("ptype format is incorrect")

        if "dindex" in self.request_dict:
            try:
                self.data_index = int(self.request_dict["dindex"])
            except:
                raise GSException("dindex format is incorrect")

        if "keys" not in self.request_dict:
            raise GSException("keys is missing")

        keys = self.request_dict["keys"]
        if not keys:
            self.is_get_all_data = True
        else:
            key_list = keys.split("\x1")
            for key in key_list:
                self.keys.append(key)
            self.is_get_all_data = False


class GetProfileIdRequest(RequestBase):
    nick: str
    keyhash: str

    def parse(self) -> None:
        super().parse()
        if "lid" not in self.request_dict and "id" not in self.request_dict:
            raise GSException("localid is missing from auth game request")

        if "nick" not in self.request_dict or "keyhash" not in self.request_dict:
            raise GSException("nick or keyhash is missing")

        if "nick" in self.request_dict:
            self.nick = self.request_dict["nick"]
        if "keyhash" in self.request_dict:
            self.keyhash = self.request_dict["keyhash"]


class NewGameRequest(RequestBase):
    is_client_local_storage_available: bool
    challenge: str = None
    connection_id: int = None
    session_key: str = None

    def parse(self) -> None:
        super().parse()
        if "sesskey" not in self.request_dict:
            raise GSException("sesskey is missing")

        self.session_key = self.request_dict["sesskey"]

        if "connid" not in self.request_dict:
            raise GSException("connid is missing")
        try:
            self.connection_id = int(self.request_dict["connid"])
        except:
            raise GSException("connid format is incorrect")

        if "challenge" in self.request_dict:
            self.challenge = self.request_dict["challenge"]


class SetPlayerDataRequest(RequestBase):
    profile_id: int
    storage_type: PersistStorageType
    data_index: int
    length: int
    report: str
    data: str

    def parse(self) -> None:
        super().parse()
        if "pid" not in self.request_dict:
            raise GSException("pid is missing")

        if "ptype" not in self.request_dict:
            raise GSException("ptype is missing")

        if "dindex" not in self.request_dict:
            raise GSException("dindex is missing")

        if "length" not in self.request_dict:
            raise GSException("length is missing")

        try:
            self.profile_id = int(self.request_dict["pid"])
        except:
            raise GSException("pid format is incorrect")

        try:
            self.storage_type = PersistStorageType(
                int(self.request_dict["ptype"]))
        except:
            raise GSException("ptype format is incorrect")

        try:
            self.data_index = int(self.request_dict["dindex"])
        except:
            raise GSException("dindex format is incorrect")

        try:
            self.length = int(self.request_dict["length"])
        except:
            raise GSException("length format is incorrect")

        if "report" in self.request_dict:
            self.report = self.request_dict["report"]

        if "data" in self.request_dict:
            self.data = self.request_dict["data"]


class UpdateGameRequest(RequestBase):
    connection_id: int = None
    is_done: bool
    is_client_local_storage_available: bool
    game_data: str
    game_data_dict: dict[str, str]
    session_key: str

    def parse(self) -> None:
        super().parse()

        if "gamedata" not in self.request_dict:
            raise GSException("gamedata is missing")
        self.game_data = self.request_dict["gamedata"]

        self.game_data_dict = convert_to_key_value(self.game_data)

        if "dl" in self.request_dict:
            self.is_client_local_storage_available = True

        if 'done' not in self.request_dict:
            raise GSException("done is missing")

        done = self.request_dict["done"]
        if done == "1":
            self.is_done = True

        elif done == "0":
            self.is_done = False
        else:
            raise GSException("done format is incorrect")

        if "sesskey" not in self.request_dict:
            raise GSException("sesskey is missing")

        self.session_key = self.request_dict["sesskey"]

        if "connid" in self.request_dict:
            try:
                self.connection_id = int(self.request_dict["connid"])
            except:
                raise GSException("connid format is incorrect")
