using Microsoft.Extensions.Logging;
using ece5010.ViewModel;
namespace ece5010
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddTransient<MergeDetailPage>();
            builder.Services.AddTransient<SplitDetailPage>();
            builder.Services.AddTransient<MergeDetailViewModel>();

            return builder.Build();
        }
    }
}
