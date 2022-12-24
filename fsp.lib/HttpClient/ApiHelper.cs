using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RestSharp;

namespace fsp.lib.HttpClient;
public class ApiHelper : IApiHelper
{
    public async Task<string?> GetApi(string URI, Dictionary<string,object> requestBody)
    {
        var client = new RestClient(URI);
        var request = new RestRequest(string.Empty, Method.Get);         
        foreach(var kv in requestBody)
        {
            request.AddParameter(kv.Key,kv.Value,ParameterType.QueryString);
        }       

        var response = await client.ExecuteAsync(request);
        var content = response.Content;
        return content;
    }

    public async Task<string?> PostApi(string URI, object requestBody)
    {
        var client = new RestClient(URI);
        var request = new RestRequest(string.Empty, Method.Post);
        request.AddHeader("Content-Type", "application/json");
        request.AddParameter("application/json", requestBody,ParameterType.RequestBody);

        var response = await client.ExecuteAsync(request);
        var content = response.Content;
        return content;
    }


    
}
