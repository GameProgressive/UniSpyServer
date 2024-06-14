# UniSpy Server
[![license](https://img.shields.io/github/license/GameProgressive/UniSpyServer.svg)](../LICENSE)
![CIPass](https://github.com/GameProgressive/UniSpyServer/workflows/CI/badge.svg)\
![platforms](https://img.shields.io/badge/platform-win32%20%7C%20win64%20%7C%20linux%20%7C%20osx-brightgreen.svg)\
[![discord-banner](https://discord.com/api/guilds/512314008079171615/widget.png?style=banner2)](https://discord.gg/NpggYaD)

UniSpy is a GameSpy Project that aims to create GameSpy services.

The server is written in C# and is inspired by the old OpenSpy project.

## Information & Documentation
See the [wiki](https://github.com/GameProgressive/UniSpyServer/wiki) for more information about project and the UniSpy Server.
You can also use our Software Development Kit to create games or learn about the [UniSpySDK](https://github.com/GameProgressive/UniSpySDK).

Technical papers and documentation about the GameSpy protocol and the games that use it, [GameSpyDocs](https://github.com/GameProgressive/GameSpyDocs).

## Credits
* The [contributors](https://github.com/GameProgressive/UniSpyServer/graphs/contributors) of the project
* [Luigi Auriemma](https://aluigi.altervista.org/papers.htm#distrust) for his gamespy papers that were used as a reference
* [BattleSpy](https://github.com/BF2Statistics/BattleSpy) for their library, that we used as a base for UniSpy
* [NetCoreServer](https://github.com/chronoxor/NetCoreServer) for their TCP and UDP server


## License
This project is licensed under the [GNU Affero General Public License v3.0](../LICENSE).


## Why rewrite C# to python
* The vscode extensions for C# development is become more and more hard to use, and microsoft abandoned the open-source OmniSharp project and replacing it with it's own closed source language server.
* The c# project seems hard to run by users, it require a lot of deploy knowledge and hard for collaborations, for the future of the gamespy emulator, I choose to rewrite this into a opensource and easy high level language - python.
