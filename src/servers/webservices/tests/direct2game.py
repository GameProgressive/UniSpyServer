import unittest

import responses

from servers.webservices.src.modules.sake.contracts.requests import CreateRecordRequest, DeleteRecordRequest, GetMyRecordsRequest, GetRandomRecordsRequest, GetRecordLimitRequest, GetSpecificRecordsRequest, RateRecordRequest, SearchForRecordsRequest, UpdateRecordRequest


CRYSIS_2_SAKE = """<?xml version="1.0" encoding="UTF-8"?>
                            <SOAP-ENV:Envelope
                                xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/"
                                xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/"
                                xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                                xmlns:xsd="http://www.w3.org/2001/XMLSchema"
                                xmlns:ns1="http://gamespy.net/sake">
                                <SOAP-ENV:Body>
                                    <ns1:SearchForRecords>
                                        <ns1:gameid>3300</ns1:gameid>
                                        <ns1:secretKey>8TTq4M</ns1:secretKey>
                                        <ns1:loginTicket>0000000000000000000000__</ns1:loginTicket>
                                        <ns1:tableid>DEDICATEDSTATS</ns1:tableid>
                                        <ns1:filter>PROFILE&#x20=&#x2035</ns1:filter>
                                        <ns1:sort>recordid</ns1:sort>
                                        <ns1:offset>0</ns1:offset>
                                        <ns1:max>1</ns1:max>
                                        <ns1:surrounding>0</ns1:surrounding>
                                        <ns1:ownerids>2</ns1:ownerids>
                                        <ns1:cacheFlag>0</ns1:cacheFlag>
                                        <ns1:fields>
                                            <ns1:string>DATA</ns1:string>
                                            <ns1:string>recordid</ns1:string>
                                        </ns1:fields>
                                    </ns1:SearchForRecords>
                                </SOAP-ENV:Body>
                            </SOAP-ENV:Envelope>"""

GET_RECORD_LIMIT = """<?xml version="1.0" encoding="UTF-8"?>
                    <SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/"
                        xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/"
                        xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                        xmlns:xsd="http://www.w3.org/2001/XMLSchema"
                        xmlns:ns1="http://gamespy.net/sake">
                        <SOAP-ENV:Body>
                            <ns1:GetRecordLimit>
                                <ns1:gameid>0</ns1:gameid>
                                <ns1:secretKey>XXXXXX</ns1:secretKey>
                                <ns1:loginTicket>xxxxxxxx_YYYYYYYYYY__</ns1:loginTicket>
                                <ns1:tableid>nicks</ns1:tableid>
                            </ns1:GetRecordLimit>
                        </SOAP-ENV:Body>
                    </SOAP-ENV:Envelope>"""

RATE_RECORD = """<?xml version="1.0" encoding="UTF-8"?>
            <SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/"
                xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/"
                xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                xmlns:xsd="http://www.w3.org/2001/XMLSchema"
                xmlns:ns1="http://gamespy.net/sake">
                <SOAP-ENV:Body>
                    <ns1:RateRecord>
                        <ns1:gameid>0</ns1:gameid>
                        <ns1:secretKey>XXXXXX</ns1:secretKey>
                        <ns1:loginTicket>xxxxxxxx_YYYYYYYYYY__</ns1:loginTicket>
                        <ns1:tableid>test</ns1:tableid>
                        <ns1:recordid>158</ns1:recordid>
                        <ns1:rating>200</ns1:rating>
                    </ns1:RateRecord>
                </SOAP-ENV:Body>
            </SOAP-ENV:Envelope>"""

GET_RANDOM_RECORDS = """<?xml version="1.0" encoding="UTF-8"?>
            <SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/"
                xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/"
                xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                xmlns:xsd="http://www.w3.org/2001/XMLSchema"
                xmlns:ns1="http://gamespy.net/sake">
                <SOAP-ENV:Body>
                    <ns1:GetRandomRecords>
                        <ns1:gameid>0</ns1:gameid>
                        <ns1:secretKey>XXXXXX</ns1:secretKey>
                        <ns1:loginTicket>xxxxxxxx_YYYYYYYYYY__</ns1:loginTicket>
                        <ns1:tableid>levels</ns1:tableid>
                        <ns1:max>1</ns1:max>
                        <ns1:fields>
                            <ns1:string>recordid</ns1:string>
                            <ns1:string>score</ns1:string>
                        </ns1:fields>
                    </ns1:GetRandomRecords>
                </SOAP-ENV:Body>
            </SOAP-ENV:Envelope>"""

