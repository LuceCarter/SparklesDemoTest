using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SuperSparklySelfie.Services
{
    public class SparkleService
    {
        private string SelfieServiceUrl = "https://t.co/sPT0Hic6H7?amp=1";

        public async Task SendSelfie(object content, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, SelfieServiceUrl))
            {
            }
        }
    }
}
