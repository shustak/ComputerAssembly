using System.Windows;
using ComputerAssembly.ViewModels;

namespace ComputerAssembly.Views
{
    public partial class AddHDD_SSDWindow : Window
    {
        public AddHDD_SSDWindow()
        {
            InitializeComponent();

            // Создаем экземпляр ViewModel и привязываем его к DataContext
            var viewModel = new AddHDD_SSDViewModel();

            // Устанавливаем действие для закрытия окна
            viewModel.CloseAction = new System.Action(this.Close);

            // Устанавливаем DataContext для окна
            this.DataContext = viewModel;
        }
    }
}
