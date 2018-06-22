using System;

namespace Plank.Net.Data
{
    public abstract class Entity
    {
        #region CONSTRUCTORS

        public Entity()
            : this(Guid.NewGuid(), DateTime.Now, DateTime.Now)
        {

        }

        public Entity(Guid id, DateTime dateCreated, DateTime dateLastModified)
        {
            Id = id;
            DateCreated = dateCreated;
            DateLastModified = dateLastModified;
        }

        #endregion

        #region PROPERTIES

        public Guid Id { get; set; }

        public abstract Guid ParentId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateLastModified { get; set; }

        #endregion
    }
}
