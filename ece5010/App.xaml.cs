namespace ece5010;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);

       const int newWidth = 750;
        const int newHeight = 600;

        window.Width = newWidth;
        window.Height = newHeight;
        window.MinimumHeight = 600;
        window.MinimumWidth = 750;
        window.MaximumHeight = 600;
        window.MaximumWidth = 750;

        return window;
    }
}