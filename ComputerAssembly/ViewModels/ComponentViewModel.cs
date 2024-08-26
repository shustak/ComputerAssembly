using ComputerAssembly.Models;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ComputerAssembly.ViewModels
{
    public class ComponentViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<OtherComponent> _components;
        private readonly HttpClient _httpClient;

        public ObservableCollection<OtherComponent> Components
        {
            get => _components;
            set
            {
                if (_components != value)
                {
                    _components = value;
                    OnPropertyChanged(nameof(Components));
                }
            }
        }

        public ICommand AddComponentCommand { get; private set; }
        public ICommand DeleteComponentCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ComponentViewModel()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:5001/api/") };
            InitializeCommands();
            LoadComponentsAsync();
        }

        public async Task LoadComponentsAsync()
        {
            try
            {
                var components = await _httpClient.GetFromJsonAsync<ObservableCollection<OtherComponent>>("OtherComponents");
                Components = components ?? new ObservableCollection<OtherComponent>();
                Debug.WriteLine("Loaded components: " + Components.Count);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error loading components: " + ex.Message);
            }
        }

        private void InitializeCommands()
        {
            AddComponentCommand = new RelayCommand<object>(async param => await AddComponent(param), CanAddComponent);
            DeleteComponentCommand = new RelayCommand<object>(async param => await DeleteComponent(param), CanDeleteComponent);
        }

        private async Task AddComponent(object parameter)
        {
            if (parameter is string componentType)
            {
                var newComponent = CreateComponentByType(componentType);
                if (newComponent != null)
                {
                    try
                    {
                        var response = await _httpClient.PostAsJsonAsync("OtherComponents", newComponent);
                        response.EnsureSuccessStatusCode();

                        // Обновление списка компонентов после добавления нового
                        await LoadComponentsAsync();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("An error occurred while adding the component: " + ex.Message);
                    }
                }
            }
        }

        private bool CanAddComponent(object parameter)
        {
            return parameter is string;
        }

        private OtherComponent CreateComponentByType(string type)
        {
            return new OtherComponent
            {
                Type = type,
                Model = "New Model",
                Manufacturer = "New Manufacturer",
                Price = 100.00M
            };
        }

        private async Task DeleteComponent(object parameter)
        {
            if (parameter is OtherComponent component && CanDeleteComponent(component))
            {
                try
                {
                    var response = await _httpClient.DeleteAsync($"OtherComponents/{component.Id}");
                    response.EnsureSuccessStatusCode();

                    // Обновление списка компонентов после удаления
                    await LoadComponentsAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("An error occurred while deleting the component: " + ex.Message);
                }
            }
        }

        private bool CanDeleteComponent(object parameter)
        {
            return parameter is OtherComponent component && Components.Contains(component);
        }
    }
}
