using System;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Services;

public class AuctionSvcHttpClient(HttpClient httpClient, IConfiguration config)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IConfiguration _config = config;

    public async Task<List<Item>> GetItemsForSearchDb()
    {
        var lastUpdated = await DB.Find<Item, string>()
            .Sort(x => x.Descending(a => a.UpdatedAt))
            .Project(x => x.UpdatedAt.ToString())
            .ExecuteFirstAsync();

        Console.WriteLine(lastUpdated + "oyeeeeeeeeeé");

        return await _httpClient.GetFromJsonAsync<List<Item>>(
            _config["AuctionServiceUrl"] + "/api/auctions?date=" + lastUpdated
        );
    }
}
