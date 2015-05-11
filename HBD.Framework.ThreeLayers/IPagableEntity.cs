using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBD.Framework.ThreeLayers
{
    public interface IPagableEntity<TEntity> :IPagable, IList<TEntity>, IEnumerable<TEntity> where TEntity : class
    {

    }
}
