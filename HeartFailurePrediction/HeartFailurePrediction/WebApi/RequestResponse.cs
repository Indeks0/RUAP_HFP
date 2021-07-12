using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HeartFailurePrediction.WebApi
{

    public class RequestResponse
    {
        public async Task<string> InvokeRequestResponseService(Dictionary<string, StringTable> inputs)
        {
            using (var client = new HttpClient())
            {
                var scoreRequest = new
                {
                    inputs,
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };
                const string apiKey = "1uni4N+cEGkdvORuMI4ZlDmnFpbA0nJsfD/Q84xxBvUK3LcckNwGhW8TPxiVHYMBNrMb+GoO4HCzPy3R2aCpVA==";
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/4250a957d9084bb3ada5a3118d4ff88f/services/0dff4b623d77468a938f31fbf9e45aac/execute?api-version=2.0&details=true");

                HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return result;
                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
            }
        }
    }
}
