using System;
using System.IO;
using System.Text;
using Model;


namespace Trees.AVL {

   public class AVLTree
    {

        private AVLNode raiz;



        // Obtener height de un nodo
        private int Obtenerheight(AVLNode nodo)
        {
            // return nodo == null ? 0 : nodo.height;
            return nodo == null ? 0 : nodo.height;
        }



        // Obtener factor de balanceo
        private int ObtenerBalance(AVLNode nodo)
        {
            return nodo == null ? 0 : Obtenerheight(nodo.left) - Obtenerheight(nodo.right);
        }



        // Rotación right
        private AVLNode Rotacionright(AVLNode y)
        {
            // Paso 1: Guardamos el subárbol izquierdo de 'y' en 'x'
            AVLNode x = y.left;

            // Paso 2: Guardamos el subárbol derecho de 'x' en 'T2', ya que lo vamos a reubicar
            AVLNode T2 = x.right;

            // Paso 3: Realizamos la rotación. 'x' se convierte en el nuevo nodo raíz del subárbol
            // 'y' pasa a ser el hijo derecho de 'x', y 'T2' pasa a ser el hijo izquierdo de 'y'
            x.right = y;
            y.left = T2;

            // Paso 4: Actualizamos las heights de los nodos después de la rotación
            // La height de 'y' es 1 más el valor máximo entre las heights de sus nuevos hijos
            y.height = Math.Max(Obtenerheight(y.left), Obtenerheight(y.right)) + 1;
            
            // La height de 'x' es 1 más el valor máximo entre las heights de sus nuevos hijos
            x.height = Math.Max(Obtenerheight(x.left), Obtenerheight(x.right)) + 1;

            // Paso 5: Retornamos 'x', que ahora es la nueva raíz del subárbol balanceado
            return x;
        }




        // Rotación left
        private AVLNode Rotacionleft(AVLNode x)
        {
            // Paso 1: Guardamos el subárbol derecho de 'x' en 'y'
            AVLNode y = x.right;

            // Paso 2: Guardamos el subárbol izquierdo de 'y' en 'T2', ya que lo vamos a reubicar
            AVLNode T2 = y.left;

            // Paso 3: Realizamos la rotación. 'y' se convierte en el nuevo nodo raíz del subárbol
            // 'x' pasa a ser el hijo izquierdo de 'y', y 'T2' pasa a ser el hijo derecho de 'x'
            y.left = x;
            x.right = T2;

            // Paso 4: Actualizamos las heights de los nodos después de la rotación
            // La height de 'x' es 1 más el valor máximo entre las heights de sus nuevos hijos
            x.height = Math.Max(Obtenerheight(x.left), Obtenerheight(x.right)) + 1;

            // La height de 'y' es 1 más el valor máximo entre las heights de sus nuevos hijos
            y.height = Math.Max(Obtenerheight(y.left), Obtenerheight(y.right)) + 1;

            // Paso 5: Retornamos 'y', que ahora es la nueva raíz del subárbol balanceado
            return y;
        }




        // Insertar 
        public void Insertar(int id, string repuesto, string detalles, double costo)
        {
            // Llamamos a la función recursiva para insertar el nodo y mantener el balance del árbol
            raiz = InsertarRecursivo(raiz, id, repuesto, detalles, costo);
        }

        private AVLNode InsertarRecursivo(AVLNode nodo, int id, string repuesto, string detalles, double costo)
        {
            // Si el nodo actual es nulo
            if (nodo == null)
                return new AVLNode(new SparePartModel(id, repuesto, detalles, costo));
                // return new AVLNode(id, repuesto, detalles, costo); // Creamos un nuevo nodo

            // Si el id es menor que el del nodo actual, insertamos en el subárbol izquierdo
            if (id < nodo.value.Id)
                nodo.left = InsertarRecursivo(nodo.left, id, repuesto, detalles, costo);
            // Si el id es mayor que el del nodo actual, insertamos en el subárbol derecho
            else if (id > nodo.value.Id)
                nodo.right = InsertarRecursivo(nodo.right, id, repuesto, detalles, costo);
            else
                return nodo; // Si el id ya existe en el árbol, no permitimos duplicados

            // Actualizamos la height del nodo actual
            nodo.height = 1 + Math.Max(Obtenerheight(nodo.left), Obtenerheight(nodo.right));

            // Obtenemos el factor de balanceo del nodo actual
            int balance = ObtenerBalance(nodo);

            // Casos de balanceo para mantener el árbol AVL balanceado

            // Caso 1: Rotación left-left (LL)
            // Si el factor de balanceo es mayor que 1 (indicando que el subárbol izquierdo está desequilibrado)
            // y el id a insertar es menor que el id del hijo izquierdo, hacemos una rotación right
            if (balance > 1 && id < nodo.left.value.Id)
                return Rotacionright(nodo);

            // Caso 2: Rotación right-right (RR)
            // Si el factor de balanceo es menor que -1 (indicando que el subárbol derecho está desequilibrado)
            // y el id a insertar es mayor que el id del hijo derecho, hacemos una rotación left
            if (balance < -1 && id > nodo.right.value.Id)
                return Rotacionleft(nodo);

            // Caso 3: Rotación left-right (LR)
            // Si el factor de balanceo es mayor que 1 (subárbol izquierdo desequilibrado)
            // y el id a insertar es mayor que el id del hijo izquierdo, primero hacemos una rotación left en el hijo izquierdo
            // y luego una rotación right en el nodo actual
            if (balance > 1 && id > nodo.left.value.Id)
            {
                nodo.left = Rotacionleft(nodo.left); // Rotación left en el subárbol izquierdo
                return Rotacionright(nodo); // Rotación right en el nodo actual
            }

            // Caso 4: Rotación right-left (RL)
            // Si el factor de balanceo es menor que -1 (subárbol derecho desequilibrado)
            // y el id a insertar es menor que el id del hijo derecho, primero hacemos una rotación right en el hijo derecho
            // y luego una rotación left en el nodo actual
            if (balance < -1 && id < nodo.right.value.Id)
            {
                nodo.right = Rotacionright(nodo.right); // Rotación right en el subárbol derecho
                return Rotacionleft(nodo); // Rotación left en el nodo actual
            }

            // Si no se aplica ninguna de las rotaciones anteriores, retornamos el nodo sin cambios
            return nodo;
        }

