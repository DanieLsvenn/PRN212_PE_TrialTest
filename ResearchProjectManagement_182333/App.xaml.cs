using System.Configuration;
using System.Data;
using System.Windows;
using Repositories.Models;

namespace ResearchProjectManagement_182333
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static UserAccount? CurrentUser { get; set; }

        
    }
}
