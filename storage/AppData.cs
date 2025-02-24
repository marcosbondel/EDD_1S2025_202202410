using System;
using ADT;
using Model;

namespace Storage {
    public static class AppData {
        public static SimplyLinkedList<User> users_data = new SimplyLinkedList<User>();
        public static CircularLinkedList<SparePart> spare_parts_data = new CircularLinkedList<SparePart>();
    }
}