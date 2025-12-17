import xml.etree.ElementTree as ET

from frontends.gamespy.protocols.web_services.aggregations.exceptions import WebException


class SoapEnvelop:
    current_element: ET.Element
    content: ET.Element

    def __init__(self, root_name: str):
        assert isinstance(root_name, str)
        self.content = ET.Element(root_name)
        self.current_element = self.content

    def change_to_element(self, name: str):
        current_element = self.content.find(
            f".//{name}")
        if current_element is None:
            raise WebException("can not find the node")
        self.current_element = current_element

    def go_to_content_element(self):
        self.current_element = self.content

    def add(self, name: str, value: object = None):
        tag = f"{name}"
        new_element = ET.SubElement(
            self.current_element, tag
        )

        if value is None:
            self.parent_element = self.current_element
            self.current_element = new_element
        else:
            new_element.text = str(value)

    def __str__(self) -> str:
        xml_str: str = ET.tostring(
            self.content,
            encoding="unicode"
        )
        return xml_str


if __name__ == "__main__":
    import xml.etree.ElementTree as ET

    s = SoapEnvelop("test_name")
    s.add("level1", "1")
    s.add("level1.1", "1.1")
    s.go_to_content_element()
    s.add("level2", "2")
    str(s)
