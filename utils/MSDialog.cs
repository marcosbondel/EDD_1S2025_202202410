using System;
using Gtk;

namespace Utils {
    public static class MSDialog {
        public static void ShowMessageDialog(Window parent, string title, string message, MessageType type){
            using (MessageDialog dialog = new MessageDialog(parent,
                DialogFlags.Modal, type, ButtonsType.Ok, message))
            {
                dialog.Title = title;
                dialog.Run();
                dialog.Destroy();
            }
        }

        // Helper method to show input dialog
        public static string ShowInputDialog(Window parent, string title, string message){
            Dialog dialog = new Dialog(title, parent, DialogFlags.Modal);
            dialog.SetDefaultSize(300, 150);

            Label label = new Label(message);
            Entry entry = new Entry();

            // Buttons
            Button okButton = new Button("OK");
            okButton.Clicked += (sender, e) => { dialog.Respond(ResponseType.Ok); };

            Button cancelButton = new Button("Cancel");
            cancelButton.Clicked += (sender, e) => { dialog.Respond(ResponseType.Cancel); };

            // Layout
            Box contentArea = (Box)dialog.ContentArea;
            contentArea.PackStart(label, false, false, 10);
            contentArea.PackStart(entry, false, false, 10);
            contentArea.PackStart(okButton, false, false, 5);
            contentArea.PackStart(cancelButton, false, false, 5);

            dialog.ShowAll();
            ResponseType response = (ResponseType)dialog.Run();
            string input = entry.Text.Trim();
            dialog.Destroy();

            return response == ResponseType.Ok ? input : string.Empty;
        }
    }
}