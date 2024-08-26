using ComputerAssembly.ViewModels;
using System;
using System.Windows;

namespace ComputerAssembly.Views
{
    public partial class ComputerAssemblyWindow : Window
    {
        public ComputerAssemblyWindow()
        {
            InitializeComponent();

            var viewModel = new ComputerComponentsViewModel();
            viewModel.CloseAction = new Action(this.Close);
            DataContext = viewModel;
        }
    }
}
