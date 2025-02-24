using Gtk;
using View;

class App {

    public static void Main(string[] args)

    {
        Application.Init();

        LoginView login = new LoginView();
        login.ShowAll();
        Application.Run();
    }

}