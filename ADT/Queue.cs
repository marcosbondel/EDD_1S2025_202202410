using System;
using System.Runtime.InteropServices;
using Model;

namespace ADT {

    public unsafe class Queue<T> where T : unmanaged, ServiceInterface {

        private SimpleNode<T>* head;
        private SimpleNode<T>* tail;
        private int size;

        public Queue(){
            head = null;
            tail = null;
            size = 0;
        }

        public int GetSize(){
            return size;
        }

        public void enqueu(T data)
        {
            SimpleNode<T>* newSimpleNode = (SimpleNode<T>*)Marshal.AllocHGlobal(sizeof(SimpleNode<T>));
            *newSimpleNode = new SimpleNode<T> { value = data, next = null };

            if (tail == null) // Si la cola está vacía, el nuevo nodo es tanto el frente como el tail.
            {
                head = newSimpleNode;
                tail = newSimpleNode;
            }
            else // Si la cola no está vacía, agrega el nodo al tail y actualiza el tail.
            {
                tail->next = newSimpleNode;
                tail = newSimpleNode;
            }

            size++;
        }

        // Método desencolar: elimina y devuelve el valor del nodo al frente de la cola.
        // Si la cola está vacía, retorna -999.
        public SimpleNode<T>* dequeu()
        {
            if (head == null) return null; // Si la cola está vacía, retorna un valor especial.
            
            SimpleNode<T>* ret = head; // Guarda el nodo head.
            // int ret = head.Data; // Guarda el valor del nodo head.
            
            head = head->next; 
            // head = head.Sig; // Mueve el head al siguiente nodo.
            
            if (head == null) tail = null; // Si la cola queda vacía, el final también se establece como null.
            
            size--; // Disminuye el tamaño de la cola.
            
            return ret; // Retorna el valor eliminado.
        }

        public void list()
        {
            SimpleNode<T>* temp = head; // Comienza desde el frente de la cola.
            while (temp != null) // Mientras haya nodos en la cola.
            {
                Console.Write(temp->value.ToString() + " <- "); // Imprime el valor del nodo.
                temp = temp->next; // Se mueve al siguiente nodo.
            }
            Console.WriteLine("NULL"); // Indica el final de la cola.
        }

        public SimpleNode<T>* GetById(int id){
            if (head == null) return null;

            SimpleNode<T>* current = head;
            while (current!= null) {
                if (current->value.GetId() == id) {
                    return current;
                }
                current = current->next;
            }
            return null;
        }

       public unsafe string GenerateDotCode()
        {
            // Si la cola está vacía, generamos un solo nodo con "NULL"
            if (head == null)
            {
                return "digraph G {\n    node [shape=record];\n    NULL [label = \"{NULL}\"];\n}\n";
            }

            // Iniciamos el código Graphviz
            var graphviz = "digraph G {\n";
            graphviz += "    node [shape=record];\n";
            graphviz += "    rankdir=LR;\n"; // De izquierda a derecha (horizontal)
            graphviz += "    subgraph cluster_0 {\n";
            graphviz += "        label = \"Cola\";\n";

            // Iterar sobre los nodos de la cola y construir la representación Graphviz
            SimpleNode<T>* current = head;
            int index = 0;

            while (current != null)
            {
                // graphviz += $"        n{index} [label = \"{{<data> ID: {current->value.GetId()} \\n Name: {current->value.GetFullname()} \\n Email: {current->value.GetEmail()} | <next> Siguiente }}\"];\n";
                current = current->next;
                index++;
            }

            // Conectar los nodos de la cola (de izquierda a derecha)
            current = head;
            for (int i = 0; current != null && current->next != null; i++)
            {
                graphviz += $"        n{i}:next -> n{i + 1}:data;\n"; // Conexión hacia la derecha
                current = current->next;
            }

            // Indicar la posición del frente y final de la cola
            graphviz += $"        n0 [style=filled, fillcolor=lightblue]; // Front of Queue\n";
            graphviz += $"        n{index - 1} [style=filled, fillcolor=lightgreen]; // Rear of Queue\n";

            graphviz += "    }\n";
            graphviz += "}\n";
            return graphviz;
        }
 

    }

}