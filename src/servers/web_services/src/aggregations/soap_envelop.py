import xml.etree.ElementTree as ET


class SoapEnvelop:
    soap_envelop_namespace = "http://schemas.xmlsoap.org/soap/envelope/"
    current_element: ET.Element
    content: ET.Element

    def __init__(self, body_namespace: str):
        self._body_namespace = body_namespace
        ET.register_namespace("SOAP-ENV", self.soap_envelop_namespace)
        self.content = ET.Element(f"{{{self.soap_envelop_namespace}}}Envelope")
        self.body = ET.Element(f"{{{self.soap_envelop_namespace}}}Body")
        self.content.append(self.body)
        self.current_element = self.body

    def finish_add_sub_element(self):
        self.current_element = ET.SubElement(self.current_element,)

    def change_to_element(self, name: str):
        self.current_element = self.body.find(
            f".//{{{self._body_namespace}}}{name}")

    def back_to_parent_element(self):
        self.current_element = self.body

    def add(self, name: str, value: object = None):
        new_element = ET.SubElement(
            self.current_element, f"{{{self._body_namespace}}}{name}"
        )

        if value is not None:
            new_element.text = value
            self.current_element = new_element

    def __str__(self) -> str:
        return ET.tostring(
            self.content, xml_declaration=True, encoding="utf-8", method="xml"
        ).decode()


if __name__ == "__main__":
    import xml.etree.ElementTree as ET

    s = SoapEnvelop("http://gamespy.net/AuthService/")
    s.add("level1", "1")
    s.add("level1.1", "1.1")
    s.back_to_parent_element()
    s.add("level2", "2")
    str(s)
