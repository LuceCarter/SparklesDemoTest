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
        private static HttpClient client = new HttpClient();

        public static async Task<String> SparkleSelfie(Stream image, CancellationToken cancellationToken)
        {
            BinaryReader binaryReader = new BinaryReader(image);
            byte[] fileBytes = binaryReader.ReadBytes((int)image.Length);          

            var imageBinaryContent = new ByteArrayContent(fileBytes);

            using (var response = await client.PostAsync(SelfieServiceUrl, imageBinaryContent, cancellationToken))
            {
                response.EnsureSuccessStatusCode();

                var url = await response.Content.ReadAsStringAsync();
                var isReady = await WaitForSuccess(url,30000);
                return url;
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
                   var type = resp.Content.Headers.ContentType.ToString();


                    isSuccess = type != "{application/json; charset=utf-8}";
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
