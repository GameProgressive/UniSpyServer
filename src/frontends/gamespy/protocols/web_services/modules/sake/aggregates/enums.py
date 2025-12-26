from enum import Enum


class SakeCode(Enum):
    SUCCESS = "Success"
    SECRET_KEY_INVALID = "SecretKeyInvalid"
    SERVICE_DISABLED = "ServiceDisabled"
    DATABASE_UNAVAILABLE = "DatabaseUnavailable"
    LOGIN_TICKET_INVALID = "LoginTicketInvalid"
    LOGIN_TICKET_EXPIRED = "LoginTicketExpired"
    TABLE_NOT_FOUND = "TableNotFound"
    RECORD_NOT_FOUND = "RecordNotFound"
    FIELD_NOT_FOUND = "FieldNotFound"
    FIELD_TYPE_INVALID = "FieldTypeInvalid"
    NO_PERMISSION = "NoPermission"
    RECORD_LIMIT_REACHED = "RecordLimitReached"
    ALREADY_RATED = "AlreadyRated"
    NOT_RATABLE = "NotRateable"
    NOT_OWNED = "NotOwned"
    FILTER_INVALID = "FilterInvalid"
    SORT_INVALID = "SortInvalid"
    TARGET_FILTER_INVALID = "TargetFilterInvalid"


class SakePlatform(Enum):
    """
    determine different platform
    """
    Windows = "WINDOWS"
    Unity = "UNITY"


class CommandName(Enum):
    CREATE_RECORD = "CreateRecord"
    GET_MY_RECORD = "GetMyRecords"
    SEARCH_FOR_RECORD = "SearchForRecords"
    GET_SPECIFIC_RECORD = "GetSpecificRecords"
    GET_RAMDOM_RECORD = "GetRandomRecords"
    GET_RECORD_LIMIT = "GetRecordLimit"
    RATE_RECORD = "RateRecord"
    DELETE_RECORD = "DeleteRecord"
    UPDATE_RECORD = "UpdateRecord"
