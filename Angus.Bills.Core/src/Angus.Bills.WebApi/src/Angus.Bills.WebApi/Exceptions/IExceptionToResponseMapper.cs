using System;

namespace Angus.Bills.WebApi.Exceptions
{
    public interface IExceptionToResponseMapper
    {
        ExceptionResponse Map(Exception exception);
    }
}