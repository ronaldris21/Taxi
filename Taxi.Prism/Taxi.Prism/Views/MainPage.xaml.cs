namespace Taxi.Prism.Views
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void RefreshView_Refreshing(object sender, System.EventArgs e)
        {
            //Whatever you want to do


            refresher.IsRefreshing = false;
        }
    }
}
