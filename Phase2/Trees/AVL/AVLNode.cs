using System;
using Model;

namespace Trees.AVL {
    public class AVLNode {
        public SparePartModel value;
        public AVLNode left;
        public AVLNode right;
        public int height;

        public AVLNode(SparePartModel value) {
            this.value = value;
            left = null;
            right = null;
            height = 1;
        }
    }

    
}