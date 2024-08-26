using ComputerAssembly.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ComputerAssembly.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ComputerViewModel ComputerViewModel { get; private set; }
        public ComponentViewModel ComponentViewModel { get; private set; }
        public FrameViewModel FrameViewModel { get; private set; }
        public MotherboardViewModel MotherboardViewModel { get; private set; }
        public PowerUnitViewModel PowerUnitViewModel { get; private set; }
        public ProcessorViewModel ProcessorViewModel { get; private set; }
        public RAMViewModel RAMViewModel { get; private set; }
        public HDD_SSDViewModel HDD_SSDViewModel { get; private set; }
        public ComputerComponentsViewModel ComputerComponentsViewModel { get; private set; }

        public ICommand BuildComputerCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand ExitCommand { get; private set; }
        public ICommand RefreshAllDataCommand { get; private set; }

        public MainViewModel()
        {
            InitializeViewModels();
            InitializeCommands();
        }

        private void InitializeViewModels()
        {
            // Инициализация каждого ViewModel
            ComputerViewModel = new ComputerViewModel();
            ComponentViewModel = new ComponentViewModel();
            FrameViewModel = new FrameViewModel();
            MotherboardViewModel = new MotherboardViewModel();
            PowerUnitViewModel = new PowerUnitViewModel();
            ProcessorViewModel = new ProcessorViewModel();
            RAMViewModel = new RAMViewModel();
            HDD_SSDViewModel = new HDD_SSDViewModel();
            ComputerComponentsViewModel = new ComputerComponentsViewModel();
        }

        private void InitializeCommands()
        {
            BuildComputerCommand = new RelayCommand(OpenComputerAssemblyWindow);
            SaveCommand = new RelayCommand(SaveData);
            ExitCommand = new RelayCommand(ExitApplication);
            RefreshAllDataCommand = new RelayCommand(async () => await RefreshAllData()); // Команда для обновления данных
        }

        private async Task RefreshAllData()
        {
            // Асинхронное обновление всех данных из различных ViewModel
            await Task.WhenAll(
                ComputerViewModel.LoadComputersAsync(),
                ComponentViewModel.LoadComponentsAsync(),
                FrameViewModel.LoadFramesAsync(),
                MotherboardViewModel.LoadMotherboardsAsync(),
                PowerUnitViewModel.LoadPowerUnitsAsync(),
                ProcessorViewModel.LoadProcessorsAsync(),
                RAMViewModel.LoadRAMsAsync(),
                HDD_SSDViewModel.LoadHDD_SSDAsync()
            );
            MessageBox.Show("Все данные обновлены.", "Обновление данных", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OpenComputerAssemblyWindow()
        {
            var assemblyWindow = new ComputerAssemblyWindow();
            assemblyWindow.DataContext = ComputerComponentsViewModel;
            assemblyWindow.ShowDialog();
        }

        private void SaveData()
        {
            try
            {
                // Метод для сохранения данных, если они не сразу отправляются на сервер
                // Предполагаем, что сохранение прошло успешно

                MessageBox.Show("Данные успешно сохранены.", "Сохранение данных", MessageBoxButton.OK, MessageBoxImage.Information);

                // Закрытие окна сборки компьютера
                ComputerComponentsViewModel.CloseAction?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ExitApplication()
        {
            Application.Current.Shutdown();
        }
    }
}
