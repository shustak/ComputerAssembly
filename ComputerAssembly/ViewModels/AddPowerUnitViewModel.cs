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
    public class AddPowerUnitViewModel : ViewModelBase
    {
        private readonly HttpClient _httpClient;

        public string Model { get; set; }
        public int Wattage { get; set; }
        public string FormFactor { get; set; }
        public decimal Price { get; set; }

        public ICommand SaveCommand { get; private set; }
        public Action CloseAction { get; set; }

        public AddPowerUnitViewModel()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:5001/api/") };
            SaveCommand = new RelayCommand(async () => await SavePowerUnit());
        }

        private async Task SavePowerUnit()
        {
            try
            {
                var powerUnit = new PowerUnit
                {
                    Model = Model,
                    Wattage = Wattage,
                    FormFactor = FormFactor,
                    Price = Price
                };

                var response = await _httpClient.PostAsJsonAsync("powerunits", powerUnit);
                response.EnsureSuccessStatusCode();

                MessageBox.Show("Блок питания успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                CloseAction?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении блока питания: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
