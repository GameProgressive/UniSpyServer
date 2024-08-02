import hashlib

from servers.presence_connection_manager.src.enums.general import GPPartnerId, LoginType

SERVER_CHALLENGE = "0000000000"


class LoginChallengeProof:

    def __init__(
        self, userData, loginType, partnerID, challenge1, challenge2, passwordHash
    ):
        self.userData = userData
        self.loginType = loginType
        self.partnerID = partnerID
        self.challenge1 = challenge1
        self.challenge2 = challenge2
        self.passwordHash = passwordHash


@staticmethod
def generate_proof(data: LoginChallengeProof):
    tempUserData = data.userData

    if data.partnerID is not None:
        if (
            data.partnerID != GPPartnerId.GAMESPY.value
            and data.loginType != LoginType.AUTH_TOKEN
        ):
            tempUserData = f"{data.partnerID}@{data.userData}"

    responseString = f"{data.passwordHash} {' ' * 48}{tempUserData}{data.challenge1}{data.challenge2}{data.passwordHash}"
    hashString = hashlib.md5(responseString.encode()).hexdigest()

    return hashString
