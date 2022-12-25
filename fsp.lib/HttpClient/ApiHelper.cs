using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RestSharp;

namespace fsp.lib.HttpClient;
public class ApiHelper : IApiHelper
{
    private readonly RestClient _client;
    public ApiHelper()
    {
        _client = new RestClient();
    }
    public async Task<string?> GetApi<T>(string URI, T requestBody) where T: class
    {
        _client.Options.BaseUrl = new Uri(URI);
        var request = new RestRequest(string.Empty, Method.Get);
        request.AddObject<T>(requestBody);

        var response = await _client.ExecuteAsync(request);
        var content = response.Content;
        return content;
    }

    public async Task<string?> PostApi<T>(string URI, T requestBody) where T: class
    {
        _client.Options.BaseUrl = new Uri(URI);
        var request = new RestRequest(string.Empty, Method.Post);      
        request.AddJsonBody<T>(requestBody);

        var response = await _client.ExecuteAsync(request);
        var content = response.Content;
        return content;
    }


    
}
