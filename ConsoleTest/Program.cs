using Accord;
using Accord.Video.FFMPEG;
using DeviceApi;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTest
{
    class MyCommunicationProtocol : ACommunicationProtocol
    {

        protected override ItemAddResult OnAddItem(Item item, int quantity)
        {
            return new ItemAddResult { result = 15, totalPrice = item.price * quantity };
        }

        protected override void OnVideoRequested(string cameraName)
        {
            string filePath = "./sample.mp4";
            using (var vFReader = new VideoFileReader())
            {
                vFReader.Open(filePath);
                for (int i = 0; i < vFReader.FrameCount; i++)
                {
                    Bitmap bmpBaseOriginal = vFReader.ReadVideoFrame(i);
                    SendBitmap(bmpBaseOriginal);
                }
                vFReader.Close();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        static async Task MainAsync(string[] args)
        {
            var communication = new MyCommunicationProtocol();
            var xmlPublicKey = "<RSAKeyValue><Modulus>l37+bbDzBGYFxEKRfBcJENV4xPP0i81LRYfJH62RrOCadt8L3kf6ANtQd1ZipzpFyWvQCt9g0VjMNsELZVAlxReWr+NeS1KruBwT2yiE6umW2AkmXv9uE/JeoZaOsgodAPtBx/WgfgHn1YLwZQOqU7O4gajpTq1GwTTYq1F96k3KNkBUdFsWRM6BonvOayPNkZIYQMySS7Sn0kkvWNqONxzEN/PGZvkGITDEoEtuMR+90XCDQJM7HiFFYXVyrihOu7uURKlA74K4eA/I6bCHkD5NDtK4SU7Aem1GzsXzM/1gUvFJ5wY9etyPSIuMIz/micOho71duPRSNB6rfyMRpQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            await DeviceApiWebHost.RunWebHostBuilder(args, xmlPublicKey, communication);
        }
    }
}
