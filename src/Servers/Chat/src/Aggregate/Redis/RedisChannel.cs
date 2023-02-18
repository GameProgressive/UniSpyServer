using UniSpy.Server.Chat.Aggregate.Redis.Contract;
using UniSpy.Server.Chat.Handler.CmdHandler.Channel;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Aggregate;

namespace UniSpy.Server.Chat.Aggregate.Redis
{
    /// <summary>
    /// When a local channel is created the user message will send to redis channel
    /// redis channel is like a broadcast platform which will broadcast the message to all the user
    /// when user is connected to unispy chat server
    /// </summary>
    public class RedisChannel : RedisChannelBase<ChannelMessage>
    {
        public RedisChannel(string chatChannelName) : base($"{RedisChannelName.ChatChannelPrefix}:{chatChannelName}") { }

        public override void ReceivedMessage(ChannelMessage message)
        {
            // base.ReceivedMessage(message);
            IHandler handler = null;
            switch (message.MessageType)
            {
                case "GETCHANKEY":
                    handler = new GetChannelKeyHandler(message.Client, message.Request);
                    break;
                case "GETCKEY":
                    handler = new GetCKeyHandler(message.Client, message.Request);
                    break;
                case "JOIN":
                    handler = new JoinHandler(message.Client, message.Request);
                    break;
                case "KICK":
                    handler = new KickHandler(message.Client, message.Request);
                    break;
                case "MODE":
                    handler = new ModeHandler(message.Client, message.Request);
                    break;
                case "NAMES":
                    handler = new NamesHandler(message.Client, message.Request);
                    break;
                case "PART":
                    handler = new PartHandler(message.Client, message.Request);
                    break;
                case "SETCHANKEY":
                    handler = new SetChannelKeyHandler(message.Client, message.Request);
                    break;
                case "SETCKEY":
                    handler = new SetCKeyHandler(message.Client, message.Request);
                    break;
                case "TOPIC":
                    handler = new TopicHandler(message.Client, message.Request);
                    break;
                default:
                    break;
            }

            handler?.Handle();
        }

    }
    /// <summary>
    /// The first join message will process here
    /// </summary>
    public class GeneralMessageChannel : RedisChannelBase<ChannelMessage>
    {
        public GeneralMessageChannel() : base(RedisChannelName.ChatChannelPrefix)
        {
        }
        public override void ReceivedMessage(ChannelMessage message)
        {
            if (message.MessageType != "JOIN")
            {
                return;
            }

            var handler = new JoinHandler(message.Client, message.Request);
            handler.Handle();
        }
    }
}