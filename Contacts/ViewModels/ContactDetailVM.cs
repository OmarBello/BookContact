using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Contacts.Models;
using Xamarin.Forms;

namespace Contacts.ViewModels
{
    public class ContactDetailVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

   
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string userId;
        private string firstname;
        private string lastName;
        private string phone;


        public string UserId
        {
            get { return userId; }
            set
            {
                userId = value;
                OnPropertyChanged("UserId");
                OnPropertyChanged("CanSave");
            }
        }
        public string FirstName
        {
            get { return firstname; }
            set
            {
                firstname = value;
                OnPropertyChanged("FirstName");
                OnPropertyChanged("FullName");
                OnPropertyChanged("CanSave");
            }
        }
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged("LastName");
                OnPropertyChanged("FullName");
            }
        }
      
        public string Phone
        {
            get { return phone; }
            set
            {
                phone = value;
                OnPropertyChanged("Phone");
                OnPropertyChanged("CanSave");
            }
        }

        

        public string FullName => FirstName + " " + LastName;

        public ObservableCollection<Contact> Contacts { get; set; }

        public ICommand SaveCommand { get; set; }

        public ContactDetailVM()
        {
            SaveCommand = new Command<bool>(SaveAction, CanExecuteSave);
        }

        public bool CanSave
        {
            get
            {
                return !string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(Phone);
            }
        }

        bool CanExecuteSave(bool arg)
        {
            return arg;
        }

        void SaveAction(bool obj)
        {
            Contact newExperience = new Contact()
            {
                UserId = UserId,
                FirstName = FirstName,
                LastName = LastName,
                Phone = Phone,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            if (!Contact.CheckIfUserExists(contact: newExperience))
            {
                bool insertSuccessfull = newExperience.InsertContacts();

                if (insertSuccessfull)
                {
                    App.Current.MainPage.Navigation.PopAsync();
                    FirstName = string.Empty;
                    LastName = string.Empty;
                    Phone = string.Empty;
                }
                else
                {
                    App.Current.MainPage.DisplayAlert("Error", "No se registro bien intentelo de nuevo.", "Ok");
                }
            }
            else
            {
                App.Current.MainPage.DisplayAlert("Error", "Solo debe de haber 1 solo ID unico para cada usuario", "OK");
            }

        }
    }
}
