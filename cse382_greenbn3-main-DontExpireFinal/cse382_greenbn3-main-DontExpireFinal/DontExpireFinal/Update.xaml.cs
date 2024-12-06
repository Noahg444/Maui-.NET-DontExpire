using static System.Net.Mime.MediaTypeNames;


namespace DontExpireFinal;

public partial class Update : ContentPage
{
    private Food currentFood;
    public Update(Food food)
    {
        InitializeComponent();
        currentFood = food;
        BindingContext = food;
        datePicker.Date = currentFood.UseByDate;
        locationPicker.SelectedItem = currentFood.Location;
        name.Text = currentFood.Name.ToString();
    }



    async void cancelButton_Clicked(System.Object sender, System.EventArgs e)
    {
        bool doAlert = await DisplayAlert("Warning", "Do you wish to discard this?", "Yes", "No");
        if (doAlert)
            await Navigation.PopAsync();
        OnAppearing();
    }

    async void updateButton_Clicked(System.Object sender, System.EventArgs e)
    {
        Food currentFood = new Food
        {
            Name = name.Text,
            UseByDate = new DateTime(datePicker.Date.Year, datePicker.Date.Month,
            datePicker.Date.Day),
            Location = (string)locationPicker.SelectedItem,
            Serving = quantityEntry.Text,
            Category = (string)categoryPicker.ToString(),
        };

        var currentDate = DateTime.Now;
        var expiringSoon = currentDate.AddDays(7);
        if (doesNotExpire.IsChecked)
        {
            currentFood.DurationTime = "DoesNotExpire";
        }
        else
        {
            currentFood.DurationTime = currentDate > currentFood.UseByDate ? "Expired" :
                expiringSoon >= currentFood.UseByDate ? "ExpiringSoon" :
                "NotExpiringSoon";

        }
        DB.conn.Update(currentFood);
        BindingContext = currentFood;
        await Navigation.PopAsync();
    }
    void doesNotExpire_CheckedChanged(System.Object sender, Microsoft.Maui.Controls.CheckedChangedEventArgs e)
    {
        //if (doesNotExpire.IsChecked)
        //currentFood.DurationTime = "DoesNotExpire";
    }

}
