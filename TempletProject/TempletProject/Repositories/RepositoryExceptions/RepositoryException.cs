using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TempletProject.ViewModels
{
    public class RepositoryException : Exception
    {
        public RepositoryException()
        {
        }

        public RepositoryException(string message) : base(message)
        {
        }

        public RepositoryException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
    public class NoRowsAffectedException : RepositoryException
    {
        public NoRowsAffectedException()
        {
        }

        public NoRowsAffectedException(string message) : base(message)
        {
        }

        public NoRowsAffectedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class FailedToConnectToDatabaseException : RepositoryException
    {
        public FailedToConnectToDatabaseException()
        {
        }

        public FailedToConnectToDatabaseException(string message) : base(message)
        {
        }

        public FailedToConnectToDatabaseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}