namespace ece5010;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    async void OnMergeImageButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(MergeDetailPage));
    }

    async void OnSplitImageButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SplitDetailPage));
    }
    async void OnInsertImageButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(InsertDetailPage));
    }
    void OnImageButtonClicked(object sender, EventArgs e)
    {
        return;
    }






}
