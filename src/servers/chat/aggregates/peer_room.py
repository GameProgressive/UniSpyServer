from servers.chat.enums.peer_room import PeerRoomType


class PeerRoom:
    
    TitleRoomPrefix = "#GSP"
    """ When game connects to the server, the player will enter the default channel for communicating with other players."""
    StagingRoomPrefix = "#GSP"
    """
    When a player creates their own game and is waiting for others to join they are placed in a separate chat room called the "staging room"\n
    Staging rooms have two title seperator like #GSP!xxxx!xxxx
    """
    GroupRoomPrefix = "#GPG"
    """
    group rooms is used split the list of games into categories (by gametype, skill, region, etc.). In this case, when entering the title room, the user would get a list of group rooms instead of a list of games\n
    Group room have one title seperator like #GPG!xxxxxx
    """
    TitleSeperator = "!"

    """
    Group room #GPG!622\n
    Staging room #GSP!worms3!Ml4lz344lM\n
    Normal room #islanbul
    """

    @staticmethod
    def get_room_type(channel_name: str) -> PeerRoomType:
        if PeerRoom.is_group_room(channel_name):
            return PeerRoomType.Group
        elif PeerRoom.is_staging_room(channel_name):
            return PeerRoomType.Staging
        elif PeerRoom.is_title_room(channel_name):
            return PeerRoomType.Title
        else:
            return PeerRoomType.Normal

    @staticmethod
    def is_staging_room(channel_name: str) -> bool:
        a = channel_name.count(PeerRoom.TitleSeperator) == 2
        b = channel_name.startswith(
            PeerRoom.StagingRoomPrefix, 0, len(PeerRoom.StagingRoomPrefix)
        )
        return a and b

    @staticmethod
    def is_title_room(channel_name: str) -> bool:
        a = channel_name.count(PeerRoom.TitleSeperator) == 1
        b = channel_name.startswith(
            PeerRoom.TitleRoomPrefix, 0, len(PeerRoom.TitleRoomPrefix)
        )
        return a and b

    @staticmethod
    def is_group_room(channel_name: str) -> bool:
        a = channel_name.count(PeerRoom.TitleSeperator) == 1
        b = channel_name.startswith(
            PeerRoom.GroupRoomPrefix, 0, len(PeerRoom.GroupRoomPrefix)
        )
        return a and b


if __name__ == "__main__":
    result = PeerRoom.get_room_type("#GSP!worms3!Ml4lz344lM")
    pass