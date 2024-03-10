namespace ece5010;
public partial class MainPage : ContentPage
{
    int clickTotal;

    public MainPage()
    {
        InitializeComponent();

    }

    async void OnMergeImageButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(MergeDetailPage));
    }
    void OnImageButtonClicked(object sender, EventArgs e)
    {
        return;
    }
    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height); // must be called
       
        var aspectRatio = 0.58; // Your image's aspect ratio (width / height)

        // If your grid's columns are equally divided, and since you have 3 columns:
        var gridWidth = width / 3; // Divide the total width by the number of columns

        var calculatedHeight = gridWidth * aspectRatio;

        // Now, set this calculatedHeight as the height of your ImageButtons
        // Ensure your ImageButtons are properly named and referenced, e.g., ImageButton1
        ImageButton0.HeightRequest = calculatedHeight;
        ImageButton1.HeightRequest = calculatedHeight;
        ImageButton2.HeightRequest = calculatedHeight;
        // Repeat for other ImageButtons as necessary

        double baseFontSize = 80; // Base font size
        double widthFactor = width / 360; // Assuming 360 is the base width you're designing for
        double heightFactor = height / 640; // Assuming 640 is the base height you're designing for
        double scaleFactor = Math.Min(widthFactor, heightFactor);

        // Adjust font size based on scaleFactor
        var adjustedFontSize = baseFontSize * scaleFactor;

        // Apply adjusted font size to your labels
        // You can access your labels if they are defined as class members
        // For example, if you have a Label with x:Name="myLabel" in XAML, ensure it's accessible here
        MyLabel1.FontSize = adjustedFontSize;
        MyLabel2.FontSize = adjustedFontSize;


    }


}