using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;
using Plugin.Media;
using Plugin.Media.Abstractions;
using SuperSparklySelfie.Services;
using Xamarin.Forms;

namespace SuperSparklySelfie.ViewModels
{
    public class TakeSelfiePageViewModel : BaseViewModel
    {
        private MediaFile selfie;
        private Uri imageSource;
        public ICommand TakePhotoCommand { get; }

        public TakeSelfiePageViewModel()
        {
            TakePhotoCommand = new Command(async () => await TakePhotoAsync());
        }

        public Uri ImageForSparkles
        {
            get => imageSource;
            set => SetProperty(ref imageSource, value);
        }

        private async Task TakePhotoAsync()
        {
            await CrossMedia.Current.Initialize();

            try
            {
                if (CrossMedia.Current.IsCameraAvailable)
                {
                    selfie = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        PhotoSize = PhotoSize.Small
                    });

                    var imageUrl = await SparkleService.SparkleSelfie(selfie.GetStream(), new CancellationToken());


                    ImageForSparkles = new Uri(imageUrl);

                }
            }
            catch (Exception e)
            {
                
            }
        }
    }
}
