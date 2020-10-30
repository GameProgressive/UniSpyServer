using GameSpyLib.Logging;
using GameSpyLib.MiscMethod;
using PresenceConnectionManager.Entity.BaseClass;
using PresenceConnectionManager.Entity.Structure;
using PresenceConnectionManager.Entity.Structure.Request.General;
using PresenceConnectionManager.Entity.Structure.Request.Profile;
using PresenceSearchPlayer.Entity.Enumerator;
using PresenceSearchPlayer.Entity.Structure.Request;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using PresenceConnectionManager.Entity.Structure.Request.Buddy;
using PresenceSearchPlayer.Handler.CommandHandler.Error;
using GameSpyLib.Abstraction.Interface;

namespace PresenceConnectionManager.Handler.CommandSwticher
{
    public class PCMRequestSerializer
    {
        public static List<object> Serialize(ISession session, string rawRequest)
        {
            List<object> requestList = new List<object>();
            rawRequest = PCMRequestBase.NormalizeRequest(rawRequest);
            if (rawRequest[0] != '\\')
            {
                LogWriter.ToLog(LogEventLevel.Error, "Invalid request recieved!");
                return null;
            }
            string[] commands = rawRequest.Split("\\final\\", StringSplitOptions.RemoveEmptyEntries);
            foreach (var command in commands)
            {
                IRequest request = GenerateRequest(command);
                var flag = (GPError)request.Parse();
                if (flag != GPError.NoError)
                {
                    session.SendAsync(ErrorMsg.BuildGPErrorMsg(flag));
                    continue;
                }
                requestList.Add(request.GetInstance());
            }
            return requestList;
        }

        /// <summary>
        /// Returns PCM request class instance
        /// </summary>
        /// <param name="command">raw request</param>
        /// <param name="specificRequest">The valid serialized request</param>
        /// <returns>error code</returns>
        private static IRequest GenerateRequest(string command)
        {
            if (command.Length < 1)
            {
                return null;
            }
            // Read client message, and parse it into key value pairs
            string[] recieved = command.TrimStart('\\').Split('\\');
            Dictionary<string, string> keyValue = GameSpyUtils.ConvertRequestToKeyValue(recieved);

            switch (keyValue.Keys.First())
            {
                case PCMRequestName.Login:
                    return new LoginRequest(keyValue);
                case PCMRequestName.Logout:
                    return new LogoutRequest(keyValue);
                case PCMRequestName.KeepAlive:
                    return new KeepAliveRequest(keyValue);
                case PCMRequestName.GetProfile:
                    return new GetProfileRequest(keyValue);
                case PCMRequestName.RegisterNick:
                    return new RegisterNickRequest(keyValue);
                case PCMRequestName.NewUser:
                    return new NewUserRequest(keyValue);
                case PCMRequestName.UpdateUI:
                    return new UpdateUIRequest(keyValue);
                case PCMRequestName.UpdatePro:
                    return new UpdateProRequest(keyValue);
                case PCMRequestName.NewProfile:
                    return new NewUserRequest(keyValue);
                case PCMRequestName.DelProfile:
                    throw new NotImplementedException();
                case PCMRequestName.AddBlock:
                    return new AddBlockRequest(keyValue);
                case PCMRequestName.RemoveBlock:
                    throw new NotImplementedException();
                case PCMRequestName.AddBuddy:
                    return new AddBuddyRequest(keyValue);
                case PCMRequestName.DelBuddy:
                    return new DelBuddyRequest(keyValue);
                case PCMRequestName.Status:
                    return new StatusRequest(keyValue);
                case PCMRequestName.StatusInfo:
                    return new StatusInfoRequest(keyValue);
                case PCMRequestName.InviteTo:
                    return new InviteToRequest(keyValue);
                default:
                    LogWriter.UnknownDataRecieved(command);
                    return null;
            }
        }
    }
}
