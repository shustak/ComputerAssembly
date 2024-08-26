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
    public class AddMotherboardViewModel : ViewModelBase
    {
        private readonly HttpClient _httpClient;

        public string Model { get; set; }
        public string Socket { get; set; }
        public string FormFactor { get; set; }
        public string RAMType { get; set; }
        public decimal Price { get; set; }

        public ICommand SaveCommand { get; private set; }
        public Action CloseAction { get; set; }

        public AddMotherboardViewModel()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:5001/api/") };
            SaveCommand = new RelayCommand(async () => await SaveMotherboard());
        }

        private async Task SaveMotherboard()
        {
            try
            {
                var motherboard = new Motherboard
                {
                    Model = Model,
                    Socket = Socket,
                    FormFactor = FormFactor,
                    RAMType = RAMType,
                    Price = Price
                };

                var response = await _httpClient.PostAsJsonAsync("motherboards", motherboard);
                response.EnsureSuccessStatusCode();

                MessageBox.Show("Материнская плата успешно добавлена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                CloseAction?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении материнской платы: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
