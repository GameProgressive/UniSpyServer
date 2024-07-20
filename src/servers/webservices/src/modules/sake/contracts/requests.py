from typing import OrderedDict
from servers.webservices.src.modules.sake.abstractions.general import (
    RequestBase,
    NAMESPACE,
)
import xmltodict

from servers.webservices.src.modules.sake.exceptions.general import SakeException


class CreateRecordRequest(RequestBase):
    values: dict

    def parse(self) -> None:
        super().parse()
        value_node = self._content_element.find(f".//{{{NAMESPACE}}}values")
        if value_node is None:
            raise SakeException("values is missing from request")

        self.values = xmltodict.parse(value_node)


class DeleteRecordRequest(RequestBase):
    record_id: int

    def parse(self) -> None:
        super().parse()
        record_id = self._content_element.find(f".//{{{NAMESPACE}}}recordid")

        if record_id is None:
            raise SakeException("recordid is missing from request")

        self.record_id = int(record_id)


class GetMyRecordsRequest(RequestBase):
    fields: dict

    def parse(self) -> None:
        super().parse()
        fields_node = self._content_element.find(f".//{{{NAMESPACE}}}fields")
        if fields_node is None:
            raise SakeException("fields is missing from request")
        self.fields = xmltodict.parse(fields_node)


class GetRandomRecordsRequest(RequestBase):
    max: str
    fields: dict

    def parse(self) -> None:
        super().parse()
        max = self._content_element.find(f".//{{{NAMESPACE}}}max")
        if max is None:
            raise SakeException("max is missing from request")
        self.max = int(max)

        fields_node = self._content_element.find(f".//{{{NAMESPACE}}}fields")
        if fields_node is None:
            raise SakeException("fields is missing from request")
        self.fields = xmltodict.parse(fields_node)


class GetRecordLimitRequest(RequestBase):
    pass


class GetSpecificRecordsRequest(RequestBase):
    record_ids: list[tuple]
    """
    [
    (field_name,field_type),
    (field_name,field_type),
    (field_name,field_type),
    ...
    (field_name,field_type)
    ]
    """
    fields: list[tuple]
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
        record_id_node = self._content_element.find(
            f".//{{{NAMESPACE}}}recordids")
        if record_id_node is None:
            raise SakeException("No record id found.")
        self.record_ids = xmltodict.parse(str(record_id_node))
        fields = self._content_element.find(
            f".//{{{NAMESPACE}}}recordids")
        if fields is None:
            raise SakeException("No record id found.")

        self.fields = xmltodict.parse(str(fields))


class RateRecordRequest(RequestBase):
    record_id: str
    rating: str

    def parse(self) -> None:
        super().parse()
        record_id = self._content_element.find(
            f".//{{{NAMESPACE}}}recordid")
        if record_id is None:
            raise SakeException("No record id found.")
        self.record_id = record_id

        rating = self._content_element.find(
            f".//{{{NAMESPACE}}}rating")
        if rating is None or rating.text is None:
            raise SakeException("No rating found.")
        self.rating = rating.text


class SearchForRecordsRequest(RequestBase):
    filter: str
    sort: str
    offset: str
    max: str
    surrounding: str
    owner_ids: str
    cache_flag: str
    fields: OrderedDict[str, object]
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
        self.filter = self._content_element.find(
            f".//{{{NAMESPACE}}}filter")
        if self.filter is None:
            raise SakeException("No filter found.")

        self.sort = self._content_element.find(
            f".//{{{NAMESPACE}}}sort")
        if self.sort is None:
            raise SakeException("No sort found.")

        self.offset = self._content_element.find(
            f".//{{{NAMESPACE}}}offset")
        if self.offset is None:
            raise SakeException("No offset found.")

        self.max = self._content_element.find(
            f".//{{{NAMESPACE}}}max")
        if self.max is None:
            raise SakeException("No max found.")

        self.surrounding = self._content_element.find(
            f".//{{{NAMESPACE}}}surrounding")
        if self.sort is None:
            raise SakeException("No surrounding found.")
        self.owner_ids = self._content_element.find(
            f".//{{{NAMESPACE}}}ownerids")
        if self.owner_ids is None:
            raise SakeException("No ownderids found.")

        self.cache_flag = self._content_element.find(
            f".//{{{NAMESPACE}}}cacheFlag")
        if self.cache_flag is None:
            raise SakeException("No cache flag found.")

        fields = self._content_element.find(
            f".//{{{NAMESPACE}}}fields")
        if fields is None:
            raise SakeException("No record id found.")

        self.fields = xmltodict.parse(str(fields))


class UpdateRecordRequest(RequestBase):
    record_id: str
    values: OrderedDict[str, object]
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
        record_id = self._content_element.find(
            f".//{{{NAMESPACE}}}recordid")
        if record_id is None:
            raise SakeException("No record id found.")
        self.record_id = record_id
        values_node = self._content_element.find(
            f".//{{{NAMESPACE}}}values")
        self.values = xmltodict.parse(str(values_node))
