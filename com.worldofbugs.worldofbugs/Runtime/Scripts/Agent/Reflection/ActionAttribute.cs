using System;
using System.Reflection;
using System.Linq;

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

        private static BindingFlags _bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

        static Tuple<Action[], Action<float>[]> ResolveActionAttributes(Unity.MLAgents.Agent agent) {
            MethodInfo[] action_methods = agent.GetType().GetMethods(_bindingFlags)
                    .Where(m => m.GetCustomAttribute<ActionAttribute>() != null)
                    .ToArray();

            MethodInfo[] discrete_methods = action_methods.Where(x => IsMethodCompatibleWithDelegate<Action>(x)).ToArray();
            Action[] discrete = discrete_methods.Select(m => (Action) Delegate.CreateDelegate(typeof(Action), agent, m, true)).ToArray();
            
            MethodInfo[] continuous_methods = action_methods.Where(x => IsMethodCompatibleWithDelegate<Action<float>>(x)).ToArray();
            Action<float>[] continuous = continuous_methods.Select(m => (Action<float>) Delegate.CreateDelegate(typeof(Action<float>), agent, m, true)).ToArray();
            
            return new Tuple<Action[], Action<float>[]>(discrete, continuous);
        }

        static bool IsMethodCompatibleWithDelegate<T>(MethodInfo method) where T : class {
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