GET_SPECIFIC_RECORDS = """<?xml version="1.0" encoding="UTF-8"?>
            <SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/"
                xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/"
                xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                xmlns:xsd="http://www.w3.org/2001/XMLSchema"
                xmlns:ns1="http://gamespy.net/sake">
                <SOAP-ENV:Body>
                    <ns1:GetSpecificRecords>
                        <ns1:gameid>0</ns1:gameid>
                        <ns1:secretKey>XXXXXX</ns1:secretKey>
                        <ns1:loginTicket>xxxxxxxx_YYYYYYYYYY__</ns1:loginTicket>
                        <ns1:tableid>scores</ns1:tableid>
                        <ns1:recordids>
                            <ns1:int>1</ns1:int>
                            <ns1:int>2</ns1:int>
                            <ns1:int>4</ns1:int>
                            <ns1:int>5</ns1:int>
                        </ns1:recordids>
                        <ns1:fields>
                            <ns1:string>recordid</ns1:string>
                            <ns1:string>ownerid</ns1:string>
                            <ns1:string>score</ns1:string>
                        </ns1:fields>
                    </ns1:GetSpecificRecords>
                </SOAP-ENV:Body>
            </SOAP-ENV:Envelope>"""

GET_MY_RECORDS = """<?xml version="1.0" encoding="UTF-8"?>
            <SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/"
                xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/"
                xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                xmlns:xsd="http://www.w3.org/2001/XMLSchema"
                xmlns:ns1="http://gamespy.net/sake">
                <SOAP-ENV:Body>
                    <ns1:GetMyRecords>
                        <ns1:gameid>0</ns1:gameid>
                        <ns1:secretKey>XXXXXX</ns1:secretKey>
                        <ns1:loginTicket>xxxxxxxx_YYYYYYYYYY__</ns1:loginTicket>
                        <ns1:tableid>test</ns1:tableid>
                        <ns1:fields>
                            <ns1:string>recordid</ns1:string>
                            <ns1:string>ownerid</ns1:string>
                            <ns1:string>MyByte</ns1:string>
                            <ns1:string>MyShort</ns1:string>
                            <ns1:string>MyInt</ns1:string>
                            <ns1:string>MyFloat</ns1:string>
                            <ns1:string>MyAsciiString</ns1:string>
                            <ns1:string>MyUnicodeString</ns1:string>
                            <ns1:string>MyBoolean</ns1:string>
                            <ns1:string>MyDateAndTime</ns1:string>
                            <ns1:string>MyBinaryData</ns1:string>
                            <ns1:string>MyFileID</ns1:string>
                            <ns1:string>num_ratings</ns1:string>
                            <ns1:string>average_rating</ns1:string>
                        </ns1:fields>
                    </ns1:GetMyRecords>
                </SOAP-ENV:Body>
            </SOAP-ENV:Envelope>"""

SEARCH_FOR_RECORDS = """<?xml version="1.0" encoding="UTF-8"?>
            <SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/"
                xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/"
                xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                xmlns:xsd="http://www.w3.org/2001/XMLSchema"
                xmlns:ns1="http://gamespy.net/sake">
                <SOAP-ENV:Body>
                    <ns1:SearchForRecords>
                        <ns1:gameid>0</ns1:gameid>
                        <ns1:secretKey>XXXXXX</ns1:secretKey>
                        <ns1:loginTicket>xxxxxxxx_YYYYYYYYYY__</ns1:loginTicket>
                        <ns1:tableid>scores</ns1:tableid>
                        <ns1:filter></ns1:filter>
                        <ns1:sort></ns1:sort>
                        <ns1:offset>0</ns1:offset>
                        <ns1:max>3</ns1:max>
                        <ns1:surrounding>0</ns1:surrounding>
                        <ns1:ownerids></ns1:ownerids>
                        <ns1:cacheFlag>0</ns1:cacheFlag>
                        <ns1:fields>
                            <ns1:string>score</ns1:string>
                            <ns1:string>recordid</ns1:string>
                        </ns1:fields>
                    </ns1:SearchForRecords>
                </SOAP-ENV:Body>
            </SOAP-ENV:Envelope>"""

