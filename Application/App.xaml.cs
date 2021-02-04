using System.Windows;

namespace Application
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App 
    {
        public void Start(object sender, StartupEventArgs e)
        {
            var app = new Main();
            app.ErrorCaught += app.HandleError;
            var startupPath = string.Empty;
            if (e.Args != null & e.Args.Length > 0) startupPath = e.Args[0];
            app.Start(startupPath);
        }
    }
}