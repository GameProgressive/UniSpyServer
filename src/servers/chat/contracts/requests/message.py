from servers.chat.abstractions.message import MessageRequestBase


class ATMRequest(MessageRequestBase):
    pass


class NoticeRequest(MessageRequestBase):
    pass


class PrivateRequest(MessageRequestBase):
    pass


class UTMRequest(MessageRequestBase):
    pass
