using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RestSharp;

namespace fsp.lib.HttpClient;
public class HttpUtil : IHttpUtil
{
    private readonly RestClient _client;
    public HttpUtil()
    {
        _client = new RestClient();
    }

    public async Task<RestResponse> Get(string URI) 
    {
        _client.Options.BaseUrl = new Uri(URI);
        var request = new RestRequest(string.Empty, Method.Get); 

        return await _client.ExecuteAsync(request);
    }
    public async Task<RestResponse> Get<T>(string URI, T requestBody) where T: class
    {
        _client.Options.BaseUrl = new Uri(URI);
        var request = new RestRequest(string.Empty, Method.Get);
        request.AddObject<T>(requestBody);

        return await _client.ExecuteAsync(request);        
    }

    public async Task<string?> Post<T>(string URI, T requestBody) where T: class
    {
        _client.Options.BaseUrl = new Uri(URI);
        var request = new RestRequest(string.Empty, Method.Post);      
        request.AddJsonBody<T>(requestBody);

        var response = await _client.ExecuteAsync(request);
        var content = response.Content;
        return content;
    }


    
}
