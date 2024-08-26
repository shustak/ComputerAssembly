using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ComputerAssembly.Models;
using System.Diagnostics;

namespace ComputerAssembly.ViewModels
{
    public class AddProcessorViewModel : ViewModelBase
    {
        private readonly HttpClient _httpClient;

        public string Model { get; set; }
        public string Socket { get; set; }
        public double Frequency { get; set; }
        public int Cores { get; set; }
        public decimal Price { get; set; }

        public ICommand SaveCommand { get; private set; }
        public Action CloseAction { get; set; }

        public AddProcessorViewModel()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:5001/api/") };
            SaveCommand = new RelayCommand(async () => await SaveProcessor());

            // Отладочное сообщение
            Debug.WriteLine("AddProcessorViewModel initialized.");
        }


        private async Task SaveProcessor()
        {
            try
            {
                var processor = new Processor
                {
                    Model = Model,
                    Socket = Socket,
                    Frequency = Frequency,
                    Cores = Cores,
                    Price = Price
                };

                var response = await _httpClient.PostAsJsonAsync("processors", processor);
                response.EnsureSuccessStatusCode();

                MessageBox.Show("Процессор успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                CloseAction?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении процессора: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
