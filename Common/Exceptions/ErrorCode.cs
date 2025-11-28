using System.Net;
using Microsoft.AspNetCore.Http;

namespace Common.Exceptions
{
    using System.Net;

    public class ErrorCode
    {
        public static readonly ErrorDetail USER_NOT_FOUND =
            new(HttpStatusCode.BadRequest, "User not found");

        public static readonly ErrorDetail UNAUTHENTICATED =
            new(HttpStatusCode.Unauthorized, "Unauthenticated");

        public static readonly ErrorDetail UNAUTHORIZED =
            new(HttpStatusCode.Forbidden, "Unauthorized access");

        public static readonly ErrorDetail INTERNAL_ERROR =
            new(HttpStatusCode.InternalServerError, "Internal server error");

        public static readonly ErrorDetail INVALID_REFRESH_TOKEN =
            new(HttpStatusCode.Unauthorized, "Invalid or expired refresh token");

        public static readonly ErrorDetail USER_EXISTED =
            new(HttpStatusCode.BadRequest, "User already exists");

        public static readonly ErrorDetail REGISTER_FAILED =
            new(HttpStatusCode.BadRequest, "Register failed");

        public static readonly ErrorDetail INVALID_INPUT =
            new(HttpStatusCode.BadRequest, "Invalid input");

        public static readonly ErrorDetail ENTITY_NOT_FOUND =
            new(HttpStatusCode.BadRequest, "not found");
    }

    public record ErrorDetail(HttpStatusCode HttpStatusCode, string Message);
}
