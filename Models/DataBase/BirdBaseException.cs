using System;

namespace HSEApiTraining.Models.DataBase
{

    [Serializable]
    public class BirdBaseException : Exception
    {
        public BirdBaseException() { }
        public BirdBaseException(string message) : base(message) { }
        public BirdBaseException(string message, Exception inner) : base(message, inner) { }
    }
}
