using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Shared
{
    public abstract class BaseEntity
    {
        public Guid id { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataModificacao { get; set; }

        protected BaseEntity()
        {
            id = Guid.NewGuid();
            DataCriacao = DateTime.Now;
        }
    }
}
