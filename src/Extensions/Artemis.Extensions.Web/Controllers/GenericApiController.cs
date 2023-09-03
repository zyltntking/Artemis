﻿using Microsoft.AspNetCore.Mvc;

namespace Artemis.Extensions.Web.Controllers;

/// <summary>
/// 泛型Api控制器
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
public abstract class GenericApiController : ControllerBase
{
    
}