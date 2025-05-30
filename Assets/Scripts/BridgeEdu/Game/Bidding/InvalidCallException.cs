using System;

namespace BridgeEdu.Game.Bidding {
    public class InvalidCallException : Exception {
        public InvalidCallException() { }
        public InvalidCallException(string message) : base(message) { }
    }
}