using Cats.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Cats.ViewModels
{
    public class CatsViewModel : INotifyPropertyChanged
    {
        private bool Busy;

        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Cat> Cats { get; set; }

        public CatsViewModel()
        {
            Cats = new ObservableCollection<Models.Cat>();
            GetCatsCommand = new Command(async () => await GetCats(), () => !IsBusy);
        }


        async Task GetCats()
        {
            if (IsBusy)
                return;

            Exception Error = null;
            try
            {
                IsBusy = true;

                var Repository = new Repository();
                var Items = await Repository.GetCats();

                Cats.Clear();
                foreach (var Cat in Items)
                {
                    Cats.Add(Cat);
                }
            }
            catch (Exception ex)
            {
                Error = ex;
            }
            finally
            {
                IsBusy = false;
            }

            if (Error != null)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error!", Error.Message, "OK");
            }

        }



        public Command GetCatsCommand { get; set; }



        public bool IsBusy
        {
            get { return Busy; }
            set { Busy = value; OnPropertyChanged(); GetCatsCommand.ChangeCanExecute(); }
        }

        private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null) => 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
