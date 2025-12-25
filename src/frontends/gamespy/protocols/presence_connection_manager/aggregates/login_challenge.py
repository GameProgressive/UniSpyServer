import hashlib

from frontends.gamespy.protocols.presence_connection_manager.aggregates.enums import GPPartnerId, LoginType

SERVER_CHALLENGE = "0000000000"


def generate_proof(userData: str, loginType: LoginType, partnerID: int, challenge1: str, challenge2: str, password_hash: str):
    temp_user_data = userData

    if partnerID is not None:
        if (
            partnerID != GPPartnerId.GAMESPY.value
            and loginType != LoginType.AUTH_TOKEN
        ):
            temp_user_data = f"{partnerID}@{userData}"

    data_to_hash = f"{password_hash} {' ' * 47}{temp_user_data}{challenge1}{challenge2}{password_hash}"
    hash_hex = hashlib.md5(data_to_hash.encode()).hexdigest()
    return hash_hex