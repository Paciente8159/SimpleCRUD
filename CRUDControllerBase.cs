using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace SimpleCRUD
{
    /// <summary>
    /// Provkeyes the basic Get/Post/Put/Delete operations over a repository.
    /// </summary>
    /// <typeparam name="T">The type of the data model.</typeparam>
    /// <typeparam name="O">The type of the repository.</typeparam>
    [Route("api/[controller]")]
    [ApiController]
    public abstract class CRUDControllerBase<T, O> : ControllerBase
    where T : class, new()
    where O : ICRUDRepository<T>
    {
        /// <summary>
        /// A read-only repository object.
        /// </summary>
        private readonly O _Repository;

        /// <summary>
        /// Gets the PropertyInfo of the primary key field of the data model
        /// </summary>
        /// <value>A PropertyInfo object.</value>
        internal PropertyInfo PrimaryKeyInfo
        {
            get;
            private set;
        } = null;

        /// <summary>
        /// Gets if primary key is an auto increment field.
        /// </summary>
        /// <value>A boolean indicating if the primary key is an auto increment field.</value>
        internal bool IsPKAutoIncrement
        {
            get;
            private set;
        } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="CRUDControllerBase{T, O}"/> class.
        /// </summary>
        /// <param name="repository">A repository object.</param>
        public CRUDControllerBase(O repository)
        {
            _Repository = repository;
            PrimaryKeyInfo = CRUDModelPKAttribute.GetPKProperty<T>();
            CRUDModelPKAttribute att = (CRUDModelPKAttribute) PrimaryKeyInfo.GetCustomAttribute(typeof(CRUDModelPKAttribute));
            IsPKAutoIncrement = att.AutoIncrement;
        }
        
        /// <summary>
        /// Gets all the objects from the table/document.
        /// </summary>
        /// <returns>A list of model objects.</returns>
        [HttpGet]
        public virtual async Task<List<T>> Get()
        {
            List<T> p = await _Repository.ReadAll();
            if(p.Count == 0)
            {
                return null;
            }

            return p;
        }

        /// <summary>
        /// Finds an object given the primary key value from the table/document.
        /// </summary>
        /// <param name="key">A string representing the primary key value. The primary key is set by decorating the primary key property int the model with the <see cref="CRUDModelPKAttribute"/>.</param>
        /// <returns>A model object.</returns>
        [HttpGet("{id}")]
        public virtual async Task<T> Get(string key)
        {
            T p = await _Repository.Read(Convert.ChangeType(key, PrimaryKeyInfo.PropertyType));
            if(p==null)
            {
                return null;
            }

            return p;
        }

        /// <summary>
        /// Inserts a new object in the table/document.
        /// </summary>
        /// <param name="value">A model object.</param>
        /// <returns>A model object if sucessful. Otherwise returns <c>null</c>.</returns>
        [HttpPost]
        public virtual async Task<T> Post(T value)
        {
            T p = await _Repository.Create(value);
            if(p == null)
            {
                return null;
            }

            return p;
        }

        /// <summary>
        /// Finds and updates an object in the table/document.
        /// </summary>
        /// <param name="key">A string representing the primary key value. The primary key is set by decorating the primary key property int the model with the <see cref="CRUDModelPKAttribute"/>.</param>
        /// <param name="value">A model object.</param>
        /// <returns>A model object if sucessful. Otherwise returns <c>null</c></returns>
        [HttpPut("{id}")]
        public virtual async Task<T> Put(string key, T value)
        {
            if(!await _Repository.Update(Convert.ChangeType(key, PrimaryKeyInfo.PropertyType), value))
            {
                return null;
            }

            return value;
        }

        /// <summary>
        /// Finds and deletes an object in the table/document.
        /// </summary>
        /// <param name="key">A string representing the primary key value. The primary key is set by decorating the primary key property int the model with the <see cref="CRUDModelPKAttribute"/>.</param>
        /// <returns><c>true</c> if the value could be found and deleted; otherwise, <c>false</c>.</returns>
        [HttpDelete("{id}")]
        public virtual async Task<bool> Delete(string key)
        {
            if(!await _Repository.Delete(Convert.ChangeType(key, PrimaryKeyInfo.PropertyType)))
            {
                return false;
            }

            return true;
        }
    }
}