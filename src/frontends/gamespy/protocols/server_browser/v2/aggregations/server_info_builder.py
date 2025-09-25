from socket import inet_aton

from frontends.gamespy.protocols.query_report.aggregates.game_server_info import GameServerInfo

# from backends.protocols.gamespy.query_report.data import get_all_groups
from frontends.gamespy.protocols.server_browser.v2.aggregations.enums import GameServerFlags

# PEER_GROUP_LIST = get_all_groups()
QUERY_REPORT_DEFAULT_PORT = 6500


def build_server_info_header(
    flag: GameServerFlags, server_info: GameServerInfo
) -> bytearray:
    header = bytearray()
    # add key flag
    header.append(flag.value)
    # we add server public ip here
    header.extend(inet_aton(server_info.host_ip_address))
    # we check host port is standard port or not
    check_non_standard_port(header, server_info)
    # check if game can directly query information from server
    check_unsolicited_udp(header, server_info)
    # we check the natneg flag
    check_nat_neg_flag(header, server_info)
    # now we check if there are private ip
    check_private_ip(header, server_info)
    # we check private port here
    check_non_standard_private_port(header, server_info)
    # we check icmp support here
    check_icmp_support(header, server_info)

    return header


def check_nat_neg_flag(header: bytearray, server_info: GameServerInfo):
    if "natneg" in server_info.server_data:
        nat_neg_flag = int(server_info.server_data["natneg"])
        unsolicited_udp = header[0] & GameServerFlags.UNSOLICITED_UDP_FLAG.value
        if nat_neg_flag == 1 and unsolicited_udp == 0:
            header[0] ^= GameServerFlags.CONNECT_NEGOTIATE_FLAG.value


def check_unsolicited_udp(header: bytearray, server_info: GameServerInfo):
    if "allow_unsolicited_udp" in server_info.server_data:
        unsolicited_udp = int(server_info.server_data["unsolicitedudp"])
        if unsolicited_udp == 1:
            header[0] ^= GameServerFlags.UNSOLICITED_UDP_FLAG.value


def check_private_ip(header: bytearray, server_info: GameServerInfo):
    #!when game create a channel chat, it will use both the public ip and private ip to build the name.
    #!known game: Worm3d
    # todo
    # if server_info.game_name in PEER_GROUP_LIST:
        if "localip0" in server_info.server_data:
            header[0] ^= GameServerFlags.PRIVATE_IP_FLAG.value
            bytes_address = inet_aton(server_info.server_data["localip0"])
            header.extend(bytes_address)


def check_non_standard_port(header: bytearray, server_info: GameServerInfo):
    # !! only dedicated server have different query report port and host port
    # !! but peer server have same query report port and host port
    # todo we have to check when we need send host port or query report port
    if server_info.query_report_port != QUERY_REPORT_DEFAULT_PORT:
        header[0] ^= GameServerFlags.NON_STANDARD_PORT_FLAG.value
        hton_port = server_info.query_report_port_bytes
        header.extend(hton_port)


def check_non_standard_private_port(header: bytearray, server_info: GameServerInfo):
    if "localport" in server_info.server_data:
        local_port = server_info.server_data["localport"]
        if local_port != "" and local_port != QUERY_REPORT_DEFAULT_PORT:
            header[0] ^= GameServerFlags.NON_STANDARD_PRIVATE_PORT_FLAG.value
            port = int(local_port).to_bytes(2, byteorder="big")
            header.extend(port)


def check_icmp_support(header: bytearray, server_info: GameServerInfo):
    if "icmp_address" in server_info.server_data:
        header[0] ^= GameServerFlags.ICMP_IP_FLAG.value
        bytes_address = inet_aton(server_info.server_data["icmp_address"])
        header.extend(bytes_address)
