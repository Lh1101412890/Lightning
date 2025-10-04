using System.Net.Http;

namespace Lightning.Extension
{
    public static class HttpClientExtension
    {
        public static string GetRequest(this HttpClient httpClient, string uri, string cookie = "")
        {
            // 构造请求  
            var request = new HttpRequestMessage(HttpMethod.Get, uri);

            if (cookie != "")
            {
                // 添加Cookie到请求头  
                request.Headers.Add("Cookie", cookie);
            }
            // 发送请求  
            var response = httpClient.SendAsync(request).Result;

            // 输出响应结果  
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
