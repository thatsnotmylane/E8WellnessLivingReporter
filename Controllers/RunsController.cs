using Microsoft.AspNetCore.Mvc;

[Route("Runs")]
public class RunsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost("GenerateNumbers")]
    public IActionResult GenerateNumbers()
    {
        var random = new Random();
        var randomNumbers = Enumerable.Range(0, 10).Select(_ => random.Next(10000, 100000)).ToList();

        // Pass the list to the view using ViewData
        ViewData["RandomNumbers"] = randomNumbers;

        return View("Index");
    }

    [HttpPost("ListRunsAsync")]
    public IActionResult ListRunsAsync()
    {
        var client = new PocketBaseClient("http://127.0.0.1:8090/");
        var records = (client.GetAllRunConfigRecordsAsync()).Result;

        foreach (var record in records)
        {
            Console.WriteLine($"ID: {record.Id}, Report Name: {record.report_name}, Run At: {record.run_at}");
        }


        return View("Index");
    }

    [HttpPost("ListRun")]
    public IActionResult ListRun()
    {
        var runRecords = new List<string>();

        var client = new PocketBaseClient("http://127.0.0.1:8090/");
        var records = (client.GetAllRunConfigRecordsAsync()).Result;

        foreach (var record in records)
        {
            runRecords.Add($"ID: {record.Id}, Report Name: {record.report_name}, Run At: {record.run_at}");
        }

        ViewData["RunRecords"] = runRecords;

        return View("Index");
    }
}
