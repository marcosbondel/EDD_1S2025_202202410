using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Model; // Asegúrate de que la ruta sea correcta para tu proyecto

namespace Merkle {
    public class MerkleNode
    {
        public string Hash { get; set; }         // Hash del nodo 
        public MerkleNode Left { get; set; }     // Hijo izquierdo
        public MerkleNode Right { get; set; }    // Hijo derecho
        public Bill Factura { get; set; }     // Factura asociada 

        // Constructor para nodos hoja
        public MerkleNode(Bill factura)
        {
            Factura = factura;
            Hash = factura.GetHash();
            Left = null;
            Right = null;
        }

        // Constructor para nodos internos (combinación de hijos)
        public MerkleNode(MerkleNode left, MerkleNode right)
        {
            Factura = null;
            Left = left;
            Right = right;
            Hash = CalculateHash(left.Hash, right?.Hash); // Si right es null, usa solo left
        }

        // Método para calcular el hash combinado de dos nodos
        private string CalculateHash(string leftHash, string rightHash)
        {
            string combined = leftHash + (rightHash ?? leftHash); // Si no hay right, duplicar left
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }

}