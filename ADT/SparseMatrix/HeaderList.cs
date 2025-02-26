using System;
using System.Runtime.InteropServices;

namespace ADT
{
    namespace Matrix
    {
        public unsafe class HeaderList<T> where T : unmanaged
        {
            public HeaderNode<int>* first; // Points to the first node in the list
            public HeaderNode<int>* last;  // Points to the last node in the list
            public string type;            // Type of the list (e.g., "Integer List")
            public int size;               // Current size of the list (number of nodes)

            // Constructor that initializes an empty list and assigns the type
            public HeaderList(string type)
            {
                first = null; // Empty list at the beginning
                last = null;  // Empty list at the beginning
                this.type = type; // Assign the list type
                size = 0; // The list starts with size 0
            }

            // Method to insert a new node with an ID into the list
            public void InsertHeaderNode(int id)
            {
                // Allocate memory for a new HeaderNode
                HeaderNode<int>* newNode = (HeaderNode<int>*)Marshal.AllocHGlobal(sizeof(HeaderNode<int>));
                if (newNode == null) throw new InvalidOperationException("Failed to allocate memory for the new node.");

                // Initialize the new node's values
                newNode->id = id;         // Assign the ID to the node
                newNode->next = null;      // Initialize the next pointer to null
                newNode->previous = null;  // Initialize the previous pointer to null
                newNode->access = null;    // Initialize the access pointer to null

                size++; // Increase the size of the list

                // If the list is empty, the new node is both the first and last node
                if (first == null)
                {
                    first = newNode; // The first node is the new node
                    last = newNode;  // The last node is also the new node
                }
                else
                {
                    // Insert the node in a sorted manner
                    if (newNode->id < first->id) // If the ID is smaller than the first node's ID
                    {
                        newNode->next = first; // The new node's next points to the first node
                        first->previous = newNode; // The first node's previous points to the new node
                        first = newNode; // The new node becomes the first node
                    }
                    else if (newNode->id > last->id) // If the ID is larger than the last node's ID
                    {
                        last->next = newNode; // The last node's next points to the new node
                        newNode->previous = last; // The new node's previous points to the last node
                        last = newNode; // The new node becomes the last node
                    }
                    else
                    {
                        // If the ID is in the middle, find the correct position
                        HeaderNode<int>* temp = first;
                        while (temp != null)
                        {
                            // Insert between nodes if the ID is smaller than the current node
                            if (newNode->id < temp->id)
                            {
                                newNode->next = temp;         // The new node's next points to the current node
                                newNode->previous = temp->previous; // The new node's previous points to the current node's previous
                                temp->previous->next = newNode; // The previous node's next points to the new node
                                temp->previous = newNode; // The current node's previous points to the new node
                                break; // Insertion complete
                            }
                            else
                            {
                                temp = temp->next; // Continue searching
                            }
                        }
                    }
                }
            }

            // Method to display all nodes in the list
            public void Show()
            {
                if (first == null)
                {
                    Console.WriteLine("Empty list."); // If the list is empty, display a message
                    return;
                }

                // Traverse the list and display the node IDs
                HeaderNode<int>* temp = first;
                while (temp != null)
                {
                    Console.WriteLine("Header " + type + " " + Convert.ToString(temp->id)); // Display ID
                    temp = temp->next; // Move to the next node
                }
            }

            // Method to get a node by its ID
            public HeaderNode<int>* GetHeader(int id)
            {
                HeaderNode<int>* temp = first;
                while (temp != null)
                {
                    if (id == temp->id) return temp; // If the node is found, return it
                    temp = temp->next; // Otherwise, continue searching
                }
                return null; // If not found, return null
            }

            // Destructor to free the memory of all nodes when the list is destroyed
            ~HeaderList()
            {
                if (first == null) return; // If the list is already empty, do nothing

                // Free each node in the list
                while (first != null)
                {
                    HeaderNode<int>* temp = first;  // Point to the first node
                    first = first->next;  // Move to the next node
                    Marshal.FreeHGlobal((IntPtr)temp); // Free memory of the current node
                }
            }
        }
    }
}
