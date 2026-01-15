import unittest

import responses

from frontends.gamespy.library.network.http_handler import HttpData
from frontends.gamespy.protocols.web_services.modules.sake.aggregates.utils import RecordConverter
from frontends.gamespy.protocols.web_services.modules.sake.contracts.requests import (
    CreateRecordRequest,
    DeleteRecordRequest,
    GetMyRecordsRequest,
    GetRandomRecordsRequest,
    GetRecordLimitRequest,
    GetSpecificRecordsRequest,
    RateRecordRequest,
    SearchForRecordsRequest,
    UpdateRecordRequest,
)


CRYSIS_2_SAKE = {
    "body": """<?xml version="1.0" encoding="UTF-8"?> <SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/" xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:ns1="http://gamespy.net/sake"> <SOAP-ENV:Body> <ns1:SearchForRecords> <ns1:gameid>3300</ns1:gameid> <ns1:secretKey>8TTq4M</ns1:secretKey> <ns1:loginTicket>0000000000000000000000__</ns1:loginTicket> <ns1:tableid>DEDICATEDSTATS</ns1:tableid> <ns1:filter>PROFILE =‵</ns1:filter> <ns1:sort>recordid</ns1:sort> <ns1:offset>0</ns1:offset> <ns1:max>1</ns1:max> <ns1:surrounding>0</ns1:surrounding> <ns1:ownerids>2</ns1:ownerids> <ns1:cacheFlag>0</ns1:cacheFlag> <ns1:fields> <ns1:string>DATA</ns1:string> <ns1:string>recordid</ns1:string> </ns1:fields> </ns1:SearchForRecords> </SOAP-ENV:Body> </SOAP-ENV:Envelope>"""
}

GET_RECORD_LIMIT = {
    "body": """<?xml version="1.0" encoding="UTF-8"?> <SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/" xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:ns1="http://gamespy.net/sake"><SOAP-ENV:Body><ns1:GetRecordLimit><ns1:gameid>0</ns1:gameid><ns1:secretKey>XXXXXX</ns1:secretKey><ns1:loginTicket>xxxxxxxx_YYYYYYYYYY__</ns1:loginTicket><ns1:tableid>nicks</ns1:tableid></ns1:GetRecordLimit></SOAP-ENV:Body></SOAP-ENV:Envelope>
"""
}

RATE_RECORD = {
    "body": """<?xml version="1.0" encoding="UTF-8"?> <SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/" xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:ns1="http://gamespy.net/sake"><SOAP-ENV:Body><ns1:RateRecord><ns1:gameid>0</ns1:gameid><ns1:secretKey>XXXXXX</ns1:secretKey><ns1:loginTicket>xxxxxxxx_YYYYYYYYYY__</ns1:loginTicket><ns1:tableid>test</ns1:tableid><ns1:recordid>158</ns1:recordid><ns1:rating>200</ns1:rating></ns1:RateRecord></SOAP-ENV:Body></SOAP-ENV:Envelope>
"""
}

GET_RANDOM_RECORDS = {
    "body": """<?xml version="1.0" encoding="UTF-8"?> <SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/" xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:ns1="http://gamespy.net/sake"><SOAP-ENV:Body><ns1:GetRandomRecords><ns1:gameid>0</ns1:gameid><ns1:secretKey>XXXXXX</ns1:secretKey><ns1:loginTicket>xxxxxxxx_YYYYYYYYYY__</ns1:loginTicket><ns1:tableid>levels</ns1:tableid><ns1:max>1</ns1:max><ns1:fields><ns1:string>recordid</ns1:string><ns1:string>score</ns1:string></ns1:fields></ns1:GetRandomRecords></SOAP-ENV:Body></SOAP-ENV:Envelope>
"""
}

