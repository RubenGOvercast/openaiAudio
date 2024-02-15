// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.22.0

using Azure.AI.OpenAI;
using Azure;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using System.Text.Json;
using System.Text;
using System.Linq;
using BotAzureOpenAI;


namespace BotAzureOpenAI.Bots
{
    public class EchoBot : ActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var replyText = $"Echo: {turnContext.Activity.Text}";
            string result = "";

            if (turnContext.Activity.Text == "audio")
            {
                await turnContext.SendActivityAsync(MessageFactory.Text("habla"), cancellationToken);
                var audio = new Audio();

                result = await audio.GenerateAudio();
            }
            else
            {
                await turnContext.SendActivityAsync(MessageFactory.Text("ya no hables"), cancellationToken);

                var httpClient = new GenerateResponseHttpClient();
                result = await httpClient.GenerateResponseAsync(replyText);
            }

            
            
            await turnContext.SendActivityAsync(MessageFactory.Text(result), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hola bienvenido al bot de Open Ai";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }

        

      
    }

    
}
