using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Model;
using Utils;

namespace Blocks
{
    public class UserBlock
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int Age { get; set; }
}

    public class Block
    {
        public int Index { get; set; }
        public string Timestamp { get; set; }
        public string Data { get; set; }
        public int Nonce { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        [JsonIgnore]
        public Block Next { get; set; } // Don't serialize Next pointer!

        public Block(int index, User user, string previousHash)
        {
            Index = index;
            Timestamp = DateTime.Now.ToString("dd-MM-yy::HH:mm:ss");
            Data = JsonConvert.SerializeObject(user);
            Nonce = 0;
            PreviousHash = previousHash;
            Hash = CalculateHash();
            Next = null;
        }

        public string CalculateHash()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                string rawData = Index + Timestamp + Data + Nonce + PreviousHash;
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public void MineBlock()
        {
            while (!Hash.StartsWith("0000"))
            {
                Nonce++;
                Hash = CalculateHash();
            }
        }
    }

    public class Blockchain
    {
        private Block Head;

        public Blockchain()
        {
            Head = null;
        }

        public void AddBlock(User user)
        {
            if (Head == null)
            {
                Block firstBlock = new Block(0, user, "0000");
                firstBlock.MineBlock();
                Head = firstBlock;
            }
            else
            {
                Block current = Head;
                while (current.Next != null)
                {
                    current = current.Next;
                }

                Block newBlock = new Block(current.Index + 1, user, current.Hash);
                newBlock.MineBlock();
                current.Next = newBlock;
            }
        }

        public string GenerateJson()
        {
            List<Block> blocks = new List<Block>();
            Block current = Head;
            while (current != null)
            {
                blocks.Add(current);
                current = current.Next;
            }
            return JsonConvert.SerializeObject(blocks, Formatting.Indented);
        }

        public void SaveBlockchainToFile()
        {
            string path = "./data/users_blockchain.json";
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, GenerateJson());
            Console.WriteLine($"Blockchain saved successfully to {path}");
        }

        public void LoadBlockchainFromFile()
        {
            string path = "./data/users_blockchain.json";

            if (!File.Exists(path))
            {
                Console.WriteLine($"File not found: {path}");
                return;
            }

            try
            {
                string jsonString = File.ReadAllText(path);

                // Deserialize the list of blocks
                List<Block> blocks = JsonConvert.DeserializeObject<List<Block>>(jsonString);

                if (blocks != null && blocks.Count > 0)
                {
                    // Iterate through blocks and ensure 'Data' is properly deserialized
                    foreach (var block in blocks)
                    {
                        // Check if 'Data' is a valid JSON string
                        if (IsValidJson(block.Data))
                        {
                            // Deserialize the 'Data' into a User object
                            var user = JsonConvert.DeserializeObject<User>(block.Data);
                            
                            // Store the User object back as a JSON string
                            block.Data = JsonConvert.SerializeObject(user);
                        }
                        else
                        {
                            Console.WriteLine($"Invalid JSON data in block: {block.Data}");
                        }
                    }

                    // Rebuild the blockchain from blocks
                    Head = blocks[0];
                    Block current = Head;
                    for (int i = 1; i < blocks.Count; i++)
                    {
                        current.Next = blocks[i];
                        current = current.Next;
                    }

                    Console.WriteLine($"Blockchain loaded successfully! {blocks.Count} blocks imported.");
                }
                else
                {
                    Console.WriteLine("No blocks found in the file.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading blockchain: {ex.Message}");
            }
        }

        private bool IsValidJson(string data)
        {
            data = data.Trim();
            return data.StartsWith("{") && data.EndsWith("}") && data.Contains(":");
        }

        public string GenerateDot()
        {
            StringBuilder dot = new StringBuilder();
            dot.AppendLine("digraph Blockchain {");
            dot.AppendLine("    node [shape=record];");
            dot.AppendLine("    graph [rankdir=LR];");
            dot.AppendLine("    subgraph cluster_0 {");
            dot.AppendLine("        label=\"Usuarios\";");

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

            dot.AppendLine("}}");
            return dot.ToString();
        }

        public bool AnalyzeBlockchain()
        {
            if (Head == null)
            {
                Console.WriteLine("La cadena está vacía.");
                return true;
            }

            Block current = Head;
            Block previous = null;

            while (current != null)
            {
                if (current.Hash != current.CalculateHash())
                {
                    Console.WriteLine($"El hash del bloque {current.Index} es inválido.");
                    return false;
                }

                if (previous != null && current.PreviousHash != previous.Hash)
                {
                    Console.WriteLine($"El PreviousHash del bloque {current.Index} no coincide.");
                    return false;
                }

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
            return null;
        }

        public bool ValidateCredentials(string email, string password)
        {
            Block current = Head;
            while (current != null)
            {
                var user = JsonConvert.DeserializeObject<User>(current.Data);
                if (user.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
                    SHA256Utils.VerificarHashSHA256(password, user.Password))
                {
                    return true;
                }
                current = current.Next;
            }
            return false;
        }

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
            return null;
        }

        public bool UpdateUser(int id, User updatedUser)
        {
            var originalUser = GetById(id);
            if (originalUser == null) return false;

            updatedUser.Id = originalUser.Id;
            AddBlock(updatedUser); // New transaction, not overwrite
            return true;
        }

        public bool DeleteById(int id)
        {
            var userToDelete = GetById(id);
            if (userToDelete == null) return false;

            userToDelete.Name = "DELETED";
            userToDelete.Lastname = "DELETED";
            userToDelete.Email = $"deleted-{Guid.NewGuid()}@example.com";
            userToDelete.Password = SHA256Utils.GenerarHashSHA256("deleted");
            userToDelete.Age = 0;

            AddBlock(userToDelete);
            return true;
        }

        // Alternative method for update using another logic
        public bool UpdateUserAlternative(int id, User updatedUser)
        {
            Block current = Head;
            while (current != null)
            {
                var user = JsonConvert.DeserializeObject<User>(current.Data);
                if (user.Id == id)
                {
                    // Soft delete original user data
                    user.Name = "DELETED";
                    user.Lastname = "DELETED";
                    user.Email = $"deleted-{Guid.NewGuid()}@example.com";
                    user.Password = SHA256Utils.GenerarHashSHA256("deleted");
                    user.Age = 0;

                    // Add new block with updated user
                    AddBlock(updatedUser);
                    return true;
                }
                current = current.Next;
            }
            return false;
        }
    }
}
