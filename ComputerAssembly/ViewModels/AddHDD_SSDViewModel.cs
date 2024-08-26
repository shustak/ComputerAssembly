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
    public class AddHDD_SSDViewModel : ViewModelBase
    {
        private readonly HttpClient _httpClient;

        public string Model { get; set; }
        public string Type { get; set; }
        public int Capacity { get; set; }
        public string Interface { get; set; }
        public decimal Price { get; set; }

        public ICommand SaveCommand { get; private set; }
        public Action CloseAction { get; set; }

        public AddHDD_SSDViewModel()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:5001/api/") };
            SaveCommand = new RelayCommand(async () => await SaveHDD_SSD());
        }

        private async Task SaveHDD_SSD()
        {
            try
            {
                var hddSsd = new HDD_SSD
                {
                    Model = Model,
                    Type = Type,
                    Capacity = Capacity,
                    Interface = Interface,
                    Price = Price
                };

                var response = await _httpClient.PostAsJsonAsync("hdd_ssd", hddSsd);
                response.EnsureSuccessStatusCode();

                MessageBox.Show("HDD/SSD успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                CloseAction?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении HDD/SSD: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
