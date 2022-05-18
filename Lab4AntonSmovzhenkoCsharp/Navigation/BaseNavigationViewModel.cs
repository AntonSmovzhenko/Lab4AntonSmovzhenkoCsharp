using Lab4AntonSmovzhenkoCsharp.Navigation;
using Lab4AntonSmovzhenkoCsharp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;


namespace Lab4AntonSmovzhenkoCsharp.Navigation
{
    
    internal abstract class BaseNavigationViewModel : INotifyPropertyChanged
    {
        private NavigationInProject viewModel;
        List<NavigationInProject> viewModels = new List<NavigationInProject>();

        public event PropertyChangedEventHandler PropertyChanged;
        public NavigationInProject ViewModel 
        { 
            get
            {
                return viewModel;
            }
            set
            {
                viewModel = value;
                OnPropertyChanged(nameof(ViewModel));
            }
        }
        internal void NavigateToRedactor(RedactorViewModel viewMdel)
        {
            ViewModel = viewMdel;
        }

        internal void Navigate(NavigationTypes type)
        {
            if (ViewModel != null && ViewModel.ViewType == type)
            {
                return;
            }
            NavigationInProject viewModel = GetViewModel(type);
            if(viewModel == null)
            {
                return;
            }
            ViewModel = viewModel;
            
        }

        protected abstract NavigationInProject CreateNewViewModel(NavigationTypes type);

        private NavigationInProject GetViewModel(NavigationTypes type)
        {
            NavigationInProject viewModel = viewModels.FirstOrDefault(vm => vm.ViewType == type);
            if(viewModel != null)
            {
                return viewModel;
            }
            viewModel = CreateNewViewModel(type);

            viewModels.Add(viewModel);

            return viewModel;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
    enum NavigationTypes
    {
        Login,
        Info,
        Redactor,
    }
}
