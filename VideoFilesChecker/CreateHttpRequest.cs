using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VideoFilesChecker
{
    public static class CreateHttpRequest
    {
        private static HttpClient client = new HttpClient();

        public static async Task CreatePOSTRequest(Videos postData)
        {
            string postResponse = await PostData(postData);

            Console.WriteLine(postResponse);
        }

        public static async Task CreateGETRequest()
        {
            string getResponse = await GetJsonData();

            Console.WriteLine(getResponse);
        }

        public static async Task<string>PostData(Videos dataToPost)
        {
            var data = new JavaScriptSerializer().Serialize(dataToPost);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "3dad037f-e4a2-49ad-abf8-f80ec281d98b");

            var response = await client.PostAsync(
                "http://post.local/test.php",
                new StringContent(data, Encoding.UTF8, "application/json") 
                );

            var responseString = await response.Content.ReadAsStringAsync();

            response.Dispose();

            return responseString;
  
        }

        private static async Task<string>GetJsonData()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "3dad037f-e4a2-49ad-abf8-f80ec281d98b");

            var data = await client.GetAsync(
                "http://post.local/video-json.php"
                );

            data.EnsureSuccessStatusCode();

            var responseBody = await data.Content.ReadAsStringAsync();

            var jsonData = JsonConvert.DeserializeObject<Videos>(responseBody);

            data.Dispose();

            return responseBody;
        }
    }
}
