using Gtk;
using View;
using Storage;

class App {

    public static void Main(string[] args)

    {
        AppData.user_blockchain.LoadBlockchainFromFile();
        AppData.automobiles_data.LoadBackup();
        AppData.spare_parts_data_avl_tree.LoadBackup();

        Application.Init();

        AppViews.renderGivenView("login");

        Application.Run();
    }

}