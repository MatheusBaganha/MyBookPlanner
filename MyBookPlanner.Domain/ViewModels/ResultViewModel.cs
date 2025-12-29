using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace MyBookPlanner.Domain.ViewModels
{

    public class Result
    {
        public HttpStatusCode StatusCode { get; set; }
        public List<string> Erros { get; set; }

        public Result()
        {
            Erros = new List<string>();
        }

        public IActionResult ToActionResult()
        {
            switch (StatusCode)
            {
                case HttpStatusCode.OK:
                    return Erros.Any() ? new ObjectResult(Erros)
                    {
                        StatusCode = (int)StatusCode
                    } : new OkResult();
                case HttpStatusCode.Created:
                    return new StatusCodeResult(201);
                case HttpStatusCode.BadRequest:
                    return new BadRequestObjectResult(Erros);
                case HttpStatusCode.NotFound:
                    return new NotFoundObjectResult(Erros);
                case HttpStatusCode.Conflict:
                    return new ConflictObjectResult(Erros);
                case HttpStatusCode.Unauthorized:
                    return new UnauthorizedResult();
                case HttpStatusCode.PreconditionFailed:
                    return new StatusCodeResult(412);
                case HttpStatusCode.InternalServerError:
                default:
                    return new ObjectResult(Erros)
                    {
                        StatusCode = (int)StatusCode
                    };
            }
        }

        public static Result Sucess(List<string> erros = null)
        {
            return (erros == null || !erros.Any()) ? new Result
            {
                StatusCode = HttpStatusCode.OK
            } :
            new Result
            {
                StatusCode = HttpStatusCode.OK,
                Erros = erros
            };
        }

        public static Result Created()
        {
            return new Result
            {
                StatusCode = HttpStatusCode.Created
            };
        }

        public static Result NotFound()
        {
            return new Result
            {
                StatusCode = HttpStatusCode.NotFound
            };
        }

        public static Result NotFound(string erroMsg = "")
        {
            var data = new Result
            {
                StatusCode = HttpStatusCode.NotFound
            };

            if (!string.IsNullOrEmpty(erroMsg))
                data.Erros.Add(erroMsg);

            return data;
        }

        public static Result BadRequest()
        {
            return new Result
            {
                StatusCode = HttpStatusCode.BadRequest
            };
        }

        public static Result BadRequest(string erroMsg = "")
        {
            var data = new Result
            {
                StatusCode = HttpStatusCode.BadRequest
            };

            if (!string.IsNullOrEmpty(erroMsg))
                data.Erros.Add(erroMsg);

            return data;
        }

        public static Result BadRequest(params string[] erros)
        {
            var data = new Result
            {
                StatusCode = HttpStatusCode.BadRequest
            };
            data.Erros.AddRange(erros);
            return data;
        }

        public static Result Conflict()
        {
            return new Result
            {
                StatusCode = HttpStatusCode.Conflict
            };
        }

        public static Result Conflict(string erroMsg = "")
        {
            var data = new Result
            {
                StatusCode = HttpStatusCode.Conflict
            };

            if (!string.IsNullOrEmpty(erroMsg))
                data.Erros.Add(erroMsg);

            return data;
        }

        public static Result PreconditionFailed()
        {
            return new Result
            {
                StatusCode = HttpStatusCode.PreconditionFailed
            };
        }

        public static Result PreconditionFailed(string erroMsg = "")
        {
            var data = new Result
            {
                StatusCode = HttpStatusCode.PreconditionFailed
            };

            if (!string.IsNullOrEmpty(erroMsg))
                data.Erros.Add(erroMsg);

            return data;
        }

        public static Result Unauthorized()
        {
            return new Result
            {
                StatusCode = HttpStatusCode.Unauthorized
            };
        }

        public static Result Unauthorized(string erroMsg = "")
        {
            var data = new Result
            {
                StatusCode = HttpStatusCode.Unauthorized
            };

            if (!string.IsNullOrEmpty(erroMsg))
                data.Erros.Add(erroMsg);

            return data;
        }

        public static Result Error(params string[] erros)
        {
            var data = new Result
            {
                StatusCode = HttpStatusCode.InternalServerError
            };
            data.Erros.AddRange(erros);
            return data;
        }

        public static Result Error(string erroMsg = "")
        {
            var data = new Result
            {
                StatusCode = HttpStatusCode.InternalServerError
            };

            if (!string.IsNullOrEmpty(erroMsg))
                data.Erros.Add(erroMsg);

            return data;
        }
    }

    public class Result<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public T Data { get; set; }
        public List<string> Erros { get; set; }

        public Result()
        {
            Erros = new List<string>();
        }

        public IActionResult ToActionResult()
        {
            switch (StatusCode)
            {
                case HttpStatusCode.OK:
                    return Erros.Any() ? new OkObjectResult(new { Data = Data, Erros = Erros })
                    {
                        StatusCode = (int)StatusCode
                    } : new OkObjectResult(Data);
                case HttpStatusCode.Created:
                    return new CreatedResult("", Data);
                case HttpStatusCode.BadRequest:
                    return new BadRequestObjectResult(Erros);
                case HttpStatusCode.NotFound:
                    return new NotFoundObjectResult(Erros);
                case HttpStatusCode.Conflict:
                    return new ConflictObjectResult(Erros);
                case HttpStatusCode.Unauthorized:
                    return new UnauthorizedResult();
                case HttpStatusCode.PreconditionFailed:
                    return new StatusCodeResult(412);
                case HttpStatusCode.InternalServerError:
                default:
                    return new ObjectResult(Erros)
                    {
                        StatusCode = (int)StatusCode
                    };
            }
        }

        public static Result<T> Sucess(T? data = default, List<string> erros = null)
        {
            return (erros == null || !erros.Any()) ? new Result<T>
            {
                StatusCode = HttpStatusCode.OK,
                Data = data
            } :
            new Result<T>
            {
                StatusCode = HttpStatusCode.OK,
                Data = data,
                Erros = erros
            };
        }

        public static Result<T> Created(T data)
        {
            return new Result<T>
            {
                StatusCode = HttpStatusCode.Created,
                Data = data
            };
        }

        public static Result<T> NotFound(T data)
        {
            return new Result<T>
            {
                StatusCode = HttpStatusCode.NotFound,
                Data = data
            };
        }

        public static Result<T> NotFound(string erroMsg = "")
        {
            var data = new Result<T>
            {
                StatusCode = HttpStatusCode.NotFound
            };

            if (!string.IsNullOrEmpty(erroMsg))
                data.Erros.Add(erroMsg);

            return data;
        }

        public static Result<T> Conflict(T data)
        {
            return new Result<T>
            {
                StatusCode = HttpStatusCode.Conflict,
                Data = data
            };
        }

        public static Result<T> Conflict(string erroMsg = "")
        {
            var data = new Result<T>
            {
                StatusCode = HttpStatusCode.Conflict
            };

            if (!string.IsNullOrEmpty(erroMsg))
                data.Erros.Add(erroMsg);

            return data;
        }

        public static Result<T> PreconditionFailed(T data)
        {
            return new Result<T>
            {
                StatusCode = HttpStatusCode.PreconditionFailed,
                Data = data
            };
        }

        public static Result<T> PreconditionFailed(string erroMsg = "")
        {
            var data = new Result<T>
            {
                StatusCode = HttpStatusCode.PreconditionFailed
            };

            if (!string.IsNullOrEmpty(erroMsg))
                data.Erros.Add(erroMsg);

            return data;
        }

        public static Result<T> Unauthorized(T data)
        {
            return new Result<T>
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Data = data
            };
        }

        public static Result<T> Unauthorized(string erroMsg = "")
        {
            var data = new Result<T>
            {
                StatusCode = HttpStatusCode.Unauthorized
            };

            if (!string.IsNullOrEmpty(erroMsg))
                data.Erros.Add(erroMsg);

            return data;
        }

        public static Result<T> BadRequest(T data)
        {
            return new Result<T>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Data = data
            };
        }

        public static Result<T> BadRequest(string erroMsg = "")
        {
            var data = new Result<T>
            {
                StatusCode = HttpStatusCode.BadRequest
            };

            if (!string.IsNullOrEmpty(erroMsg))
                data.Erros.Add(erroMsg);

            return data;
        }

        public static Result<T> BadRequest(params string[] erros)
        {
            var data = new Result<T>
            {
                StatusCode = HttpStatusCode.BadRequest
            };
            data.Erros.AddRange(erros);
            return data;
        }

        public static Result<T> Error(params string[] erros)
        {
            var data = new Result<T>
            {
                StatusCode = HttpStatusCode.InternalServerError
            };
            data.Erros.AddRange(erros);
            return data;
        }

        public static Result<T> Error(string erroMsg = "")
        {
            var data = new Result<T>
            {
                StatusCode = HttpStatusCode.InternalServerError
            };

            if (!string.IsNullOrEmpty(erroMsg))
                data.Erros.Add(erroMsg);

            return data;
        }
    }
}

