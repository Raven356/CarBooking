namespace CarBookingUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override void OnSleep()
        {
            SecureStorage.RemoveAll();

            base.OnSleep();
        }
    }
}
