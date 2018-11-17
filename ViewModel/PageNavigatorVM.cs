using System;
using System.ComponentModel;
using EbayCrawlerWPF.Messages;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using EbayCrawlerWPF.Pages.Home.View;
using EbayCrawlerWPF.SearchItems.View;
using EbayCrawlerWPF.Pages.Crawler.View;

namespace EbayCrawlerWPF.ViewModel
{
    public class PageNavigatorVM : INotifyPropertyChanged
    {
        public const string HomeViewName = "HomeView";
        public const string CrawlerViewName = "CrawlerView";
        public const string FilterDataViewName = "FilterDataView";

        public RelayCommand<object> TabSelectionChangedCommand
        {
            get; set;
        }

        private object _currentPage;
        public object CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                if (value == _currentPage)
                {
                    return;
                }

                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get
            {
                return _selectedTabIndex;
            }
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged(nameof(SelectedTabIndex));
            }
        }

        private FilterDataView _filterDataView;
        public FilterDataView FilterDataView
        {
            get
            {
                if (_filterDataView == null)
                {
                    _filterDataView = new FilterDataView();
                }

                return _filterDataView;
            }
        }

        private HomeView _homeView;
        public HomeView HomeView
        {
            get
            {
                if (_homeView == null)
                {
                    _homeView = new HomeView();
                }

                return _homeView;
            }
        }

        private CrawlerView _crawlerView;
        public CrawlerView CrawlerView
        {
            get
            {
                if (_crawlerView == null)
                {
                    _crawlerView = new CrawlerView();
                }

                return _crawlerView;
            }
        }

        public PageNavigatorVM()
        {
            Messenger.Default.Register<NavigatePageMessage>(this, (message) => OnNavigatePageMessage(message));

            TabSelectionChangedCommand = new RelayCommand<object>((o) => OnTabSelectionChanged(o));

        }

        private void OnTabSelectionChanged(object tabIndex)
        {
            if (int.TryParse(tabIndex.ToString(), out int newTabIndex))
            {
                SetSelectedPage(newTabIndex);
            }
        }

        private void OnNavigatePageMessage(NavigatePageMessage message)
        {
            switch (message.PageName)
            {
                case HomeViewName:
                    {
                        SetSelectedPage(0);
                        break;
                    }

                case CrawlerViewName:
                    {
                        SetSelectedPage(1);
                        break;
                    }

                case FilterDataViewName:
                    {
                        SetSelectedPage(2);
                        break;
                    }
            }
        }

        private void SetSelectedPage(int index)
        {
            SelectedTabIndex = index;

            switch (index)
            {
                case 0:
                    {
                        CurrentPage = HomeView;
                        break;
                    }

                case 1:
                    {
                        CurrentPage = CrawlerView;
                        break;
                    }

                case 2:
                    {
                        CurrentPage = FilterDataView;
                        break;
                    }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
