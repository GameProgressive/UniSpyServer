using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Enumerate;
using System.Collections.Generic;

namespace GameStatus.Entity.Structure.Request
{
    public class NewGameRequest : GSRequestBase
    {
        public NewGameRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override object Parse()
        {
           var flag = (GSError)base.Parse();
            if (flag != GSError.NoError)
            {
                return flag;
            }

            return GSError.NoError;
        }
    }
}
