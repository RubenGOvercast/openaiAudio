using Azure.AI.OpenAI;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BotAzureOpenAI
{
    internal class Audio
    {      
        public async Task<string> GenerateAudio()
        {
            try
            {
                var speechConfig = SpeechConfig.FromEndpoint(new Uri("https://eastus.api.cognitive.microsoft.com/"), "f8fed4aa60a74b37bf22ae0de3f8536e");

                speechConfig.SpeechRecognitionLanguage = "es-MX";
                using var audioConfi = AudioConfig.FromDefaultMicrophoneInput();
                using var recognizer = new SpeechRecognizer(speechConfig, audioConfi);
                var result = await recognizer.RecognizeOnceAsync();
                return result.ToString();

            }
            catch (Exception ex)
            {
                
                throw new Exception($"Error en la solicitud HTTP: {ex.Message}");
            }
        }
    }
}
