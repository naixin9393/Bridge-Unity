using System;

namespace BridgeEdu.Game.Play.Exceptions {
    public abstract class InvalidPlayException : Exception {
        public InvalidPlayException(string message) : base(message) { }
    }
}