﻿namespace ece5010;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }
    /*
    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);

        const double aspectRatio = 20/7;
        //window.MinimumHeight = 450;
       // window.MinimumWidth = 850;

        // Handle the SizeChanged event to maintain aspect ratio
        window.SizeChanged += (sender, args) =>
        {
            // Calculate new width and height with the aspect ratio
            var newWidth = window.Height * aspectRatio;
            var newHeight = window.Width / aspectRatio;

            // Determine which dimension to adjust based on the window's current aspect ratio
            if (window.Width / window.Height > aspectRatio)
            {
                // If the window is too wide, adjust the width
                window.Width = newWidth;
            }
            else
            {
                // If the window is too tall, adjust the height
                window.Height = newHeight;
            }
        };

        return window;
    }
    */
}
