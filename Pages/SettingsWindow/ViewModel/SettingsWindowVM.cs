using System;
using System.ComponentModel;
using GalaSoft.MvvmLight.Command;
using EbayCrawlerWPF.Controllers.Settings;
using GalaSoft.MvvmLight.Messaging;
using EbayCrawlerWPF.Messages;

namespace EbayCrawlerWPF.Pages.SettingsPanel.ViewModel
{
    public class SettingsWindowVM
    {
        public Action CloseAction
        {
            get; set;
        }

        public RelayCommand SaveButtonClickCommand
        {
            get; set;
        }

        public string DatabaseName
        {
            get
            {
                return SettingsController.Settings.DatabaseName;
            }
            set
            {
                SettingsController.Settings.DatabaseName = value;
                OnPropertyChanged(nameof(DatabaseName));
            }
        }

        public string DatabaseUsername
        {
            get
            {
                return SettingsController.Settings.DatabaseUsername;
            }
            set
            {
                SettingsController.Settings.DatabaseUsername = value;
                OnPropertyChanged(nameof(DatabaseUsername));
            }
        }

        public string DatabasePassword
        {
            get
            {
                return SettingsController.Settings.DatabasePassword;
            }
            set
            {
                SettingsController.Settings.DatabasePassword = value;
                OnPropertyChanged(nameof(DatabasePassword));
            }
        }

        public SettingsWindowVM()
        {
            SaveButtonClickCommand = new RelayCommand(() => OnSaveButtonClick());
        }

        private void OnSaveButtonClick()
        {
            SettingsController.SaveSettings();

            Messenger.Default.Send(new CloseSettingsWindowMessage());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}