        // Método público para buscar por ID
        public SparePartModel BuscarPorId(int id)
        {
            AVLNode nodoEncontrado = BuscarPorIdRecursivo(raiz, id);
            return nodoEncontrado?.value;
        }

        // Método privado recursivo para buscar por ID
        private AVLNode BuscarPorIdRecursivo(AVLNode nodo, int id)
        {
            // Caso base: nodo es null o encontramos el ID
            if (nodo == null || nodo.value.Id == id)
            {
                return nodo;
            }

            // Si el ID buscado es menor que el ID del nodo actual, buscamos en el subárbol izquierdo
            if (id < nodo.value.Id)
            {
                return BuscarPorIdRecursivo(nodo.left, id);
            }
            // Si el ID buscado es mayor, buscamos en el subárbol derecho
            else
            {
                return BuscarPorIdRecursivo(nodo.right, id);
            }
        }




        // Generar Graphviz
        public string  GraficarGraphviz()
        {
            StringBuilder dot = new StringBuilder();
            dot.AppendLine("digraph ArbolBinario {");
            dot.AppendLine("    node [shape=rectangle];");
            GenerarNodo_RepuestosGraphviz(raiz, dot);
            GenerarConexionesGraphviz(raiz, dot);
            dot.AppendLine("}");
            return dot.ToString();
        }

        private void GenerarNodo_RepuestosGraphviz(AVLNode nodo, StringBuilder dot)
        {
            if (nodo == null) return;

            dot.AppendLine($"    \"{nodo.value.Id}\" [label=\"ID: {nodo.value.Id}\\nRepuesto: {nodo.value.Name}\\nDetalles: {nodo.value.Details}\\nCosto: {nodo.value.Cost}\"];");
            GenerarNodo_RepuestosGraphviz(nodo.left, dot);
            GenerarNodo_RepuestosGraphviz(nodo.right, dot);
        }

        private void GenerarConexionesGraphviz(AVLNode nodo, StringBuilder dot)
        {
            if (nodo == null) return;

            if (nodo.left != null)
                dot.AppendLine($"    \"{nodo.value.Id}\" -> \"{nodo.left.value.Id}\";");
            if (nodo.right != null)
                dot.AppendLine($"    \"{nodo.value.Id}\" -> \"{nodo.right.value.Id}\";");

            GenerarConexionesGraphviz(nodo.left, dot);
            GenerarConexionesGraphviz(nodo.right, dot);
        }


        


        // Recorridos

        // Recorrido InOrden (left-Raíz-right)
        public AVLNode[] TablaInOrden()
        {
            List<AVLNode> resultado = new List<AVLNode>();
            InOrdenRecursivo(raiz, resultado);
            return resultado.ToArray();
        }

        private void InOrdenRecursivo(AVLNode nodo, List<AVLNode> resultado)
        {
            if (nodo == null) return;
            
            InOrdenRecursivo(nodo.left, resultado);
            resultado.Add(nodo);
            InOrdenRecursivo(nodo.right, resultado);
        }

        // Recorrido PreOrden (Raíz-left-right)
        public AVLNode[] TablaPreOrden()
        {
            List<AVLNode> resultado = new List<AVLNode>();
            PreOrdenRecursivo(raiz, resultado);
            return resultado.ToArray();
        }

        private void PreOrdenRecursivo(AVLNode nodo, List<AVLNode> resultado)
        {
            if (nodo == null) return;
            
            resultado.Add(nodo);
            PreOrdenRecursivo(nodo.left, resultado);
            PreOrdenRecursivo(nodo.right, resultado);
        }

        // Recorrido PostOrden (left-right-Raíz)
        public AVLNode[] TablaPostOrden()
        {
            List<AVLNode> resultado = new List<AVLNode>();
            PostOrdenRecursivo(raiz, resultado);
            return resultado.ToArray();
        }

        private void PostOrdenRecursivo(AVLNode nodo, List<AVLNode> resultado)
        {
            if (nodo == null) return;
            
            PostOrdenRecursivo(nodo.left, resultado);
            PostOrdenRecursivo(nodo.right, resultado);
            resultado.Add(nodo);
        }



    }

    
}