using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.ChatCommand
{
    public class LISTLIMITRequest : ChatRequestBase
    {
        public LISTLIMITRequest(string rawRequest) : base(rawRequest)
        {
        }

        public int MaxNumberOfChannels { get; protected set; }
        public string Filter { get; protected set; }

        public override object Parse()
        {
            if(!(bool)base.Parse())
            {
                return false;
            }


            if (_cmdParams.Count != 2)
            {
                return false;
            }
            int max;
            if (!int.TryParse(_cmdParams[0], out max))
            {
                return false;
            }
            MaxNumberOfChannels = max;

            Filter = _cmdParams[1];

            return true;
        }
    }
}
