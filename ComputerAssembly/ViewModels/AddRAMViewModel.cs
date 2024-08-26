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
    public class AddRAMViewModel : ViewModelBase
    {
        private readonly HttpClient _httpClient;

        public string Model { get; set; }
        public string Type { get; set; }
        public int Size { get; set; }
        public int Frequency { get; set; }
        public decimal Price { get; set; }

        public ICommand SaveCommand { get; private set; }
        public Action CloseAction { get; set; }

        public AddRAMViewModel()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:5001/api/") };
            SaveCommand = new RelayCommand(async () => await SaveRAM());
        }

        private async Task SaveRAM()
        {
            try
            {
                var ram = new RAM
                {
                    Model = Model,
                    Type = Type,
                    Size = Size,
                    Frequency = Frequency,
                    Price = Price
                };

                var response = await _httpClient.PostAsJsonAsync("ram", ram);
                response.EnsureSuccessStatusCode();

                MessageBox.Show("RAM успешно добавлена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                CloseAction?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении RAM: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
