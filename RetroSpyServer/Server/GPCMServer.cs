using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using GameSpyLib.Database;
using GameSpyLib.Network;
using GameSpyLib.Server;
using GameSpyLib.Logging;

namespace RetroSpyServer.Server
{
    public class GPCMServer : PresenceServer
    {
        /// <summary>
        /// A connection counter, used to create unique connection id's
        /// </summary>
        private long ConnectionCounter = 0;

        /// <summary>
        /// Indicates the timeout of when a connecting client will be disconnected
        /// </summary>
        public const int Timeout = 20000;

        public GPCMServer(DatabaseDriver databaseDriver) : base(databaseDriver)
        {
        }

        protected override void OnException(Exception e)
        {
            throw e;
        }

        protected override void ProcessAccept(TCPStream Stream)
        {
            long connectionId = Interlocked.Increment(ref ConnectionCounter);
            GPCMClient client;

            /*try
            {
                client = new GPCMClient(Stream, connectionId);
                Processing.TryAdd(connectionId, client);

                client.SendServerChallenge();
            }
            catch (Exception e)
            {
                Processing.TryRemove(connectionId, out client);
                base.Release(Stream);
            }*/
        }
    }
}
