from typing import Optional
import xml.etree.ElementTree as ET

from frontends.gamespy.protocols.web_services.aggregations.exceptions import WebException
from frontends.gamespy.protocols.web_services.modules.sake.abstractions.generals import (
    RequestBase,
    NAMESPACE,
)
import xmltodict

from frontends.gamespy.protocols.web_services.modules.sake.exceptions.general import SakeException


class CreateRecordRequest(RequestBase):
    values: list[tuple[str, str, str]] = []
    """
    (name,type,value)
    """

    def parse(self) -> None:
        super().parse()
        values = self._content_element.find(f".//{{{NAMESPACE}}}values")
        if values is None:
            raise SakeException("values is missing from request")
        record_fields = values.findall(f".//{{{NAMESPACE}}}RecordField")
        for f in record_fields:
            temp = []
            name = f.find(f".//{{{NAMESPACE}}}name")
            if name is None or name.text is None:
                raise WebException("name can not be None")
            temp.append(name.text)
            value = f.find(f".//{{{NAMESPACE}}}value")
            if value is None or value.text is None:
                raise WebException("value can not be None")
            for v in value:
                temp.append(v.tag.split("}")[1])
                for i in v:
                    temp.append(i.text)

            self.values.append(tuple(temp))


class DeleteRecordRequest(RequestBase):
    record_id: int

    def parse(self) -> None:
        super().parse()
        record_id = self._content_element.find(f".//{{{NAMESPACE}}}recordid")

        if record_id is None or record_id.text is None:
            raise SakeException("recordid is missing from request")

        self.record_id = int(record_id.text)


class GetMyRecordsRequest(RequestBase):
    fields: list[tuple[Optional[str], str]]

    def __init__(self, raw_request: str) -> None:
        super().__init__(raw_request)
        self.fields = []

    def parse(self) -> None:
        super().parse()
        fields = self._content_element.find(f".//{{{NAMESPACE}}}fields")
        if fields is None:
            raise SakeException("fields is missing from request")
        for e in fields:
            data = (e.text, e.tag.split("}")[1])
            if data is None:
                raise WebException("data can not be None")
            self.fields.append(data)


class GetRandomRecordsRequest(RequestBase):
    max: int
    fields: list[tuple[Optional[str], str]] = []

    def parse(self) -> None:
        super().parse()
        max = self._content_element.find(f".//{{{NAMESPACE}}}max")
        if max is None or max.text is None:
            raise SakeException("max is missing from request")
        self.max = int(max.text)

        fields = self._content_element.find(f".//{{{NAMESPACE}}}fields")
        if fields is None:
            raise SakeException("fields is missing from request")
        for e in fields:
            data = (e.text, e.tag.split("}")[1])
            if data is None:
                raise WebException("data can not be None")
            self.fields.append(data)


class GetRecordLimitRequest(RequestBase):
    pass


class GetSpecificRecordsRequest(RequestBase):

    record_ids: list[tuple] = []
    """
    [
    (field_name,field_type),
    (field_name,field_type),
    (field_name,field_type),
    ...
    (field_name,field_type)
    ]
    """
    fields: list[tuple] = []
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
        record_ids = self._content_element.find(
            f".//{{{NAMESPACE}}}recordids")
        if record_ids is None:
            raise SakeException("No record id found.")
        for e in record_ids:
            data = (e.text, e.tag.split("}")[1])
            self.record_ids.append(data)
        fields = self._content_element.find(
            f".//{{{NAMESPACE}}}fields")
        if fields is None:
            raise SakeException("No record id found.")

        for e in fields:
            data = (e.text, e.tag.split("}")[1])
            self.fields.append(data)


class RateRecordRequest(RequestBase):
    record_id: str
    rating: str

    def parse(self) -> None:
        super().parse()
        record_id = self._content_element.find(
            f".//{{{NAMESPACE}}}recordid")
        if record_id is None or record_id.text is None:
            raise SakeException("No record id found.")
        self.record_id = record_id.text

        rating = self._content_element.find(
            f".//{{{NAMESPACE}}}rating")
        if rating is None or rating.text is None:
            raise SakeException("No rating found.")
        self.rating = rating.text


class SearchForRecordsRequest(RequestBase):
    filter: Optional[str]
    sort: Optional[str]
    offset: str
    max: str
    surrounding: str
    owner_ids: Optional[str]
    cache_flag: str
    fields: list[tuple[Optional[str], str]]
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
        filter = self._content_element.find(
            f".//{{{NAMESPACE}}}filter")
        if filter is None:
            raise SakeException("No filter found.")
        self.filter = filter.text

        sort = self._content_element.find(
            f".//{{{NAMESPACE}}}sort")
        if sort is None:
            raise SakeException("No sort found.")
        self.sort = sort.text

        offset = self._content_element.find(
            f".//{{{NAMESPACE}}}offset")
        if offset is None or offset.text is None:
            raise SakeException("No offset found.")
        self.offset = offset.text

        vmax = self._content_element.find(
            f".//{{{NAMESPACE}}}max")
        if vmax is None or vmax.text is None:
            raise SakeException("No max found.")
        self.max = vmax.text

        surrounding = self._content_element.find(
            f".//{{{NAMESPACE}}}surrounding")
        if surrounding is None or surrounding.text is None:
            raise SakeException("No surrounding found.")
        self.surrounding = surrounding.text

        owner_ids = self._content_element.find(
            f".//{{{NAMESPACE}}}ownerids")
        if owner_ids is None:
            raise SakeException("No ownderids found.")
        self.owner_ids = owner_ids.text

        cache_flag = self._content_element.find(
            f".//{{{NAMESPACE}}}cacheFlag")
        if cache_flag is None or cache_flag.text is None:
            raise SakeException("No cache flag found.")
        self.cache_flag = cache_flag.text

        fields = self._content_element.find(
            f".//{{{NAMESPACE}}}fields")
        self.fields = []
        if fields is None:
            raise SakeException("No record id found.")
        for e in fields:
            data = (e.text, e.tag.split("}")[1])
            if data is not None:
                self.fields.append(data)


class UpdateRecordRequest(RequestBase):
    record_id: str
    values: list
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
        if record_id is None or record_id.text is None:
            raise SakeException("No record id found.")
        self.record_id = record_id.text
        values_node = self._content_element.find(
            f".//{{{NAMESPACE}}}values")
        # todo fix this
        if values_node is None:
            raise WebException("value node can not be none")
        temp_str = ET.tostring(
            element=values_node, encoding="unicode").replace("ns0:", "")
        self.values = xmltodict.parse(temp_str)['values']["RecordField"]
