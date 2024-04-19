using Quartz;
using ASPdemo.Database;
using Newtonsoft.Json; 

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
                    var currencies = dbContext.Currencies;

                    var prices = await ApiCaller.getListings();
                    if (prices != null)
                    {
                        dynamic results = JsonConvert.DeserializeObject<dynamic>(prices).data;
                        foreach (dynamic result in results)
                        {
                            double price = result.quote.USD.price;
                            this.logger.LogInformation("awefawef"); 
                        }
                    }
                    
                }
                catch (Microsoft.Data.Sqlite.SqliteException) //catches if table is crapped
                {
                    Console.WriteLine("TABLE DOES NOT EXIST");
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
