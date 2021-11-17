using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EngineerNotebook.Shared;
using EngineerNotebook.Shared.Models;
using MongoDB.Driver;

namespace EngineerNotebook.Core.Interfaces
{
    /// <summary>
    /// Abstraction layer for how we will retrieve information we'll use throughout our application
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAsyncRepository<T> where T: class, IEntityWithTypedId<string>
    {
        /// <summary>
        /// Get all items from <typeparamref name="T"/>'s collection
        /// </summary>
        /// <returns></returns>
        Task<IList<T>> Get(CancellationToken cancellationToken = default);

        /// <summary>
        /// Determines if any records exist in the table
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>True if records exist, otherwise false</returns>
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Find <typeparamref name="T"/> record where Id is equal to <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Record of <typeparamref name="T"/> if found</returns>
        Task<ResultOf<T>> Find(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Find <typeparamref name="T"/> records where filter matches
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<T>> Find(FilterDefinition<T> filter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Find all records that meet the criteria specified in <paramref name="condition"/>
        /// </summary>       
        /// <param name="condition">Condition that must be met for record to be included</param>
        /// <returns>List of records that fit the defined criteria</returns>
        /// <example>
        /// If you had a record with a Category Property (string)
        /// Find(x=>x.Category == "C# Programming")
        /// </example>
        Task<IList<T>> Find(Predicate<T> condition, CancellationToken cancellationToken = default);

        /// <summary>
        /// Inserts a new record of <typeparamref name="T"/>
        /// </summary>
        /// <param name="data">Record to insert</param>
        /// <returns>Result of <typeparamref name="T"/>, otherwise a failure response if not found</returns>
        Task<ResultOf<T>> Create(T data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates record <typeparamref name="T"/> in database
        /// </summary>
        /// <param name="model">Populated record that will replace the existing one</param>
        /// <returns></returns>
        Task<ResultOf<T>> Update(T model, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete record of <typeparamref name="T"/> with ID of <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResultOf<T>> Delete(string id, CancellationToken cancellationToken = default);


        /// <summary>
        /// Delete all records of <typeparamref name="T"/> that fit the criteria specified in <paramref name="condition"/>
        /// </summary>
        /// <param name="condition">Condition that must be met for a record to be deleted</param>
        /// <returns></returns>
        /// <example>
        /// If you have an object with a CreatedAt property (date time)
        /// If you wanted to delete ALL records with a date time less than today
        /// Delete(x=>x.CreatedAt &lt; DateTime.Today);
        /// </example>
        Task<ResultOf<T>> Delete(Predicate<T> condition, CancellationToken cancellationToken = default);        
    }
}