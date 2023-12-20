using System.ComponentModel;
using Microsoft.Maui.Controls;

namespace KentekenApp
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public void SendKenteken(object sender, EventArgs e)
        {
            // Pushes data to info page
            string textToPass = KentekenPlaat.Text;
            Navigation.PushAsync(new Info(textToPass));
            
        }
    }
}
