using Gtk;
using View;
using Storage;

class App {

    public static void Main(string[] args)

    {
        Application.Init();

        AppViews.renderGivenView("login");

        Application.Run();
    }

}