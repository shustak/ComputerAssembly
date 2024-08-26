using ComputerAssembly.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ComputerAssembly
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var viewModel = new MainViewModel();
            DataContext = viewModel;

            viewModel.ComputerComponentsViewModel.ComputersUpdated += (s, e) => UpdateComputersUI();
        }

        private void UpdateComputersUI()
        {
            var listView = this.FindName("ComputerList") as ListView;
            listView?.Items.Refresh();
        }
    }
}
