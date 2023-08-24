using AbstractPaintingClassifier.Data;
using Emgu.CV;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AbstractPaintingClassifier
{
    public partial class MainPage : ContentPage
    {
        //private Predicter predicter;
        HttpClient client;

        public MainPage()
        {
            InitializeComponent();

            client = new HttpClient();
            client.DefaultRequestHeaders.ExpectContinue = false;

            //Task.Run(AnimateBackground);

        }

        private async void AnimateBackground()
        {
            Action<double> forward = input => bdGradient.AnchorY = input;
            Action<double> backward = input => bdGradient.AnchorY = input;

            while (true)
            {
                bdGradient.Animate(name: "forward", callback: forward, start: 0, end: 1, length: 5000, easing: Easing.SinIn);
                await Task.Delay(5000);
                bdGradient.Animate(name: "backward", callback: backward, start: 1, end: 0, length: 5000, easing: Easing.SinIn);
                await Task.Delay(5000);
            }
        }

        private async void Pick_Image(object sender, EventArgs e)
        {
            var photo = await MediaPicker.PickPhotoAsync();

            if (photo != null)
            {
                var stream = await photo.OpenReadAsync();
                this.PostImage(stream);
            }

        }

        async private void Take_Image(object sender, EventArgs e)
        {
            var photo = await MediaPicker.CapturePhotoAsync();

            if (photo != null)
            {
                var stream = await photo.OpenReadAsync();
                this.PostImage(stream);
            }
        }
        private async void PostImage(Stream stream)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(stream));

            resultImage.Source = ImageSource.FromStream(() => stream);

            using (var req = new HttpRequestMessage(HttpMethod.Post, "https://nnpxsh3r-7290.euw.devtunnels.ms/Classify"))
            {
                req.Content = content;
                var response = await client.SendAsync(req);
                if (response.IsSuccessStatusCode)
                {
                    style.Text = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    style.Text = "Bad request!";
                }
            }

        }
        private float[] CalculateDescriptors(string imagepath)
        {
            Mat image = new Mat(imagepath);

            Descriptor desc = new Descriptor();

            float[] descriptors = new float[3];

            descriptors[0] = desc.GetSIFT(image); 
            descriptors[1] = desc.GetORB(false, image); 
            descriptors[2] = desc.GetORB(true, image); 

            return descriptors;

        }
    }
}
