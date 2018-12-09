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
                _ebayItemsToDisplay = value;

                OnPropertyChanged(nameof(EbayItemsToDisplay));
            }
        }

        public FilterDataVM()
        {
            Messenger.Default.Register<SearchMessage>(this, (message) => OnSearchMessageReceived(message));

            StartSearchCommand = new RelayCommand(() => OnStartSearch());
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
            ShowTestData(DbController.FindItemsByKeywords(HasToSearchForFullWords, keywords));
        }

        private void ShowTestData(List<EbayItem> ebayItems)
        {
            if (ebayItems == null)
            {
                Console.WriteLine("no items found");
                return;
            }

            EbayItemsToDisplay = new ObservableCollection<EbayItem>(ebayItems);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}
