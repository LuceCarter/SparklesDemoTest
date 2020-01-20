using System;
using SuperSparklySelfie.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SuperSparklySelfie
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new TakeSelfiePage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
