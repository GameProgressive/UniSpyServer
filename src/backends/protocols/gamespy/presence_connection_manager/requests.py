from typing import Any, Optional, Union

from pydantic import ValidationError

from frontends.gamespy.protocols.presence_connection_manager.aggregates.enums import (
    GPStatusCode,
    LoginType,
    SdkRevisionType,
)

from backends.library.abstractions.contracts import RequestBase
from frontends.gamespy.protocols.presence_search_player.aggregates.exceptions import (
    GPException,
)
import frontends.gamespy.protocols.presence_connection_manager.contracts.requests as fnt


class ErrorOnParse(RequestBase):
    raw_request: str


# region buddy


class AddBlockRequest(RequestBase):
    data: fnt.AddBlockRequest


class RemoveBlockRequest(RequestBase):
    data: fnt.RemoveBlockRequest


class BuddyListRetriveRequest(RequestBase):
    data: fnt.BuddyListRetriveRequest


class BlockListRetriveRequest(RequestBase):
    data: fnt.BlockListRetriveRequest


class AddBuddyRequest(RequestBase):
    data: fnt.AddBuddyRequest


class BuddyMessageFriendAddRequest(RequestBase):
    data: fnt.BuddyMessageFriendAddRequest


class DelBuddyRequest(RequestBase):
    data: fnt.DelBuddyRequest


class AuthAddBuddyRequest(RequestBase):
    data: fnt.AuthAddBuddyRequest


class InviteToRequest(RequestBase):
    data: fnt.InviteToRequest


class StatusInfoRequest(RequestBase):
    data: fnt.StatusInfoRequest


class StatusRequest(RequestBase):
    data: fnt.StatusRequest


# region general


class KeepAliveRequest(RequestBase):
    data: fnt.KeepAliveRequest


class NewUserRequest(RequestBase):
    data: fnt.NewUserRequest


class LoginRequest(RequestBase):
    data: fnt.LoginRequest

    def model_post_init(self, __context: Any) -> None:
        super().model_post_init(__context)
        if self.data.type == LoginType.AUTH_TOKEN:
            if self.data.auth_token is None:
                raise GPException("authtoken is missing.")
        elif self.data.type == LoginType.UNIQUENICK_NAMESPACE_ID:
            if self.data.unique_nick is None:
                raise GPException("unique nick is missing.")
            if self.data.namespace_id is None:
                raise GPException("namespace is missing.")
        elif self.data.type == LoginType.NICK_EMAIL:
            if self.data.nick is None:
                raise GPException("nick name is missing.")
            if self.data.email is None:
                raise GPException("email is missing.")
        else:
            raise GPException(f"request type {self.data.type} not found.")


class LogoutRequest(RequestBase):
    data: fnt.LogoutRequest



# region profile


class GetProfileRequest(RequestBase):
    data: fnt.GetProfileRequest



class NewProfileRequest(RequestBase):
    data: fnt.NewProfileRequest



class RegisterCDKeyRequest(RequestBase):
    data: fnt.RegisterCDKeyRequest



class RegisterNickRequest(RequestBase):
    data: fnt.RegisterNickRequest



class UpdateProfileRequest(RequestBase):
    data: fnt.UpdateProfileRequest



class UpdateUserInfoRequest(RequestBase):
    data: fnt.UpdateUserInfoRequest



if __name__ == "__main__":
    pass
