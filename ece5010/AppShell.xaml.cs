namespace ece5010
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MergeDetailPage),typeof(MergeDetailPage));
            Routing.RegisterRoute(nameof(SplitDetailPage), typeof(SplitDetailPage));
            Routing.RegisterRoute(nameof(InsertDetailPage), typeof(InsertDetailPage));
            Routing.RegisterRoute(nameof(InsertEmptyDetailPage), typeof(InsertEmptyDetailPage));
            Routing.RegisterRoute(nameof(SecureDetailPage), typeof(SecureDetailPage));
            Routing.RegisterRoute(nameof(InvertColorDetailPage), typeof(InvertColorDetailPage));
        }
    }
}
