﻿using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Structure.Redis;

namespace QueryReport.Entity.Structure.Result
{
    internal sealed class EchoResult : ResultBase
    {
        public GameServerInfo Info { get; set; }
        public EchoResult()
        {
        }
    }
}