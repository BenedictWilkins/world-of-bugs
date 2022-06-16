using System;


namespace WorldOfBugs {

    /// <summary>
    /// Allows validation of class fields when in edit mode, shows error in unity inspector.
    /// <see cref="AttributeValidatorEditor">
    /// </summary>
    public abstract class ValidationAttribute : Attribute {
        public abstract bool Validate(System.Reflection.FieldInfo field, UnityEngine.Object instance);
        public abstract string ErrorMessage { get; }
    }

}
