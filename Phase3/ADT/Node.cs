using System;
using Model;


namespace ADT {

    public class SimpleNode {
        public User value;
        public SimpleNode next;

        public SimpleNode(User value) {
            this.value = value;
            next = null;
        }
    }
    
    public class SimpleNodeService {
        public Service value;
        public SimpleNodeService next;

        public SimpleNodeService(Service value) {
            this.value = value;
            next = null;
        }
    }
    
    public class SimpleNodeBill {
        public Bill value;
        public SimpleNodeBill next;

        public SimpleNodeBill(Bill value) {
            this.value = value;
            next = null;
        }
    }

    public class DoublePointerNode {
        public Automobile value;
        public DoublePointerNode next;
        public DoublePointerNode previous;

        public DoublePointerNode(Automobile value) {
            this.value = value;
            this.next = null;
            this.previous = null;
        }
    }
}