using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MicroServiceDemo
{
    public class WebApiHelper
    {
        public static string InvokeApi(string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Get;
                httpRequestMessage.RequestUri = new Uri(url);
                var result = httpClient.SendAsync(httpRequestMessage).Result;
                return result.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