GET_SPECIFIC_RECORDS = {
    "body": """<?xml version="1.0" encoding="UTF-8"?> <SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/" xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:ns1="http://gamespy.net/sake"><SOAP-ENV:Body><ns1:GetSpecificRecords><ns1:gameid>0</ns1:gameid><ns1:secretKey>XXXXXX</ns1:secretKey><ns1:loginTicket>xxxxxxxx_YYYYYYYYYY__</ns1:loginTicket><ns1:tableid>scores</ns1:tableid><ns1:recordids><ns1:int>1</ns1:int><ns1:int>2</ns1:int><ns1:int>4</ns1:int><ns1:int>5</ns1:int></ns1:recordids><ns1:fields><ns1:string>recordid</ns1:string><ns1:string>ownerid</ns1:string><ns1:string>score</ns1:string></ns1:fields></ns1:GetSpecificRecords></SOAP-ENV:Body></SOAP-ENV:Envelope>
"""
}

GET_MY_RECORDS = {
    "body": """<?xml version="1.0" encoding="UTF-8"?> <SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/" xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:ns1="http://gamespy.net/sake"><SOAP-ENV:Body><ns1:GetMyRecords><ns1:gameid>0</ns1:gameid><ns1:secretKey>XXXXXX</ns1:secretKey><ns1:loginTicket>xxxxxxxx_YYYYYYYYYY__</ns1:loginTicket><ns1:tableid>test</ns1:tableid><ns1:fields><ns1:string>recordid</ns1:string><ns1:string>ownerid</ns1:string><ns1:string>MyByte</ns1:string><ns1:string>MyShort</ns1:string><ns1:string>MyInt</ns1:string><ns1:string>MyFloat</ns1:string><ns1:string>MyAsciiString</ns1:string><ns1:string>MyUnicodeString</ns1:string><ns1:string>MyBoolean</ns1:string><ns1:string>MyDateAndTime</ns1:string><ns1:string>MyBinaryData</ns1:string><ns1:string>MyFileID</ns1:string><ns1:string>num_ratings</ns1:string><ns1:string>average_rating</ns1:string></ns1:fields></ns1:GetMyRecords></SOAP-ENV:Body></SOAP-ENV:Envelope>
"""
}

SEARCH_FOR_RECORDS = {
    "body": """<?xml version="1.0" encoding="UTF-8"?> <SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/" xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:ns1="http://gamespy.net/sake"><SOAP-ENV:Body><ns1:SearchForRecords><ns1:gameid>0</ns1:gameid><ns1:secretKey>XXXXXX</ns1:secretKey><ns1:loginTicket>xxxxxxxx_YYYYYYYYYY__</ns1:loginTicket><ns1:tableid>scores</ns1:tableid><ns1:filter/><ns1:sort/><ns1:offset>0</ns1:offset><ns1:max>3</ns1:max><ns1:surrounding>0</ns1:surrounding><ns1:ownerids/><ns1:cacheFlag>0</ns1:cacheFlag><ns1:fields><ns1:string>score</ns1:string><ns1:string>recordid</ns1:string></ns1:fields></ns1:SearchForRecords></SOAP-ENV:Body></SOAP-ENV:Envelope>
"""
}

DELETE_RECORD = {
    "body": """<?xml version="1.0" encoding="UTF-8"?> <SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/" xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:ns1="http://gamespy.net/sake"><SOAP-ENV:Body><ns1:DeleteRecord><ns1:gameid>0</ns1:gameid><ns1:secretKey>XXXXXX</ns1:secretKey><ns1:loginTicket>xxxxxxxx_YYYYYYYYYY__</ns1:loginTicket><ns1:tableid>test</ns1:tableid><ns1:recordid>150</ns1:recordid></ns1:DeleteRecord></SOAP-ENV:Body></SOAP-ENV:Envelope>
"""
}

