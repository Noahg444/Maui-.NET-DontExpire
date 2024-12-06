

using System.Diagnostics;
using Plugin.Maui.Audio;

namespace DontExpireFinal;

public partial class Add : ContentPage
{

    private Food currentFood = new Food();
    public Add()
    {
        InitializeComponent();
        BindingContext = currentFood;
        locationPicker.SelectedIndex = 1;
        
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();



    }


    async void Button_Clicked_Cancel(System.Object sender, System.EventArgs e)
    {
        bool doAlert = await DisplayAlert("Warning", "Do you wish to discard this?", "Yes", "No");
        if (doAlert)
            await Navigation.PopAsync();
        OnAppearing();
    }

    private async void Button_Clicked_Add(System.Object sender, System.EventArgs e)
    {


        Food currentFood = new Food
        {
            Name = name.Text,
            UseByDate = new DateTime(datePicker.Date.Year, datePicker.Date.Month,
            datePicker.Date.Day),
            Location = (string)locationPicker.SelectedItem,
            Serving = quantityEntry.Text,
            Category = (string)categoryPicker.ToString(),
            ImageFile = (string)imagePicker.SelectedItem,
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

        DB.conn.Insert(currentFood);
        BindingContext = currentFood;
        MessagingCenter.Send(this, "FoodAdded", currentFood);
        await Navigation.PopAsync();
    }

    void doesNotExpire_CheckedChanged(System.Object sender, Microsoft.Maui.Controls.CheckedChangedEventArgs e)
    {
        //if (doesNotExpire.IsChecked)
        //currentFood.DurationTime = "DoesNotExpire";
    }
}
