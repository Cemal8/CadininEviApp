namespace CadininEvi
{
    public partial class SplashView : ContentPage
    {
        public SplashView()
        {
            InitializeComponent();
            Start();
        }

        private async void Start()
        {
            await Task.Delay(2000);
            Application.Current.MainPage = new MainPage();
        }
    }
}
