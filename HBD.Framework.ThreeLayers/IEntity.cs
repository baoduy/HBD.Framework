using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBD.Framework.ThreeLayers
{
    public interface IEntity
    {
        System.DateTime CreatedDate { get; set; }
        string CreatedBy { get; set; }
        Nullable<System.DateTime> UpdatedDate { get; set; }
        string UpdatedBy { get; set; }
    }
}
