namespace Utils {
    public static class Backcup {

        public static void GenerateUsersBackup(){
            
        }

        public static void RestoreBackup(string backupFilePath, string originalFilePath)
        {
            try
            {
                // Read the backup file
                string jsonContent = File.ReadAllText(backupFilePath);

                // Write to the original file
                File.WriteAllText(originalFilePath, jsonContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error restoring backup: {ex.Message}");
            }
        }
    }
}