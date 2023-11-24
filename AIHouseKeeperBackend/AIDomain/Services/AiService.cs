using System.Text;
using AIHouseKeeper.Models.DbEntities;
using AIHouseKeeperBackend.AIDomain.Constants;
using AIHouseKeeperBackend.AIDomain.ViewModels;
using AIHouseKeeperBackend.Configs;
using AIHouseKeeperBackend.Database;
using AIHouseKeeperBackend.DependencyInjections;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AIHouseKeeperBackend.AIDomain.Services;

public interface IAiService
{
    Task<string> DetectPromptTypeAsync(PromptRequestViewModel viewModel);

    Task<MessageContent> GetAnswerFromPromptAsync(PromptRequestViewModel viewModel);

    Task StoreMemoryAsync(PromptRequestViewModel viewModel);

    Task<List<string>> GetMemoryList(long userId);
}

public class AiService : IAiService, IScopedService
{
    private readonly AppDbContext _appDbContext;
    private readonly IAiConfig _aiConfig;
    private readonly HttpClient _httpClient;

    public AiService(AppDbContext appDbContext, IAiConfig aiConfig)
    {
        _appDbContext = appDbContext;
        _aiConfig = aiConfig;
        _httpClient = new HttpClient();
    }

    public async Task<string> DetectPromptTypeAsync(PromptRequestViewModel viewModel)
    {
        var message = new Message
        {
            Messages = new List<MessageContent>
            {
                new()
                {
                    Role = Role.Assistant,
                    Content = PromptBackground.Ask,
                },
                new()
                {
                    Role = Role.User,
                    Content = viewModel.Content,
                }
            }
        };

        var respond = await GetGptRespond(message);

        return respond.Choices[0].Message.Content.ToLower().Contains(PromptType.Statement) 
            ? 
            PromptType.Statement 
            : 
            PromptType.Question;
    }

    public async Task<MessageContent> GetAnswerFromPromptAsync(PromptRequestViewModel viewModel)
    {
        var memory = await _appDbContext.Memories.FirstOrDefaultAsync(x=>x.UserId == viewModel.UserId);
        if (memory == null)
        {
            throw new InvalidOperationException("no memory");
        }
        var memoryStr = "";
        for (var i = 0; i < memory.InformationList.Count; i++)
        {
            var info = memory.InformationList[i];
            memoryStr += $"{i}. {info}";
        }

        var message = new Message
        {
            Messages = new List<MessageContent>
            {
                new()
                {
                    Role = Role.Assistant,
                    Content = PromptBackground.Answer + memoryStr,
                },
                new()
                {
                    Role = Role.User,
                    Content = viewModel.Content,
                }
            }
        };

        var respond = await GetGptRespond(message);

        return respond.Choices[0].Message;
    }

    public async Task StoreMemoryAsync(PromptRequestViewModel viewModel)
    {
        var existingMemory = await _appDbContext.Memories.FirstOrDefaultAsync(x => x.UserId == viewModel.UserId);
        if (existingMemory == null)
        {
            var newMemory = new Memory
            {
                UserId = viewModel.UserId,
                InformationList = new List<string>{ viewModel.Content }
            };

            await _appDbContext.Memories.AddAsync(newMemory);
            await _appDbContext.SaveChangesAsync();
            
            return;
        }
        
        existingMemory.InformationList.Add(viewModel.Content);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<List<string>> GetMemoryList(long userId)
    {
        var res = (await _appDbContext.Memories.FirstOrDefaultAsync(x => x.UserId == userId))!.InformationList;
        return res;
    }

    private async Task<Respond> GetGptRespond(Message message)
    {
        var settings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };
        
        var httpRequestMsg = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_aiConfig.Url),
            Content = new StringContent(
                JsonConvert.SerializeObject(message, settings),
                Encoding.UTF8,
                "application/json")
        };
        
        httpRequestMsg.Headers.Add("api-key", _aiConfig.Key);

        var responseContent = await _httpClient.SendAsync(httpRequestMsg);

        if (!responseContent.IsSuccessStatusCode)
        {
            throw new Exception(responseContent.Content.ToString());
        }

        var respond = JsonConvert.DeserializeObject<Respond>(await responseContent.Content.ReadAsStringAsync());

        return respond!;
    }
}

