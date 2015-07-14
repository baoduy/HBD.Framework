using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HBD.Framework.Core
{
    public interface IHBDFeature : IDisposable
    {
        string Status { get; }
        void Start();
    }
}
