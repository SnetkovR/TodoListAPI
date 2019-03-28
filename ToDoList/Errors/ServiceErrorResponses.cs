using System;
using System.Net;
using ClientModels.Errors;

namespace ToDoList.Errors
{
    public class ServiceErrorResponses
    {
        public static ServiceErrorResponse TodoNotFound(string todoId)
        {
            if (todoId == null)
            {
                throw new ArgumentNullException(nameof(todoId));
            }

            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.NotFound,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.NotFound,
                    Message = $"A todo with \"{todoId}\" not found.",
                    Target = "todo"
                }
            };

            return error;
        }

        public static ServiceErrorResponse IncorrectPassword()
        {
            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.NotFound,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.ValidationError,
                    Message = $"Incorrect password entered.",
                    Target = "password"
                }
            };

            return error;
        }

        public static ServiceErrorResponse UserNotFound(string login)
        {
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.NotFound,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.NotFound,
                    Message = $"A note with \"{login}\" not found.",
                    Target = "note"
                }
            };

            return error;
        }


        public static ServiceErrorResponse BodyIsMissing(string target)
        {
            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.BadRequest,
                    Message = "Request body is empty.",
                    Target = target
                }
            };

            return error;
        }
    }
}