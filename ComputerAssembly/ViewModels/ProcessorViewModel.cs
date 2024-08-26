using ComputerAssembly.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ComputerAssembly.Models;

namespace ComputerAssembly.ViewModels
{
    public class ProcessorViewModel : ViewModelBase
    {
        private readonly HttpClient _httpClient;

        public ObservableCollection<Processor> Processors { get; private set; } = new ObservableCollection<Processor>();

        public ICommand AddProcessorCommand { get; private set; }
        public ICommand DeleteProcessorCommand { get; private set; }

        public ProcessorViewModel()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:5001/api/") };
            AddProcessorCommand = new RelayCommand(OpenAddProcessorWindow);
            DeleteProcessorCommand = new RelayCommand<Processor>(async (processor) => await DeleteProcessor(processor));
            LoadProcessorsAsync();
        }

        private void OpenAddProcessorWindow()
        {
            var addProcessorWindow = new AddProcessorWindow();
            var addProcessorViewModel = new AddProcessorViewModel();

            // Привязываем ViewModel к окну
            addProcessorViewModel.CloseAction = new Action(addProcessorWindow.Close);
            addProcessorWindow.DataContext = addProcessorViewModel;

            // Открываем окно
            addProcessorWindow.ShowDialog();

            // Обновляем список процессоров после добавления
            LoadProcessorsAsync();
        }

        public async Task LoadProcessorsAsync()
        {
            try
            {
                var processors = await _httpClient.GetFromJsonAsync<ObservableCollection<Processor>>("processors");
                Processors.Clear();
                if (processors != null)
                {
                    foreach (var processor in processors)
                    {
                        Processors.Add(processor);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error loading processors: " + ex.Message);
            }
        }

        private async Task DeleteProcessor(Processor processor)
        {
            if (processor == null)
            {
                return;
            }

            var result = MessageBox.Show($"Вы уверены, что хотите удалить процессор '{processor.Model}'?",
                                         "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var response = await _httpClient.DeleteAsync($"processors/{processor.Id}");
                    response.EnsureSuccessStatusCode();

                    Processors.Remove(processor);

                    MessageBox.Show("Процессор успешно удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка при удалении процессора: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
