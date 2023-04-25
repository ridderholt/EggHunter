using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace EggHunter.Server.Controllers;

[ApiController]
[Route("speech")]
public class SpeechController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;
    private readonly SpeechSettings _speechSettings;

    public SpeechController(IWebHostEnvironment environment, SpeechSettings speechSettings)
    {
        _environment = environment;
        _speechSettings = speechSettings;
    }

    [HttpGet]
    [Route("{text}")]
    public async Task<IActionResult> GetAudio(string text)
    {
        var speechConfig = SpeechConfig.FromSubscription(_speechSettings.Key, _speechSettings.Region);
        speechConfig.SpeechSynthesisVoiceName = _speechSettings.Voice;

        var tempSoundWav = $"{Hash(text)}.wav";
        var fileName = Path.Combine(_environment.ContentRootPath, "Audio", tempSoundWav);

        if (System.IO.File.Exists(fileName))
        {
            return Ok(new
            {
                Text = text,
                Path = tempSoundWav
            });
        }

        using var audioConfig = AudioConfig.FromWavFileOutput(fileName);
        using var speechSyth = new SpeechSynthesizer(speechConfig, audioConfig);

        await speechSyth.SpeakTextAsync(text);

        return Ok(new
        {
            Text = text,
            Path = tempSoundWav
        });
    }

    private string Hash(string text)
    {
        var sb = new StringBuilder();
        using var md5 = MD5.Create();
        var hashValue = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
        foreach (var b in hashValue)
        {
            sb.Append($"{b:X2}");
        }

        return sb.ToString();
    }
}