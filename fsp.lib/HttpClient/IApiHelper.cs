using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fsp.lib.HttpClient;
public interface IApiHelper
{
    public Task<string?> GetApi(string URI, Dictionary<string, object> requestBody);
    public Task<string?> PostApi<T>(string URI, T requestBody) where T : class;
}
