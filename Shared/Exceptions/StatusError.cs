using Microsoft.AspNetCore.Mvc;

namespace RiwiTalent.Shared.Exceptions
{
    public class StatusError
    {
        //404
        public static ProblemDetails CreateNotFound(string detail, string instance)
        {
            return new ProblemDetails
            {
                Title = "Error 404 - Not Found",
                Status = StatusCodes.Status404NotFound,
                Detail = detail,
                Instance = instance
            };
        }

        //400
        public static ProblemDetails CreateBadRequest(string instance)
        {
            return new ProblemDetails
            {
                Title = "Error 400 - Bad Request",
                Status = StatusCodes.Status400BadRequest,
                Detail = "The request you are trying to make is not valid. Please check the data sent and try again.",
                Instance = instance
            };
        }


        //500
        public static ProblemDetails CreateInternalServerError(Exception ex)
        {
            return new ProblemDetails
            {
                Title = "Error 500 - Internal Server Error",
                Status = StatusCodes.Status500InternalServerError,
                Detail = ex.Message,
                Instance = Guid.NewGuid().ToString()
            };
        }

        //Exceptions
        public class InvalidKeyException : Exception
        {
            public InvalidKeyException(string message) : base(message){}
        }

        public class ExternalKeyNotFound : Exception
        {
            public ExternalKeyNotFound(string message) : base(message){}
        }

        public class ObjectIdNotFound : Exception
        {
            public ObjectIdNotFound(string message) : base(message){}
        }

        public class EmailNotFound : Exception
        {
            public EmailNotFound(string message) : base(message){}
        }

        public class CoderAlreadyInGroup : Exception
        {
            public CoderAlreadyInGroup(string message) : base(message){}
        }
    }
}   
