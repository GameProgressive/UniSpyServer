using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameSpyLib.Network.Component
{
    public class BufferDataToken
    {
        public readonly int BufferOffset;
        //public readonly int SendBufferOffset;
        public readonly int BufferBlockSize;

        /// <summary>
        /// Creates a new instance of BufferDataToken
        /// </summary>
        /// <param name="BufferOffset">The offest in the Buffer block allocated to this object</param>
        /// <param name="BlockSize">The total size in the buffer allocated to this object</param>
        public BufferDataToken(int BufferOffset, int BlockSize)
        {
            this.BufferBlockSize = BlockSize; 
            this.BufferOffset = BufferOffset;            
        }
    }
}