DELETE_RECORD = """<?xml version="1.0" encoding="UTF-8"?>
            <SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/"
                xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/"
                xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                xmlns:xsd="http://www.w3.org/2001/XMLSchema"
                xmlns:ns1="http://gamespy.net/sake">
                <SOAP-ENV:Body>
                    <ns1:DeleteRecord>
                        <ns1:gameid>0</ns1:gameid>
                        <ns1:secretKey>XXXXXX</ns1:secretKey>
                        <ns1:loginTicket>xxxxxxxx_YYYYYYYYYY__</ns1:loginTicket>
                        <ns1:tableid>test</ns1:tableid>
                        <ns1:recordid>150</ns1:recordid>
                    </ns1:DeleteRecord>
                </SOAP-ENV:Body>
            </SOAP-ENV:Envelope>"""

UPDATE_RECORD = """<?xml version="1.0" encoding="UTF-8"?>
            <SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/"
                xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/"
                xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                xmlns:xsd="http://www.w3.org/2001/XMLSchema"
                xmlns:ns1="http://gamespy.net/sake">
                <SOAP-ENV:Body>
                    <ns1:UpdateRecord>
                        <ns1:gameid>0</ns1:gameid>
                        <ns1:secretKey>XXXXXX</ns1:secretKey>
                        <ns1:loginTicket>xxxxxxxx_YYYYYYYYYY__</ns1:loginTicket>
                        <ns1:tableid>test</ns1:tableid>
                        <ns1:recordid>158</ns1:recordid>
                        <ns1:values>
                            <ns1:RecordField>
                                <ns1:name>MyByte</ns1:name>
                                <ns1:value>
                                    <ns1:byteValue>
                                        <ns1:value>123</ns1:value>
                                    </ns1:byteValue>
                                </ns1:value>
                            </ns1:RecordField>
                            <ns1:RecordField>
                                <ns1:name>MyShort</ns1:name>
                                <ns1:value>
                                    <ns1:shortValue>
                                        <ns1:value>12345</ns1:value>
                                    </ns1:shortValue>
                                </ns1:value>
                            </ns1:RecordField>
                            <ns1:RecordField>
                                <ns1:name>MyInt</ns1:name>
                                <ns1:value>
                                    <ns1:intValue>
                                        <ns1:value>123456789</ns1:value>
                                    </ns1:intValue>
                                </ns1:value>
                            </ns1:RecordField>
                            <ns1:RecordField>
                                <ns1:name>MyFloat</ns1:name>
                                <ns1:value>
                                    <ns1:floatValue>
                                        <ns1:value>3.141593</ns1:value>
                                    </ns1:floatValue>
                                </ns1:value>
                            </ns1:RecordField>
                            <ns1:RecordField>
                                <ns1:name>MyAsciiString</ns1:name>
                                <ns1:value>
                                    <ns1:asciiStringValue>
                                        <ns1:value>ascii</ns1:value>
                                    </ns1:asciiStringValue>
                                </ns1:value>
                            </ns1:RecordField>
                            <ns1:RecordField>
                                <ns1:name>MyUnicodeString</ns1:name>
                                <ns1:value>
                                    <ns1:unicodeStringValue>
                                        <ns1:value>unicode</ns1:value>
                                    </ns1:unicodeStringValue>
                                </ns1:value>
                            </ns1:RecordField>
                            <ns1:RecordField>
                                <ns1:name>MyBoolean</ns1:name>
                                <ns1:value>
                                    <ns1:booleanValue>
                                        <ns1:value>1</ns1:value>
                                    </ns1:booleanValue>
                                </ns1:value>
                            </ns1:RecordField>
                            <ns1:RecordField>
                                <ns1:name>MyDateAndTime</ns1:name>
                                <ns1:value>
                                    <ns1:dateAndTimeValue>
                                        <ns1:value>2020-05-21T11:13:41Z</ns1:value>
                                    </ns1:dateAndTimeValue>
                                </ns1:value>
                            </ns1:RecordField>
                            <ns1:RecordField>
                                <ns1:name>MyBinaryData</ns1:name>
                                <ns1:value>
                                    <ns1:binaryDataValue>
                                        <ns1:value>EjRWq80=</ns1:value>
                                    </ns1:binaryDataValue>
                                </ns1:value>
                            </ns1:RecordField>
                        </ns1:values>
                    </ns1:UpdateRecord>
                </SOAP-ENV:Body>
            </SOAP-ENV:Envelope>"""

