using System;

public class InvalidCallException : Exception {
    public InvalidCallException() { }
    public InvalidCallException(string message) : base(message) { }
}