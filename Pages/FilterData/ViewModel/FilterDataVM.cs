using System;
using EbayCrawlerWPF.Model;
using EbayCrawlerWPF.model;
using System.ComponentModel;
using EbayCrawlerWPF.Messages;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;

namespace EbayCrawlerWPF.SearchItems.ViewModel
{
    public class FilterDataVM : INotifyPropertyChanged
    {
        public RelayCommand StartSearchCommand
        {
            get; set;
        }

        public RelayCommand OpenSelectedEbayItemInBrowser
        {
            get; set;
        }

        private string _mainSearchBoxText;
        public string MainSearchBoxText
        {
            get
            {
                return _mainSearchBoxText;
            }
            set
            {
                if (value == _mainSearchBoxText)
                {
                    return;
                }

                _mainSearchBoxText = value;
                OnPropertyChanged(nameof(MainSearchBoxText));
            }
        }

        private bool _hasToSearchForFullWords;
        public bool HasToSearchForFullWords
        {
            get
            {
                return _hasToSearchForFullWords;
            }
            set
            {
                if (value == _hasToSearchForFullWords)
                {
                    return;
                }

                _hasToSearchForFullWords = value;
                OnPropertyChanged(nameof(HasToSearchForFullWords));
            }
        }

        //the collection, namechange required!
        private ObservableCollection<EbayItem> _ebayItemsToDisplay;
        public ObservableCollection<EbayItem> EbayItemsToDisplay
        {
            get
            {
                return _ebayItemsToDisplay;
            }
            set
            {
                if (_ebayItemsToDisplay == value)
                {
                    return;
                }

                _ebayItemsToDisplay = value;
                OnPropertyChanged(nameof(EbayItemsToDisplay));
            }
        }

        private EbayItem _selectedEbayItem;
        public EbayItem SelectedEbayItem
        {
            get
            {
                return _selectedEbayItem;
            }
            set
            {
                if (_selectedEbayItem == value)
                {
                    return;
                }

                _selectedEbayItem = value;
                OnPropertyChanged(nameof(SelectedEbayItem));
            }
        }

        public FilterDataVM()
        {
            Messenger.Default.Register<SearchMessage>(this, (message) => OnSearchMessageReceived(message));

            StartSearchCommand = new RelayCommand(() => OnStartSearch());
            OpenSelectedEbayItemInBrowser = new RelayCommand(() => OnOpenSelectedEbayItemInBrowserClicked());
        }

        private void OnSearchMessageReceived(SearchMessage message)
        {
            Console.WriteLine("search implementeren: " + message.KeyWord);
        }

        private void OnStartSearch()
        {
            SearchItems(MainSearchBoxText);
        }

        private void SearchItems(string keywords)
        {
            ShowEbayItems(DbController.FindItemsByKeywords(HasToSearchForFullWords, keywords));
        }

        private void OnOpenSelectedEbayItemInBrowserClicked()
        {
            if (SelectedEbayItem == null)
            {
                return;
            }

            // Open link to ebayitem in default browser
            System.Diagnostics.Process.Start(SelectedEbayItem.Url);
        }

        private void ShowEbayItems(List<EbayItem> ebayItems)
        {
            var listToDisplay = new ObservableCollection<EbayItem>();

            if (ebayItems == null)
            {
                Console.WriteLine("no items found");
            }
            else
            {
                listToDisplay = new ObservableCollection<EbayItem>(ebayItems);
            }

            EbayItemsToDisplay = listToDisplay;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}
