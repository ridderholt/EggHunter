# EggHunter

This project combines my love for tech and my sons love of treasure hunting. In order for my son to find his easter egg he needed to scan several QR codes where I had embedded a clue to where the next code was hidden.

## Technologies

This app was built using Blazor WASM and a ASP.NET API. In the Blazor app this wrapper for ZXing Barcode Scanner was used: [https://github.com/sabitertan/BlazorBarcodeScanner](https://github.com/sabitertan/BlazorBarcodeScanner)

When the QR scanner successfully scans a code it will send the text to the API which will use Microsoft Cognitive Service to synthesise the text to speech and save it as a .wav file. The path to the .wav file is then sent back to the Blazor app and will be used in an `audio` tag to play it to the user.

## If you want to run it

You will need an Azure account in order to setup Cognetive Service, documention: [https://learn.microsoft.com/en-us/azure/cognitive-services/speech-service/get-started-text-to-speech?tabs=windows%2Cterminal&pivots=programming-language-csharp](https://learn.microsoft.com/en-us/azure/cognitive-services/speech-service/get-started-text-to-speech?tabs=windows%2Cterminal&pivots=programming-language-csharp)

Get your Cognetive Services Key, Region and choose a voice and configure it in `appsettings.json`:

```json
{
  "Speech": {
    "Key": "",
    "Region": "",
    "Voice": ""
  }
}
```
