using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace fsp.lib.DependencyInjection.UseInjection;
public static class CustomHealthCheck
{
    public static WebApplication UserCustomHealthCheck(this WebApplication app)
    {
        app.UseHealthChecks("/health");

        return app;
    }
}
