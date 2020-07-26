using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoleBase.Helper
{
    public interface IUnityContainer : IDisposable
    {
        void Register<TInterface, TImplementation>() where TImplementation : TInterface;
        TInterface Resolve<TInterface>();
        void Release();
    }
}
