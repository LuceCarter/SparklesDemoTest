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
        private static string SelfieServiceUrl = "https://supersparkly.azurewebsites.net/api/Upload?code=I8ZpTz91GHqru77rIVsx9eMp/XhJU0usKIHqT0Wc7jFx28mPAvpNmg==";
        private static HttpClient client = new HttpClient();
        private static string downloadURL;
        private static string sparkleGifUrl;

        public static async Task<String> SparkleSelfie(Stream image, CancellationToken cancellationToken)
        {
            BinaryReader binaryReader = new BinaryReader(image);
            byte[] fileBytes = binaryReader.ReadBytes((int)image.Length);          

            var imageBinaryContent = new ByteArrayContent(fileBytes);

            using (var response = await client.PostAsync(SelfieServiceUrl, imageBinaryContent, cancellationToken))
            {
                response.EnsureSuccessStatusCode();

                downloadURL = await response.Content.ReadAsStringAsync();                
                var isReady = await WaitForSuccess(downloadURL,30000);                
            }

            using (var response = await client.GetAsync(downloadURL))
            {
                response.EnsureSuccessStatusCode();

                sparkleGifUrl = await response.Content.ReadAsStringAsync();

                return sparkleGifUrl;
            }

            
        }

        public static async Task<bool> WaitForSuccess(string url, int timeout)
        {
            bool shouldContinue = true;
            var successTask = Task.Run(async ()=>{
                var isSuccess = false;
                while(!isSuccess)
                {
                    if(!shouldContinue)
                        break;
                    var resp = await client.GetAsync(url);
                    // isSuccess = resp.IsSuccessStatusCode;

                   var respString = await resp.Content.ReadAsStringAsync();

                    if (respString.Equals("102")) { isSuccess = false; }
                    else {                       
                        isSuccess = true;                  
                    
                    }

                    
                    if(!isSuccess)
                        await Task.Delay(1000);
                    else
                         return true;
                }
                return isSuccess;
            });
            var result = await Task.WhenAny(Task.Delay(timeout)); 
            shouldContinue = false;
            if(result == successTask)
            {
                return successTask.Result;
            }
            return false;
        }       
    }
}
