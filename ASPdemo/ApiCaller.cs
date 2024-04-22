//##############################################################################################

//TEMPLATE CODE CREDIT https://coinmarketcap.com/api/documentation/v1/#section/Quick-Start-Guide

//##############################################################################################
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RestSharp;
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

    public static async Task<string> getTokensAndBalances(string walletAddress)
    {
        var url = "https://eth-mainnet.g.alchemy.com/v2/zHyOaZ4njRcWrjIiO-8yy-C-4swslzEG";

        var options = new RestClientOptions(url)
        {
            RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
        };

        var client = new RestClient(options);
        var request = new RestRequest("");
        request.AddHeader("accept", "application/json");
        request.AddJsonBody("{\"id\":1,\"jsonrpc\":\"2.0\",\"method\":\"alchemy_getTokenBalances\",\"params\":[" +
        '"' + walletAddress + '"' + "]}", false);
        var response = await client.PostAsync(request);

        return response.Content;
    }

    public static async Task<string> getTokenNameFromContract(string contractAddress)
    {
        // 0x9f8f72aa9304c8b593d555f12ef6589cc3a579a2
        var url = "https://api.etherscan.io/api?module=account&action=tokentx&contractaddress=" + contractAddress + "" +
            "&apikey=HCM6XPJS2XHRWI4UATNGYA4ZK2EBKPWGAG+&page=1&offset=5";
        var options = new RestClientOptions(url)
        {
            RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
        };

        var request = new RestRequest("");

        var client = new RestClient(options);
        var response = await client.GetAsync(request);

        return response.Content;
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

    public static async Task<string> GetLatestQuotesFromPairsId(string id)
    {
        var URL = new UriBuilder("https://pro-api.coinmarketcap.com/v2/cryptocurrency/quotes/latest");

        var queryString = HttpUtility.ParseQueryString(URL.Query);

        queryString["id"] = id;

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


    public static async Task<string> GetLatestQuotesFromPairs(string slug)
    {
        var URL = new UriBuilder("https://pro-api.coinmarketcap.com/v2/cryptocurrency/quotes/latest");

        var queryString = HttpUtility.ParseQueryString(URL.Query);

        queryString["slug"] = slug;

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


        queryString["limit"] = "200";
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