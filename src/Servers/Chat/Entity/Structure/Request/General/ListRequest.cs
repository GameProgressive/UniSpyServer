﻿using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;
using Chat.Entity.Exception;

namespace Chat.Entity.Structure.Request.General
{
    [RequestContract("LIST")]
    internal sealed class ListRequest : RequestBase
    {
        public ListRequest(string rawRequest) : base(rawRequest)
        {
        }

        public bool IsSearchingChannel { get; private set; }
        public bool IsSearchingUser { get; private set; }
        public string Filter { get; private set; }

        public override void Parse()
        {
            base.Parse();


            if (_cmdParams.Count == 0)
            {
                throw new Exception.Exception("Search filter is missing.");
            }
            IsSearchingChannel = true;
            Filter = _cmdParams[0];
        }

    }
}
