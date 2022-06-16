using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using _Debug = UnityEngine.Debug;

namespace WorldOfBugs {

    class DEPLogger {

        public void Log(string msg) {
            _Debug.Log(msg);
        }

        public void Log<T>(T[] array) {
            string msg = string.Join(",", array.Select(x => x.ToString()));
            _Debug.Log($"[{msg}]");
        }
    }
}
