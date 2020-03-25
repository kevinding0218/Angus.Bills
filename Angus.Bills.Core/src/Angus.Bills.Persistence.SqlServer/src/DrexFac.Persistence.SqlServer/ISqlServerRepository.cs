using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Angus.Bills.Initializers;
using Angus.Bills.Types.Queries;
using Microsoft.EntityFrameworkCore.Query;

namespace Angus.Bills.Persistence.SqlServer
{
    public interface ISqlServerRepository<TEntity, in TIdentifiable> where TEntity : class, IIdentifiable<TIdentifiable>
    {
        #region READ

        /// <summary>
        ///     overload for get object by primary key
        /// </summary>
        /// <param name="id">id(primary key)</param>
        /// <returns></returns>
        Task<TEntity> GetSingleAsync(TIdentifiable id);

        /// <summary>
        ///     overload for get self-contained object
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <returns></returns>
        Task<TEntity> GetSingleAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true);

        /// <summary>
        ///     Gets the first or default entity based on a predicate, orderby delegate and include delegate. This method default
        ///     no-tracking query.
        ///     https://github.com/Arch/UnitOfWork
        /// </summary>
        /// <param name="selector">The selector for projection.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="disableTracking">
        ///     <c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>
        ///     .
        /// </param>
        /// <returns>
        ///     An <see cref="IPagedList{TEntity}" /> that contains elements that satisfy the condition specified by
        ///     <paramref name="predicate" />.
        /// </returns>
        /// <remarks>This method default no-tracking query.</remarks>
        Task<TResult> GetSingleAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector = null,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true);

        /// <summary>
        ///     overload for get self-contained object
        ///     https://github.com/Arch/UnitOfWork
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="disableTracking">
        ///     <c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>
        ///     .
        /// </param>
        /// <returns>
        ///     An <see cref="IPagedList{TEntity}" /> that contains elements that satisfy the condition specified by "predicate"
        ///     <remarks>This method default no-tracking query.</remarks>
        Task<IEnumerable<TEntity>> GetListAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true);

        /// <summary>
        ///     Gets the list of entity based on a predicate, orderby delegate and include delegate. This method default
        ///     no-tracking query.
        ///     https://github.com/Arch/UnitOfWork
        /// </summary>
        /// <param name="selector">The selector for projection.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="disableTracking">
        ///     <c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>
        ///     .
        /// </param>
        /// <returns>
        ///     An <see cref="IPagedList{TEntity}" /> that contains elements that satisfy the condition specified by "predicate"
        ///     <remarks>This method default no-tracking query.</remarks>
        Task<IEnumerable<TResult>> GetListAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector = null,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true);

        #endregion

        #region CREATE/UPDATE/DELETE

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        void Delete(TIdentifiable id);
        void DeleteRange(IEnumerable<TEntity> entities);
        Task<int> CommitChangesAsync();

        #endregion

        #region SHARED

        Task<bool> IsExistedAsync(Expression<Func<TEntity, bool>> predicate);

        Task<PagedResult<TEntity>> BrowseAsync<TQuery>(Expression<Func<TEntity, bool>> predicate, TQuery query)
            where TQuery : PagedQueryBase;

        #endregion
    }
}