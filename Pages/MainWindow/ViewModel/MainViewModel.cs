using GalaSoft.MvvmLight;
using EbayCrawlerWPF.Messages;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using EbayCrawlerWPF.Pages.SettingsPanel.View;
using EbayCrawlerWPF.Model;

namespace EbayCrawlerWPF.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private SettingsWindow _settingsWindow;

        public RelayCommand WindowLoadedCommand
        {
            get; set;
        }

        public RelayCommand ShowSettingsWindowCommand
        {
            get; set;
        }

        public MainViewModel()
        {
            WindowLoadedCommand = new RelayCommand(() => OnWindowLoaded());
            ShowSettingsWindowCommand = new RelayCommand(() => OnShowSettingsWindow());

            Messenger.Default.Register<CloseSettingsWindowMessage>(this, (o) => OnCloseSettingsWindowMessage(o));
        }

        private void OnWindowLoaded()
        {
            Director.Boot();

            GoToHomePage();
        }

        private void GoToHomePage()
        {
            Messenger.Default.Send(new NavigatePageMessage()
            {
                PageName = PageNavigatorVM.HomeViewName
            });
        }

        private void OnCloseSettingsWindowMessage(object o)
        {
            CloseSettingsWindow();
        }

        private void OnShowSettingsWindow()
        {
            CloseSettingsWindow();
            ShowSettingsWindow();
        }

        private void ShowSettingsWindow()
        {
            _settingsWindow = new SettingsWindow();
            _settingsWindow.Show();
        }

        private void CloseSettingsWindow()
        {
            if (_settingsWindow != null)
            {
                _settingsWindow.Close();
            }
        }
    }
}
