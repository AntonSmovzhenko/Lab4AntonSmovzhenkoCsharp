using System;


namespace Lab4AntonSmovzhenkoCsharp.Exceptions
{
    internal class BirthdayInFutureException : Exception
    {
        public BirthdayInFutureException(string message) : base(message)
        {
        }
    }
}
