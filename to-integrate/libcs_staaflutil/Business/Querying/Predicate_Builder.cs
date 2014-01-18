﻿

#if NET30
using System.Linq.Expressions;
#endif

namespace Fairweather.Service
{
#if NET30
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
                                                           Expression<Func<T, bool>> expr2) {

            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            var ret = Expression.Lambda<Func<T, bool>>
                     (Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);

            return ret;

        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                             Expression<Func<T, bool>> expr2) {

            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            var ret = Expression.Lambda<Func<T, bool>>
                      (Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);

            return ret;

        }
    }
#endif
}