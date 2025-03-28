using System;

namespace Model {

    public interface LogInterface {

    }

    public unsafe struct Log {
        
    }

    public class LogModel {
        public string user { get; set; }
        public string loggedInAt { get; set; }
        public string loggedOutAt { get; set; }

        public LogModel(string user, string loggedInAt, string loggedOutAt) {
            this.user = user;
            this.loggedInAt = loggedInAt;
            this.loggedOutAt = loggedOutAt;
        }
    }

}