//##############################################################################################

//TEMPLATE CODE CREDIT https://coinmarketcap.com/api/documentation/v1/#section/Quick-Start-Guide

//##############################################################################################
using System;
using System.Net;
using System.Web;
namespace ASPdemo;

public class ApiCaller
{
    private static string API_KEY = "beabbf43-6590-4d45-8e2f-09d1f219d80f"; //NOT THE PRODUCTION KEY, ONLY DEV TESTING ENVIRONMENT

  public static void Main(string[] args)
  {
  }

    public static async Task<string> getCategoryWithCoins(string id)
    {
        var URL = new UriBuilder("https://pro-api.coinmarketcap.com/v1/cryptocurrency/category"); 

        var queryString = HttpUtility.ParseQueryString(URL.Query);

        queryString["id"] = id; 
        queryString["start"] = "1";


        queryString["limit"] = "200";

        URL.Query = queryString.ToString();
        //note:
        //VS code was saying that WebClient was obsolete, so I changed it to Http client, method was made async Task rather than string


        //var client = new WebClient();
        //client.Headers.Add("X-CMC_PRO_API_KEY", API_KEY);
        //client.Headers.Add("Accepts", "application/json");
        //return client.DownloadString(URL.ToString());
        HttpClient client = new();
        client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", API_KEY); //I think these are going to be universal so having them as default is fine ???
        client.DefaultRequestHeaders.Add("Accepts", "application/json");
        string page = await client.GetStringAsync(URL.ToString());

        return page;
    }

    public static async Task<string> getCategories()
    {
        var URL = new UriBuilder("https://pro-api.coinmarketcap.com/v1/cryptocurrency/categories");

        var queryString = HttpUtility.ParseQueryString(string.Empty);
        queryString["start"] = "1";


        queryString["limit"] = "200";

        URL.Query = queryString.ToString();
        //note:
        //VS code was saying that WebClient was obsolete, so I changed it to Http client, method was made async Task rather than string


        //var client = new WebClient();
        //client.Headers.Add("X-CMC_PRO_API_KEY", API_KEY);
        //client.Headers.Add("Accepts", "application/json");
        //return client.DownloadString(URL.ToString());
        HttpClient client = new();
        client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", API_KEY); //I think these are going to be universal so having them as default is fine ???
        client.DefaultRequestHeaders.Add("Accepts", "application/json");
        string page = await client.GetStringAsync(URL.ToString());

        return page; 
    }

    //RIP airdrop functionality
    //public static async Task<string> getAirdrops()
    //{
    //    return "";
    //}

  public static async Task<string> getListings()
  {
    var URL = new UriBuilder("https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest");

    var queryString = HttpUtility.ParseQueryString(string.Empty);
        queryString["start"] = "1";


        queryString["limit"] = "5000";
    queryString["convert"] = "USD";

    URL.Query = queryString.ToString();
    //note:
    //VS code was saying that WebClient was obsolete, so I changed it to Http client, method was made async Task rather than string


    //var client = new WebClient();
    //client.Headers.Add("X-CMC_PRO_API_KEY", API_KEY);
    //client.Headers.Add("Accepts", "application/json");
    //return client.DownloadString(URL.ToString());
    HttpClient client = new();
    client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", API_KEY); //I think these are going to be universal so having them as default is fine ???
    client.DefaultRequestHeaders.Add("Accepts", "application/json");
    string page = await client.GetStringAsync(URL.ToString());

    return page;
  }

}