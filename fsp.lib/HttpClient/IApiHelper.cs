using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fsp.lib.HttpClient;
public interface IApiHelper
{
    public Task<string?> GetApi<T>(string URI,T requestBody) where T: class;
    public Task<string?> PostApi<T>(string URI, T requestBody) where T : class;
}
