using Gtk;
using View;

class App {

    public static void Main(string[] args)

    {
        Application.Init();
        // Crear la ventana principal
        // Dashboard dashhboard = new Dashboard();
        // dashhboard.ShowAll();

        // Application.Run();

        LoginWindow login = new LoginWindow();
        login.ShowAll();
        Application.Run();
    }

}