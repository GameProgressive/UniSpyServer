
from frontends.gamespy.library.network.http_handler import HttpData
from frontends.gamespy.protocols.web_services.modules.sake.abstractions.contracts import (
    RequestBase,)
from frontends.gamespy.protocols.web_services.modules.sake.aggregates.exceptions import SakeException


class CreateRecordRequest(RequestBase):
    values: dict
    """
    (name,type,value)
    """

    def parse(self) -> None:
        super().parse()
        self.values = self._get_dict("values")

    def _get_dict(self, attr_name: str) -> dict:
        try:
            return super()._get_dict(attr_name)
        except:
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
        self.fields = self._get_dict("fields")['string']


class GetRandomRecordsRequest(RequestBase):
    max: int
    fields: list

    def parse(self) -> None:
        super().parse()
        self.max = self._get_int("max")
        self.fields = self._get_dict("fields")['string']


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
    surrounding: str
    cache_flag: str
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

    def __init__(self, raw_request: HttpData) -> None:
        super().__init__(raw_request)
        self.filter = None
        self.sort = None
        self.owner_ids = None

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

        self.surrounding = self._get_str("surrounding")

        owner_ids = self._get_value_by_key("ownerids")
        if owner_ids is not None:
            self.owner_ids = self._get_str("ownerids")
        self.cache_flag = self._get_str("cacheFlag")
        self.fields = self._get_dict("fields")['string']


class UpdateRecordRequest(RequestBase):
    record_id: str
    values: dict
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
        self.values = self._get_dict("values")
