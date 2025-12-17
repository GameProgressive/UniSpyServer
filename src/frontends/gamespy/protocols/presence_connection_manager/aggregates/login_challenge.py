import hashlib

from frontends.gamespy.protocols.presence_connection_manager.aggregates.enums import GPPartnerId, LoginType

SERVER_CHALLENGE = "0000000000"


class LoginChallengeProof:

    def __init__(
        self, userData: str, loginType: LoginType, partnerID: int, challenge1: str, challenge2: str, passwordHash: str
    ):
        self.userData = userData
        self.loginType = loginType
        self.partnerID = partnerID
        self.challenge1 = challenge1
        self.challenge2 = challenge2
        self.passwordHash = passwordHash

    def generate_proof(self):
        tempUserData = self.userData

        if self.partnerID is not None:
            if (
                self.partnerID != GPPartnerId.GAMESPY.value
                and self.loginType != LoginType.AUTH_TOKEN
            ):
                tempUserData = f"{self.partnerID}@{self.userData}"

        responseString = f"{self.passwordHash} {
            ' ' * 48}{tempUserData}{self.challenge1}{self.challenge2}{self.passwordHash}"
        hashString = hashlib.md5(responseString.encode()).hexdigest()
        return hashString
