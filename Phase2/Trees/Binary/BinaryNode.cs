using System;
using Model;

namespace Trees.Binary {
    public class BinaryNode {
        public ServiceModel Value { get; set; }
        public BinaryNode Left { get; set; }
        public BinaryNode Right { get; set; }

        public BinaryNode(ServiceModel value) {
            Value = value;
            Left = null;
            Right = null;
        }

        // public BinaryNode(ServiceModel value, BinaryNode left, BinaryNode right) {
        //     Value = value;
        //     Left = left;
        //     Right = right;
        // }
    }
}