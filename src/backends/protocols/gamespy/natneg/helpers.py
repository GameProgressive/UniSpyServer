import enum
from backends.library.database.pg_orm import InitPacketCaches
from frontends.gamespy.protocols.natneg.aggregations.enums import (
    NatClientIndex,
    NatPortMappingScheme,
    NatPortType,
    NatType,
)
from frontends.gamespy.protocols.natneg.aggregations.exceptions import NatNegException


class NatStrategy(enum.IntEnum):
    USE_PUBLIC_IP = 0
    USE_PRIVATE_IP = 1
    USE_GAME_TRAFFIC_RALEY = 2


class NatNegVersion(enum.IntEnum):
    VERSION2 = 2
    VERSION3 = 3
    VERSION4 = 4


class NatProtocolHelper:
    address_infos: dict[NatPortType, InitPacketCaches]
    nat_type: NatType
    port_mapping: NatPortMappingScheme
    guessed_next_port: int
    cookie: int
    version: int
    client_index: NatClientIndex
    public_ip: str
    public_port: int
    private_ip: str
    private_port: int

    def __init__(self, init_caches: list[InitPacketCaches]) -> None:
        if len(init_caches) < 3:
            raise NatNegException(
                "init cache length not enough for NAT determination")
        self.nat_type = NatType.NO_NAT
        self.port_mapping = NatPortMappingScheme.CONSISTENT_PORT
        self.guessed_next_port = 0
        self.address_infos = {}
        for cache in init_caches:
            assert isinstance(cache.port_type, NatPortType)
            self.address_infos[cache.port_type] = cache

        last_address_info = list(self.address_infos.values())[-1]
        assert isinstance(last_address_info.cookie, int)
        assert isinstance(last_address_info.version, int)
        assert isinstance(last_address_info.client_index, NatClientIndex)
        assert isinstance(last_address_info.public_ip, str)
        assert isinstance(last_address_info.public_port, int)
        assert isinstance(last_address_info.private_ip, str)
        assert isinstance(last_address_info.private_port, int)

        self.cookie = last_address_info.cookie
        self.version = last_address_info.version
        self.client_index = last_address_info.client_index
        self.public_ip = last_address_info.public_ip
        self.public_port = last_address_info.public_port
        self.private_ip = last_address_info.private_ip
        self.private_port = last_address_info.private_port

        if self.version not in NatNegVersion:
            raise NatNegException("Unknown natneg version")
        version = NatNegVersion(self.version)
        if version == NatNegVersion.VERSION2:
            raise NatNegException("Version 1 not implemented")
        elif version == NatNegVersion.VERSION3:
            self._validate_version3()
            NatProtocolHelper._determine_nat_type_version3(self)
        elif version == NatNegVersion.VERSION4:
            self._validate_version4()
            NatProtocolHelper._determine_nat_type_version4(self)

    def _validate_version3(self):
        if not (
            NatPortType.NN1 in self.address_infos
            and NatPortType.NN2 in self.address_infos
        ):
            raise NatNegException("Incomplete init packets")
        assert isinstance(self.address_infos[NatPortType.NN1].cookie, int)
        assert isinstance(self.address_infos[NatPortType.NN2].cookie, int)

        if (
            self.address_infos[NatPortType.NN1].cookie
            != self.address_infos[NatPortType.NN2].cookie
        ):  # type: ignore
            raise NatNegException("Broken cookie")
        if (
            self.address_infos[NatPortType.NN1].version
            != self.address_infos[NatPortType.NN2].version
        ):  # type: ignore
            raise NatNegException("Broken version")
        if (
            self.address_infos[NatPortType.NN1].client_index
            != self.address_infos[NatPortType.NN2].client_index
        ):  # type: ignore
            raise NatNegException("Broken client index")

        if NatPortType.GP in self.address_infos:
            if (
                self.address_infos[NatPortType.GP].cookie
                != self.address_infos[NatPortType.NN1].cookie
                or self.address_infos[NatPortType.GP].version
                != self.address_infos[NatPortType.NN1].version
                or self.address_infos[NatPortType.GP].client_index
                != self.address_infos[NatPortType.NN1].client_index
                or self.address_infos[NatPortType.GP].use_game_port
                != self.address_infos[NatPortType.NN1].use_game_port
            ):  # type: ignore
                raise NatNegException("GP packet info is not correct")

    def _validate_version4(self):
        # TODO: some games will not send GP packet to NAT negotiation server; currently, the reason is unknown and requires more games for analysis.
        # This will happen in GameClient

        if not (
            NatPortType.NN1 in self.address_infos
            and NatPortType.NN2 in self.address_infos
            and NatPortType.NN3 in self.address_infos
        ):
            raise NatNegException("Incomplete init packets")

        if (
            self.address_infos[NatPortType.NN1].cookie
            != self.address_infos[NatPortType.NN2].cookie
            or self.address_infos[NatPortType.NN1].cookie
            != self.address_infos[NatPortType.NN3].cookie
        ):  # type: ignore
            raise NatNegException("Broken cookie")

        if (
            self.address_infos[NatPortType.NN1].version
            != self.address_infos[NatPortType.NN2].version
            or self.address_infos[NatPortType.NN1].version
            != self.address_infos[NatPortType.NN3].version
        ):  # type: ignore
            raise NatNegException("Broken version")

        if (
            self.address_infos[NatPortType.NN1].client_index
            != self.address_infos[NatPortType.NN2].client_index
            or self.address_infos[NatPortType.NN1].client_index
            != self.address_infos[NatPortType.NN3].client_index
        ):  # type: ignore
            raise NatNegException("Broken client index")

        if (
            self.address_infos[NatPortType.NN1].use_game_port
            != self.address_infos[NatPortType.NN2].use_game_port
            or self.address_infos[NatPortType.NN1].use_game_port
            != self.address_infos[NatPortType.NN3].use_game_port
        ):  # type: ignore
            raise NatNegException("Broken use game port")

        if (
            self.address_infos[NatPortType.NN2].private_ip
            != self.address_infos[NatPortType.NN3].private_ip
        ):  # type: ignore
            raise NatNegException("Client is sending wrong init packet.")

        if NatPortType.GP in self.address_infos:
            if (
                self.address_infos[NatPortType.GP].cookie
                != self.address_infos[NatPortType.NN1].cookie
                or self.address_infos[NatPortType.GP].version
                != self.address_infos[NatPortType.NN1].version
                or self.address_infos[NatPortType.GP].client_index
                != self.address_infos[NatPortType.NN1].client_index
                or self.address_infos[NatPortType.GP].use_game_port
                != self.address_infos[NatPortType.NN1].use_game_port
            ):  # type: ignore
                raise NatNegException("GP packet info is not correct")

    @staticmethod
    def _determine_nat_type_version3(info: "NatProtocolHelper"):
        if len(info.address_infos) < 3:
            raise NatNegException(
                "We need 3 init records to determine the nat type.")

        nn1 = info.address_infos[NatPortType.NN1]
        nn2 = info.address_infos[NatPortType.NN2]
        # no nat
        if nn1.public_ip == nn1.private_ip and (
            nn2.public_ip == nn2.private_ip and nn2.public_port == nn1.public_port
        ):  # type: ignore
            info.nat_type = NatType.NO_NAT
        # detect cone
        elif nn1.public_ip == nn2.public_ip and nn1.public_port == nn2.public_port:  # type: ignore
            info.nat_type = NatType.FULL_CONE
        elif nn1.public_ip == nn2.public_ip and nn1.public_port != nn2.public_port:  # type: ignore
            info.nat_type = NatType.SYMMETRIC
            info.port_mapping = NatPortMappingScheme.INCREMENTAL
            # todo: get all interval of the port increment value
            port_interval = nn2.public_port - nn1.public_port
            info.guessed_next_port = nn2.public_port + port_interval  # type: ignore
        else:
            info.nat_type = NatType.UNKNOWN

    @staticmethod
    def _determine_nat_type_version4(info: "NatProtocolHelper"):
        if len(info.address_infos) < 3:
            raise NatNegException(
                "We need 3 init records to determine the nat type.")

        nn1 = info.address_infos[NatPortType.NN1]
        nn2 = info.address_infos[NatPortType.NN2]
        nn3 = info.address_infos[NatPortType.NN3]

        # no nat
        if (
            nn1.public_ip == nn1.private_ip
            and (
                nn2.public_ip == nn2.private_ip and nn2.public_port == nn2.private_port
            )
            and (
                nn3.public_ip == nn3.private_ip and nn3.public_port == nn3.private_port
            )
        ):  # type: ignore
            info.nat_type = NatType.NO_NAT
        # detect cone
        elif (
            nn1.public_ip == nn2.public_ip and nn1.public_port == nn2.public_port
        ) and (nn2.public_ip == nn3.public_ip and nn2.public_port == nn3.public_port):  # type: ignore
            info.nat_type = NatType.FULL_CONE
        elif (
            nn1.public_ip == nn2.public_ip and nn1.public_port != nn2.public_port
        ) or (nn2.public_ip == nn3.public_ip and nn2.public_port != nn3.public_port):  # type: ignore
            info.nat_type = NatType.SYMMETRIC
            info.port_mapping = NatPortMappingScheme.INCREMENTAL

            # Calculate port increments
            port_increment_1: int = nn2.public_port - nn1.public_port  # type: ignore
            port_increment_2: int = nn3.public_port - nn2.public_port  # type: ignore
            assert isinstance(port_increment_1, int)
            assert isinstance(port_increment_2, int)
            if port_increment_1 == port_increment_2:
                info.guessed_next_port = port_increment_1
            else:
                increase_interval = port_increment_2 - port_increment_1
                info.guessed_next_port = port_increment_2 + increase_interval
        else:
            info.nat_type = NatType.UNKNOWN

    @staticmethod
    def choose_nat_strategy(
        helper1: "NatProtocolHelper", helper2: "NatProtocolHelper"
    ) -> NatStrategy:
        is_both_have_same_public_ip = helper1.public_ip == helper2.public_ip
        # Check if the first 3 bytes of the private IP addresses are the same
        is_both_in_same_private_ip_range = (
            helper1.private_ip.split(
                ".")[:3] == helper2.private_ip.split(".")[:3]
        )

        # we use p2p strategy when nat punch is available
        p2p_nat_types = [
            NatType.NO_NAT,
            NatType.FIRE_WALL_ONLY,
            NatType.FULL_CONE,
            NatType.ADDRESS_RESTRICTED_CONE,
            NatType.PORT_RESTRICTED_CONE,
        ]

        if helper1.nat_type in p2p_nat_types and helper2.nat_type in p2p_nat_types:
            if is_both_have_same_public_ip:
                if is_both_in_same_private_ip_range:
                    return NatStrategy.USE_PRIVATE_IP
                else:
                    return NatStrategy.USE_GAME_TRAFFIC_RALEY
            else:
                return NatStrategy.USE_PUBLIC_IP
        else:
            return NatStrategy.USE_GAME_TRAFFIC_RALEY
