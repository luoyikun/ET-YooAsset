using System;
using System.IO;
using System.Net.Http;
using Cysharp.Threading.Tasks;

namespace ET.Client
{
    public static class HttpClientHelper
    {
        public static async UniTask<string> Get(string link)
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                HttpResponseMessage response =  await httpClient.GetAsync(link);
                string result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception($"http request fail: {link.Substring(0,link.IndexOf('?'))}\n{e}");
            }
        }
    }
}