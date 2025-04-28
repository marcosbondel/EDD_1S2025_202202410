using Gtk;
using View;
using Storage;

class App {

    public static void Main(string[] args)

    {
        AppData.user_blockchain.LoadBlockchainFromFile();

        Application.Init();

        AppViews.renderGivenView("login");

        Application.Run();
    }

}