﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebServices.RetroSpyServices.Sake.Entity.Structure.Model.Basic
{
    [CollectionDataContract
    (Name = "values",
    ItemName = "RecordField",
    KeyName = "name",
    ValueName = "value")]
    public class SakeKeyValues : Dictionary<string, string> { }
}