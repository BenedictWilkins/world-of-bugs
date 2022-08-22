using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace WorldOfBugs {

    /// <summary>
    /// Check that an field does not contain null.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class NotEmpty : ValidationAttribute {

        public override string ErrorMessage {
            get {
                return error;
            }
        }
        private string error = string.Empty;

        public override bool Validate(System.Reflection.FieldInfo field,
                                      UnityEngine.Object instance) {
            MonoBehaviour mb = instance as MonoBehaviour;
            error = $"Property '{field.Name}' on GameObject '{mb.name}' cannot be empty";
            object obj = field.GetValue(instance);

            if(obj is string) {
                return ((string)obj).Length > 0;
            }

            try {
                ICollection value = (ICollection)obj;
                return value.Count > 0;
            } catch(Exception) {
                error = $"Property '{field.Name}' on GameObject '{mb.name}' is not enumerable";
                return false;
            }
        }
    }
}
