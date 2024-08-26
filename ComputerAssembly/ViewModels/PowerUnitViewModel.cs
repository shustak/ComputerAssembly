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
    public class PowerUnitViewModel : ViewModelBase
    {
        private readonly HttpClient _httpClient;

        public ObservableCollection<PowerUnit> PowerUnits { get; private set; }

        public ICommand AddPowerUnitCommand { get; private set; }
        public ICommand DeletePowerUnitCommand { get; private set; }

        public PowerUnitViewModel()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:5001/api/") };
            PowerUnits = new ObservableCollection<PowerUnit>();

            AddPowerUnitCommand = new RelayCommand(OpenAddPowerUnitWindow);
            DeletePowerUnitCommand = new RelayCommand<PowerUnit>(async powerUnit => await DeletePowerUnit(powerUnit));

            LoadPowerUnitsAsync();
        }

        private void OpenAddPowerUnitWindow()
        {
            var addPowerUnitWindow = new AddPowerUnitWindow();
            addPowerUnitWindow.ShowDialog();

            // Обновляем список после добавления нового блока питания
            LoadPowerUnitsAsync();
        }

        public async Task LoadPowerUnitsAsync()
        {
            var powerUnits = await _httpClient.GetFromJsonAsync<ObservableCollection<PowerUnit>>("powerunits");
            PowerUnits.Clear();

            if (powerUnits != null)
            {
                foreach (var powerUnit in powerUnits)
                {
                    PowerUnits.Add(powerUnit);
                }
            }
        }

        private async Task DeletePowerUnit(PowerUnit powerUnit)
        {
            if (powerUnit != null)
            {
                var response = await _httpClient.DeleteAsync($"powerunits/{powerUnit.Id}");
                if (response.IsSuccessStatusCode)
                {
                    PowerUnits.Remove(powerUnit);
                }
            }
        }
    }
}
