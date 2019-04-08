using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleCRUD
{
    /// <summary>
    /// Defines generalized methods that class implements to create CRUD operations on a database table/document.
    /// </summary>
    /// <typeparam name="T">The type of object that models the database table/document.</typeparam>
    public interface ICRUDRepository<T>
    where T : class, new()
    {
        /// <summary>
        /// Reads all values in the table/document.
        /// </summary>
        /// <returns>A list of model objects.</returns>
        Task<List<T>> ReadAll();

        /// <summary>
        /// Finds and reads an object in the table/document given a primary key.
        /// </summary>
        /// <param name="key">The primary key object of the model.</param>
        /// <returns>If an entry with the given primary key is found returns a model object, else return null.</returns>
        Task<T> Read(object key);
        
        /// <summary>
        /// Inserts an object in the table/document.
        /// </summary>
        /// <param name="value">The object to be inserted in the table/document.</param>
        /// <returns>Returns the new object if sucessful, else returns null</returns>
        Task<T> Create(T value);
        
        /// <summary>
        /// Finds a table/document object with the given primary key and updates it.
        /// </summary>
        /// <param name="key">The primary key object of the model.</param>
        /// <param name="value">The modified model object.</param>
        /// <returns>Returns true if sucessful, else return false.</returns>
        Task<bool> Update(object key, T value);

        /// <summary>
        /// Finds and deletes a model object with a specific primary key.
        /// </summary>
        /// <param name="key">The primary key object of the model.</param>
        /// <returns>>Returns true if sucessful, else return false.</returns>
        Task<bool> Delete(object key);
    }
}