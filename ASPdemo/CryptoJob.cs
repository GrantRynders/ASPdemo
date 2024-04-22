using Quartz;
using ASPdemo.Database;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace ASPdemo
{
    [DisallowConcurrentExecution]
    public class CryptoJob : IJob
    {
        /**
         * TODO IN MORNING
         * */
        // https://andrewlock.net/using-scoped-services-inside-a-quartz-net-hosted-service-with-asp-net-core/
        // https://andrewlock.net/using-quartz-net-with-asp-net-core-and-worker-services/
        private readonly ILogger<CryptoJob> logger;
        private readonly IServiceProvider serviceProvider;

        public CryptoJob(ILogger<CryptoJob> logger, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                this.logger.LogInformation("updating prices");

                try
                {
                    var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    var currencies = dbContext.Currencies.ToList(); 

                    foreach (var currency in currencies)
                    {
                        string cmcId = currency.CMCId;

                        string tokens = await ApiCaller.GetLatestQuotesFromPairsId(cmcId);

                        dynamic response = JsonConvert.DeserializeObject<dynamic>(tokens);

                        dynamic data = response.data;

                        var cmc = data[cmcId]; 

                        if (cmc.quote != null)
                        {
                            double? price = cmc.quote.USD.price;
                            currency.Price = price;

                            dbContext.Update(currency);
                            dbContext.SaveChanges();

                            this.logger.LogInformation("Updated price");
                        }
                    }
                }
                catch (Microsoft.Data.Sqlite.SqliteException) //catches if table is crapped
                {
                    this.logger.LogInformation("TABLE DOES NOT EXIST");
                }
                catch
                {
                    this.logger.LogInformation("Failed to update prices"); return;
                }
                
            }

            this.logger.LogInformation("Updated Prices");
        }
    };
}
