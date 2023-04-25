using System.Net.Http.Json;
using BlazorBarcodeScanner.ZXing.JS;
using EggHunter.Shared;

namespace EggHunter.Client.Pages;

public interface IAction { }

public interface IAsyncAction {}

public class IndexState
{
    private readonly HttpClient _client;

    public IndexState(HttpClient client)
    {
        _client = client;
    }

    public State State { get; set; } = new(false, "", new List<VideoInputDevice>(), "");

    public void Dispatch(IAction action)
    {
        State = action switch
        {
            StartLoading => State with { IsLoading = true },
            StopLoading => State with { IsLoading = false },
            DevicesFound df => State with { Devices = df.Devices },
            _ => throw new ArgumentException()
        };
    }

    public async Task DispatchAsync(IAsyncAction action)
    {
        State = action switch
        {
            CodeScanned cs => State with { AudioPath = await GetSpeech(cs.Text), CapturedText = cs.Text},
            _ => throw new ArgumentException()
        };
    }

    private async Task<string> GetSpeech(string text)
    {
        var result = await _client.GetFromJsonAsync<SpeechResult>($"speech/{text}");
        return result.Path;
    }
}

public record StartLoading() : IAction;

public record State(bool IsLoading, string CapturedText, IEnumerable<VideoInputDevice> Devices, string AudioPath);

public record StopLoading() : IAction;

public record DevicesFound(VideoInputDevice[] Devices) : IAction;

public record CodeScanned(string Text) : IAsyncAction;