CREATE_RECORD = """<?xml version="1.0" encoding="UTF-8"?>
            <SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/"
                xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/"
                xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                xmlns:xsd="http://www.w3.org/2001/XMLSchema"
                xmlns:ns1="http://gamespy.net/sake">
                <SOAP-ENV:Body>
                    <ns1:CreateRecord>
                        <ns1:gameid>0</ns1:gameid>
                        <ns1:secretKey>XXXXXX</ns1:secretKey>
                        <ns1:loginTicket>xxxxxxxx_YYYYYYYYYY__</ns1:loginTicket>
                        <ns1:tableid>test</ns1:tableid>
                        <ns1:values>
                            <ns1:RecordField>
                                <ns1:name>MyAsciiString</ns1:name>
                                <ns1:value>
                                    <ns1:asciiStringValue>
                                        <ns1:value>this is a record</ns1:value>
                                    </ns1:asciiStringValue>
                                </ns1:value>
                            </ns1:RecordField>
                        </ns1:values>
                    </ns1:CreateRecord>
                </SOAP-ENV:Body>
            </SOAP-ENV:Envelope>"""


class Direct2GameTest(unittest.TestCase):
    @responses.activate
    def test_get_record_limit(self):
        request = GetRecordLimitRequest(GET_RECORD_LIMIT)
        request.parse()
        self.assertEqual(0, request.game_id)
        self.assertEqual("XXXXXX", request.secret_key)
        self.assertEqual("xxxxxxxx_YYYYYYYYYY__", request.login_ticket)
        self.assertEqual("nicks", request.table_id)

    @responses.activate
    def test_rate_record(self):
        request = RateRecordRequest(RATE_RECORD)
        request.parse()
        self.assertEqual(0, request.game_id)
        self.assertEqual("XXXXXX", request.secret_key)
        self.assertEqual("xxxxxxxx_YYYYYYYYYY__", request.login_ticket)
        self.assertEqual("test", request.table_id)
        self.assertEqual("158", request.record_id)
        self.assertEqual("200", request.rating)

    @responses.activate
    def test_get_random_records(self):
        request = GetRandomRecordsRequest(GET_RANDOM_RECORDS)
        request.parse()
        self.assertEqual(0, request.game_id)
        self.assertEqual("XXXXXX", request.secret_key)
        self.assertEqual("xxxxxxxx_YYYYYYYYYY__", request.login_ticket)
        self.assertEqual("levels", request.table_id)
        self.assertEqual(1, request.max)
        self.assertEqual("recordid", request.fields[0][0])
        self.assertEqual("string", request.fields[0][1])
        self.assertEqual("score", request.fields[1][0])
        self.assertEqual("string", request.fields[1][1])

    @responses.activate
    def test_get_specific_record(self):
        request = GetSpecificRecordsRequest(GET_SPECIFIC_RECORDS)
        request.parse()
        self.assertEqual(0, request.game_id)
        self.assertEqual("XXXXXX", request.secret_key)
        self.assertEqual("xxxxxxxx_YYYYYYYYYY__", request.login_ticket)
        self.assertEqual("scores", request.table_id)
        self.assertEqual("1", request.record_ids[0][0])
        self.assertEqual("int", request.record_ids[0][1])
        self.assertEqual("2", request.record_ids[1][0])
        self.assertEqual("int", request.record_ids[1][1])
        self.assertEqual("4", request.record_ids[2][0])
        self.assertEqual("int", request.record_ids[2][1])
        self.assertEqual("5", request.record_ids[3][0])
        self.assertEqual("int", request.record_ids[3][1])
        self.assertEqual("recordid", request.fields[0][0])
        self.assertEqual("string", request.fields[0][1])
        self.assertEqual("ownerid", request.fields[1][0])
        self.assertEqual("string", request.fields[1][1])
        self.assertEqual("score", request.fields[2][0])
        self.assertEqual("string", request.fields[2][1])

    @responses.activate
    def test_get_my_record(self):
        request = GetMyRecordsRequest(GET_MY_RECORDS)
        request.parse()
        self.assertEqual(0, request.game_id)
        self.assertEqual("XXXXXX", request.secret_key)
        self.assertEqual("xxxxxxxx_YYYYYYYYYY__", request.login_ticket)
        self.assertEqual("test", request.table_id)

        self.assertEqual("recordid", request.fields[0][0])
        self.assertEqual("string", request.fields[0][1])
        self.assertEqual("ownerid", request.fields[1][0])
        self.assertEqual("string", request.fields[1][1])
        self.assertEqual("MyByte", request.fields[2][0])
        self.assertEqual("string", request.fields[2][1])
        self.assertEqual("MyShort", request.fields[3][0])
        self.assertEqual("string", request.fields[3][1])
        self.assertEqual("MyInt", request.fields[4][0])
        self.assertEqual("string", request.fields[4][1])
        self.assertEqual("MyFloat", request.fields[5][0])
        self.assertEqual("string", request.fields[5][1])
        self.assertEqual("MyAsciiString", request.fields[6][0])
        self.assertEqual("string", request.fields[6][1])
        self.assertEqual("MyUnicodeString", request.fields[7][0])
        self.assertEqual("string", request.fields[7][1])
        self.assertEqual("MyBoolean", request.fields[8][0])
        self.assertEqual("string", request.fields[8][1])
        self.assertEqual("MyDateAndTime", request.fields[9][0])
        self.assertEqual("string", request.fields[9][1])
        self.assertEqual("MyBinaryData", request.fields[10][0])
        self.assertEqual("string", request.fields[10][1])
        self.assertEqual("MyFileID", request.fields[11][0])
        self.assertEqual("string", request.fields[11][1])
        self.assertEqual("num_ratings", request.fields[12][0])
        self.assertEqual("string", request.fields[12][1])
        self.assertEqual("average_rating", request.fields[13][0])
        self.assertEqual("string", request.fields[13][1])

    @responses.activate
    def test_search_for_records(self):
        request = SearchForRecordsRequest(SEARCH_FOR_RECORDS)
        request.parse()
        self.assertEqual(0, request.game_id)
        self.assertEqual("XXXXXX", request.secret_key)
        self.assertEqual("xxxxxxxx_YYYYYYYYYY__", request.login_ticket)
        self.assertEqual("scores", request.table_id)
        self.assertEqual(None, request.filter)
        self.assertEqual(None, request.sort)
        self.assertEqual("0", request.offset)
        self.assertEqual("0", request.surrounding)
        self.assertEqual(None, request.owner_ids)
        self.assertEqual("0", request.cache_flag)
        # todo check how to implement this
        # self.assertEqual("score", request.fields[0][0])
        # self.assertEqual("string", request.fields[0][1])
        # self.assertEqual("recordid", request.fields[1][0])
        # self.assertEqual("string", request.fields[1][1])

    @responses.activate
    def test_delete_record(self):
        request = DeleteRecordRequest(DELETE_RECORD)
        request.parse()

        self.assertEqual(0, request.game_id)
        self.assertEqual("XXXXXX", request.secret_key)
        self.assertEqual("xxxxxxxx_YYYYYYYYYY__", request.login_ticket)
        self.assertEqual("test", request.table_id)
        self.assertEqual(150, request.record_id)

    @responses.activate
    def test_update_record(self):
        request = UpdateRecordRequest(UPDATE_RECORD)
        request.parse()

        self.assertEqual(0, request.game_id)
        self.assertEqual("XXXXXX", request.secret_key)
        self.assertEqual("xxxxxxxx_YYYYYYYYYY__", request.login_ticket)
        self.assertEqual("test", request.table_id)
        self.assertEqual("158", request.record_id)

        # TODO: Deserialization of RecordFields
        self.assertEqual("MyByte", request.values[0]["name"])
        self.assertEqual(
            "123", request.values[0]["value"]["byteValue"]['value'])

    @responses.activate
    def test_create_record(self):
        request = CreateRecordRequest(CREATE_RECORD)
        request.parse()
        self.assertEqual(0, request.game_id)
        self.assertEqual("XXXXXX", request.secret_key)
        self.assertEqual("xxxxxxxx_YYYYYYYYYY__", request.login_ticket)
        self.assertEqual("test", request.table_id)

        self.assertEqual("MyAsciiString",request.values[0][0])
        self.assertEqual("asciiStringValue",request.values[0][1])
        self.assertEqual("this is a record",request.values[0][2])




if __name__ == "__main__":
    unittest.main()
