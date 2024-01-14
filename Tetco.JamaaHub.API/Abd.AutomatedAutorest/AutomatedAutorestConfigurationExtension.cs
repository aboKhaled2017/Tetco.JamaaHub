using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Abd.AutomatedAutorest;

public class Output
{
    public string LanguageGenerator
    {
        get; set;
    }
    public string OutputFolder
    {
        get; set;
    }

    /// <summary>
    /// document name such as v1
    /// </summary>
    public string SwaggerDoc
    {
        get; set;
    }
}

public class AutorestSettings
{

    public List<Output> Outputs
    {
        get; set;
    }
    public string Namespace
    {
        get; set;
    }

    /// <summary>
    /// path to the API Dll
    /// </summary>
    public string StartupAssemblyPath
    {
        get; set;
    }



    public string SwaggerJsonInputPath
    {
        get; set;
    }

    public bool AddCredentials
    {
        get; set;
    }

}

public class AutorestSettingsProvider
{
    public AutorestSettings Settings
    {
        get;
    }

    public AutorestSettingsProvider(IConfiguration configuration)
    {
        Settings = configuration.Get<AutorestSettings>();
    }
}



// must call commands:
// dotnet new tool-manifest
// dotnet tool install  Swashbuckle.AspNetCore.Cli
// npm install -g autorest
// autorest –latest



public class AutoRestMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AutorestSettings _autorestSettings;
    private readonly ILogger<AutoRestMiddleware> _logger;

    public AutoRestMiddleware(RequestDelegate next
        , AutorestSettingsProvider autorestSettingsProvider
        , ILogger<AutoRestMiddleware> logger)
    {
        _next = next;
        _autorestSettings = autorestSettingsProvider.Settings;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            var swaggerTasks = GenerateSwaggerCommands().Select(RunCommand);

            await Task.WhenAll(swaggerTasks);

            var autorestTasks = GenerateAutorestCommands().Select(RunCommand);

            await Task.WhenAll(autorestTasks);

            // Call the next middleware
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception when trying to generate client api proxy");
        }

        await _next(context);
    }


    private string[] GenerateAutorestCommands()
    {

        // e.g. "autorest --input-file=swagger.json --typescript --namespace=EarlyRegistration.APIs.Controllers --add-credentials --output-folder=apiClientProxy";
        var r = _autorestSettings.Outputs.Select(e => string.Format("autorest --input-file={0} --{1} --namespace={2} {3} --output-folder={4}"
                   , "swagger-v1.json"
                   , e.LanguageGenerator
                   , _autorestSettings.Namespace
                   , _autorestSettings.AddCredentials ? "--add-credentials" : string.Empty
                   , Path.Combine(e.OutputFolder, e.LanguageGenerator))).ToArray();

        return r;


    }

    private string[] GenerateSwaggerCommands()
    {

        // e.g. "autorest --input-file=swagger.json --typescript --namespace=EarlyRegistration.APIs.Controllers --add-credentials --output-folder=apiClientProxy";
        var r = _autorestSettings.Outputs
            .GroupBy(e => e.SwaggerDoc)
            .Select(e => string.Format("dotnet swagger tofile --output swagger-{0}.json \"{1}\" {0}"
                   , e.Key
                   , Path.Combine(AppContext.BaseDirectory, _autorestSettings.StartupAssemblyPath)))
            .ToArray();

        return r;


    }

    private async Task ConsumeStreamReaderAsync(StreamReader reader, string prefix)
    {
        string line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            if (prefix.ToLower().Contains("error"))
                _logger.LogError($"{prefix}{line}"); // Log to the ILogger
            else
                _logger.LogInformation($"{prefix}{line}"); // Log to the ILogger
        }
    }


    private async Task RunCommand(string command)
    {
        using Process process = GenerateProcess();

        process.Start();

        // Send the AutoRest CLI command to the standard input of the process
        using (StreamWriter sw = process.StandardInput)
        {
            if (sw.BaseStream.CanWrite)
            {
                await sw.WriteLineAsync(command);
            }
        }

        // Do not wait for the process to exit, and let it run in the background

        // Handle the output and error streams asynchronously
        var outputTask = ConsumeStreamReaderAsync(process.StandardOutput, "Output: ");
        var errorTask = ConsumeStreamReaderAsync(process.StandardError, "Error: ");

        // Continue processing other requests while the batch file is running
        Task.WhenAll(outputTask, errorTask);

        // await process.WaitForExitAsync();

    }

    private Process GenerateProcess()
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        return new Process { StartInfo = startInfo };
    }
}



public static class AutomatedAutorestConfigurationExtension
{
    public static IServiceCollection AddAutomatedAutorest(this IServiceCollection services)
    {
        IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
             .SetBasePath(AppContext.BaseDirectory)
             .AddJsonFile("autorest.json", optional: false, reloadOnChange: true);

        IConfiguration configuration = configurationBuilder.Build();

        services.AddSingleton(new AutorestSettingsProvider(configuration));

        return services;
    }
}
