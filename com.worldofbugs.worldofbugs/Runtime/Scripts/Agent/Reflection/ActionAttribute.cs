using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Actuators;

namespace WorldOfBugs {

    [System.AttributeUsage(
         System.AttributeTargets.Method,
         AllowMultiple = false)]
    public class ActionAttribute : System.Attribute {

        public ActionAttribute() {}

        public static IActuator CreateActuator(Unity.MLAgents.Agent agent) {
            Tuple<Action[], Action<float>[]> action_methods = ResolveActionAttributes(agent);
            return new ReflectionActuator(agent, action_methods.Item1, action_methods.Item2);
        }

        public static string[] ActionMeanings(Unity.MLAgents.Agent agent) {
            IEnumerable<string> discrete_info = GetMethodInfo<Action>(agent).Select(x => x.Name);
            IEnumerable<string> continuous_info = GetMethodInfo<Action<float>>(agent).Select(
                    x => x.Name);
            return discrete_info.Concat(continuous_info).ToArray();
        }

        private static BindingFlags _bindingFlags = BindingFlags.NonPublic | BindingFlags.Public
                | BindingFlags.Instance;

        static MethodInfo[] GetMethodInfo<D>(Unity.MLAgents.Agent agent) where D :
            System.Delegate {
            MethodInfo[] action_methods = agent.GetType().GetMethods(_bindingFlags)
                                          .Where(m => m.GetCustomAttribute<ActionAttribute>() != null)
                                          .ToArray();
            return action_methods.Where(x => IsMethodCompatibleWithDelegate<D>(x)).ToArray();
        }

        static D[] GetDelegateMethods<D>(Unity.MLAgents.Agent agent) where D : System.Delegate {
            MethodInfo[] methods = GetMethodInfo<D>(agent);
            return methods.Select(m => (D) Delegate.CreateDelegate(typeof(D), agent, m,
                                  true)).ToArray();
        }

        static Tuple<Action[], Action<float>[]> ResolveActionAttributes(
            Unity.MLAgents.Agent agent) {
            Action[] discrete = GetDelegateMethods<Action>(agent);
            Action<float>[] continuous = GetDelegateMethods<Action<float>>(agent);
            return new Tuple<Action[], Action<float>[]>(discrete, continuous);
        }

        static bool IsMethodCompatibleWithDelegate<T>(MethodInfo method) where T :
            System.Delegate  {
            Type delegateType = typeof(T);
            MethodInfo delegateSignature = delegateType.GetMethod("Invoke");
            bool parametersEqual = delegateSignature
                                   .GetParameters()
                                   .Select(x => x.ParameterType)
                                   .SequenceEqual(method.GetParameters()
                                                  .Select(x => x.ParameterType));
            return delegateSignature.ReturnType == method.ReturnType &&
                   parametersEqual;
        }
    }
}
