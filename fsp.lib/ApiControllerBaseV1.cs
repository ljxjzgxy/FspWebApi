using fsp.lib.Swagger;

namespace fsp.lib;
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
[ApiExplorerSettings(GroupName = nameof(ApiVersions.v1))]
public class ApiControllerBaseV1 : ControllerBase
{
}