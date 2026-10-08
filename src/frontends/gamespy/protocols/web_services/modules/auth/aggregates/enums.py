from enum import Enum


class AuthCode(Enum):
    SUCCESS = 0
    SERVER_INIT_FAILED = 1
    USER_NOT_FOUND = 2
    INVALID_PASSWORD = 3
    INVALID_PROFILE = 4
    UNIQUENICK_EXPIRED = 5
    DB_ERROR = 6
    SERVER_ERROR = 7
    FAILURE_MAX = 0
    HTTP_ERROR = 100
    PARSE_ERROR = 101
    INVALID_GAMEID = 200
    INVALID_ACCESS_KEY = 201


class CommandName(Enum):
    LOGIN_PROFILE = "LoginProfile"
    LOGIN_PROFILE_WITH_GAME_ID = "LoginProfileWithGameId"
    LOGIN_PS3_CERT = "LoginPs3Cert"
    LOGIN_PS3_CERT_WITH_GAME_ID = "LoginPs3CertWithGameId"
    LOGIN_REMOTE_AUTH = "LoginRemoteAuth"
    LOGIN_REMOTE_AUTH_WITH_GAME_ID = "LoginRemoteAuthWithGameId"
    LOGIN_UNIQUENICK = "LoginUniqueNick"
    LOGIN_UNIQUENICK_WITH_GAME_ID = "LoginUniqueNickWithGameId"
    CREATE_USER_ACCOUNT = "CreateUserAccount"