
using System;
using System.Windows.Data;
using EbayCrawlerWPF.model;
using System.ComponentModel;
using EbayCrawlerWPF.Messages;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using EbayCrawlerWPF.Model;

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
        private ObservableCollection<EbayItem> _entityCollection;
        public ObservableCollection<EbayItem> EntityCollection
        {
            get
            {
                return _entityCollection;
            }
            set
            {
                _entityCollection = value;

                OnPropertyChanged(nameof(EntityCollection));
                //OnPropertyChanged(nameof(CollectionView));
            }
        }

        /*private CollectionView _collectionView;
        public CollectionView CollectionView
        {
            get
            {
                _collectionView = (CollectionView)CollectionViewSource.GetDefaultView(EntityCollection);

                if (_collectionView != null)
                {
                    _collectionView.SortDescriptions.Add(new SortDescription("PropertyName", ListSortDirection.Ascending));
                }

                return _collectionView;
            }
        }*/

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

            EntityCollection = new ObservableCollection<EbayItem>(ebayItems);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}
