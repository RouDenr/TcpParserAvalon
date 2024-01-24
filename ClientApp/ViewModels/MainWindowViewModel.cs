using System;
using System.Windows.Input;
using DynamicData;
using ReactiveUI;

namespace ClientApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public static event Action LoginSuccessEvent = delegate { };
        
        public MainWindowViewModel()
        {
            // Set current page to first on start up
            _currentPage = _pages[0];

            // Create Observables which will activate to deactivate our commands based on CurrentPage state
            var canNavNext = this.WhenAnyValue(x => x.CurrentPage.CanNavigateNext);
            var canNavPrev = this.WhenAnyValue(x => x.CurrentPage.CanNavigatePrevious);

            NavigateNextCommand = ReactiveCommand.Create(NavigateNext, canNavNext);
            NavigatePreviousCommand = ReactiveCommand.Create(NavigatePrevious, canNavPrev);

            LoginSuccessEvent += NavigateNext;
        }

        // A readonly array of possible pages
        private readonly PageViewModelBase[] _pages = 
        { 
            new LoginViewModel(),
            new ClientViewModel()
        };

        // The default is the first page
        private PageViewModelBase _currentPage;

        /// <summary>
        /// Gets the current page. The property is read-only
        /// </summary>
        public PageViewModelBase CurrentPage
        {
            get => _currentPage;
            private set => this.RaiseAndSetIfChanged(ref _currentPage, value);
        }

        /// <summary>
        /// Gets a command that navigates to the next page
        /// </summary>
        public ICommand NavigateNextCommand { get; }

        private void NavigateNext()
        {
            // get the current index and add 1
            var index = _pages.IndexOf(CurrentPage) + 1;

            //  /!\ Be aware that we have no check if the index is valid. You may want to add it on your own. /!\
            CurrentPage = _pages[index];
        }

        /// <summary>
        /// Gets a command that navigates to the previous page
        /// </summary>
        public ICommand NavigatePreviousCommand { get; }

        private void NavigatePrevious()
        {
            // get the current index and subtract 1
            var index = _pages.IndexOf(CurrentPage) - 1;

            //  /!\ Be aware that we have no check if the index is valid. You may want to add it on your own. /!\
            CurrentPage = _pages[index];
        }

        public static void OnLoginSuccessEvent()
        {
            LoginSuccessEvent.Invoke();
        }
    }
}