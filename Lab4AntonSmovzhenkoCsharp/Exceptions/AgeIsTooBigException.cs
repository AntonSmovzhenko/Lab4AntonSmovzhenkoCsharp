using System;

namespace Lab4AntonSmovzhenkoCsharp.Exceptions
{
    internal class AgeIsTooBigException : Exception
    {
        public AgeIsTooBigException(string message) : base(message)
        {
        }
    }
}
