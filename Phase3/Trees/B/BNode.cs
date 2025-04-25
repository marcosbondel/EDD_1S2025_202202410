using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Model;

namespace Trees.B
{
    public class BNode {
        private const int ORDEN = 5;
        private const int MAX_CLAVES = ORDEN - 1;
        private const int MIN_CLAVES = (ORDEN / 2) - 1;
        
        public List<BillModel> Claves { get; set; }
        public List<BNode> Hijos { get; set; }
        public bool EsHoja { get; set; }

        public BNode()
        {
            Claves = new List<BillModel>(MAX_CLAVES);
            Hijos = new List<BNode>(ORDEN);
            EsHoja = true;
        }

        // Verifica si el nodo está lleno
        public bool EstaLleno()
        {
            return Claves.Count >= MAX_CLAVES;
        }

        // Verifica si el nodo tiene el mínimo de claves requerido
        public bool TieneMinimoClaves()
        {
            return Claves.Count >= MIN_CLAVES;
        }
    }
}