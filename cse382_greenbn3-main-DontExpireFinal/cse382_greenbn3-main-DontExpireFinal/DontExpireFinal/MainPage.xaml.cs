namespace DontExpireFinal;

using Plugin.Maui.Audio;
using Microsoft.Maui;


public partial class MainPage : ContentPage
{

    private readonly IAudioManager audioManager;
    private IAudioPlayer player;


    public MainPage(IAudioManager audioManager)
    {
        InitializeComponent();
        DB.OpenConnection();
        DB.conn.CreateTable<Food>();
        //DB.conn.DeleteAll<Food>();
        //PrePopDataBase();
        LoadData();
        this.audioManager = audioManager;
        MessagingCenter.Subscribe<Add, Food>(this, "FoodAdded", (sender, food) =>
        {
            Console.WriteLine("Food item added: " + food.Name);
            LoadData();
        });

    }


    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadData();
        if (player != null)
        {
            player.Pause();
        }


        //ExpireList.ItemsSource = DB.conn.Table<Food>().OrderByDescending(s => s.Date);
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();

    }


    private void PrePopDataBase()
    {
        //DB.conn.DeleteAll<Food>();
        Food food1 = new Food
        {
            UseByDate = new DateTime(2023, 10, 1),
            Location = "Pantry",
            Category = "Other",
            Serving = "1",
            Name = "Pasta"
        };
        DB.conn.Insert(food1);

        Food food2 = new Food
        {
            UseByDate = new DateTime(2023, 10, 19),
            Location = "Basement",
            Category = "Other",
            Serving = "1",
            Name = "Pickles"
        };
        DB.conn.Insert(food2);

        Food food3 = new Food
        {
            UseByDate = new DateTime(2023, 10, 4),
            Location = "Kitchen",
            Category = "Other",
            Serving = "1",
            Name = "Bread"
        };
        DB.conn.Insert(food3);

        Food food4 = new Food
        {
            UseByDate = new DateTime(2023, 10, 5),
            Location = "Kitchen",
            Category = "Other",
            Serving = "1",
            Name = "Salmon"
        };
        DB.conn.Insert(food4);

        Food food5 = new Food
        {
            UseByDate = new DateTime(2023, 10, 8),
            Location = "Kitchen",
            Category = "Other",
            Serving = "1",
            Name = "Orange"
        };
        DB.conn.Insert(food5);

        Food food6 = new Food
        {
            UseByDate = new DateTime(2023, 10, 10),
            Location = "Kitchen",
            Category = "Other",
            Serving = "1",
            Name = "Chicken"
        };
        DB.conn.Insert(food6);

        Food food7 = new Food
        {
            UseByDate = new DateTime(2023, 10, 15),
            Location = "Kitchen",
            Category = "Other",
            Serving = "1",
            Name = "Onion"
        };
        DB.conn.Insert(food7);

        Food food8 = new Food
        {
            UseByDate = new DateTime(2023, 10, 18),
            Location = "Pantry",
            Category = "Other",
            Serving = "1",
            Name = "Soup"
        };
        DB.conn.Insert(food8);
    }
    private void LoadData()
    {

        var currentDate = DateTime.Now;
        var expiringSoon = currentDate.AddDays(7);
        var allFoodItems = DB.conn.Table<Food>().ToList();
        foreach (var foodItem in allFoodItems)
        {
            if (foodItem.DurationTime == "DoesNotExpire")
                return;
            else
            {
                if (currentDate > foodItem.UseByDate)
                {
                    foodItem.DurationTime = "Expired";

                }
                else if (expiringSoon >= foodItem.UseByDate)
                {
                    foodItem.DurationTime = "ExpiringSoon";
                }
                else { foodItem.DurationTime = "DoesNotExpire"; }

            }


        }


        //var allFoodItems = DB.conn.Table<Food>().ToList();
        ExpireSoonList.ItemsSource =
                    allFoodItems.Where(s => s.DurationTime == "ExpiringSoon").ToList();
        ExpireList.ItemsSource =
            allFoodItems.Where(p => p.DurationTime == "Expired").ToList();
        NoExpireList.ItemsSource =
            allFoodItems.Where(p => p.DurationTime == "DoesNotExpire").ToList();
        //ExpireSoonList.ItemsSource = allFoodItems.Where(s => s.selectDuration == "ExpiringSoon").ToList();
        //var foods = DB.conn.Table<Food>().ToList().Where(s => s.DurationTime == "Expired").ToList();
        //ExpireList.ItemsSource = foods;

        //foods = DB.conn.Table<Food>().ToList().Where(s => s.DurationTime == "ExpiringSoon").ToList();
        //ExpireSoonList.ItemsSource = foods;
        // ExpireSoonList.ItemsSource = DB.conn.Table<Food>().ToList().Where(s => s.DurationTime == "ExpiringSoon").ToList();

        //ExpireList.ItemsSource = foods;
        //ExpireSoonList.ItemsSource = DB.conn.Table<Food>().OrderByDescending(s => s.DurationTime).ToList(); ;
    }

    void sortBtn_CheckedChanged(System.Object sender, Microsoft.Maui.Controls.CheckedChangedEventArgs e)
    {
    }

    private async void OnAddItemClicked(object sender, EventArgs e)
    {

        var addFood = new Add();

        await Navigation.PushAsync(addFood);
        try
        {

            var audioStream = await FileSystem.OpenAppPackageFileAsync("RamBellSound.mp3");
            player = audioManager.CreatePlayer(audioStream);
            player.Volume = 0.5;


            Console.WriteLine("Attempting to play sound");
            player.Play();
            Console.WriteLine("Sound played");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error playing sound: {ex.Message}");
        }
    }

    private async void OnPrefsPageClicked(object sender, EventArgs e)
    {

        var prefs = new Prefs();
        

        await Navigation.PushAsync(prefs);
        
    }



    async void ExireList_ItemTapped(System.Object sender, Microsoft.Maui.Controls.ItemTappedEventArgs e)
    {
        var action = await DisplayActionSheet("Operation", "Cancel", "Modify record", "Delete record");
        if (action == "Modify record")
        {
            var updateFood = new Update((Food)e.Item);
            await Navigation.PushAsync(updateFood);
            LoadData();
        }
        else if (action == "Delete record")
        {
            var symptom = (Food)e.Item;
            var answer = await DisplayAlert("Confirm", "Are you sure you want to delete this record?", "Yes", "No");
            if (answer)
            {
                DB.conn.Delete(symptom);
                await DisplayAlert("Success", "Record Deleted", "OK");
                LoadData();
            }
        }
        ((ListView)sender).SelectedItem = null;
    }

    void AddNewEntry_Button_Clicked(System.Object sender, System.EventArgs e)
    {
        var addFood = new Add();
        Navigation.PushAsync(addFood);
    }

    void CheckBox_CheckedChanged(System.Object sender, Microsoft.Maui.Controls.CheckedChangedEventArgs e)
    {
    }

    async void ExpireSoonList_ItemTapped(System.Object sender, Microsoft.Maui.Controls.ItemTappedEventArgs e)
    {
        var action = await DisplayActionSheet("Operation", "Cancel", "Modify record", "Delete record");
        if (action == "Modify record")
        {
            var updateFood = new Update((Food)e.Item);
            await Navigation.PushAsync(updateFood);
            LoadData();
        }
        else if (action == "Delete record")
        {
            var symptom = (Food)e.Item;
            var answer = await DisplayAlert("Confirm", "Are you sure you want to delete this record?", "Yes", "No");
            if (answer)
            {
                DB.conn.Delete(symptom);
                await DisplayAlert("Success", "Record Deleted", "OK");
                LoadData();
            }
        }
        ((ListView)sender).SelectedItem = null;
    }

    async void NoExpireList_ItemTapped(System.Object sender, Microsoft.Maui.Controls.ItemTappedEventArgs e)
    {
        var action = await DisplayActionSheet("Operation", "Cancel", "Modify record", "Delete record");
        if (action == "Modify record")
        {
            var updateFood = new Update((Food)e.Item);
            await Navigation.PushAsync(updateFood);
            LoadData();
        }
        else if (action == "Delete record")
        {
            var symptom = (Food)e.Item;
            var answer = await DisplayAlert("Confirm", "Are you sure you want to delete this record?", "Yes", "No");
            if (answer)
            {
                DB.conn.Delete(symptom);
                await DisplayAlert("Success", "Record Deleted", "OK");
                LoadData();
            }
        }
        ((ListView)sender).SelectedItem = null;
    }

    private bool isChecked;
    public bool IsChecked
    {
        get { return isChecked; }
        set { isChecked = value; }
    }

    private async void DeleteSelectedItems(ListView listview)
    {
        var selectedItems = (listview.ItemsSource as List<Food>).ToList();
        var itemsToDelete = selectedItems.Where(f => f.IsChecked).ToList();

        foreach (var item in itemsToDelete)
        {
            DB.conn.Delete(item);
            selectedItems.Remove(item);
        }
        listview.ItemsSource = null;
        listview.ItemsSource = selectedItems;
        try
        {

            var audioStream = await FileSystem.OpenAppPackageFileAsync("TrashCanSoundEffect.mp3");
            player = audioManager.CreatePlayer(audioStream);
            player.Volume = 0.2;


            Console.WriteLine("Attempting to play sound");
            player.Play();
            Console.WriteLine("Sound played");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error playing sound: {ex.Message}");
        }
    }
    async void DeleteSelected_Clicked(System.Object sender, System.EventArgs e)
    {
        var confirmDelete = await DisplayAlert("Confirm", "Are you sure you want to delete this record?", "Yes", "No");
        if (confirmDelete)
        {
            DeleteSelectedItems(ExpireList);
            DeleteSelectedItems(ExpireSoonList);
            DeleteSelectedItems(NoExpireList);
            LoadData();
        }
    }
}





