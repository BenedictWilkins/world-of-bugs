using System;
using UnityEngine;

namespace WorldOfBugs {

    /// <summary>
    /// Check that an field does not contain null.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class NotNull : ValidationAttribute {

        public override string ErrorMessage { get { return error; } }
        private string error = string.Empty;
    
        public override bool Validate(System.Reflection.FieldInfo field,  UnityEngine.Object instance) {
            bool isValid;
            MonoBehaviour mb = instance as MonoBehaviour;
            error = $"Property '{field.Name}' on GameObject '{mb.name}' cannot be NULL";
            try {
                var value = field.GetValue(instance);
                isValid = !(value.Equals(null));
            } catch (Exception) {
                isValid = false;
            }
            return isValid;
        }
    }
}