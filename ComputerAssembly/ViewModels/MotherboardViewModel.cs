using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using ComputerAssembly.Models;
using ComputerAssembly.Views;
using System.Net.Http.Json;
using System.Windows.Input;
using System;

namespace ComputerAssembly.ViewModels
{
    public class MotherboardViewModel : ViewModelBase
    {
        private readonly HttpClient _httpClient;

        public ObservableCollection<Motherboard> Motherboards { get; private set; }

        public ICommand AddMotherboardCommand { get; private set; }
        public ICommand DeleteMotherboardCommand { get; private set; }

        public MotherboardViewModel()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:5001/api/") };
            Motherboards = new ObservableCollection<Motherboard>();

            AddMotherboardCommand = new RelayCommand(OpenAddMotherboardWindow);
            DeleteMotherboardCommand = new RelayCommand<Motherboard>(async motherboard => await DeleteMotherboard(motherboard));

            LoadMotherboardsAsync();
        }

        private void OpenAddMotherboardWindow()
        {
            var addMotherboardWindow = new AddMotherboardWindow();
            addMotherboardWindow.ShowDialog();

            // Обновляем список после добавления новой материнской платы
            LoadMotherboardsAsync();
        }

        public async Task LoadMotherboardsAsync()
        {
            var motherboards = await _httpClient.GetFromJsonAsync<ObservableCollection<Motherboard>>("motherboards");
            Motherboards.Clear();

            if (motherboards != null)
            {
                foreach (var motherboard in motherboards)
                {
                    Motherboards.Add(motherboard);
                }
            }
        }

        private async Task DeleteMotherboard(Motherboard motherboard)
        {
            if (motherboard != null)
            {
                var response = await _httpClient.DeleteAsync($"motherboards/{motherboard.Id}");
                if (response.IsSuccessStatusCode)
                {
                    Motherboards.Remove(motherboard);
                }
            }
        }
    }
}
