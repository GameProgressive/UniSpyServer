import xml.etree.ElementTree as ET

from frontends.gamespy.protocols.web_services.aggregations.exceptions import WebException


class SoapEnvelop:
    soap_envelop_namespace = "http://schemas.xmlsoap.org/soap/envelope/"
    current_element: ET.Element
    parent_element: ET.Element
    content: ET.Element

    def __init__(self, body_namespace: str):
        self._body_namespace = body_namespace
        ET.register_namespace("SOAP-ENV", self.soap_envelop_namespace)
        self.content = ET.Element(f"{{{self.soap_envelop_namespace}}}Envelope")
        self.body = ET.Element(f"{{{self.soap_envelop_namespace}}}Body")
        self.content.append(self.body)
        self.current_element = self.body

    def finish_add_sub_element(self):
        self.current_element = ET.SubElement(
            self.current_element, self._body_namespace)

    def change_to_element(self, name: str):
        current_element = self.body.find(
            f".//{{{self._body_namespace}}}{name}")
        if current_element is None:
            raise WebException("can not find the node")
        self.current_element = current_element

    def go_to_content_element(self):
        content_element = list(self.body.iter())[1]
        self.current_element = content_element

    def add(self, name: str, value: object = None):
        tag = f"{{{self._body_namespace}}}{name}"
        new_element = ET.SubElement(
            self.current_element, tag
        )

        if value is None:
            self.parent_element = self.current_element
            self.current_element = new_element
        else:
            new_element.text = str(value)

    def __str__(self) -> str:
        return ET.tostring(
            self.content, xml_declaration=True, encoding="utf-8", method="xml"
        ).decode("utf-8").replace("'", '"')


if __name__ == "__main__":
    import xml.etree.ElementTree as ET

    s = SoapEnvelop("http://gamespy.net/AuthService/")
    s.add("level1", "1")
    s.add("level1.1", "1.1")
    s.go_to_content_element()
    s.add("level2", "2")
    str(s)
