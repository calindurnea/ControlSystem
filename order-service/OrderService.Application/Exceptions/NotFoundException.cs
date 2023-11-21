using System.Runtime.Serialization;

namespace OrderService.Application.Exceptions;

[Serializable]
public class NotFoundException : Exception
{
    protected NotFoundException(SerializationInfo info, StreamingContext context)
    {

    }

    public NotFoundException(int id) : base($"Entity wit id: '{id}' was not found")
    {
    }
}
