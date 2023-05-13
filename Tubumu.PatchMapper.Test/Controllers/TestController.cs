using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tubumu.PatchMapper.Test.Models;

namespace Tubumu.PatchMapper.Test.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TestController(ILogger<TestController> logger, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public ActionResult Get()
    {
        return Ok();
    }

    [HttpPatch]
    [Route("patch")]
    public ActionResult Patch([FromForm] PersonInput input)
    {
        // 1. 目前仅支持 `FromForm`，即 `x-www-form_urlencoded` 和 `form-data`；暂不支持 `FromBody` 如 `raw` 等。
        // 2. 使用 ModelBinderFractory 创建 ModelBinder 而不是 ModelBinderProvider 以便于未来支持更多的输入格式。
        // 3. 目前还没有支持多级结构。
        // 4. 测试代码暂时将 AutoMapper 配置放在方法内。

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<PersonInput, PersonEntity>().ConvertUsing(new PatchConverter<PersonEntity>());
        });
        var mapper = config.CreateMapper();

        // PersionEntity 有 3 属性，客户端如果提供 0 或 2 个参数，在 Map 时未提供参数的属性值不会被改变。
        var entity = new PersonEntity
        {
            Name = "姓名",
            Age = 18,
            Gender = "如果客户端没有提供本参数，那我的值不会被改变"
        };
        mapper.Map(input, entity);

        return Ok();
    }
}
