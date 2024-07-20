from servers.presence_connection_manager.src.abstractions.contracts import ResultBase
from servers.presence_connection_manager.src.aggregates.user_status import UserStatus
from servers.presence_connection_manager.src.aggregates.user_status_info import (
    UserStatusInfo,
)


class AddBuddyResult(ResultBase):
    pass


class BlockListResult(ResultBase):
    profile_ids: list[int]


class BuddyListResult(ResultBase):
    profile_ids: list[int]


class StatusInfoResult(ResultBase):
    profile_id: int
    product_id: int
    status_info: UserStatusInfo


class StatusResult(ResultBase):
    status: UserStatus


# class NewUserResult()