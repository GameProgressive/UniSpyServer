from backends.library.abstractions.contracts import DataResponse
from frontends.gamespy.protocols.presence_connection_manager.contracts.results import BlockListResult, BuddyListResult, BuddyMessageFriendAddResult, GetProfileResult, LoginResult


class LoginResponse(DataResponse):
    result: LoginResult


class BuddyListRetriveResponse(DataResponse):
    result: BuddyListResult


class BlockListRetriveResponse(DataResponse):
    result: BlockListResult


class GetProfileResponse(DataResponse):
    result: GetProfileResult


class BuddyMessageFriendAddResponse(DataResponse):
    result: BuddyMessageFriendAddResult
