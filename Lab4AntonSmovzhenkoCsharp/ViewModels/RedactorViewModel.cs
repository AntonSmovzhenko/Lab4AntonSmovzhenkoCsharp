using Lab4AntonSmovzhenkoCsharp.Exceptions;
using Lab4AntonSmovzhenkoCsharp.Models;
using Lab4AntonSmovzhenkoCsharp.Navigation;
using Lab4AntonSmovzhenkoCsharp.Repository;
using Lab4AntonSmovzhenkoCsharp.Tools;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace Lab4AntonSmovzhenkoCsharp.ViewModels
{
    internal class RedactorViewModel : INotifyPropertyChanged, NavigationInProject
    {
        private Person person;
        private static PersonFileRepository PersonFileRepository = new PersonFileRepository();
        private RelayCommand<object>? cancelCommand;
        private RelayCommand<object>? gotoInfoCommand;
        private Action gotoInfo;
        private DateTime changedBirthday = DateTime.Today;
        public event PropertyChangedEventHandler? PropertyChanged;

        public NavigationTypes ViewType => NavigationTypes.Redactor;

        public Person Person
        {
            get { return person; }
        }
        public string ChangedFirstName
        {
            get;
            set;
        }
        public string ChangedLastName
        {
            get;
            set;
        }
        public DateTime ChangedBirthday
        {
            get { return changedBirthday; }
            set { changedBirthday = value; }
        }
        public string FirstName
        {
            get { return person.FirstName; }
        }
        public string LastName
        {
            get { return person.LastName; }
        }
        public string Email
        {
            get { return person.Email; }
        }
        public DateTime Birthday
        {
            get { return person.Birthday; }
        }
        public bool IsAdult
        {
            get {return person.IsAdult;}
        }
        public bool IsBirthday
        {
            get { return person.IsBirthday; }
        }
        public string ChineseSign
        {
            get { return person.ChineseSign; }

        }
        public string SunSign
        {
            get { return person.SunSign; }
        }
        public string BirthdayInString
        {
            get { return person.BirthdayInString; }
        }
        public RedactorViewModel(Person person, Action gotoInfo)
        {
            this.person = person;
            this.gotoInfo = gotoInfo;
        }
        

        private async Task Proceed()
        {
            bool isAdult;
            string sunSign;
            string chineseSign;
            bool isBirthday;
            Task<int> t = Task.Run(() => Person.getAge(ChangedBirthday));
            int age = await t;
            Task<bool> t1 = Task.Run(() => Person.CalculateIsAdult(age));
            Task<string> t2 = Task.Run(() => Person.CalculateSunSign(ChangedBirthday));
            Task<string> t3 = Task.Run(() => Person.CalculateChineseSign(ChangedBirthday));
            Task<bool> t4 = Task.Run(() => Person.CalculateIsBirthday(ChangedBirthday));
            isAdult = await t1;
            sunSign = await t2;
            chineseSign = await t3;
            isBirthday = await t4;
            try
            {
                Person changedPerson = new Person(ChangedFirstName, ChangedLastName, Email, ChangedBirthday, isAdult, sunSign, chineseSign, isBirthday, age);
                person = changedPerson;
            }
            catch (IncorrectEmailException ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return;
            }
            catch (AgeIsTooBigException ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return;
            }
            catch (BirthdayInFutureException ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return;
            }
            await PersonFileRepository.AddToRepositoryOrUpdateAsync(person);
            gotoInfo.Invoke();
        }
        public RelayCommand<object> CancelCommand
        {
            get
            {
                return cancelCommand ??= new RelayCommand<object>(o => Cancel());
            }
        }
        public RelayCommand<object> GotoInfoCommand
        {
            get
            {
                return gotoInfoCommand ??= new RelayCommand<object>(_ => Proceed(), CanExecute);
            }
        }
        private void Cancel()
        {
            gotoInfo.Invoke();
        }
        private bool CanExecute(object o)
        {
            return !String.IsNullOrWhiteSpace(ChangedFirstName) && !String.IsNullOrWhiteSpace(ChangedLastName) ;
        }
    }
}
