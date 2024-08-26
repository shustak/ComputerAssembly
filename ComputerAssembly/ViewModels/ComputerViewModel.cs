using ComputerAssembly.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ComputerAssembly.ViewModels
{
    public class ComputerViewModel : ViewModelBase
    {
        private readonly HttpClient _httpClient;

        public ObservableCollection<Computer> Computers { get; private set; } = new ObservableCollection<Computer>();

        public ICommand AddComputerCommand { get; private set; }
        public ICommand DeleteComputerCommand { get; private set; }

        public ComputerViewModel()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:5001/api/") };
            InitializeCommands();
            LoadComputersAsync();
        }

        private void InitializeCommands()
        {
            AddComputerCommand = new RelayCommand(() => AddComputer(CreateNewComputer()));
            DeleteComputerCommand = new RelayCommand<Computer>(async computer => await DeleteComputer(computer), CanDeleteComputer);
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
                Debug.WriteLine("Loaded computers: " + Computers.Count);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error loading computers: " + ex.Message);
            }
        }

        private void AddComputer(Computer newComputer)
        {
            try
            {
                Computers.Add(newComputer);
                RaisePropertyChanged(nameof(Computers));

                // Отправка нового компьютера на сервер
                var response = _httpClient.PostAsJsonAsync("computers", newComputer).Result;
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error adding computer: " + ex.Message);
            }
        }

        private async Task DeleteComputer(Computer computer)
        {
            if (CanDeleteComputer(computer))
            {
                try
                {
                    var response = await _httpClient.DeleteAsync($"computers/{computer.Id}");
                    response.EnsureSuccessStatusCode();

                    Computers.Remove(computer);
                    RaisePropertyChanged(nameof(Computers));
                    Debug.WriteLine("Computer deleted successfully.");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error deleting computer: " + ex.Message);
                }
            }
        }

        private bool CanDeleteComputer(Computer computer)
        {
            return computer != null && Computers.Contains(computer);
        }

        private Computer CreateNewComputer()
        {
            return new Computer
            {
                Name = "New Computer",
                ProcessorId = 1, // Замените на нужное значение
                RAMId = 1, // Замените на нужное значение
                MotherboardId = 1, // Замените на нужное значение
                StorageId = 1, // Замените на нужное значение
                PowerUnitId = 1, // Замените на нужное значение
                FrameId = 1, // Замените на нужное значение
                Price = 1000.00M
            };
        }
    }
}
