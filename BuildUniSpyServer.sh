#!/bin/bash
echo "Building UniSpyServer!"
cd "$( cd -- "$(dirname "$0")" >/dev/null 2>&1 ; pwd -P )"
dotnet build
echo "Copy UniSpyServer.cfg to build directory"
cp ./common/UniSpyServer.cfg ./build/Debug/net5.0/