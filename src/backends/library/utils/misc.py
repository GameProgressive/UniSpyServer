from frontends.gamespy.library.configs import CONFIG
from frontends.gamespy.library.exceptions.general import UniSpyException


def check_public_ip(real_ip: str, report_ip: str):
    if CONFIG.backend.is_check_public_ip:
        if real_ip != report_ip:
            raise UniSpyException(
                "client real ip is not equal to its config ip")
