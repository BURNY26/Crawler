using EbayCrawlerWPF.Messages;
using EbayCrawlerWPF.ViewModel;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.ComponentModel;

namespace EbayCrawlerWPF.Pages.Home.ViewModel
{
    public class HomeVM
    {
        public RelayCommand SearchButtonClickCommand
        {
            get; set;
        }

        private string _searchBarText;
        public string SearchBarText
        {
            get
            {
                return _searchBarText;
            }
            set
            {
                _searchBarText = value;
                OnPropertyChanged(nameof(SearchBarText));
            }
        }

        public HomeVM()
        {
            SearchButtonClickCommand = new RelayCommand(() => OnSearchButtonClick());
        }

        private void OnSearchButtonClick()
        {
            Messenger.Default.Send(new NavigatePageMessage
            {
                PageName = PageNavigatorVM.FilterDataViewName
            });

            Messenger.Default.Send(new SearchMessage()
            {
                KeyWord = SearchBarText
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
