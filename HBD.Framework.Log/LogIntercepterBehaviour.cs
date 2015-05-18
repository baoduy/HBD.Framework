using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace HBD.Framework.Log
{
    public class LogIntercepterBehaviour : IInterceptionBehavior
    {
        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            // Before invoking the method on the original target.
            LogManager.Write(String.Format("Invoking method '{0}'", input.MethodBase));

            // Invoke the next behavior in the chain.
            var result = getNext()(input, getNext);

            // After invoking the method on the original target.
            if (result.Exception != null)
            {
                LogManager.Write(new Exception(
                    String.Format("Method '{0}' threw exception: '{1}'", input.MethodBase, result.Exception.Message),
                    result.Exception));
            }
            else LogManager.Write(String.Format("Method '{0}' returned '{1}'", input.MethodBase, result.ReturnValue));

            return result;
        }

        public bool WillExecute
        {
            get { return true; }
        }
    }
}
