
using System.Windows;

namespace Application
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public void Start(object sender, StartupEventArgs e)
        {
            new Application.Main().Start();
        }
    }
}