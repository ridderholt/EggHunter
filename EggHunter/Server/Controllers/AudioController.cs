using Microsoft.AspNetCore.Mvc;

namespace EggHunter.Server.Controllers;

[ApiController]
[Route("audio")]
public class AudioController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;

    public AudioController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    [HttpGet]
    [Route("{file}")]
    public IActionResult GetAudio(string file)
    {
        var fs = System.IO.File.OpenRead(Path.Combine(_environment.ContentRootPath, "Audio", file));
        return File(fs, "audio/wav");
    }
}