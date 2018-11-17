using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using EbayCrawlerWPF.Pages.Home.ViewModel;
using EbayCrawlerWPF.SearchItems.ViewModel;
using EbayCrawlerWPF.Pages.Crawler.ViewModel;
using EbayCrawlerWPF.Pages.SettingsPanel.ViewModel;

namespace EbayCrawlerWPF.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<FilterDataVM>();
            SimpleIoc.Default.Register<PageNavigatorVM>();
            SimpleIoc.Default.Register<HomeVM>();
            SimpleIoc.Default.Register<CrawlerVM>();
            SimpleIoc.Default.Register<SettingsWindowVM>();
        }

        public MainViewModel MainVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public FilterDataVM FilterDataVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<FilterDataVM>();
            }
        }

        public PageNavigatorVM PageNavigatorVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PageNavigatorVM>();
            }
        }

        public HomeVM HomeVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<HomeVM>();
            }
        }

        public CrawlerVM CrawlerVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CrawlerVM>();
            }
        }

        public SettingsWindowVM SettingsWindowVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SettingsWindowVM>();
            }
        }
    }
}
