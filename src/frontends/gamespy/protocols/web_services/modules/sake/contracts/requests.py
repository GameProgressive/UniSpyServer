from frontends.gamespy.protocols.web_services.modules.sake.abstractions.contracts import (
    RequestBase,
)
from frontends.gamespy.protocols.web_services.modules.sake.aggregates.exceptions import (
    SakeException,
)


class CreateRecordRequest(RequestBase):
    records: list
    """
    (name,type,value)
    """

    def parse(self) -> None:
        super().parse()
        self.records = self._get_record_field()

    def _get_dict(self, attr_name: str) -> dict:
        try:
            return super()._get_dict(attr_name)
        except Exception as _:
            raise SakeException(f"{attr_name} is missing", self.command_name)


class DeleteRecordRequest(RequestBase):
    record_id: int

    def parse(self) -> None:
        super().parse()
        self.record_id = self._get_int("recordid")


class GetMyRecordsRequest(RequestBase):
    fields: list

    def parse(self) -> None:
        super().parse()
        fields = self._get_dict("fields")["string"]
        if isinstance(fields, str):
            fields = [fields]
        if not isinstance(fields, list):
            raise SakeException("fields is not list type", self.command_name)
        self.fields = fields


class GetRandomRecordsRequest(RequestBase):
    max: int
    fields: list

    def parse(self) -> None:
        super().parse()
        self.max = self._get_int("max")
        self.fields = self._get_dict("fields")["string"]


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

    def parse(self) -> None:
        super().parse()
        self.record_ids = self._get_dict("recordids")
        self.fields = self._get_dict("fields")


class RateRecordRequest(RequestBase):
    record_id: str
    rating: str

    def parse(self) -> None:
        super().parse()
        self.record_id = self._get_str("recordid")
        self.rating = self._get_str("rating")


class SearchForRecordsRequest(RequestBase):
    offset: str
    max: int
    surrounding: str | None
    cache_flag: str | None
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
    owner_ids: str | None
    sort: str | None
    filter: str | None

    def __init__(self, raw_request: str) -> None:
        super().__init__(raw_request)
        self.filter = None
        self.sort = None
        self.owner_ids = None
        self.surrounding = None
        self.catch_flag = None

    def parse(self) -> None:
        super().parse()
        filter = self._get_value_by_key("filter")
        if filter is not None:
            self.filter = self._get_str("filter")
        sort = self._get_value_by_key("sort")
        if sort is not None:
            self.sort = self._get_str("sort")
        self.offset = self._get_str("offset")
        self.max = self._get_int("max")

        surrounding = self._get_value_by_key("surrounding")
        if surrounding is not None:
            self.surrounding = self._get_str("surrounding")

        owner_ids = self._get_value_by_key("ownerids")
        if owner_ids is not None:
            owner_dict = self._get_dict("ownerids")
            self.owner_ids = list(owner_dict.values())[0]
        cache_flag = self._get_value_by_key("cacheFlag")
        if cache_flag is not None:
            self.cache_flag = self._get_str("cacheFlag")
            self.fields = self._get_dict("fields")["string"]


class UpdateRecordRequest(RequestBase):
    record_id: str
    records: list
    """
    [
    (field_name,field_type,field_value),
    (field_name,field_type,field_value),
    (field_name,field_type,field_value),
    ...
    (field_name,field_type,field_value)
    ]
    """

    def parse(self) -> None:
        super().parse()
        self.record_id = self._get_str("recordid")
        self.records = self._get_record_field()
