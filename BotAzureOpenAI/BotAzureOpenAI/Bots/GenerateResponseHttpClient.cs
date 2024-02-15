using Azure.AI.OpenAI;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BotAzureOpenAI
{
    internal class GenerateResponseHttpClient
    {
        private readonly HttpClient _httpClient;

        public GenerateResponseHttpClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GenerateResponseAsync(string userMessage)
        {
            try
            {
                Configuration.OpenAIEndpoint = "https://openai-service-prueba.openai.azure.com/";
                Configuration.OpenAIImplementationName = "prueba1";
                Configuration.OpenAISecret = "41815dd1b65e4b2bba588480b77ef220";

                string chatGptUrl = $"{Configuration.OpenAIEndpoint}openai/deployments/{Configuration.OpenAIImplementationName}/chat/completions?api-version=2023-03-15-preview";

                // Construir el cuerpo de la solicitud
                var requestBody = new
                {
                    messages = new[]
                    {
                        new
                        {
                            role = "user",
                            content = userMessage
                        }
                    }
                };

                var jsonBody = JsonSerializer.Serialize(requestBody);
                var request = new HttpRequestMessage(HttpMethod.Post, chatGptUrl);

                request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                // Agregar el encabezado de autenticación
                request.Headers.Add("api-key", Configuration.OpenAISecret);

                // Enviar la solicitud HTTP y recibir la respuesta
                var response = await _httpClient.SendAsync(request);

                // Leer y devolver el contenido de la respuesta si es exitosa
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Deserializar el JSON
                    var responseObject = JsonSerializer.Deserialize<OpenAIResponse>(responseContent);

                    // Acceder al contenido del primer mensaje
                    return responseObject.choices[0].message.content;                    
                }
                else
                {
                    // Manejar el error si la solicitud no es exitosa
                    throw new Exception($"Error al enviar la solicitud: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que se pueda producir
                throw new Exception($"Error en la solicitud HTTP: {ex.Message}");
            }
        }

        public class OpenAIResponse
        {
            public string id { get; set; }
            public string @object { get; set; }
            public int created { get; set; }
            public Choice[] choices { get; set; }
        }

        public class Message
        {
            public string role { get; set; }
            public string content { get; set; }
        }

        public class Choice
        {
            public string finish_reason { get; set; }
            public Message message { get; set; }
        }


    }
}
