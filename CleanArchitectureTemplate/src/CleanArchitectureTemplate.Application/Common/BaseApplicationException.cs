namespace CleanArchitectureTemplate.Application.Common;

public class BaseApplicationException : Exception
{
    protected BaseApplicationException(string message) : base(message){}
}