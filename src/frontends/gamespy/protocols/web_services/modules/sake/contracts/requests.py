from frontends.gamespy.library.abstractions.contracts import HttpData
from frontends.gamespy.protocols.web_services.abstractions.contracts import RequestBase
from frontends.gamespy.protocols.web_services.modules.sake.abstractions.contracts import (
    RequestBase,
)
from frontends.gamespy.protocols.web_services.modules.sake.aggregates.exceptions import (
    SakeException,
)


class CreateRecordRequest(RequestBase):
    records: dict
    """
    (name,type,value)
    """

    @staticmethod
    def gs_parse(raw_request: HttpData) -> "CreateRecordRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["records"] = RequestBase._get_record_field(data)
        data = CreateRecordRequest(**data_dict)
        return data


class DeleteRecordRequest(RequestBase):
    record_id: int

    @staticmethod
    def gs_parse(raw_request: HttpData) -> "DeleteRecordRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["record_id"] = data.int_mapping["recordid"]
        data = DeleteRecordRequest(**data_dict)
        return data


class GetMyRecordsRequest(RequestBase):
    fields: list

    @staticmethod
    def gs_parse(raw_request: HttpData) -> "GetMyRecordsRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["fields"] = data.dict_mapping["fields"]["string"]
        data = GetMyRecordsRequest(**data_dict)
        return data


class GetRandomRecordsRequest(RequestBase):
    max_value: int
    fields: list

    @staticmethod
    def gs_parse(raw_request: HttpData) -> "GetRandomRecordsRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["max_value"] = data.int_mapping["max"]
        data_dict["fields"] = data.dict_mapping["fields"]["string"]
        data = GetRandomRecordsRequest(**data_dict)
        return data


class GetRecordLimitRequest(RequestBase):
    pass


class GetSpecificRecordsRequest(RequestBase):
    record_ids: dict
    """
    [
    (field_name,field_type),
    (field_name,field_type),
    (field_name,field_type),
    ...
    (field_name,field_type)
    ]
    """
    fields: dict
    """
    [
    (field_name,field_type),
    (field_name,field_type),
    (field_name,field_type),
    ...
    (field_name,field_type)
    ]
    """

    @staticmethod
    def gs_parse(raw_request: HttpData) -> "GetSpecificRecordsRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["record_ids"] = data.dict_mapping["recordids"]
        data_dict["fields"] = data.dict_mapping["fields"]
        data = GetSpecificRecordsRequest(**data_dict)
        return data


class RateRecordRequest(RequestBase):
    record_id: str
    rating: str

    @staticmethod
    def gs_parse(raw_request: HttpData) -> "RateRecordRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["record_id"] = data.str_mapping["recordid"]
        data_dict["rating"] = data.str_mapping["rating"]

        data = RateRecordRequest(**data_dict)
        return data


class SearchForRecordsRequest(RequestBase):
    offset: str
    max_value: int
    surrounding: str | None
    cache_flag: str | None
    fields: list[str]
    """
    [
    (field_name, field_type),
    (field_name, field_type),
    (field_name, field_type),
    ...
    (field_name, field_type)
    ]
    """
    owner_ids: list | None
    sort: str | None
    filter: str | None

    @staticmethod
    def gs_parse(raw_request: HttpData) -> "SearchForRecordsRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["offset"] = data.str_mapping["offset"]
        data_dict["max_value"] = data.int_mapping["max"]
        data_dict["fields"] = data.dict_mapping["fields"]

        data_dict["surrounding"] = data.str_mapping.get("surrounding")
        data_dict["sort"] = data.str_mapping.get("sort")

        data_dict["owner_ids"] = (
            list(data.dict_mapping["ownerids"].values())
            if data.dict_mapping.get("ownerids") is not None
            else None
        )

        data_dict["cache_flag"] = data.str_mapping.get("cacheFlag")

        temp_filter = data.str_mapping.get("filter")
        data_dict["filter"] = (
            temp_filter.translate(
                str.maketrans(
                    {
                        " &gt; ": " > ",
                        " &lt; ": " < ",
                        " &ge; ": " >= ",
                        " &le; ": " <= ",
                    }
                )
            )
            if temp_filter is not None
            else None
        )

        data = SearchForRecordsRequest(**data_dict)
        return data


class UpdateRecordRequest(RequestBase):
    record_id: str
    records: dict
    """
    [
    (field_name,field_type,field_value),
    (field_name,field_type,field_value),
    (field_name,field_type,field_value),
    ...
    (field_name,field_type,field_value)
    ]
    """

    @staticmethod
    def gs_parse(raw_request: HttpData) -> "UpdateRecordRequest":
        data = RequestBase.gs_parse(raw_request)
        data_dict = data.model_dump()
        data_dict["record_id"] = data.str_mapping["recordid"]
        data_dict["records"] = RequestBase._get_record_field(data)
        data = UpdateRecordRequest(**data_dict)
        return data


class GetRecordCountRequest(RequestBase):
    pass
