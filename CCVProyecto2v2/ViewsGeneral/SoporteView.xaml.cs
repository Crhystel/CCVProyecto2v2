namespace CCVProyecto2v2.ViewsGeneral;

public partial class SoporteView : ContentPage
{
    private async void OnWhatsAppTapped(object sender, EventArgs e)
    {
        string telefono = "593969716019";
        string mensaje = "Hola, necesito ayuda con mi cuenta.";
        string url = $"https://wa.me/{telefono}?text={Uri.EscapeDataString(mensaje)}";

        try
        {
            await Browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo abrir WhatsApp: {ex.Message}", "OK");
        }
    }
    private async void OnInstagramTapped(object sender, EventArgs e)
    {
        string instagramUrl = "https://instagram.com/crhysvelasco";
        try
        {
            await Browser.OpenAsync(instagramUrl, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo abrir Instagram: {ex.Message}", "OK");
        }
    }
}