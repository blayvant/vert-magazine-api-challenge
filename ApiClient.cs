using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using MagazineAPI;

namespace MagazineAPI {
    class ApiClient {
        private readonly HttpClient httpClient;
        private readonly string token;
        public ApiClient() {
            httpClient = new HttpClient {
                BaseAddress = new Uri("http://magazinestore.azurewebsites.net/api/")
            };
            token = GetTokenAsync().Result;
        }

        public async Task<List<string>> GetCategoriesAsync()
        {
            ApiResponseData<List<string>> data = await GetAsync<ApiResponseData<List<string>>>($"categories/{token}");
            return data.data;
        }

        public async Task<List<Magazine>> GetMagazinesWithCategoryAsync(string category)
        {
            ApiResponseData<List<Magazine>> data = await GetAsync<ApiResponseData<List<Magazine>>>($"magazines/{token}/{category}");
            return data.data;
        }

        public async Task<List<Subscriber>> GetSubscribersAsync()
        {
            ApiResponseData<List<Subscriber>> data = await GetAsync<ApiResponseData<List<Subscriber>>>($"subscribers/{token}");
            return data.data;
        }

        public async Task<PostResult> SubmitAnswerAsync(List<string> subscriberIds)
        {
            Dictionary<string, List<string>> requestData = new Dictionary<string, List<string>>();
            requestData["subscribers"] = subscriberIds;
            string json = JsonConvert.SerializeObject(requestData, Formatting.Indented);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            PostResultData data = await PostAsync<PostResultData>($"answer/{token}", content);
            return data.data;
        }

        private async Task<string> GetTokenAsync() {
            ApiResponseData data = await GetAsync<ApiResponseData>("token");
            return data.token;
        }
        private async Task<T> GetAsync<T>(string path) {
            string response = await httpClient.GetStringAsync(path);
            return JsonConvert.DeserializeObject<T>(response);
        }

        private async Task<T> PostAsync<T>(string path, HttpContent content) {
            HttpResponseMessage response = await httpClient.PostAsync(path, content);
            string result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        } 
    }
}