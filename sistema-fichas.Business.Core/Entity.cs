using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistema_fichas.Business
{
    public class Entity<T> : BaseEntity, IEntity<T>
    {
        public virtual T ID { get; set; }
    }
}
