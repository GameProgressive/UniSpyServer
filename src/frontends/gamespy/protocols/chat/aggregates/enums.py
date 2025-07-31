from enum import Enum, IntEnum


class LoginRequestType(IntEnum):
    UNIQUE_NICK_LOGIN = 1
    NICK_AND_EMAIL_LOGIN = 2


class WhoRequestType(IntEnum):
    GET_CHANNEL_USER_INFO = 0
    GET_USER_INFO = 1


class MessageType(IntEnum):
    CHANNEL_MESSAGE = 0
    USER_MESSAGE = 1


class GetKeyRequestType(IntEnum):
    GET_CHANNEL_ALL_USER_KEY_VALUE = 0
    GET_CHANNEL_SPECIFIC_USER_KEY_VALUE = 1


class ModeOperation(Enum):
    SET = "+"
    UNSET = "-"


class ModeName(Enum):
    USER_QUIET_FLAG = "q"
    CHANNEL_PASSWORD = "k"
    CHANNEL_USER_LIMITS = "l"
    BAN_ON_USER = "b"
    CHANNEL_OPERATOR = "co"
    USER_VOICE_PERMISSION = "cv"
    INVITED_ONLY = "i"
    PRIVATE_CHANNEL_FLAG = "p"
    SECRET_CHANNEL_FLAG = "s"
    MODERATED_CHANNEL_FLAG = "m"
    EXTERNAL_MESSAGES_FLAG = "n"
    TOPIC_CHANGE_BY_OPERATOR_FLAG = "t"
    OPERATOR_ABEY_CHANNEL_LIMITS = "e"


# class ModeOperationType(Enum):
#     ENABLE_USER_QUIET_FLAG = "+q"
#     DISABLE_USER_QUIET_FLAG = "-q"
#     ADD_CHANNEL_PASSWORD = "+k"
#     REMOVE_CHANNEL_PASSWORD = "-k"
#     ADD_CHANNEL_USER_LIMITS = "+l"
#     REMOVE_CHANNEL_USER_LIMITS = "-l"
#     ADD_BAN_ON_USER = "+b"
#     GET_BANNED_USERS = "+b"
#     REMOVE_BAN_ON_USER = "-b"
#     ADD_CHANNEL_OPERATOR = "+co"
#     REMOVE_CHANNEL_OPERATOR = "-co"
#     ENABLE_USER_VOICE_PERMISSION = "+cv"
#     DISABLE_USER_VOICE_PERMISSION = "-cv"
#     SET_INVITED_ONLY = "+i"
#     REMOVE_INVITED_ONLY = "-i"
#     SET_PRIVATE_CHANNEL_FLAG = "+p"
#     REMOVE_PRIVATE_CHANNEL_FLAG = "-p"
#     SET_SECRET_CHANNEL_FLAG = "+s"
#     REMOVE_SECRET_CHANNEL_FLAG = "-s"
#     SET_MODERATED_CHANNEL_FLAG = "+m"
#     REMOVE_MODERATED_CHANNEL_FLAG = "-m"
#     ENABLE_EXTERNAL_MESSAGES_FLAG = "+n"
#     DISABLE_EXTERNAL_MESSAGES_FLAG = "-n"
#     SET_TOPIC_CHANGE_BY_OPERATOR_FLAG = "+t"
#     REMOVE_TOPIC_CHANGE_BY_OPERATOR_FLAG = "-t"
#     SET_OPERATOR_ABEY_CHANNEL_LIMITS = "+e"
#     REMOVE_OPERATOR_ABEY_CHANNEL_LIMITS = "-e"


class ModeRequestType(IntEnum):
    GET_CHANNEL_MODES = 0
    SET_CHANNEL_MODES = 1
    SET_CHANNEL_USER_MODES = 2


class TopicRequestType(IntEnum):
    GET_CHANNEL_TOPIC = 0
    SET_CHANNEL_TOPIC = 1


class ResponseCode(Enum):
    WELCOME = "001"
    USRIP = "302"
    WHOISUSER = "311"
    ENDOFWHO = "315"
    ENDOFWHOIS = "318"
    WHOISCHANNELS = "319"
    LISTSTART = "321"
    LIST = "322"
    LISTEND = "323"
    CHANNELMODEIS = "324"
    NOTOPIC = "331"
    TOPIC = "332"
    WHOREPLY = "352"
    NAMEREPLY = "353"
    ENDOFNAMES = "366"
    BANLIST = "367"
    ENDOFBANLIST = "368"
    GETKEY = "700"
    ENDGETKEY = "701"
    GETCKEY = "702"
    ENDGETCKEY = "703"
    GETCHANKEY = "704"
    SECUREKEY = "705"
    CDKEY = "706"
    LOGIN = "707"
    GETUDPRELAY = "712"
    PONG = "PONG"
    JOIN = "JOIN"
    KICK = "KICK"
    QUIT = "QUIT"
    PART = "PART"
    ATM = "ATM"
    UTM = "UTM"
    PRIVMSG = "PRIVMSG"
    NOTICE = "NOTICE"


# region IRC error code
class IRCErrorCode(Enum):
    NO_SUCH_NICK = "401"
    NO_SUCH_CHANNEL = "403"
    TOO_MANY_CHANNELS = "405"
    ERR_ONE_US_NICK_NAME = "432"
    NICK_NAME_IN_USE = "433"
    MORE_PARAMETERS = "461"
    CHANNEL_IS_FULL = "471"
    INVITE_ONLY_CHAN = "473"
    BANNED_FROM_CHAN = "474"
    BAD_CHANNEL_KEY = "475"
    BAD_CHAN_MASK = "476"
    LOGIN_FAILED = "708"
    NO_UNIQUE_NICK = "709"
    UNIQUE_NICK_EXPIRED = "710"
    REGISTER_NICK_FAILED = "711"


# region Peer room


class PeerRoomType(IntEnum):
    Group = 0
    Staging = 1
    Title = 2
    Normal = 3


class RequestType(Enum):
    CRYPT = "CRYPT"
    CDKEY = "CDKEY"
    GETKEY = "GETKEY"
    LIST = "LIST"
    LOGIN = "LOGIN"
    NICK = "NICK"
    PING = "PING"
    QUIT = "QUIT"
    SETKEY = "SETKEY"
    USER = "USER"
    USRIP = "USRIP"
    WHO = "WHO"
    WHOIS = "WHOIS"
    GETCHANKEY = "GETCHANKEY"
    GETCKEY = "GETCKEY"
    JOIN = "JOIN"
    KICK = "KICK"
    MODE = "MODE"
    NAMES = "NAMES"
    PART = "PART"
    SETCHANKEY = "SETCHANKEY"
    SETCKEY = "SETCKEY"
    TOPIC = "TOPIC"
    ATM = "ATM"
    NOTICE = "NOTICE"
    PRIVMSG = "PRIVMSG"
    UTM = "UTM"
