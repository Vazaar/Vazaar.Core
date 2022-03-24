// ---------------------------------------------------------------
// Copyright (c) Yasir Thite All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Vazaar.Core.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController, Authorize(Policy = "AccessPermission")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get() =>
            Ok("Hello, Mario. The princesse is in another castle.");
    }
}
