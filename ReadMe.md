# AzFunctionSerilog 
This Azure Function is used to demenstrate a current bug with the [serilog-settings-configuration](https://github.com/serilog/serilog-settings-configuration) package when used
with an Azure Function. It is unable to resolve the Assembly Dependencies to resolve the "Enrich" configuration. As of creating this solution (7/19/2019), I've not seen any 
other issues with it reading values from the configuration. 

## Setup
This function is also using dependency injection to create the Serilog.ILogger object. The creation of the Logger is in the `Startup.cs` file.

## Seeing the Enrichments loaded
To see the expected "enrichments" loaded in the Serilog.Log.Logger instance, you can modify the `Startup.cs` file - uncommenting out lines #39 through #41: 
```csharp
    var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(GetConfiguration())
        // The Following Values should be un-commented to see them loaded and work as expected.
        .Enrich.FromLogContext()
        .Enrich.WithThreadId()
        .Enrich.WithMachineName()
        .CreateLogger();
```

## Current Dependencies
```xml
    <PackageReference Include="Microsoft.NET.Sdk.Functions" Version="1.0.29" />
    <PackageReference Include="Serilog" Version="2.8.0" />
    <PackageReference Include="Serilog.Enrichers.Environment" Version="2.1.3" />
    <PackageReference Include="Serilog.Enrichers.Thread" Version="3.1.0" />
    <PackageReference Include="Serilog.Settings.Configuration" Version="3.1.0" />
    <PackageReference Include="Serilog.Sinks.ApplicationInsights" Version="3.0.3" />
    <PackageReference Include="Serilog.Sinks.Console" Version="3.1.1" />
```

## Running the Application 
1. Clone Repository
2. Create a `local.settings.json` file in the root of the directory with the following configuration.
    ```json
    {
        "IsEncrypted": false,
        "Values": {
            "AzureWebJobsStorage": "UseDevelopmentStorage=true",
            "FUNCTIONS_WORKER_RUNTIME": "dotnet"
        }
    }
    ```

1. *Optional* - Application Insights. To see the results published to your Application Insights container: Update the `appsettings.json` setting the values for ApplicationInsights.

    ### Azure Function Settings
    ```json
    {
    "ApplicationInsights": {
        "InstrumentationKey": "[ENTER VALUE]"
    }
    }
    ```

    ### Serilog Configuration
    ```json

    {
        "Name": "ApplicationInsights",
        "Args": {
            "restrictedToMinimumLevel": "Verbose",
            "instrumentationKey": "[ENTER VALUE]"
        }
    }
    ```

4. Build and Run
5. Make a `GET` request to: [http://localhost:7071/api/ExampleFunction](http://localhost:7071/api/ExampleFunction)