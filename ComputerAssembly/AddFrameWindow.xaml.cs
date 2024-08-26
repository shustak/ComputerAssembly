using System.Windows;
using ComputerAssembly.ViewModels;

namespace ComputerAssembly.Views
{
    public partial class AddFrameWindow : Window
    {
        public AddFrameWindow()
        {
            InitializeComponent();

            // Создаем экземпляр ViewModel и привязываем его к DataContext
            var viewModel = new AddFrameViewModel();

            // Устанавливаем действие для закрытия окна
            viewModel.CloseAction = new System.Action(this.Close);

            // Устанавливаем DataContext для окна
            this.DataContext = viewModel;
        }
    }
}
