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
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, SelfieServiceUrl))
                using(var httpContent = CreateHttpContent(image))
                {
                    request.Content = httpContent;

                    using (var response = await client
                        .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                        .ConfigureAwait(false))
                    {
                        response.EnsureSuccessStatusCode();

                        var photoUrl = response.Content.ReadAsStringAsync();

                        return await response.Content.ReadAsStreamAsync();
                    }
            }
        }
        
        private static HttpContent CreateHttpContent(Stream content)
        {
            HttpContent httpContent = null;

            if (content != null)
            {
                var ms = new MemoryStream();
                ms.Seek(0, SeekOrigin.Begin);
                httpContent = new StreamContent(ms);
            }

            return httpContent;
        }
    }
}
