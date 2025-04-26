using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Model;
using Utils;

namespace Blocks
{
    // Clase que representa un bloque (nodo) 
    public class Block
    {
        public int Index { get; set; }           // Número del bloque en la cadena (comienza en 0)
        public string Timestamp { get; set; }    // Fecha y hora de creación del bloque
        public string Data { get; set; }         // Información del usuario en formato JSON
        public int Nonce { get; set; }           // Número que se itera para la prueba de trabajo
        public string PreviousHash { get; set; } // Hash del bloque anterior
        public string Hash { get; set; }         // Hash único del bloque actual
        public Block Next { get; set; }          // Enlace al siguiente bloque en la lista

    
        public Block(int index, User user, string previousHash)
        {
            Index = index;
            Timestamp = DateTime.Now.ToString("dd-MM-yy::HH:mm:ss"); 
            Data = JsonConvert.SerializeObject(user);                   // Serializar el usuario a JSON
            Nonce = 0;                                                  // Inicia en 0 para la prueba de trabajo
            PreviousHash = previousHash;
            Hash = CalculateHash();                                     // Calcula el hash inicial
            Next = null;                                    
        }

        // Método para calcular el hash del bloque usando SHA-256
        public string CalculateHash()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Concatenar todos los atributos sin espacios ni saltos de línea
                string rawData = Index + Timestamp + Data + Nonce + PreviousHash;
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));   // Convertir a hexadecimal
                }
                return builder.ToString();
            }
        }

        // Método para realizar la prueba de trabajo (encontrar un hash con prefijo "0000")
        public void MineBlock()
        {
            while (!Hash.StartsWith("0000")) // Condición de la prueba de trabajo
            {
                Nonce++;                    // Incrementar el nonce
                Hash = CalculateHash();     // Recalcular el hash
            }
        }
    }



    // Clase que representa la cadena de bloques como una lista simplemente enlazada
    public class Blockchain
    {
        private Block Head; 


        public Blockchain()
        {
            Head = null;
        }


        // Método para agregar un nuevo bloque a la cadena
        public void AddBlock(User user)
        {
            // User user = new User(id, nombres, apellidos, correo, contrasenia, edad);

            if (Head == null)
            {
                Block firstBlock = new Block(0, user, "0000");  // PreviousHash inicial como "0000"
                firstBlock.MineBlock();                         // Minar el primer bloque
                Head = firstBlock;                              // Establecer como cabeza
                return;
            }


            Block current = Head;
            while (current.Next != null)
            {
                current = current.Next;
            }

            // Crear un nuevo bloque con el índice siguiente al último
            Block newBlock = new Block(current.Index + 1, user, current.Hash);
            newBlock.MineBlock();                               
            current.Next = newBlock; 
        }



        // Método para generar un string JSON con toda la cadena
        public string GenerateJson()
        {
            if (Head == null)
            {
                return "[]"; 
            }

            List<object> blocks = new List<object>(); 
            Block current = Head;

            // Recorrer la lista y agregar cada bloque
            while (current != null)
            {
                blocks.Add(new
                {
                    Index = current.Index,
                    Timestamp = current.Timestamp,
                    Data = JsonConvert.DeserializeObject(current.Data), 
                    Nonce = current.Nonce,
                    PreviousHash = current.PreviousHash,
                    Hash = current.Hash
                });
                current = current.Next;
            }

            return JsonConvert.SerializeObject(blocks, Formatting.Indented);
        }


        
        // Dentro de la clase Blockchain
        public string GenerateDot()
        {
            StringBuilder dot = new StringBuilder();
            dot.AppendLine("digraph Blockchain {"); 
            dot.AppendLine("    node [shape=record];");
            dot.AppendLine("    graph [rankdir=LR];"); 
            dot.AppendLine("    subgraph cluster_0 {"); 
            dot.AppendLine("        label=\"Usuarios\";"); 
            if (Head == null)
            {
                dot.AppendLine("  empty [label=\"Cadena vacía\"];"); 
            }
            else
            {
                Block current = Head;
                while (current != null)
                {
                    string hashDisplay = current.Hash.Length >= 8 ? current.Hash.Substring(0, 8) + "..." : current.Hash;
                    string prevHashDisplay = current.PreviousHash.Length >= 8 ? current.PreviousHash.Substring(0, 8) + "..." : current.PreviousHash;

                    string nodeLabel = $"\"Bloque {current.Index}\\nHash: {hashDisplay}\\nPrevious: {prevHashDisplay}\"";
                    dot.AppendLine($"  block{current.Index} [label={nodeLabel}];");

                    if (current.Next != null)
                    {
                        dot.AppendLine($"  block{current.Index} -> block{current.Next.Index};");
                    }
                    current = current.Next;
                }
            }

            dot.AppendLine("}}"); // Cerrar el grafo
            return dot.ToString();
        }



        // Método para analizar la integridad de la cadena
        public bool AnalyzeBlockchain()
        {
            if (Head == null)
            {
                Console.WriteLine("La cadena está vacía.");
                return true; // Una cadena vacía se considera válida
            }

            Block current = Head;
            Block previous = null;

            while (current != null)
            {
                // Verificar que el hash almacenado sea correcto
                if (current.Hash != current.CalculateHash())
                {
                    Console.WriteLine($"El hash del bloque {current.Index} es inválido.");
                    return false;
                }

                // Si no es el primer bloque, verificar el enlace con el bloque anterior
                if (previous != null && current.PreviousHash != previous.Hash)
                {
                    Console.WriteLine($"El PreviousHash del bloque {current.Index} no coincide.");
                    return false;
                }

                // Verificar que el hash cumple con la prueba de trabajo
                if (!current.Hash.StartsWith("0000"))
                {
                    Console.WriteLine($"El hash del bloque {current.Index} no cumple con la prueba de trabajo.");
                    return false;
                }

                previous = current;
                current = current.Next;
            }

            Console.WriteLine("La cadena de bloques es válida.");
            return true;
        }



        public void ViewBlock(int index)
        {
            Block current = Head;
            while (current != null)
            {
                if (current.Index == index)
                {
                    Console.WriteLine($"=====================================");
                    Console.WriteLine($"INDEX: {current.Index}");
                    Console.WriteLine($"TIMESTAMP: {current.Timestamp}");
                    Console.WriteLine($"DATA: {current.Data}");
                    Console.WriteLine($"NONCE: {current.Nonce}");
                    Console.WriteLine($"PREVIOUS HASH: {current.PreviousHash}");
                    Console.WriteLine($"HASH: {current.Hash}");
                    Console.WriteLine($"=====================================");
                    return;
                }
                current = current.Next;
            }
            Console.WriteLine("Índice de bloque inválido.");
        }

        // Method to find a user by email
        public User FindByEmail(string email)
        {
            Block current = Head;
            while (current != null)
            {
                var user = JsonConvert.DeserializeObject<User>(current.Data);
                if (user.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
                {
                    return user;
                }
                current = current.Next;
            }
            return null; // Return null if not found
        }

        // Method to validate user credentials (email and password)
        public bool ValidateCredentials(string email, string password)
        {
            Block current = Head;
            while (current != null)
            {
                var user = JsonConvert.DeserializeObject<User>(current.Data);
                if (user.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && 
                    SHA256Utils.VerificarHashSHA256(password, user.Password)) // Note: In real applications, never store passwords in plain text
                {
                    return true;
                }
                current = current.Next;
            }
            return false;
        }

        // Method to get a user by Id
        public User GetById(int id)
        {
            Block current = Head;
            while (current != null)
            {
                var user = JsonConvert.DeserializeObject<User>(current.Data);
                if (user.Id == id)
                {
                    return user;
                }
                current = current.Next;
            }
            return null; // Return null if not found
        }

        
        public bool DeleteById(int id)
        {
            // If blockchain is empty
            if (Head == null)
            {
                return false;
            }

            // Check if the head block contains the user to delete
            var headUser = JsonConvert.DeserializeObject<User>(Head.Data);
            if (headUser.Id == id)
            {
                Head = Head.Next;
                // After removing head, we need to update the PreviousHash of the new head if it exists
                if (Head != null)
                {
                    Head.PreviousHash = "0000"; // Reset to genesis hash
                    // Recalculate hashes for the remaining chain
                    RecalculateChainHashes();
                }
                return true;
            }

            // Search for the block to delete
            Block current = Head;
            Block previous = null;

            while (current != null)
            {
                var currentUser = JsonConvert.DeserializeObject<User>(current.Data);
                if (currentUser.Id == id)
                {
                    // Found the block to delete
                    previous.Next = current.Next;
                    
                    // Update the PreviousHash of the next block if it exists
                    if (current.Next != null)
                    {
                        current.Next.PreviousHash = previous.Hash;
                    }
                    
                    // Recalculate hashes for the remaining chain
                    RecalculateChainHashes();
                    return true;
                }
                
                previous = current;
                current = current.Next;
            }

            // User with specified Id not found
            return false;
        }

        // Helper method to recalculate hashes after deletion
        private void RecalculateChainHashes()
        {
            if (Head == null) return;

            Block current = Head;
            string previousHash = "0000"; // Genesis block hash
            
            while (current != null)
            {
                current.PreviousHash = previousHash;
                current.Hash = current.CalculateHash();
                previousHash = current.Hash;
                current = current.Next;
            }
        }
        
        public bool UpdateUser(int id, User updatedUser)
        {
            if (Head == null)
            {
                return false; // Blockchain is empty
            }

            Block current = Head;
            bool userFound = false;

            while (current != null)
            {
                var currentUser = JsonConvert.DeserializeObject<User>(current.Data);
                
                if (currentUser.Id == id)
                {
                    // Create a new block with updated data (preserving immutability)
                    Block updatedBlock = new Block(
                        index: current.Index,
                        user: updatedUser,
                        previousHash: current.PreviousHash
                    );
                    
                    // Copy other properties
                    updatedBlock.Timestamp = DateTime.Now.ToString("dd-MM-yy::HH:mm:ss");
                    updatedBlock.Nonce = current.Nonce;
                    
                    // Recalculate hash for the updated block
                    updatedBlock.Hash = updatedBlock.CalculateHash();
                    
                    // Replace the block data (while maintaining the same position in chain)
                    current.Data = JsonConvert.SerializeObject(updatedUser);
                    current.Hash = updatedBlock.Hash;
                    current.Timestamp = updatedBlock.Timestamp;
                    
                    // Recalculate hashes for subsequent blocks
                    RecalculateChainHashes();
                    
                    userFound = true;
                    break;
                }
                
                current = current.Next;
            }

            return userFound;
        }

        // Alternative implementation that adds a new "update" transaction instead of modifying
        public bool UpdateUserAlternative(int id, User updatedUser)
        {
            if (Head == null)
            {
                return false;
            }

            Block current = Head;
            Block lastBlock = null;

            while (current != null)
            {
                var currentUser = JsonConvert.DeserializeObject<User>(current.Data);
                
                if (currentUser.Id == id)
                {
                    // Find the last block
                    lastBlock = Head;
                    while (lastBlock.Next != null)
                    {
                        lastBlock = lastBlock.Next;
                    }
                    
                    User testUser = new User(
                        Id: updatedUser.Id,
                        Name: updatedUser.Name,
                        Lastname: updatedUser.Lastname,
                        Email: updatedUser.Email,
                        Password: updatedUser.Password,
                        Age: updatedUser.Age
                    );

                    // Add a new block with the updated user information
                    AddBlock(testUser);
                    
                    return true;
                }
                
                current = current.Next;
            }

            return false;
        }

    }

}