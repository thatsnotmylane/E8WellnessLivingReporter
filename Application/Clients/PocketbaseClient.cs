using RestSharp;
using System;
using System.Threading.Tasks;

public class PocketBaseClient
{
    private readonly RestClient _client;

    public PocketBaseClient(string baseUrl)
    {
        _client = new RestClient(baseUrl);
    }

    public async Task<List<RunConfigItem>> GetAllRunConfigRecordsAsync()
    {
        var request = new RestRequest("/api/collections/run_record/records", Method.Get);

        try
        {
            var response = await _client.ExecuteAsync<RunConfigResponse>(request);

            if (response.IsSuccessful == false || response.Data == null)
            {
                Console.WriteLine($"Error: {response.ErrorMessage}");
                return new List<RunConfigItem>();
            }

            return response.Data.Items;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred: {ex.Message}");
            return new List<RunConfigItem>();
        }
    }
}
