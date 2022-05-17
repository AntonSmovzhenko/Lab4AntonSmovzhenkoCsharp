using System;

namespace Lab4AntonSmovzhenkoCsharp.Exceptions
{
    internal class IncorrectEmailException : Exception
    {
        public IncorrectEmailException(string message) : base(message)
        {
        }
    }
}
