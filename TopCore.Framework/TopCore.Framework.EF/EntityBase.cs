﻿#region	License

//------------------------------------------------------------------------------------------------
// <Auto-generated>
//     <Author> Top Nguyen (http://topnguyen.net) </Author>
//     <Project> TopCore.Auth.Domain </Project>
//     <File> 
//         <Name> EntityBase.cs </Name>
//         <Created> 27 03 2017 02:39:27 PM </Created>
//         <Key> 9BBFB04B-177D-4D87-B047-2F94F957E579 </Key>
//     </File>
//     <Summary>
//         EntityBase is abstract entity to another entity inherit
//     </Summary>
// </Auto-generated>
//------------------------------------------------------------------------------------------------

#endregion License

using System;
using System.ComponentModel.DataAnnotations;

namespace TopCore.Framework.EF
{
    public interface IBaseEntity
    {
        DateTime CreatedOnUtc { get; set; }

        DateTime? LastUpdatedOnUtc { get; set; }

        bool IsDeleted { get; set; }

        DateTime? DeletedOnUtc { get; set; }

        [Timestamp]
        byte[] Version { get; set; }
    }

    /// <summary>
    /// Entity Base for Entity Framework
    /// </summary>
    /// <typeparam name="TId">Id type of this entity</typeparam>
    /// <typeparam name="TOwnerId">Id type of who do the action of entity (for tracking)</typeparam>
    public abstract class BaseEntity<TId, TOwnerId> : IBaseEntity
    {
        protected BaseEntity(TOwnerId createdBy)
        {
            CreatedBy = createdBy;
        }

        public TId Id { get; set; }

        /// <summary>
        ///     Unique key
        /// </summary>
        public Guid Key { get; set; } = Guid.NewGuid();

        public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// For tracking
        /// </summary>
        public TOwnerId CreatedBy { get; set; }

        public DateTime? LastUpdatedOnUtc { get; set; }

        /// <summary>
        /// For tracking
        /// </summary>
        public TOwnerId UpdatedBy { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOnUtc { get; set; }

        /// <summary>
        /// For tracking
        /// </summary>
        public TOwnerId DeletedBy { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }
    }

    public abstract class BaseEntity : BaseEntity<int, int>
    {
        protected BaseEntity(int createdBy) : base(createdBy)
        {
        }
    }
}