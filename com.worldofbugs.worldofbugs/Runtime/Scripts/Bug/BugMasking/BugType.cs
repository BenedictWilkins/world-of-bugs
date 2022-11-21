using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldOfBugs {

    [Serializable]
    public class BugType {

        public static HashSet<BugType> BugTypes = new HashSet<BugType>();

        [SerializeField]
        public Color32 type;

        public BugType(Color32 _type) {
            this.type = _type;
            this.type.a = 255;
            BugType.BugTypes.Add(this);
        }

        public override int GetHashCode() {
            return type.GetHashCode();
        }

        public static implicit operator int(BugType b) => 0xFFFF * b.type.r + 0xFF * b.type.g +
        b.type.b;
        public static implicit operator Color32(BugType b) => b.type;
        public static implicit operator Color(BugType b) => (Color)b.type;
        public static explicit operator BugType(Color32 c) => new BugType(c);
        public static explicit operator BugType(Color c) => new BugType(c);


    }

}
