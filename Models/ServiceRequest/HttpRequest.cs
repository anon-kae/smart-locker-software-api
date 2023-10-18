using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace smartlocker.software.api.Models.ServiceRequest
{
    public class HttpRequest
    {
        private readonly Dictionary<string, string> dictionary;
        public HttpRequest(Dictionary<string, string> dictionary)
        {
            this.dictionary = dictionary;
        }

        async void HTTP_PAYMENT()
        {
            var BASEURL = "http://en.wikipedia.org/";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASEURL);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", "");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = new FormUrlEncodedContent(dictionary);
                var response = await client.PostAsync("Test", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    // return responseString;
                }
            }
        }
    }
}