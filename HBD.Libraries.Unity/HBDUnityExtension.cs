using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HBD.Framework.Log;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.Unity.Configuration;

namespace HBD.Libraries.Unity
{
    public static class HBDUnityExtension
    {
        private static InjectionMember[] GetInjectedMembers()
        {
            return new InjectionMember[]{ 
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LogIntercepterBehaviour>()
            };
        }

        private static InjectionMember[] GetInjectedMembers(params IInterceptionBehavior[] interceptors)
        {
            IList<InjectionMember> list = new List<InjectionMember>(GetInjectedMembers());
            foreach (var inter in interceptors)
                list.Add(new InterceptionBehavior(inter));
            return list.ToArray();
        }
        /// <summary>
        /// Auto register item if not existed and Resolve it
        /// </summary>
        /// <param name="t">Type</param>
        /// <param name="name">Name</param>
        /// <returns>Resolved Instance</returns>
        public static object RegisterResolve(this IUnityContainer container, Type t, string name = null, params IInterceptionBehavior[] interceptors)
        {
            if (!container.IsRegistered(t, name) && !(t.IsAbstract || t.IsInterface))
                container.RegisterType(t, name, GetInjectedMembers(interceptors));
            return container.Resolve(t, name);
        }

        /// <summary>
        /// Auto register item if not existed and Resolve it
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static TType RegisterResolve<TType>(this IUnityContainer container, string name = null)
        {
            return (TType)container.RegisterResolve(typeof(TType), name);
        }

        public static object RegisterResolve(this IUnityContainer container, Type from, Type to, params IInterceptionBehavior[] interceptors)
        {
            if (!container.IsRegistered(from))
                container.RegisterType(from, to, GetInjectedMembers(interceptors));
            return container.Resolve(from);
        }


        public static void RegisterTypeLogingInjection<TFrom, TTo>(this IUnityContainer container, string name = null) where TTo : TFrom
        {
            container.RegisterType<TFrom, TTo>(name, GetInjectedMembers());
        }

        public static object RegisterResolveWithLogingInjection(this IUnityContainer container, Type from, Type to, string name)
        {
            if (!container.IsRegistered(from, name))
                container.RegisterTypeWithLogingInjection(from, to, name);
            return container.Resolve(from, name);
        }

        public static void RegisterTypeWithLogingInjection(this IUnityContainer container, Type from, Type to, string name)
        {
            container.RegisterType(from, to, name, GetInjectedMembers());
        }

        public static TFrom RegisterResolveWithLogingInjection<TFrom, TTo>(this IUnityContainer container, string name = null) where TTo : TFrom
        {
            return (TFrom)container.RegisterResolveWithLogingInjection(typeof(TFrom), typeof(TTo), name);
        }
    }
}
