using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Model;

namespace Merkle
{

    public class MerkleTree
    {
        private List<MerkleNode> Leaves; // Lista de nodos hoja 
        private MerkleNode Root;         // Raíz del árbol

        // Constructor del árbol
        public MerkleTree()
        {
            Leaves = new List<MerkleNode>();
            Root = null;
        }


        // Método para insertar una nueva factura
        public void Insert(Bill factura)
        {
            // Verificar unicidad del Id
            foreach (var leaf in Leaves)
            {
                if (leaf.Factura.Id == factura.Id)
                {
                    Console.WriteLine($"Error: Ya existe una factura con Id {factura.Id}.");
                    return;
                }
            }

            // Crear la factura y el nodo hoja
            // Bill factura = new Bill(id, idServicio, total, fecha, metodoPago);
            MerkleNode newLeaf = new MerkleNode(factura);
            Leaves.Add(newLeaf);

            // Reconstruir el árbol con las hojas actuales
            BuildTree();
        }




        // Método privado para construir el árbol a partir de las hojas
        private void BuildTree()
        {
            if (Leaves.Count == 0)
            {
                Root = null;
                return;
            }

            List<MerkleNode> currentLevel = new List<MerkleNode>(Leaves);

            while (currentLevel.Count > 1)
            {
                List<MerkleNode> nextLevel = new List<MerkleNode>();

                for (int i = 0; i < currentLevel.Count; i += 2)
                {
                    MerkleNode left = currentLevel[i];
                    MerkleNode right = (i + 1 < currentLevel.Count) ? currentLevel[i + 1] : null;
                    MerkleNode parent = new MerkleNode(left, right);
                    nextLevel.Add(parent);
                }

                currentLevel = nextLevel;
            }

            Root = currentLevel[0]; // La raíz es el único nodo que queda
        }




        public string GenerateDot()
        {
            StringBuilder dot = new StringBuilder();
            dot.AppendLine("digraph MerkleTree {"); 
            dot.AppendLine("  node [shape=record];"); 
            dot.AppendLine("  graph [rankdir=TB];"); 
            dot.AppendLine("  subgraph cluster_0 {"); 
            dot.AppendLine("    label=\"Facturas\";");

            if (Root == null)
            {
                dot.AppendLine("    empty [label=\"Árbol vacío\"];");
            }
            else
            {
                Dictionary<string, int> nodeIds = new Dictionary<string, int>(); 
                int idCounter = 0;
                GenerateDotRecursive(Root, dot, nodeIds, ref idCounter);
            }

            dot.AppendLine("  }");
            dot.AppendLine("}"); 
            return dot.ToString();
        }

        private void GenerateDotRecursive(MerkleNode node, StringBuilder dot, Dictionary<string, int> nodeIds, ref int idCounter)
        {
            if (node == null) return;

            if (!nodeIds.ContainsKey(node.Hash))
            {
                nodeIds[node.Hash] = idCounter++;
            }
            int nodeId = nodeIds[node.Hash];

            string label;
            if (node.Factura != null) 
            {
                label = $"\"Factura {node.Factura.Id}\\nTotal: {node.Factura.TotalCost}\\nHash: {node.Hash.Substring(0, 8)}...\"";
            }
            else 
            {
                label = $"\"Hash: {node.Hash.Substring(0, 8)}...\"";
            }
            dot.AppendLine($"  node{nodeId} [label={label}];");

            if (node.Left != null)
            {
                if (!nodeIds.ContainsKey(node.Left.Hash))
                {
                    nodeIds[node.Left.Hash] = idCounter++;
                }
                int leftId = nodeIds[node.Left.Hash];
                dot.AppendLine($"  node{nodeId} -> node{leftId};");
                GenerateDotRecursive(node.Left, dot, nodeIds, ref idCounter);
            }

            if (node.Right != null)
            {
                if (!nodeIds.ContainsKey(node.Right.Hash))
                {
                    nodeIds[node.Right.Hash] = idCounter++;
                }
                int rightId = nodeIds[node.Right.Hash];
                dot.AppendLine($"  node{nodeId} -> node{rightId};");
                GenerateDotRecursive(node.Right, dot, nodeIds, ref idCounter);
            }
        }
    }
}