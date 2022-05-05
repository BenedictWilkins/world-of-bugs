using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;


using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

namespace WorldOfBugs {

    // TODO support continuous actions, this can be done by adding a delegate Action<GameObject, Float> ? 
    public abstract class Actions<A> {

        protected FieldInfo[] Fields { get { return this.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public); } }
        protected MethodInfo[] Methods { get { return this.GetType().GetMethods(BindingFlags.Static | BindingFlags.Public); } }
        
        public ActionSpec ActionSpec { get { return ActionSpec.MakeDiscrete(Fields.Where(x => (bool) x.GetValue(this)).ToArray().Length); }}
        public string[] ActionMeanings { get { return Fields.Where(x => (bool) x.GetValue(this)).Select(x => x.Name).ToArray(); } }
        
        public Action<A>[] ActionDelegates() {
            // use inspection? 
            FieldInfo[] fields = Fields;
            MethodInfo[] methods = Methods;
            // methods = fields.Select(x => methods.Where(n => x.Name.ToLower() == n.Name).First()).ToArray(); // order by field
            var joined = fields.Join(methods, x => x.Name.ToLower(), y => y.Name, (x,y) => new { field=x, method=y });
            Action<A>[] result = joined.Where(x => (bool) x.field.GetValue(this)).Select(
                x => (Action<A>)Delegate.CreateDelegate(typeof(Action<A>), null, x.method)
                ).ToArray();
            return result;
        }
    }
}