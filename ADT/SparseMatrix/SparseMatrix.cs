using System;
using System.Runtime.InteropServices;

namespace ADT {
    namespace Matrix
    {
        public unsafe class SparseMatrix<T> where T : unmanaged
        {
            public int layer; // Represents an additional layer that can be used in matrix visualization.
            public HeaderList<int> rows; // Header list for rows
            public HeaderList<int> columns; // Header list for columns

            // Constructor for the SparseMatrix class
            public SparseMatrix(int layer)
            {
                this.layer = layer; // Initialize the layer
                rows = new HeaderList<int>("Row"); // Initialize the header list for rows
                columns = new HeaderList<int>("Column"); // Initialize the header list for columns
            }

            // Method to insert a new node into the sparse matrix
            public void Insert(int posX, int posY, string name)
            {
                // Create a new internal node to be inserted into the matrix
                InternalNode<int>* newNode = (InternalNode<int>*)Marshal.AllocHGlobal(sizeof(InternalNode<int>));
                newNode->id = 1; // Assign an ID to the node
                newNode->name = name; // Assign the provided name to the node
                newNode->coordinateX = posX; // Assign the X coordinate (row)
                newNode->coordinateY = posY; // Assign the Y coordinate (column)
                newNode->up = null;
                newNode->down = null;
                newNode->right = null;
                newNode->left = null;

                // Check if the row and column headers already exist in the matrix
                HeaderNode<int>* nodeX = rows.GetHeader(posX); // Get the row header
                HeaderNode<int>* nodeY = columns.GetHeader(posY); // Get the column header

                // If the row header does not exist, create it
                if (nodeX == null)
                {
                    rows.InsertHeaderNode(posX); // Create a header for the row
                    nodeX = rows.GetHeader(posX); // Retrieve the newly created row header
                }

                // If the column header does not exist, create it
                if (nodeY == null)
                {
                    columns.InsertHeaderNode(posY); // Create a header for the column
                    nodeY = columns.GetHeader(posY); // Retrieve the newly created column header
                }

                // Ensure both headers were created successfully
                if (nodeX == null || nodeY == null)
                {
                    throw new InvalidOperationException("Error creating headers.");
                }

                // Insert the new node in the corresponding row
                if (nodeX->access == null)
                {
                    nodeX->access = newNode; // If the row is empty, assign the new node as the first access
                }
                else
                {
                    // If nodes already exist in the row, find the appropriate position to insert the new node
                    InternalNode<int>* tmp = nodeX->access;
                    while (tmp != null)
                    {
                        if (newNode->coordinateY < tmp->coordinateY)
                        {
                            newNode->right = tmp;
                            newNode->left = tmp->left;
                            tmp->left->right = newNode;
                            tmp->left = newNode;
                            break;
                        }
                        else if (newNode->coordinateX == tmp->coordinateX && newNode->coordinateY == tmp->coordinateY)
                        {
                            break; // Avoid duplicate nodes with the same coordinates
                        }
                        else
                        {
                            if (tmp->right == null)
                            {
                                tmp->right = newNode;
                                newNode->left = tmp;
                                break;
                            }
                            else
                            {
                                tmp = tmp->right;
                            }
                        }
                    }
                }

                // Insert the new node in the corresponding column
                if (nodeY->access == null)
                {
                    nodeY->access = newNode;
                }
                else
                {
                    InternalNode<int>* tmp2 = nodeY->access;
                    while (tmp2 != null)
                    {
                        if (newNode->coordinateX < tmp2->coordinateX)
                        {
                            newNode->down = tmp2;
                            newNode->up = tmp2->up;
                            tmp2->up->down = newNode;
                            tmp2->up = newNode;
                            break;
                        }
                        else if (newNode->coordinateX == tmp2->coordinateX && newNode->coordinateY == tmp2->coordinateY)
                        {
                            break;
                        }
                        else
                        {
                            if (tmp2->down == null)
                            {
                                tmp2->down = newNode;
                                newNode->up = tmp2;
                                break;
                            }
                            else
                            {
                                tmp2 = tmp2->down;
                            }
                        }
                    }
                }
            }

            // Method to display the sparse matrix
            public void Display()
            {
                HeaderNode<int>* columnY = columns.first;
                Console.Write("\t");
                while (columnY != null)
                {
                    Console.Write(columnY->id + "\t");
                    columnY = columnY->next;
                }
                Console.WriteLine();

                HeaderNode<int>* rowX = rows.first;
                while (rowX != null)
                {
                    Console.Write(rowX->id + "\t");
                    InternalNode<int>* internalNode = rowX->access;
                    HeaderNode<int>* columnIterator = columns.first;
                    while (columnIterator != null)
                    {
                        if (internalNode != null && internalNode->coordinateY == columnIterator->id)
                        {
                            Console.Write(internalNode->name + "\t");
                            internalNode = internalNode->right;
                        }
                        else
                        {
                            Console.Write("0\t");
                        }
                        columnIterator = columnIterator->next;
                    }
                    Console.WriteLine();
                    rowX = rowX->next;
                }
            }
        }
    }
}
