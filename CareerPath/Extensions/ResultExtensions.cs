using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace CareerPath.Extensions
{
    public static class ResultExtensions
    {
        public static ObjectResult ToProblem(this Result result)
        {
            return CreateProblemResult(result);
        }

        public static ObjectResult ToProblem<T>(this Result<T> result)
        {
            return CreateProblemResult(result);
        }

        private static ObjectResult CreateProblemResult(ResultBase result)
        {
            var firstError = result.Errors.FirstOrDefault();

            // نحاول نقرأ الكود وحالة HTTP من الـ Metadata إن وجدت
            var statusCode = firstError?.Metadata.TryGetValue("StatusCode", out var statusObj) == true
                ? Convert.ToInt32(statusObj)
                : StatusCodes.Status400BadRequest;

            var code = firstError?.Metadata.TryGetValue("Code", out var codeObj) == true
                ? codeObj?.ToString()
                : "BAD_REQUEST";

            var description = firstError?.Message ?? "An unknown error occurred.";

            // نستخدم Results.Problem لبناء ProblemDetails قياسي
            var problem = Results.Problem(statusCode: statusCode);
            var problemDetails = problem.GetType()
                                        .GetProperty(nameof(ProblemDetails))!
                                        .GetValue(problem) as ProblemDetails;

            problemDetails!.Extensions = new Dictionary<string, object?>
            {
                {
                    "errors",
                    new[]
                    {
                        code,
                        description
                    }
                }
            };

            return new ObjectResult(problemDetails)
            {
                StatusCode = statusCode
            };
        }
    }
}