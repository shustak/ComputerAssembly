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
    public class FrameViewModel : ViewModelBase
    {
        private readonly HttpClient _httpClient;

        public ObservableCollection<Frame> Frames { get; private set; }

        public ICommand AddFrameCommand { get; private set; }
        public ICommand DeleteFrameCommand { get; private set; }

        public FrameViewModel()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:5001/api/") };
            Frames = new ObservableCollection<Frame>();

            AddFrameCommand = new RelayCommand(OpenAddFrameWindow);
            DeleteFrameCommand = new RelayCommand<Frame>(async frame => await DeleteFrame(frame));

            LoadFramesAsync();
        }

        private void OpenAddFrameWindow()
        {
            var addFrameWindow = new AddFrameWindow();
            addFrameWindow.ShowDialog();

            // Обновляем список корпусов после добавления нового
            LoadFramesAsync();
        }

        public async Task LoadFramesAsync()
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

        private async Task DeleteFrame(Frame frame)
        {
            if (frame != null)
            {
                var response = await _httpClient.DeleteAsync($"frames/{frame.Id}");
                if (response.IsSuccessStatusCode)
                {
                    Frames.Remove(frame);
                }
            }
        }
    }
}
