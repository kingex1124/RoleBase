using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoleBase.Helper
{
    public class UnityContainer : IUnityContainer
    {
        Dictionary<Type, Type> Maps = new Dictionary<Type, Type>();

        public void Register<TInterface, TImplementation>() where TImplementation : TInterface
        {
            if (Maps.ContainsKey(typeof(TInterface)))
            {
                // Maps[typeof(TAbstractType)].Add(typeof(TConcreteType));
                return;
            }

            Maps.Add(typeof(TInterface), typeof(TImplementation));
        }

        public TInterface Resolve<TInterface>()
        {
            Type fooConcreteType = Maps[typeof(TInterface)]; //.Find(o => o == typeof(TInstanceType));
            Object instance = Activator.CreateInstance(fooConcreteType);
            return (TInterface)instance;
        }

        public void Release()
        {
            Dispose();
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}