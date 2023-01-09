using RestSharp;

namespace fsp.lib.HttpClient;
public interface IHttpUtil
{
    public Task<RestResponse> Get(string URI);
    public Task<RestResponse> Get<T>(string URI,T requestBody) where T: class;
    public Task<string?> Post<T>(string URI, T requestBody) where T : class;
}
