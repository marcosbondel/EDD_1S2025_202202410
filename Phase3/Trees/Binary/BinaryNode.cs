using System;
using Model;

namespace Trees.Binary {
    public class BinaryNode {
        public Service Value { get; set; }
        public BinaryNode Left { get; set; }
        public BinaryNode Right { get; set; }

        public BinaryNode(Service value) {
            Value = value;
            Left = null;
            Right = null;
        }

        // public BinaryNode(Service value, BinaryNode left, BinaryNode right) {
        //     Value = value;
        //     Left = left;
        //     Right = right;
        // }
    }
}