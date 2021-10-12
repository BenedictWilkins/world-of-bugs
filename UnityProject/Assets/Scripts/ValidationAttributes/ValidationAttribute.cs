using System;
 
public abstract class ValidationAttribute : Attribute {
    public abstract bool Validate(System.Reflection.FieldInfo field, UnityEngine.Object instance);
    public abstract string ErrorMessage { get; }
}