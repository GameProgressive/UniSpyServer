# UniSpy Server
[![license](https://img.shields.io/github/license/GameProgressive/UniSpyServer.svg)](../LICENSE)
![CIPass](https://github.com/GameProgressive/UniSpyServer/workflows/CI/badge.svg)\
![platforms](https://img.shields.io/badge/platform-win32%20%7C%20win64%20%7C%20linux%20%7C%20osx-brightgreen.svg)\
[![discord-banner](https://discord.com/api/guilds/512314008079171615/widget.png?style=banner2)](https://discord.gg/NpggYaD)

UniSpy is a GameSpy Project that aims to create GameSpy services.

The server is written in python and is inspired by the old OpenSpy project.

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


## Why rewrite to python
* The vscode extensions for C# development is become more and more hard to use, and microsoft abandoned the open-source OmniSharp project and replacing it with it's own closed source language server.
* The c# project seems hard to run by users, it require a lot of deploy knowledge and hard for collaborations, for the future of the gamespy emulator, I choose to rewrite this into a opensource and easy high level language - python.

## RSA key replacement
* Replace the rsa key in auth service ```BF05D63E93751AD4A59A4A7389CF0BE8A22CCDEEA1E7F12C062D6E194472EFDA5184CCECEB4FBADF5EB1D7ABFE91181453972AA971F624AF9BA8F0F82E2869FB7D44BDE8D56EE50977898F3FEE75869622C4981F07506248BD3D092E8EA05C12B2FA37881176084C8F8B8756C4722CDC57D2AD28ACD3AD85934FB48D6B2D2027```

  to
   ``D4536886309F2BDCFCC54F1B41B90D9C4934540F895DD312A22D9AD53E013B12F8F6658364679F7EBAAD8D8F088C4908E0D2392AB072085AFE48B7B7D1F10105D0BBA0662D17ECCAF52E6AEAFB0BA70728A3E8716306B3D2E6DBE9447EF7B6187388DE70CE0A83941D55BF2F686EC4521BAB419A615E3F06DF83AEF72BB3EAD1``

## How to run
* Run ```docker network create unispy``` to setup a network for unispy
* Run ```docker build -f Dockerfile -t unispy-python .``` to build the docker image used for compose services
* Run ```docker compose -f docker-compose-unispy-env.yml up -d``` to setup postgresql and redis
* Run ```export UNISPY_CONFIG='<path of config.json>'``` to setup env config file path, remember replace symbol ```<path of config.json>``` with config file path (This is optional, if not config env the program will read the config file in common folder by default)

## How to develop
* Open with local vscode:
  * Use vscode to open the src folder in devcontainer (make sure your vscode have devcontainer extension)
  * Run ```pip install -r requirements.txt``` to install the pip packages
* Open with github codespace:
  * Github codespace vscode web can not install devcontainer, the project can not running in docker network ```unispy```. Therefore, replace ```http://unispy_backends:8080``` to ```http://localhost:8080``` and ```unispy_postgresql```, ```unispy_redis``` to ```localhost``` in config.json to use localhost postgres and redis in docker (we already setup in docker-compose-unispy-env.yml)
