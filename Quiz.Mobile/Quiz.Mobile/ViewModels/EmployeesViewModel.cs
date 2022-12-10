using System;
using System.Threading.Tasks;
using Quiz.Mobile.CommunityToolkit.Commands;
using Quiz.Mobile.CommunityToolkit;
using Xamarin.Forms;
using System.Windows.Input;
using Quiz.Mobile.Interfaces;
using Quiz.Mobile.ViewModels.Abstract;
using Quiz.Mobile.Views.Employee;
using Quiz.Mobile.Shared.ViewModels;

namespace Quiz.Mobile.ViewModels
{
    public class EmployeesViewModel : ItemsCollectionViewModel<EmployeeViewModel>
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesViewModel()
        {
            base.Title = "Wszyscy pracownicy";
            //EmployeeGroups = new ObservableRangeCollection<Grouping<string, EmployeeViewModel>>();

            //RefreshCommand = new AsyncCommand(Refresh);
            //LoadMoreCommand = new Command(LoadMore);
            //ClearCommand = new Command(Clear);
            //DelayLoadMoreCommand = new Command(DelayLoadMore);
            SelectedCommand = new AsyncCommand<object>(Selected);

            _employeeService = DependencyService.Get<IEmployeeService>(DependencyFetchTarget.GlobalInstance);
        }

        protected override async Task Add()
        {
            /*var name = await App.Current.MainPage.DisplayPromptAsync("Name", "Insert name");
            var roaster = await App.Current.MainPage.DisplayPromptAsync("Roaster", "Insert roaster");
            await CoffeeService.AddCoffee(name, roaster);
            await Refresh();*/
            //można też przekazać parametr przy nawigacji do innej strony! (sic!)
            //?Name=SomeName&Roaster=SomeRoaster
            //i odrazu po otworzeniu stronu, właściwości będą uzupełnione przesłanymi danymi!
            //Wystarczy dodać kolejny QueryProperty u odbiorcy
            //var route = $"{nameof(AddEmployeePage)}?Name=Motz&Roaster=SomeRoaster";
            //przejście do określonej przez string strony
            var route = nameof(AddEmployeePage);
            await AppShell.Current.GoToAsync(route);
        }

        private async Task Remove(EmployeeViewModel employee)
        {
            //teraz można odwołać się do metody z zarejestrowanego serwisu
            await _employeeService.RemoveEmployee(employee.Id);
            await Refresh();
        }

        //przykładowa komenda wykonujaca operacje asynchroniczne
        public ICommand CallServerCommand { get; }
        async Task CallServer()
        {
            //można dodawać całą listę zamiast pojedynczych elementów
            var list = new List<string> { "Yes Plz", "Tonx", "Blue Bottle" };
            //Coffee.AddRange(list);
            //Coffee.Add("Yes Plz");
            //Coffee.Add("Tonx");
            //Coffee.Add("Blue Bottle");
        }

        //kolekcja, w której każda zmiana bedzie generowała informację (notyfikacje)
        //do UI WPF'a/Xamarin'a
        public ObservableRangeCollection<Coffee> Coffees { get; }
        //problem jest taki, że NIE można dodać range - zakresu do tej kolekcji
        //rozwiązanie skorzystać z własnego ObservableRangeCollection
        //Ponieważ OC przy dodaniu 100 elementów wygeneruje 100 powiadomień
        //co może znacząco spowolnić działanie aplikacji

        public ObservableRangeCollection<Grouping<string, Coffee>> CoffeeGroups { get; }

        async Task Refresh()
        {
            IsBusy = true;

            await Task.Delay(2000);

            Coffees.Clear();

            var coffees = await coffeeService.GetCoffees();

            Coffees.AddRange(coffees);

            IsBusy = false;

            DependencyService.Get<IToast>()?.MakeToast("Refreshed");
        }

