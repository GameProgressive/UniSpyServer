

from frontends.gamespy.library.extentions.debug_helper import DebugHelper


if __name__ == "__main__":
    from frontends.gamespy.protocols.chat.applications.server_launcher import Service as chat
    from frontends.gamespy.protocols.game_status.applications.server_launcher import Service as gs
    from frontends.gamespy.protocols.game_traffic_relay.applications.server_launcher import Service as gtr
    from frontends.gamespy.protocols.natneg.applications.server_launcher import Service as nn
    from frontends.gamespy.protocols.presence_connection_manager.applications.server_launcher import Service as pcm
    from frontends.gamespy.protocols.presence_search_player.applications.server_launcher import Service as psp
    from frontends.gamespy.protocols.query_report.applications.server_launcher import Service as qr
    from frontends.gamespy.protocols.server_browser.applications.server_launcher import ServiceV2 as sb
    from frontends.gamespy.protocols.web_services.applications.server_launcher import Service as web

    from frontends.gamespy.library.abstractions.server_launcher import ServicesFactory
    launchers = [
        chat(),
        gs(),
        gtr(),
        nn(),
        pcm(),
        psp(),
        qr(),
        sb(),
        web()
    ]

    factory = ServicesFactory(launchers)
    helper = DebugHelper("./frontends", factory)
    helper.start()
