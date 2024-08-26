using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ComputerAssembly.Models;

namespace ComputerAssembly.ViewModels
{
    public class AddFrameViewModel : ViewModelBase
    {
        private readonly HttpClient _httpClient;

        public string Model { get; set; }
        public string FormFactor { get; set; }
        public decimal Price { get; set; }

        public ICommand SaveCommand { get; private set; }
        public Action CloseAction { get; set; }

        public AddFrameViewModel()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:5001/api/") };
            SaveCommand = new RelayCommand(async () => await SaveFrame());
        }

        private async Task SaveFrame()
        {
            try
            {
                var frame = new Frame
                {
                    Model = Model,
                    FormFactor = FormFactor,
                    Price = Price
                };

                var response = await _httpClient.PostAsJsonAsync("frames", frame);
                response.EnsureSuccessStatusCode();

                MessageBox.Show("Корпус успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                CloseAction?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении корпуса: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
