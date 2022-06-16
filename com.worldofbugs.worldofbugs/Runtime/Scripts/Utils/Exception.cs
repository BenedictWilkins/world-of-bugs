using System;

namespace WorldOfBugs {
    public class WorldOfBugsException : Exception {
        public WorldOfBugsException(string message) : base(message) {}
        public WorldOfBugsException(string message, Exception inner): base(message, inner) {}
    }
}
