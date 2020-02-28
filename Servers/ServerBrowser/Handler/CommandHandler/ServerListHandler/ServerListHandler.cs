﻿using GameSpyLib.Encryption;
using ServerBrowser.Entity.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ServerBrowser.Handler.CommandHandler.ServerListHandler
{
    public class ServerListHandler
    {
        public ServerListHandler(SBSession session, byte[] recv)
        {
            Handle(session, recv);
        }

        public void Handle(SBSession session, byte[]  recv)
        {
            QR2Query query = new QR2Query(recv);

            byte[] remoteIP = ((IPEndPoint)session.Socket.RemoteEndPoint).Address.GetAddressBytes();
            //TODO we have to make sure the port number
            byte[] remotePort = BitConverter.GetBytes( (ushort)(((IPEndPoint)session.Socket.RemoteEndPoint).Port & 0xFFFF) );


            List<byte> data = new List<byte>();
            data.AddRange(remoteIP);
            data.AddRange(remotePort);
            data.Add((byte)query.DataField.Length);
            data.Add(0);
            foreach (var field in query.DataField)
            {
                data.AddRange(Encoding.ASCII.GetBytes(field));
                data.AddRange(new byte[] { 0, 0 });
            }

            string gamename = Encoding.ASCII.GetString(query.QueryFromGameName);
            var onlineServers = QueryReport.Server.QRServer.GameServerList.
                Where(c => c.Value.ServerInfo.Data["gamename"] == gamename);

            foreach (var server in onlineServers)
            {
                byte[] portBytes = Encoding.ASCII.GetBytes(server.Value.ServerInfo.Data["hostport"]);
                //maybe is ping
                data.Add(81);
                data.AddRange(server.Value.RemoteIP);
                //check port
                data.AddRange(server.Value.RemotePort);
                data.Add(255);

                for (int i = 0; i < query.DataField.Length; i++)
                {
                    string temp = server.Value.ServerInfo.Data[query.DataField[i]];
                    data.AddRange(Encoding.ASCII.GetBytes(temp));

                    if (i < query.DataField.Length - 1)
                        data.AddRange(new byte[] { 0, 255 }); // Field Seperator
                }
                data.Add(0);
            }
            data.AddRange(new byte[] { 0, 255, 255, 255, 255 });
            byte[] sendingbuffer = data.ToArray();
            string secretkey = "HA6zkS";
         
            byte[] _sendingBuffer = new EnctypeX()
                .EncryptData
                (
                    Encoding.ASCII.GetBytes(secretkey),
                    query.Challenge,
                    sendingbuffer, 0
                ).
                ToArray();
            session.SendAsync(_sendingBuffer) ;
        }
    }
}

