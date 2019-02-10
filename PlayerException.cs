using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
    {
        public PlayerException()
        {
        }
        public PlayerException(string message) : base(message)
        {
        }
        public PlayerException(string message, Exception innerException) : base(message, innerException)
        {
        }
        protected PlayerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class FailedToPlayException : PlayerException
    {
        public FailedToPlayException()
        {
        }
        public FailedToPlayException(string message) : base(message)
        {
        }
        public FailedToPlayException(string message, Exception innerException) : base(message, innerException)
        {
        }
        protected FailedToPlayException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        public string Path { get; set; }
    }
}