UPDATE_RECORD = {
    "body": """<?xml version="1.0" encoding="UTF-8"?> <SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/" xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:ns1="http://gamespy.net/sake"><SOAP-ENV:Body><ns1:UpdateRecord><ns1:gameid>0</ns1:gameid><ns1:secretKey>XXXXXX</ns1:secretKey><ns1:loginTicket>xxxxxxxx_YYYYYYYYYY__</ns1:loginTicket><ns1:tableid>test</ns1:tableid><ns1:recordid>158</ns1:recordid><ns1:values><ns1:RecordField><ns1:name>MyByte</ns1:name><ns1:value><ns1:byteValue><ns1:value>123</ns1:value></ns1:byteValue></ns1:value></ns1:RecordField><ns1:RecordField><ns1:name>MyShort</ns1:name><ns1:value><ns1:shortValue><ns1:value>12345</ns1:value></ns1:shortValue></ns1:value></ns1:RecordField><ns1:RecordField><ns1:name>MyInt</ns1:name><ns1:value><ns1:intValue><ns1:value>123456789</ns1:value></ns1:intValue></ns1:value></ns1:RecordField><ns1:RecordField><ns1:name>MyFloat</ns1:name><ns1:value><ns1:floatValue><ns1:value>3.141593</ns1:value></ns1:floatValue></ns1:value></ns1:RecordField><ns1:RecordField><ns1:name>MyAsciiString</ns1:name><ns1:value><ns1:asciiStringValue><ns1:value>ascii</ns1:value></ns1:asciiStringValue></ns1:value></ns1:RecordField><ns1:RecordField><ns1:name>MyUnicodeString</ns1:name><ns1:value><ns1:unicodeStringValue><ns1:value>unicode</ns1:value></ns1:unicodeStringValue></ns1:value></ns1:RecordField><ns1:RecordField><ns1:name>MyBoolean</ns1:name><ns1:value><ns1:booleanValue><ns1:value>1</ns1:value></ns1:booleanValue></ns1:value></ns1:RecordField><ns1:RecordField><ns1:name>MyDateAndTime</ns1:name><ns1:value><ns1:dateAndTimeValue><ns1:value>2020-05-21T11:13:41Z</ns1:value></ns1:dateAndTimeValue></ns1:value></ns1:RecordField><ns1:RecordField><ns1:name>MyBinaryData</ns1:name><ns1:value><ns1:binaryDataValue><ns1:value>EjRWq80=</ns1:value></ns1:binaryDataValue></ns1:value></ns1:RecordField></ns1:values></ns1:UpdateRecord></SOAP-ENV:Body></SOAP-ENV:Envelope>"""
}

CREATE_RECORD = {
    "body": """<?xml version="1.0" encoding="UTF-8"?> <SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/" xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:ns1="http://gamespy.net/sake"><SOAP-ENV:Body><ns1:CreateRecord><ns1:gameid>0</ns1:gameid><ns1:secretKey>XXXXXX</ns1:secretKey><ns1:loginTicket>xxxxxxxx_YYYYYYYYYY__</ns1:loginTicket><ns1:tableid>test</ns1:tableid><ns1:values><ns1:RecordField><ns1:name>MyAsciiString</ns1:name><ns1:value><ns1:asciiStringValue><ns1:value>this is a record</ns1:value></ns1:asciiStringValue></ns1:value></ns1:RecordField></ns1:values></ns1:CreateRecord></SOAP-ENV:Body></SOAP-ENV:Envelope>
"""
}
SAMUZOMBIE2_GET_MY_RECORD = """<?xml version="1.0" encoding="UTF-8"?> <SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/" xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:ns1="http://gamespy.net/sake"><SOAP-ENV:Body><ns1:GetMyRecords><ns1:gameid>0</ns1:gameid><ns1:certificate><ns1:length>303</ns1:length><ns1:version>1</ns1:version><ns1:partnercode>0</ns1:partnercode><ns1:namespaceid>0</ns1:namespaceid><ns1:userid>1</ns1:userid><ns1:profileid>1</ns1:profileid><ns1:expiretime>1766112278</ns1:expiretime><ns1:profilenick>UniSpy</ns1:profilenick><ns1:uniquenick>UniSpy</ns1:uniquenick><ns1:cdkeyhash></ns1:cdkeyhash><ns1:peerkeymodulus>aefb5064bbd1eb632fa8d57aab1c49366ce0ee3161cbef19f2b7971b63b811790ecbf6a47b34c55f65a0766b40c261c5d69c394cd320842dd2bccba883d30eae8fdba5d03b21b09bfc600dcb30b1b2f3fbe8077630b006dcb54c4254f14891762f72e7bbfe743eb8baf65f9e8c8d11ebe46f6b59e986b4c394cfbc2c8606e29f</ns1:peerkeymodulus><ns1:peerkeyexponent>000001</ns1:peerkeyexponent><ns1:serverdata>95980bf5011ce73f2866b995a272420c36f1e8b4ac946f0b5bfe87c9fef0811036da00cfa85e77e00af11c924d425ec06b1dd052feab1250376155272904cbf9da831b0ce3d52964424c0a426b869e2c0ad11ffa3e70496e27ea250adb707a96b3496bff190eafc0b6b9c99db75b02c2a822bb1b5b3d954e7b2c0f9b1487e3e1</ns1:serverdata><ns1:signature>0001FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF003020300C06082A864886F70D0205050004106c8c997569806a4b994653842cd80c45</ns1:signature><ns1:timestamp></ns1:timestamp></ns1:certificate><ns1:proof>380E1E55DF5600FE67EE74C667C27AAE</ns1:proof><ns1:tableid>UserData</ns1:tableid><ns1:fields><ns1:string>recordid</ns1:string><ns1:string>ownerid</ns1:string></ns1:fields></ns1:GetMyRecords></SOAP-ENV:Body></SOAP-ENV:Envelope>"""

