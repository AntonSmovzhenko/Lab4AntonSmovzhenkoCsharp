using Lab4AntonSmovzhenkoCsharp.Models;
using Lab4AntonSmovzhenkoCsharp.Navigation;
using Lab4AntonSmovzhenkoCsharp.Repository;
using Lab4AntonSmovzhenkoCsharp.Sending;
using Lab4AntonSmovzhenkoCsharp.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab4AntonSmovzhenkoCsharp.ViewModels
{
    internal class InfoViewModel : INavigatable, INotifyPropertyChanged
    {
        public NavigationTypes ViewType => NavigationTypes.Info;
        private Action gotoLogin;
        private Action gotoInfo;

        private Action<RedactorViewModel> gotoPerson;
        private RelayCommand<object>? gotoLoginCommand;
        private RelayCommand<object>? sortByEmailsCommand;
        private RelayCommand<object>? changeSelectedCommand;
        private RelayCommand<object>? exitCommand;
        private RelayCommand<object>? removePersonCommand;
        private RelayCommand<object>? filterPeopleCommand;
        private RelayCommand<object>? cancelFilterCommand;
        public event PropertyChangedEventHandler? PropertyChanged;
        private static PersonFileRepository personFileRepository;
        private static ObservableCollection<RedactorViewModel> people;
        private static ObservableCollection<RedactorViewModel> gridPeople;
        private bool filtered;
        public ObservableCollection<RedactorViewModel> People
        {
            get
            {
                return people;
            }
            set
            {
                people = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<RedactorViewModel> GridPeople
        {
            get
            {
                return gridPeople;
            }
            set
            {
                gridPeople = value;
                OnPropertyChanged();
            }
        }
        public RedactorViewModel SelectedPerson
        {
            get;
            set;
        }
        public RelayCommand<object> GotoLoginCommand
        {
            get
            {
                return gotoLoginCommand ??= new RelayCommand<object>(_ => GotoLogin());
            }
        }
        public RelayCommand<object> SortByEmailsCommand
        {
            get
            {
                return sortByEmailsCommand ??= new RelayCommand<object>(_ => SortByEmails());
            }
        }
        public RelayCommand<object> ChangeSelectedCommand
        {
            get
            {
                return changeSelectedCommand ??= new RelayCommand<object>(_ => GoToChangingWindow(),CanExecuteEditOrRemoveSelected);
            }
        }
        public RelayCommand<object> ExitCommand
        {
            get
            {
                return exitCommand ??= new RelayCommand<object>(o => Close());
            }
        }
        public RelayCommand<object> FilterPeopleCommand
        {
            get
            {
                return filterPeopleCommand ??= new RelayCommand<object>(o => FilterPeople());
            }
        }
        public RelayCommand<object> CancelFilterCommand
        {
            get
            {
                return cancelFilterCommand ??= new RelayCommand<object>(o => CancelFilter(),CanExecute);
            }
        }
        public RelayCommand<object> RemovePersonCommand
        {
            get
            {
                return removePersonCommand ??= new RelayCommand<object>(o => RemovePerson(),CanExecuteEditOrRemoveSelected);
            }
        }
        public InfoViewModel(Action gotoLogin, Action<RedactorViewModel> gotoPerson, Action gotoInfo)
        {
            this.gotoLogin = gotoLogin;
            this.gotoPerson = gotoPerson;
            this.gotoInfo = gotoInfo;
            personFileRepository = new PersonFileRepository();
            people = new ObservableCollection<RedactorViewModel>(personFileRepository.GetAllPersons(gotoInfo));
            gridPeople = new ObservableCollection<RedactorViewModel>(personFileRepository.GetAllPersons(gotoInfo));
        }

        public static void AddOnePerson(RedactorViewModel person)
        {
            if(people != null)
            {
                people.Add(person);
                gridPeople.Add(person);
            }
        }
        private void SortByEmails()
        {
            People = new ObservableCollection<RedactorViewModel>(people.OrderBy(person => person.Email).ToList());
            GridPeople = new ObservableCollection<RedactorViewModel>(gridPeople.OrderBy(person => person.Email).ToList());
            
        }
        private void FilterPeople()
        {
            if(!filtered)
            {
                GridPeople = new ObservableCollection<RedactorViewModel>(gridPeople.Where(p => p.IsAdult).ToList());
                filtered = true;
            }
        }
        private void CancelFilter()
        {
            GridPeople = new ObservableCollection<RedactorViewModel>(People);
            filtered = false;
        }
        private void GotoLogin()
        {
            gotoLogin.Invoke();
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void GoToChangingWindow()
        {
            gotoPerson.Invoke(SelectedPerson);
        }
        private void Close()
        {
            Environment.Exit(0);
        }
        private async Task RemovePerson()
        {
            if(SelectedPerson != null)
            {
                await Task.Run(() => personFileRepository.RemoveFromRepository(SelectedPerson.Person));
                People.Remove(SelectedPerson);
                OnPropertyChanged(nameof(people));
                GridPeople.Remove(SelectedPerson);
                OnPropertyChanged(nameof(gridPeople));
            }
        }
        private bool CanExecute(object o)
        {
            return filtered;
        }
        private bool CanExecuteEditOrRemoveSelected(object o)
        {
            return SelectedPerson != null;
        }
    }
}
