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
    public class HDD_SSDViewModel : ViewModelBase
    {
        private readonly HttpClient _httpClient;

        public ObservableCollection<HDD_SSD> HDD_SSDs { get; private set; }

        public ICommand AddHDD_SSDCommand { get; private set; }
        public ICommand DeleteHDD_SSDCommand { get; private set; }

        public HDD_SSDViewModel()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:5001/api/") };
            HDD_SSDs = new ObservableCollection<HDD_SSD>();

            AddHDD_SSDCommand = new RelayCommand(OpenAddHDD_SSDWindow);
            DeleteHDD_SSDCommand = new RelayCommand<HDD_SSD>(async hddSsd => await DeleteHDD_SSD(hddSsd));

            LoadHDD_SSDAsync();
        }

        private void OpenAddHDD_SSDWindow()
        {
            var addHddSsdWindow = new AddHDD_SSDWindow();
            addHddSsdWindow.ShowDialog();

            // Обновляем список после добавления нового HDD/SSD
            LoadHDD_SSDAsync();
        }

        public async Task LoadHDD_SSDAsync()
        {
            var hddSsds = await _httpClient.GetFromJsonAsync<ObservableCollection<HDD_SSD>>("hdd_ssd");
            HDD_SSDs.Clear();

            if (hddSsds != null)
            {
                foreach (var hddSsd in hddSsds)
                {
                    HDD_SSDs.Add(hddSsd);
                }
            }
        }

        private async Task DeleteHDD_SSD(HDD_SSD hddSsd)
        {
            if (hddSsd != null)
            {
                var response = await _httpClient.DeleteAsync($"hdd_ssd/{hddSsd.Id}");
                if (response.IsSuccessStatusCode)
                {
                    HDD_SSDs.Remove(hddSsd);
                }
            }
        }
    }
}