SAMUZOMBIE2_SEARCH_FOR_RECORD = """<?xml version="1.0" encoding="UTF-8"?><SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/" xmlns:SOAP-ENC="http://schemas.xmlsoap.org/soap/encoding/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:ns1="http://gamespy.net/sake"><SOAP-ENV:Body><ns1:SearchForRecords><ns1:gameid>5989</ns1:gameid><ns1:certificate><ns1:length>303</ns1:length><ns1:version>1</ns1:version><ns1:partnercode>99</ns1:partnercode><ns1:namespaceid>102</ns1:namespaceid><ns1:userid>6</ns1:userid><ns1:profileid>3</ns1:profileid><ns1:expiretime>1767491159</ns1:expiretime><ns1:profilenick>4C84AA7017F21B0A5B83</ns1:profilenick><ns1:uniquenick>4C84AA7017F21B0A5B83B56D6D06C86D22E93662</ns1:uniquenick><ns1:cdkeyhash></ns1:cdkeyhash><ns1:peerkeymodulus>D4536886309F2BDCFCC54F1B41B90D9C4934540F895DD312A22D9AD53E013B12F8F6658364679F7EBAAD8D8F088C4908E0D2392AB072085AFE48B7B7D1F10105D0BBA0662D17ECCAF52E6AEAFB0BA70728A3E8716306B3D2E6DBE9447EF7B6187388DE70CE0A83941D55BF2F686EC4521BAB419A615E3F06DF83AEF72BB3EAD1</ns1:peerkeymodulus><ns1:peerkeyexponent>010001</ns1:peerkeyexponent><ns1:serverdata>95980bf5011ce73f2866b995a272420c36f1e8b4ac946f0b5bfe87c9fef0811036da00cfa85e77e00af11c924d425ec06b1dd052feab1250376155272904cbf9da831b0ce3d52964424c0a426b869e2c0ad11ffa3e70496e27ea250adb707a96b3496bff190eafc0b6b9c99db75b02c2a822bb1b5b3d954e7b2c0f9b1487e3e1</ns1:serverdata><ns1:signature>30A616E6FD90144C4BC31C42A5F9D1A1DF53636968DD7A0A7D60FE81F2B9CF87D3F00E40A0209091B87AB4014252CE04ABE132FE564CAF53EB628515A54DDB45BA52AA5221D542D9742B924FAE002F48347CD9BA0D9837363F096119AF9D32E19FD540322DE5616B603F86F05E86555B90DBEBE96BE5A19F3CEFC0A159DD57D0</ns1:signature><ns1:timestamp></ns1:timestamp></ns1:certificate><ns1:proof>D1154B0CF1437F69FFC44E584134DE6E</ns1:proof><ns1:tableid>UserData</ns1:tableid><ns1:filter></ns1:filter><ns1:sort></ns1:sort><ns1:offset>-1</ns1:offset><ns1:max>1</ns1:max><ns1:ownerids><ns1:int>-1</ns1:int></ns1:ownerids><ns1:fields><ns1:string>profile</ns1:string><ns1:string>username</ns1:string><ns1:string>gamecenterid</ns1:string><ns1:string>facebookid</ns1:string><ns1:string>androidid</ns1:string><ns1:string>loginTime</ns1:string><ns1:string>pushIDs</ns1:string><ns1:string>seeded</ns1:string><ns1:string>defenseBuff1</ns1:string><ns1:string>defenseBuff2</ns1:string><ns1:string>loadout</ns1:string><ns1:string>attackRating</ns1:string><ns1:string>language</ns1:string></ns1:fields></ns1:SearchForRecords></SOAP-ENV:Body></SOAP-ENV:Envelope>"""


