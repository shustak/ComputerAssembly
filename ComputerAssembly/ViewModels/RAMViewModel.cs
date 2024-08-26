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
    public class RAMViewModel : ViewModelBase
    {
        private readonly HttpClient _httpClient;

        public ObservableCollection<RAM> RAMs { get; private set; }

        public ICommand AddRAMCommand { get; private set; }
        public ICommand DeleteRAMCommand { get; private set; }

        public RAMViewModel()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:5001/api/") };
            RAMs = new ObservableCollection<RAM>();

            AddRAMCommand = new RelayCommand(OpenAddRAMWindow);
            DeleteRAMCommand = new RelayCommand<RAM>(async ram => await DeleteRAM(ram));

            LoadRAMsAsync();
        }

        private void OpenAddRAMWindow()
        {
            var addRAMWindow = new AddRAMWindow();
            addRAMWindow.ShowDialog();

            // Обновляем список после добавления новой оперативной памяти
            LoadRAMsAsync();
        }

        public async Task LoadRAMsAsync()
        {
            var rams = await _httpClient.GetFromJsonAsync<ObservableCollection<RAM>>("ram");
            RAMs.Clear();

            if (rams != null)
            {
                foreach (var ram in rams)
                {
                    RAMs.Add(ram);
                }
            }
        }

        private async Task DeleteRAM(RAM ram)
        {
            if (ram != null)
            {
                var response = await _httpClient.DeleteAsync($"ram/{ram.Id}");
                if (response.IsSuccessStatusCode)
                {
                    RAMs.Remove(ram);
                }
            }
        }
    }
}
