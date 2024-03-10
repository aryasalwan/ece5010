namespace ece5010
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MergeDetailPage),typeof(MergeDetailPage));
        }
    }
}