class SakeTests(unittest.TestCase):
    @responses.activate
    def test_get_record_limit(self):
        request = GetRecordLimitRequest(
            str(HttpData.from_dict(GET_RECORD_LIMIT)))
        request.parse()
        self.assertEqual(0, request.game_id)
        self.assertEqual("XXXXXX", request.secret_key)
        self.assertEqual("xxxxxxxx_YYYYYYYYYY__", request.login_ticket)
        self.assertEqual("nicks", request.table_id)

    @responses.activate
    def test_rate_record(self):
        request = RateRecordRequest(str(HttpData.from_dict(RATE_RECORD)))
        request.parse()
        self.assertEqual(0, request.game_id)
        self.assertEqual("XXXXXX", request.secret_key)
        self.assertEqual("xxxxxxxx_YYYYYYYYYY__", request.login_ticket)
        self.assertEqual("test", request.table_id)
        self.assertEqual("158", request.record_id)
        self.assertEqual("200", request.rating)

    @responses.activate
    def test_get_random_records(self):
        request = GetRandomRecordsRequest(
            str(HttpData.from_dict(GET_RANDOM_RECORDS)))
        request.parse()
        self.assertEqual(0, request.game_id)
        self.assertEqual("XXXXXX", request.secret_key)
        self.assertEqual("xxxxxxxx_YYYYYYYYYY__", request.login_ticket)
        self.assertEqual("levels", request.table_id)
        self.assertEqual(1, request.max)
        self.assertEqual("recordid", request.fields[0])
        self.assertEqual("score", request.fields[1])

    @responses.activate
    def test_get_specific_record(self):
        request = GetSpecificRecordsRequest(
            str(HttpData.from_dict(GET_SPECIFIC_RECORDS))
        )
        request.parse()
        self.assertEqual(0, request.game_id)
        self.assertEqual("XXXXXX", request.secret_key)
        self.assertEqual("xxxxxxxx_YYYYYYYYYY__", request.login_ticket)
        self.assertEqual("scores", request.table_id)
        self.assertEqual("1", request.record_ids["int"][0])
        self.assertEqual("2", request.record_ids["int"][1])
        self.assertEqual("4", request.record_ids["int"][2])
        self.assertEqual("5", request.record_ids["int"][3])
        self.assertEqual("recordid", request.fields["string"][0])
        self.assertEqual("ownerid", request.fields["string"][1])
        self.assertEqual("score", request.fields["string"][2])

    @responses.activate
    def test_get_my_record(self):
        request = GetMyRecordsRequest(str(HttpData.from_dict(GET_MY_RECORDS)))
        request.parse()
        self.assertEqual(0, request.game_id)
        self.assertEqual("XXXXXX", request.secret_key)
        self.assertEqual("xxxxxxxx_YYYYYYYYYY__", request.login_ticket)
        self.assertEqual("test", request.table_id)

        self.assertEqual("recordid", request.fields[0])
        self.assertEqual("ownerid", request.fields[1])
        self.assertEqual("MyByte", request.fields[2])
        self.assertEqual("MyShort", request.fields[3])
        self.assertEqual("MyInt", request.fields[4])
        self.assertEqual("MyFloat", request.fields[5])
        self.assertEqual("MyAsciiString", request.fields[6])
        self.assertEqual("MyUnicodeString", request.fields[7])
        self.assertEqual("MyBoolean", request.fields[8])
        self.assertEqual("MyDateAndTime", request.fields[9])
        self.assertEqual("MyBinaryData", request.fields[10])
        self.assertEqual("MyFileID", request.fields[11])
        self.assertEqual("num_ratings", request.fields[12])
        self.assertEqual("average_rating", request.fields[13])

    @responses.activate
    def test_search_for_records(self):
        request = SearchForRecordsRequest(
            str(HttpData.from_dict(SEARCH_FOR_RECORDS)))
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

    @responses.activate
    def test_delete_record(self):
        request = DeleteRecordRequest(str(HttpData.from_dict(DELETE_RECORD)))
        request.parse()

        self.assertEqual(0, request.game_id)
        self.assertEqual("XXXXXX", request.secret_key)
        self.assertEqual("xxxxxxxx_YYYYYYYYYY__", request.login_ticket)
        self.assertEqual("test", request.table_id)
        self.assertEqual(150, request.record_id)

    @responses.activate
    def test_update_record(self):
        request = UpdateRecordRequest(str(HttpData.from_dict(UPDATE_RECORD)))
        request.parse()

        self.assertEqual(0, request.game_id)
        self.assertEqual("XXXXXX", request.secret_key)
        self.assertEqual("xxxxxxxx_YYYYYYYYYY__", request.login_ticket)
        self.assertEqual("test", request.table_id)
        self.assertEqual("158", request.record_id)

        # TODO: Deserialization of RecordFields
        self.assertEqual("MyByte", list(request.records)[0])
        self.assertEqual(
            "123", request.records['MyByte']["value"])

    @responses.activate
    def test_create_record(self):
        request = CreateRecordRequest(str(HttpData.from_dict(CREATE_RECORD)))
        request.parse()
        self.assertEqual(0, request.game_id)
        self.assertEqual("XXXXXX", request.secret_key)
        self.assertEqual("xxxxxxxx_YYYYYYYYYY__", request.login_ticket)
        self.assertEqual("test", request.table_id)

        self.assertEqual("MyAsciiString", list(request.records)[0])
        self.assertEqual("asciiStringValue",
                         request.records['MyAsciiString']['type'])
        self.assertEqual(
            "this is a record",
            request.records['MyAsciiString']['value'])

    @responses.activate
    def test_samuzombie2_get_my_records(self):
        request = GetMyRecordsRequest(
            str(
                HttpData(
                    body=SAMUZOMBIE2_GET_MY_RECORD,
                    headers={"SessionToken": "example_token"},
                )
            )
        )
        request.parse()
        self.assertEqual(0, request.game_id)

    @responses.activate
    def test_samuzombie2_search_for_records(self):
        request = SearchForRecordsRequest(
            str(
                HttpData(
                    headers={"SessionToken": "example_token"},
                    body=SAMUZOMBIE2_SEARCH_FOR_RECORD,
                )
            )
        )
        request.parse()
        self.assertEqual(5989, request.game_id)

    def test_records_format_convert(self):
        records = [{"name": "MyAsciiString", "value": {
            "asciiStringValue": {"value": "this is a record"}}}]
        values = RecordConverter.to_searchable_format(records)
        records2 = RecordConverter.to_gamespy_format(values)
        self.assertEqual(records, records2)


if __name__ == "__main__":
    unittest.main()
