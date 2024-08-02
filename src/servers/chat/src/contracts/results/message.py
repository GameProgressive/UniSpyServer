from servers.chat.src.abstractions.message import MessageResultBase


class ATMResult(MessageResultBase):
    pass


class NoticeResult(MessageResultBase):
    pass


class PrivateResult(MessageResultBase):
    is_broadcast_message: bool = False


class UTMResult(MessageResultBase):
    pass
