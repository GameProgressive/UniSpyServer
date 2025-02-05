from frontends.gamespy.protocols.presence_connection_manager.aggregates.enums import SdkRevisionType


class SdkRevision:
    def __init__(self, sdk_type: SdkRevisionType):
        assert isinstance(sdk_type, SdkRevisionType)
        self.sdk_type = sdk_type

    @property
    def is_sdk_revision_valid(self):
        return False if self.sdk_type == 0 else True

    @property
    def is_support_gpi_new_auth_notification(self):
        return (
            True
            if (self.sdk_type ^ SdkRevisionType.GPINEW_AUTH_NOTIFICATION) != 0
            else False
        )

    @property
    def is_support_gpi_new_revoke_notification(self):
        return (
            True
            if (self.sdk_type ^ SdkRevisionType.GPINEW_REVOKE_NOTIFICATION) != 0
            else False
        )

    @property
    def is_support_gpi_new_status_notification(self):
        return (
            True
            if (self.sdk_type ^ SdkRevisionType.GPINEW_STATUS_NOTIFICATION) != 0
            else False
        )

    @property
    def is_support_gpi_new_list_retreval_on_login(self):
        return (
            True
            if (self.sdk_type ^ SdkRevisionType.GPINEW_LIST_RETRIEVAL_ON_LOGIN) != 0
            else False
        )

    @property
    def is_support_gpi_remote_auth_ids_notification(self):
        return (
            True
            if (self.sdk_type ^ SdkRevisionType.GPIREMOTE_AUTH_IDS_NOTIFICATION) != 0
            else False
        )

    @property
    def is_support_gpi_new_cdkey_registration(self):
        return (
            True
            if (self.sdk_type ^ SdkRevisionType.GPINEW_CD_KEY_REGISTRATION) != 0
            else False
        )
