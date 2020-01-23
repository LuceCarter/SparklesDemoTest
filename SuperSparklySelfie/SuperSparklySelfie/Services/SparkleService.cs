using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace SuperSparklySelfie.Services
{
    public static class SparkleService
    {
        private static string SelfieServiceUrl = "https://t.co/sPT0Hic6H7?amp=1";
      
        public static async Task<Stream> SparkleSelfie(Stream image, CancellationToken cancellationToken)
        {

            byte[] fileBytes;
            BinaryReader binaryReader = new BinaryReader(image);
            fileBytes = binaryReader.ReadBytes((int) image.Length);

            using (var client = new HttpClient())
            {
                var apiUrl = new Uri(SelfieServiceUrl);

                var imageBinaryContent = new ByteArrayContent(fileBytes);

                using (var response = await client.PostAsync(SelfieServiceUrl, imageBinaryContent, cancellationToken))
                {
                    response.EnsureSuccessStatusCode();


                    // Eventually when the http side is working, need to check the response is not 102 (the code for processing)
                    // Then find URL in response and that is the location of the image with sparkles applied
                    return await response.Content.ReadAsStreamAsync();
                }
            }
        }
        
        private static HttpContent CreateHttpContent(Stream content)
        {
            HttpContent httpContent = null;

            if (content != null)
            {
                byte[] fileBytes;
                var binararyReader = new BinaryReader(content);
                fileBytes = binararyReader.ReadBytes((int) content.Length);

                var imageBinaryContent = new ByteArrayContent(fileBytes);
            }

            return httpContent;
        }
    }
}
