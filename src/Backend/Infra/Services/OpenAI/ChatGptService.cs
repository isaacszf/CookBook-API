using Domain.Dtos;
using Domain.Enums;
using Domain.Services.OpenAI;
using OpenAI_API;
using OpenAI_API.Chat;

namespace Infra.Services.OpenAI;

public class ChatGptService : IGenerateRecipeAi
{
    private const string ChatModel = "gpt-3.5-turbo";
    
    private readonly IOpenAIAPI _openAiApi;
    
    public ChatGptService(IOpenAIAPI openAiApi)
    {
        _openAiApi = openAiApi;
    }
    
    public async Task<GeneratedRecipeDto> Generate(IList<string> ingredients)
    {
        string response;
        try
        {
            var conversation = _openAiApi.Chat.CreateConversation(new ChatRequest { Model = ChatModel });

            conversation.AppendSystemMessage(ResourcesOpenAiMessages.INPUT_TEXT);
            conversation.AppendUserInput(string.Join(";", ingredients));

            response = await conversation.GetResponseFromChatbotAsync();
        }
        catch (HttpRequestException)
        {
            // default if call fails
            response = "Macarrão à Carbonara com Tomate\n2\n2 tomates;1 ovo;1 cebola;lkg macarrão;panceta;queijo parmesão;sal;pimenta preta;azeite\nCozinhe o macarrão conforme as instruções da embalagem refogue a cebola picada e os tomates em azeite até ficarem macios adicione a panceta e cozinhe até dourar bata o ovo com queijo parmesão ralado escorra o macarrão e misture com a panceta e os tomates @ retire do fogo e misture rapidamente com a mistura de ovo e queijo tempere com sal e pimenta preta a gosto @sirva quente.";
        }

        var step = 1;
        var parts = response
            .Split("\n")
            .Where(part => !string.IsNullOrWhiteSpace(part))
            .Select(part => part.Replace("[", "").Replace("]", ""))
            .ToList();

        return new GeneratedRecipeDto()
        {
            Title = parts[0],
            CookingTime = (CookingTimeEnum) Enum.Parse(typeof(CookingTimeEnum), parts[1]),
            Ingredients = parts[2].Split(';'),
            Instructions = parts[3]
                .Split('@')
                .Select(instruction => new GeneratedInstructionDto
                {
                    Step = step++,
                    Description = instruction.Trim()
                })
                .ToList()
        };
    }
}