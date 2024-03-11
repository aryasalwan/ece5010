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

    void OnImageButtonClicked(object sender, EventArgs e)
    {
        return;
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height); // This must be called.

        // Adjust ImageButtons' size
        AdjustImageButtonsSize(width);

        // Adjust font size of labels
    }

    private void AdjustImageButtonsSize(double width)
    {
        var aspectRatio = 0.58; // Replace with your actual image aspect ratio.

        // Assuming your layout is divided into equal columns, and you have 3 ImageButtons:
        var gridWidth = width / 3; // Dividing the total width by the number of columns.
        var calculatedHeight = gridWidth * aspectRatio;

        // Adjusting the height of each ImageButton to maintain aspect ratio.
        // Ensure your ImageButtons are properly named in your XAML and code-behind.
        ImageButton0.HeightRequest = calculatedHeight;
        ImageButton1.HeightRequest = calculatedHeight;
        ImageButton2.HeightRequest = calculatedHeight;
        // Add more if necessary.
    }



}
