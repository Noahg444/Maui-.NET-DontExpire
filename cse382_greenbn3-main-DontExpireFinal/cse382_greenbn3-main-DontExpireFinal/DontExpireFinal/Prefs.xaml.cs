namespace DontExpireFinal;

public partial class Prefs : ContentPage
{
    public Prefs()
    {
        InitializeComponent();
        appearanceSwitch.IsToggled = Preferences.Get("AppearanceMode", false) ;

    }

    private void appearanceSwitch_Toggled(System.Object sender, Microsoft.Maui.Controls.ToggledEventArgs e)
    {
        Application.Current.UserAppTheme = e.Value ? AppTheme.Dark : AppTheme.Light;
        Preferences.Set("AppearanceMode", e.Value);
    }
    /*
    private void theDatePicker_DateSelected(System.Object sender, Microsoft.Maui.Controls.DateChangedEventArgs e)
    {
        
        DatePicker date = sender as DatePicker;

        Preferences.Set("theDatePicker", date.Date);
    }
    */
}
