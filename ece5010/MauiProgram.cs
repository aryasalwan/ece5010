using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
namespace ece5010
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<IFileSaver, FileSaverImplementation>();
            builder.Services.AddTransient<MergeDetailPage>();
            builder.Services.AddTransient<SplitDetailPage>();
            builder.Services.AddTransient<InsertDetailPage>();
            builder.Services.AddTransient<InsertEmptyDetailPage>();
            builder.Services.AddTransient<SecureDetailPage>();
            builder.Services.AddTransient<InvertColorDetailPage>();
            return builder.Build();
        }
    }
}
