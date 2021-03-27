using System;
using System.Linq;

using System.Linq.Expressions;

namespace YrsWeb
{
    public static class LinqEx
    {
        public static IQueryable<TSource> WhereIsNotNullOrEmpty<TSource>
    (this IQueryable<TSource> Source, decimal? Condition, Expression<Func<TSource, bool>> Predicate)
        {
            if (Condition.HasValue)
                return Source.Where(Predicate);
            else
                return Source;
        }

        public static IQueryable<TSource> WhereIf<TSource>
            (this IQueryable<TSource> Source, bool Condition, Expression<Func<TSource, bool>> Predicate)
        {
            if (Condition)
                return Source.Where(Predicate);
            else
                return Source;
        }

        public static IQueryable<TSource> WhereIsNotNullOrEmpty<TSource>
            (this IQueryable<TSource> Source, string Condition, Expression<Func<TSource, bool>> Predicate)
        {
            if (!String.IsNullOrEmpty(Condition))
                return Source.Where(Predicate);
            else
                return Source;
        }
        public static IQueryable<TSource> WhereIsNotNullOrEmpty<TSource>
            (this IQueryable<TSource> Source, DateTime? Condition, Expression<Func<TSource, bool>> Predicate)
        {
            if (Condition.HasValue)
                return Source.Where(Predicate);
            else
                return Source;
        }
        public static IQueryable<TSource> WhereIsNotNullOrEmpty<TSource>
            (this IQueryable<TSource> Source, byte? Condition, Expression<Func<TSource, bool>> Predicate)
        {
            if (Condition.HasValue)
                return Source.Where(Predicate);
            else
                return Source;
        }

        public static IQueryable<TSource> WhereIsNotNullOrEmpty<TSource>
            (this IQueryable<TSource> Source, bool? Condition, Expression<Func<TSource, bool>> Predicate)
        {
            if (Condition.HasValue)
                return Source.Where(Predicate);
            else
                return Source;
        }

        public static IQueryable<TSource> WhereIsNotMinus<TSource>
            (this IQueryable<TSource> Source, int Condition, Expression<Func<TSource, bool>> Predicate)
        {
            if (Condition >= 0)
                return Source.Where(Predicate);
            else
                return Source;
        }

        public static IQueryable<TSource> WhereIsNotMinus<TSource>
            (this IQueryable<TSource> Source, long Condition, Expression<Func<TSource, bool>> Predicate)
        {
            if (Condition >= 0)
                return Source.Where(Predicate);
            else
                return Source;
        }

    }
}
