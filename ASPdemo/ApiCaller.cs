//##############################################################################################

//TEMPLATE CODE CREDIT https://coinmarketcap.com/api/documentation/v1/#section/Quick-Start-Guide

//##############################################################################################
using System;
using System.Net;
using System.Web;
namespace ASPdemo;

public class ApiCaller //I don't know if this is really a necessary class, I just needed a place to put the code
{
  private static string API_KEY = "b54bcf4d-1bca-4e8e-9a24-22ff2c3d462c"; //NOT THE PRODUCTION KEY, ONLY DEV TESTING ENVIRONMENT

  public static void Main(string[] args)
  {
    try
    {
    Console.WriteLine(makeAPICall());
    }
    catch (WebException e)
    {
    Console.WriteLine(e.Message);
    }
  }

  static async Task makeAPICall()
  {
    var URL = new UriBuilder("https://sandbox-api.coinmarketcap.com/v1/cryptocurrency/listings/latest");

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
    
  }

}