        Coffee previouslySelected;
        Coffee selectedCoffee;
        //odwzorowanie zdarzenia ItemSelected z CodeBehindu
        public Coffee SelectedCoffee
        {
            get => selectedCoffee;
            set => SetProperty(ref selectedCoffee, value);
            //tutaj było wywoływanie asynchronicznych metod które nie były oczekiwane
            // - niezbyt dobre...
            //dlatego lepiej zostawić to jako normalną właściwość
            //i dodać komendę asynchroniczną dzięki EventToCommand z Xamarin.CommunityToolkit
            //set
            //{
            //    if(value != null)
            //    {
            //        Application.Current.MainPage.DisplayAlert("Seleceted", value.Name, "OK");
            //        previouslySelected = value;
            //        //natychmiastowe odznaczenie przedmiotu
            //        value = null;
            //    }
            //    selectedCoffee = value;
            //    OnPropertyChanged();
            //}
        }

        //zamiana właściwości na komendę
        public AsyncCommand<object> SelectedCommand { get; }
        //metoda wywoływana podczas komendy SelectedCommand
        async Task Selected(object args)
        {
            //przesłanie argumentu dzięki EventToCommand 
            //oraz ItemSelectedEventArgsConverter
            var coffee = args as Coffee;
            if (coffee == null)
                return;

            //należy przekazać jakiś parametr (najczęściej Id) który określi
            //której kawy szczegóły chcemy oglądać,
            //a w podstronie dodać atrybut QueryProperty
            var route = $"{nameof(MyCoffeeDetailsPage)}?CoffeeId={coffee.Id}";
            //można dodać & i coś jeszcze dopisać - jak w tworzeniu Urli
            await AppShell.Current.GoToAsync(route);

            //natychmiastowe odznaczenie
            SelectedCoffee = null;

            //await Application.Current.MainPage.DisplayAlert("Selected", coffee.Name, "OK");
        }

        //chcemy przekazac powiązaną Coffee (z binding contextu) do asychronicznej komendy
        public AsyncCommand<Coffee> FavoriteCommand { get; }
        async Task Favorite(Coffee coffee)
        {
            if (coffee == null)
                return;

            //await Application.Current.MainPage.DisplayAlert("Favorite", coffee.Name, "OK");
            await AppShell.Current.GoToAsync(nameof(AddMyCoffeePage));
        }

        public MyCoffeApp.Commands.Command LoadMoreCommand { get; }
        void LoadMore()
        {
            if (Coffees.Count >= 20)
                return;

            var image = "https://cdn-icons-png.flaticon.com/512/174/174848.png";

            Coffees.Add(new Coffee { Roaster = "Yes Plz", Name = "Sip of Sunshine", Image = image });
            Coffees.Add(new Coffee { Roaster = "Yes Plz", Name = "Potent Potable", Image = image });
            Coffees.Add(new Coffee { Roaster = "Blue Bottle", Name = "Kenya Kiambu", Image = image });
            Coffees.Add(new Coffee { Roaster = "Blue Bottle", Name = "Kenya Kiambu", Image = image });
            Coffees.Add(new Coffee { Roaster = "Blue Bottle", Name = "Kenya Kiambu", Image = image });
            Coffees.Add(new Coffee { Roaster = "Blue Bottle", Name = "Kenya Kiambu Handege", Image = image });
            Coffees.Add(new Coffee { Roaster = "Blue Bottle", Name = "Kenya Kiambu Handege", Image = image });

            CoffeeGroups.Clear();

            CoffeeGroups.Add(new Grouping<string, Coffee>("Blue Bottle", Coffees.Where(c => c.Roaster == "Blue Bottle")));
            CoffeeGroups.Add(new Grouping<string, Coffee>("Yes Plz", Coffees.Where(c => c.Roaster == "Yes Plz")));
        }

        public MyCoffeApp.Commands.Command DelayLoadMoreCommand { get; }
        void DelayLoadMore()
        {
            if (Coffees.Count <= 10)
                return;

            LoadMore();
        }

        public MyCoffeApp.Commands.Command ClearCommand { get; }
        void Clear()
        {
            Coffees.Clear();
            CoffeeGroups.Clear();
        }
    }
}

