using System.Text.RegularExpressions;
using Discord;
using Discord.WebSocket;
using EngineerNotebook.Core.Interfaces;
using EngineerNotebook.Shared.Models;
using TagType = EngineerNotebook.Shared.Models.TagType;

namespace EngineerNotebook.Bot;

public class Bot : BackgroundService
{
    private readonly DiscordSocketClient _client;
    private readonly ILogger<Bot> _logger;
    private readonly IConfiguration _config;
    private readonly IAsyncRepository<Tag> _tagRepo;
    private readonly IGuideService _guideService;
    
    private List<Tag> cachedTags;
    private Regex regex;

    private readonly IServiceScope _scope;
    private CancellationTokenSource tokenSource = new();
    
    public override void Dispose()
    {
        base.Dispose();
        _scope?.Dispose();
    }

    public Bot(DiscordSocketClient client, IServiceProvider serviceProvider,
        IConfiguration config, 
        ILogger<Bot> logger)
    {
        _client = client;
        _logger = logger;

        _scope = serviceProvider.CreateScope();
        _tagRepo = _scope.ServiceProvider.GetRequiredService<IAsyncRepository<Tag>>();
        _guideService = _scope.ServiceProvider.GetRequiredService<IGuideService>();
        
        _config = config;
        
        _client.Log += ClientOnLog;
        _client.MessageReceived += ClientOnMessageReceived;

        Task.Run(async () =>
        {
            await RefreshInterval();
        });
    }
    
    private async Task ClientOnMessageReceived(SocketMessage arg)
    {
        if (arg.Author.IsBot)
            return;
        
        List<Tag> tags = new();

        try
        {
            tags = await GetTagsFromDiscordMessage(arg.Content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return;
        }

        if (!tags.Any())
            return;

        byte[] guide;

        try
        {
            guide = await _guideService.GetGuide(tags);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return;
        }

        if (guide is null)
            return;

        using var stream = new MemoryStream(guide);
        
        string filename = $"{arg.Author.Username}_guide.pdf";
        var embed = new EmbedBuilder
        {
            ImageUrl = $"attachment://{filename}",
            Author = new EmbedAuthorBuilder
            {
                Name = "Engineering Bot"
            },
            Description = $"Hey, {arg.Author.Username}, found something that might help you out!",
            Title = "Guide"
        };

        try
        {
            await arg.Channel.SendFileAsync(stream, filename, embed: embed.Build());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    Task<List<Tag>> GetTagsFromDiscordMessage(string message)
    {
        if (string.IsNullOrEmpty(message))
            return Task.FromResult<List<Tag>>(new());

        var matches = regex.Matches(message.ToLower());
        var tags = matches
            .SelectMany(x => x.Groups
                .Values
                .SelectMany(y => y.Value
                    .Split(" ")))
            .Distinct();

        return Task.FromResult(cachedTags.Where(x => tags.Contains(x.Name.ToLower())).ToList());
    }

    private async Task RefreshInterval()
    {
        TimeSpan span = TimeSpan.FromMinutes(1);
        
        while (!tokenSource.Token.IsCancellationRequested)
        {
            cachedTags = (await _tagRepo.Get()).ToList();

            string all = string.Join("|", cachedTags.Where(x => x.TagType != TagType.Phrase)
                    .Select(x => x.Name))
                .Replace("#",@"\043");
        
            regex = new($@"({all})\s({all})", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);
            await Task.Delay(span);
        }
    }

    private Task ClientOnLog(LogMessage arg)
    {
        _logger.Log((LogLevel)arg.Severity, arg.Message);

        return Task.CompletedTask;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _client.LoginAsync(TokenType.Bot, _config["DiscordToken"]);
        await _client.StartAsync();
        
        while (!stoppingToken.IsCancellationRequested)
            await Task.Delay(1000, stoppingToken);
    }
}