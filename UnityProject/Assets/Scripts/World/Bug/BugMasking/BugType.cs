using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BugType {

     public enum Group { 
        Null,
        Visual,
        Logical,
        Progression,
    }

    public static HashSet<BugType> BugTypes = new HashSet<BugType>();

    [SerializeField]
    public Color32 type;
    [SerializeField]
    public Group group;

    public BugType(Color32 _type, Group _group) { 
        this.type = _type; 
        this.group = _group;
        BugType.BugTypes.Add(this); }
    
    public override int GetHashCode() {
        return type.GetHashCode();
    }

    public static implicit operator int(BugType b) => 0xFFFF * b.type.r + 0xFF * b.type.g + b.type.b;
    public static implicit operator Color32(BugType b) => b.type;
    public static implicit operator Color(BugType b) => (Color)b.type;
    public static explicit operator BugType(Color32 c) => new BugType(c, Group.Null);


   
}