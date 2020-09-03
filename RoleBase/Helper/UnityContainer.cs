using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoleBase.Helper
{
    public class UnityContainer : IUnityContainer
    {
        Dictionary<Type, List<Type>> Maps = new Dictionary<Type, List<Type>>();

        public void Register<TInterface, TImplementation>() where TImplementation : TInterface
        {
            if (Maps.ContainsKey(typeof(TInterface)))
            {
                var list = Maps[typeof(TInterface)];
                var insance = list.Where(o => o == typeof(TImplementation));

                if (insance.Any())
                    return;
                else
                    Maps[typeof(TInterface)].Add(typeof(TImplementation));
                return;
            }

            Maps.Add(typeof(TInterface), new List<Type>() { typeof(TImplementation) });
        }

        public TInterface Resolve<TInterface, TImplementation>() where TImplementation : TInterface
        {
            var list = Maps[typeof(TInterface)];
            var insance = list.Where(o => o == typeof(TImplementation)).FirstOrDefault();

            if (insance != null)
            {
                Type fooConcreteType = insance; //.Find(o => o == typeof(TInstanceType));
                Object instance = Activator.CreateInstance(fooConcreteType);
                return (TInterface)instance;
            }
            else
                return default;
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