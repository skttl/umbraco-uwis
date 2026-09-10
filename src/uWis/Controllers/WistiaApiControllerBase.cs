using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Api.Common.Attributes;
using Umbraco.Cms.Web.Common.Authorization;
using Umbraco.Cms.Web.Common.Routing;

namespace uWis.Controllers;

[ApiController]
[BackOfficeRoute("uwis/api/v{version:apiVersion}")]
[Authorize(Policy = AuthorizationPolicies.SectionAccessContent)]
[MapToApi(Constants.Swagger.ApiName)]
public class WistiaApiControllerBase : ControllerBase { }
