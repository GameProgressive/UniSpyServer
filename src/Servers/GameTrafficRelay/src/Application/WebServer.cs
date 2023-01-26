using System.Threading.Tasks;
using System;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace UniSpyServer.Servers.GameTrafficRelay.Application
{
    public class WebServer : UniSpyLib.Abstraction.Interface.IServer
    {
        public Guid ServerID { get; private set; }
        public string ServerName { get; private set; }
        public IPEndPoint ListeningEndPoint { get; private set; }
        public WebServer(Guid serverID, string serverName, IPEndPoint endPoint)
        {
            ServerID = serverID;
            ServerName = serverName;
            ListeningEndPoint = endPoint;
        }

        public void Start()
        {
            var builder = WebApplication.CreateBuilder();

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
            // asp.net will block here, we do not want to block our codes, so we let them run in background
            Task.Run(() => app.Run($"http://{ListeningEndPoint}"));
        }
    }
}