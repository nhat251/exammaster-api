using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Requests;
using Domain.Entities;

namespace Application.Specifications
{
    public class UserSpecifications
    {
        public static Expression<Func<ApplicationUser, bool>> GetUserByRequest(GetUserRequest request)
        {
            // B?t d?u v?i bi?u th?c true d? có th? chain thêm filter
            Expression<Func<ApplicationUser, bool>> predicate = u => true;

            if (!string.IsNullOrWhiteSpace(request.Username))
            {
                predicate = predicate.AndAlso(u =>
                    u.UserName.ToLower().Contains(request.Username.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                predicate = predicate.AndAlso(u =>
                    u.Email.ToLower().Contains(request.Email.ToLower()));
            }

            if (request.CreatedFrom.HasValue)
            {
                predicate = predicate.AndAlso(u => u.CreatedAt >= request.CreatedFrom.Value);
            }

            if (request.CreatedTo.HasValue)
            {
                predicate = predicate.AndAlso(u => u.CreatedAt <= request.CreatedTo.Value);
            }

            return predicate;
        }
    }

    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> AndAlso<T>(
            this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var body = Expression.AndAlso(
                Expression.Invoke(expr1, parameter),
                Expression.Invoke(expr2, parameter)
            );

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
    }
}
