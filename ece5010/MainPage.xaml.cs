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
    async void OnInsertEmptyButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(InsertEmptyDetailPage));
    }
    async void OnSecureButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SecureDetailPage));
    }
    async void OnInvertColorButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(InvertColorDetailPage));
    }
    //This function is included in the code as a dummy call for an image button.
    //This is quite useful when adding new features/buttons to test the layout.
    void OnImageButtonClicked(object sender, EventArgs e)
    {
        return;
    }
}
