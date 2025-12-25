import xmltodict


class SoapEnvelop:
    current_element: dict
    content: dict
    _root_name: str

    def __init__(self, root_name: str):
        assert isinstance(root_name, str)
        self.current_element = {}
        self.content = {root_name: self.current_element}
        self._root_name = root_name

    def go_to_content_element(self):
        self.current_element = self.content[self._root_name]

    def add(self, name: str, value: object = None):
        if value is None:
            self.current_element[name] = {}
            self.current_element = self.current_element[name]
        else:
            self.current_element[name] = str(value)

    def __str__(self) -> str:
        xml_str: str = xmltodict.unparse(self.content).replace("\n","")
        return xml_str


if __name__ == "__main__":

    s = SoapEnvelop("test_name")
    s.add("level1")
    s.add("level1.1", "1.1")
    s.go_to_content_element()
    s.add("level2", "2")
    if str(s) != '<?xml version="1.0" encoding="utf-8"?>\n<test_name><level1><level1.1>1.1</level1.1></level1><level2>2</level2></test_name>':
        raise ValueError("xml wrong")
