from frontends.gamespy.library.exceptions.general import UniSpyException, get_exceptions_dict


class QRException(UniSpyException):
    pass


EXCEPTIONS = get_exceptions_dict(__name__)

