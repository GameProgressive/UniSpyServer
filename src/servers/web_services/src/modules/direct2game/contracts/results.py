from enum import IntEnum

from servers.web_services.src.modules.direct2game.abstractions.contracts import ResultBase


class GetPurchaseHistoryResult(ResultBase):
    code: int = 0


class AvailableCode(IntEnum):
    STORE_ONLINE = 10
    STORE_OFFLINE_FOR_MAINTAIN = 20
    STORE_OFFLINE_RETIRED = 50
    STORE_NOT_YET_LAUNCHED = 100


class GetStoreAvailabilityResult(ResultBase):
    code: int = 0
    store_status_id: AvailableCode = AvailableCode.STORE_ONLINE
