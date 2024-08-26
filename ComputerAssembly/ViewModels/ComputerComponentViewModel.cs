using ComputerAssembly.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ComputerAssembly.ViewModels
{
    public class ComputerComponentsViewModel : ViewModelBase
    {
        private readonly HttpClient _httpClient;

        public ObservableCollection<Processor> Processors { get; private set; } = new ObservableCollection<Processor>();
        public ObservableCollection<RAM> RAMs { get; private set; } = new ObservableCollection<RAM>();
        public ObservableCollection<Motherboard> Motherboards { get; private set; } = new ObservableCollection<Motherboard>();
        public ObservableCollection<Frame> Frames { get; private set; } = new ObservableCollection<Frame>();
        public ObservableCollection<PowerUnit> PowerUnits { get; private set; } = new ObservableCollection<PowerUnit>();
        public ObservableCollection<HDD_SSD> HDD_SSDs { get; private set; } = new ObservableCollection<HDD_SSD>();
        public ObservableCollection<Computer> Computers { get; private set; } = new ObservableCollection<Computer>();

        public Processor SelectedProcessor { get; set; }
        public RAM SelectedRAM { get; set; }
        public Motherboard SelectedMotherboard { get; set; }
        public Frame SelectedFrame { get; set; }
        public PowerUnit SelectedPowerUnit { get; set; }
        public HDD_SSD SelectedHDD_SSD { get; set; }
        public string PCName { get; set; }

        public ICommand BuildComputerCommand { get; private set; }
        public ICommand RefreshDataCommand { get; private set; }

        public event EventHandler ComputersUpdated;

        public Action CloseAction { get; set; }

        public ComputerComponentsViewModel()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:5001/api/") };
            InitializeCommands();
            LoadAndRefreshData();
        }

        private void InitializeCommands()
        {
            BuildComputerCommand = new RelayCommand(async () => await BuildComputer());
            RefreshDataCommand = new RelayCommand(async () => await LoadAndRefreshData());
        }

        private async Task LoadAndRefreshData()
        {
            await LoadProcessorsAsync();
            await LoadRAMsAsync();
            await LoadMotherboardsAsync();
            await LoadFramesAsync();
            await LoadPowerUnitsAsync();
            await LoadHDD_SSDsAsync();
            await LoadComputersAsync();
        }

        public async Task LoadProcessorsAsync()
        {
            try
            {
                var processors = await _httpClient.GetFromJsonAsync<ObservableCollection<Processor>>("processors");
                Processors.Clear();
                if (processors != null)
                {
                    foreach (var processor in processors)
                    {
                        Processors.Add(processor);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error loading processors: " + ex.Message);
            }
        }

        public async Task LoadRAMsAsync()
        {
            try
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
            catch (Exception ex)
            {
                Debug.WriteLine("Error loading RAMs: " + ex.Message);
            }
        }

        public async Task LoadMotherboardsAsync()
        {
            try
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
            catch (Exception ex)
            {
                Debug.WriteLine("Error loading motherboards: " + ex.Message);
            }
        }

        public async Task LoadFramesAsync()
        {
            try
            {
                var frames = await _httpClient.GetFromJsonAsync<ObservableCollection<Frame>>("frames");
                Frames.Clear();
                if (frames != null)
                {
                    foreach (var frame in frames)
                    {
                        Frames.Add(frame);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error loading frames: " + ex.Message);
            }
        }

        public async Task LoadPowerUnitsAsync()
        {
            try
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
            catch (Exception ex)
            {
                Debug.WriteLine("Error loading power units: " + ex.Message);
            }
        }

        public async Task LoadHDD_SSDsAsync()
        {
            try
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
            catch (Exception ex)
            {
                Debug.WriteLine("Error loading HDD/SSD: " + ex.Message);
            }
        }

        public async Task LoadComputersAsync()
        {
            try
            {
                var computers = await _httpClient.GetFromJsonAsync<ObservableCollection<Computer>>("computers");
                Computers.Clear();
                if (computers != null)
                {
                    foreach (var computer in computers)
                    {
                        Computers.Add(computer);
                    }
                }

                ComputersUpdated?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error loading computers: " + ex.Message);
            }
        }

        private async Task BuildComputer()
        {
            if (IsComponentsCompatible())
            {
                try
                {
                    var newComputer = new Computer
                    {
                        Name = PCName,
                        Processor = SelectedProcessor,
                        RAM = SelectedRAM,
                        Motherboard = SelectedMotherboard,
                        Frame = SelectedFrame,
                        PowerUnit = SelectedPowerUnit,
                        HDD_SSD = SelectedHDD_SSD,
                    };

                    var response = await _httpClient.PostAsJsonAsync("computers", newComputer);

                    if (response.IsSuccessStatusCode)
                    {
                        // После успешного добавления компьютера удаляем использованные компоненты
                        await DeleteUsedComponentsAsync(newComputer);

                        Computers.Add(newComputer);
                        ComputersUpdated?.Invoke(this, EventArgs.Empty);

                        MessageBox.Show("Компьютер успешно собран и сохранен в базу данных.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Закрытие окна сборки компьютера
                        CloseWindow();

                        // Вызов команды обновления данных из MainViewModel
                        /*if (Application.Current.MainWindow.DataContext is MainViewModel mainViewModel)
                        {
                            mainViewModel.RefreshAllDataCommand.Execute(null);
                        }*/
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Произошла ошибка при сохранении компьютера: {response.StatusCode} - {errorContent}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка при сохранении компьютера: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task DeleteUsedComponentsAsync(Computer computer)
        {
            try
            {
                if (computer.Processor != null)
                {
                    await _httpClient.DeleteAsync($"processors/{computer.Processor.Id}");
                }
                if (computer.RAM != null)
                {
                    await _httpClient.DeleteAsync($"ram/{computer.RAM.Id}");
                }
                if (computer.Motherboard != null)
                {
                    await _httpClient.DeleteAsync($"motherboards/{computer.Motherboard.Id}");
                }
                if (computer.Frame != null)
                {
                    await _httpClient.DeleteAsync($"frames/{computer.Frame.Id}");
                }
                if (computer.PowerUnit != null)
                {
                    await _httpClient.DeleteAsync($"powerunits/{computer.PowerUnit.Id}");
                }
                if (computer.HDD_SSD != null)
                {
                    await _httpClient.DeleteAsync($"hdd_ssd/{computer.HDD_SSD.Id}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при удалении компонентов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsComponentsCompatible()
        {
            if (SelectedProcessor == null || SelectedRAM == null || SelectedMotherboard == null ||
                   SelectedFrame == null || SelectedPowerUnit == null || SelectedHDD_SSD == null/* || !string.IsNullOrWhiteSpace(PCName)*/)
            {
                MessageBox.Show("Вы не заполнили все данные.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (SelectedProcessor.Socket != SelectedMotherboard.Socket)
            {
                MessageBox.Show("Процессор и материнская плата несовместимы по сокету.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private decimal CalculateTotalPrice()
        {
            decimal totalPrice =
                (SelectedProcessor?.Price ?? 0) +
                (SelectedRAM?.Price ?? 0) +
                (SelectedMotherboard?.Price ?? 0) +
                (SelectedFrame?.Price ?? 0) +
                (SelectedPowerUnit?.Price ?? 0) +
                (SelectedHDD_SSD?.Price ?? 0);

            return totalPrice;
        }

        private void CloseWindow()
        {
            CloseAction?.Invoke();
        }
    }
}
