using System;

public abstract class InvalidPlayException : Exception { 
    public InvalidPlayException(string message) : base(message) { }
}