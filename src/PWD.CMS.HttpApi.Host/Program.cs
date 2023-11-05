using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using PWD.CMS.Services;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using PWD.CMS.DtoModels;
using IdentityModel.Client;
using System.Net.Http.Headers;
using PWD.CMS.EntityFrameworkCore;
using PWD.CMS.Interfaces;
using System.Linq;
using System.Net;
//using System.Timers;

namespace PWD.CMS;

public class Program
{
   
    private static System.Threading.Timer timer;
    private static string clientUrl = PermissionHelper._selfClientUrlDev;
    //private static OrganizaitonUnitAppService organizaitonUnitService;
    //private static NotificationAppService notificationAppService;
    public async static Task<int> Main(string[] args)
    {
        Program program = new Program();
        Thread ThreadObject1 = new Thread(Example1);
        ThreadObject1.Start();

        Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Async(c => c.File("Logs/logs.txt"))
#if DEBUG
            .WriteTo.Async(c => c.Console())
#endif
            .CreateLogger();

        try
        {
            Log.Information("Starting PWD.CMS.HttpApi.Host.");
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.AddAppSettingsSecretsJson()
                .UseAutofac()
                .UseSerilog();
            await builder.AddApplicationAsync<CMSHttpApiHostModule>();
            var app = builder.Build();
            //CreateDbIfNotExists(app);
            await app.InitializeApplicationAsync();
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }

    }
    private static void Example1()
    {
        for (int i = 0; i <= 99999; i++)
        {
            SetUpTimer(new TimeSpan(15, 57, 01));
        }
        Thread.Sleep(1000);
    }
    private static void SetUpTimer(TimeSpan alertTime)
    {
        //HttpClient client = new HttpClient();
        //string url = "https://localhost:44373/api​/app​/complain​/example2";
        DateTime current = DateTime.Now;
        TimeSpan timeToGo = alertTime - current.TimeOfDay;
        if (timeToGo < TimeSpan.Zero)
        {
            return;//time already passed
        }
        timer = new Timer(async x =>        
        {
            ////////////////////////////
            // //var httpResponse = await client.PostAsync(url, null);
            ////////////////////////
            ///
            var httpClientHandler = new HttpClientHandler { Proxy = WebRequest.GetSystemWebProxy() };
            using (var client = new HttpClient(httpClientHandler))
            {
                var tokenResponse = await GetToken();
                client.BaseAddress = new Uri(clientUrl);
                client.SetBearerToken(tokenResponse.AccessToken);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //GET Method
                HttpResponseMessage response = await client.PostAsync($"api​/app​/complain​/example2", null);
                //HttpResponseMessage response = await client.PostAsync($"api​/common/example", null);
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        // put the code here that may raise exceptions
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Internal server Error");
                }
            }

        }, null, timeToGo, Timeout.InfiniteTimeSpan);
    }
    private static async Task<TokenResponse> GetToken()
    {
        var authorityUrl = $"{PermissionHelper._authority}";

        var authority = new HttpClient();
        var discoveryDocument = await authority.GetDiscoveryDocumentAsync(authorityUrl);
        if (discoveryDocument.IsError)
        {
            //return null;
        }

        // Request Token
        var tokenResponse = await authority.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = discoveryDocument.TokenEndpoint,
            ClientId = PermissionHelper._clientId,
            ClientSecret = PermissionHelper._clientSecret,
            Scope = PermissionHelper._scope
        });

        if (tokenResponse.IsError)
        {
            //return null;
        }
        return tokenResponse;
    }

    public static async void CreateDbIfNotExists(IHost host)
    {
        Program p = new Program();

        DateTime day = (DateTime.Now).Date;
        var sdePhone = "";
        using (var scope = host.Services.CreateScope())  //IServiceScope Used for Scoped Services
        {
            var Services = scope.ServiceProvider;     //Resolve Dependency
            try
            {
                var context =  Services.GetRequiredService<CMSDbContext>();
                var complain = context.Complains;
                if (complain.Any())
                {
                    var items = complain.Where(c => c.ComplainStatus == CMSEnums.ComplainStatus.New).ToList();
                    if (items.Count() > 0)
                    {
                        foreach (var c in items)
                        {
                            var dc = (day - c.Date.Date).TotalDays;
                            if (dc >= 3)
                            {
                                //sdePhone = organizaitonUnitService.GetPostingById(c.PostingId).Result.EmpPhoneMobile;
                                if (!string.IsNullOrEmpty(sdePhone))
                                {
                                    SmsRequestInput otpInput2 = new SmsRequestInput();
                                    otpInput2.Sms = String.Format("adfadfadf");
                                    otpInput2.CsmsId = sdePhone;
                                    otpInput2.Msisdn = "";

                                    //var res2 = await notificationAppService.SendSmsTestAlpha(otpInput2);
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //var logger = Services.GetRequiredService<ILogger<Program>>();
                //logger.LogError(ex, "An Error Occured Creating in DB");
                //throw;
            }
        }
    }
}



