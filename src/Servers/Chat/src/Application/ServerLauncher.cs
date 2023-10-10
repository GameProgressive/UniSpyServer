using System.Collections.Generic;
using UniSpy.Server.Core.Abstraction.BaseClass.Factory;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Application
{
    public sealed class ServerLauncher : ServerLauncherBase
    {
        public static IServer Server => ServerInstances[0];
        protected override List<IServer> LaunchNetworkService() => new List<IServer> { new Server() };
        public override void Start()
        {
            base.Start();
            // initialize peergrouplist
            _ = Chat.Application.StorageOperation.Persistance.PeerGroupList;
        }
    }
}