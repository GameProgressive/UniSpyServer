﻿namespace GameStatus.Entity.Enumerate
{
    /// <summary>
    /// In gamespy protocol there are no error response message
    /// from server to client, which mean we only need to make
    /// internal error system.
    /// </summary>
    public enum GSError
    {
        General,
        Parse,
        Database,
        NoError
    }
}
