namespace BillNumParser;

using System;

public class InvalidBillNumberException : Exception
{
    public InvalidBillNumberException() { }

    public InvalidBillNumberException(string message) : base(message) { }

    public InvalidBillNumberException(string message, Exception inner) : base(message, inner) { }

}