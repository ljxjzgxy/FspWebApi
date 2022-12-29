using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fsp.lib;

public class ApiResult<T> : ApiResultBase where T : class
{
    public T? Data { get; set; }
}

public class ApiResultBase
{
    public int Code { get; set; }
    public bool? Success { get; set; }
    public string? Message { get; set; }
}

