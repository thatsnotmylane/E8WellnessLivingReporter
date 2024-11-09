using System;
using System.Collections.Generic;

public class RunConfigItem
{
    public string Id { get; set; } = "";
    public string CollectionId { get; set; } = "";
    public string CollectionName { get; set; } = "";
    public string Created { get; set; } = "";
    public string Updated { get; set; } = "";
    public string report_name { get; set; } = "";
    public string run_at { get; set; } = "";
}

public class RunConfigResponse
{
    public int Page { get; set; }
    public int PerPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
    public List<RunConfigItem> Items { get; set; } = new List<RunConfigItem>();
}
