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
        public ServiceModel value;
        public SimpleNodeService next;

        public SimpleNodeService(ServiceModel value) {
            this.value = value;
            next = null;
        }
    }
    
    public class SimpleNodeBill {
        public BillModel value;
        public SimpleNodeBill next;

        public SimpleNodeBill(BillModel value) {
            this.value = value;
            next = null;
        }
    }

    public class DoublePointerNode {
        public AutomobileModel value;
        public DoublePointerNode next;
        public DoublePointerNode previous;

        public DoublePointerNode(AutomobileModel value) {
            this.value = value;
            this.next = null;
            this.previous = null;
        }
    }
}