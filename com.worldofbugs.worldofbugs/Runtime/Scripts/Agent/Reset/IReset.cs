using System;
using UnityEngine;

namespace WorldOfBugs {
    public interface IReset {
        public bool ShouldReset(GameObject agent);
        public void Reset();
    }
}
