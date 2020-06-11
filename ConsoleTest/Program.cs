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

        /*
         *  Is called by the dll when an authenticated user joins
         */
        protected override void OnUserConnected(string userId)
        {
            Console.WriteLine($"user {userId} connected");
            // You can use this method to share a video url with connected users
            // The url can include a random suffix for more security
            // This is a open mp4 sample link 
            SendVideoUrl("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4");
        }

        protected override void OnUserDiconnected(string userId)
        {
            Console.WriteLine($"user {userId} disconnected");
            // Remoke the url you served to the user {userId}
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
