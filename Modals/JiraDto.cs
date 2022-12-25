using System.Text.Json.Serialization;

namespace iHub.API.Modals;

public class JiraDto
{
    public required Fields fields { get; set; }
}

public class Fields
{
    public required Project project { get; set; }
    public required Issuetype issuetype { get; set; }
    
    public required string summary { get; set; }
    public required string description { get; set; }
    
    // custom fields
    [JsonPropertyName("customfield_11405")]
    public required string Address { get; set; }

    [JsonPropertyName("customfield_11402")]
    public required string Company { get; set; }

    [JsonPropertyName("customfield_11403")]
    public required string Email { get; set; }

    [JsonPropertyName("customfield_11401")]
    public required string Name { get; set; }

    [JsonPropertyName("customfield_11406")]
    public required string Phone { get; set; }
 
    [JsonPropertyName("customfield_11404")]
    public required string Website { get; set; }
 
    [JsonPropertyName("customfield_11418")]
    public required object Challange { get; set; }
 
    [JsonPropertyName("customfield_11420")]
    public required object Rating { get; set; }
}

public class Project
{
    public required string key { get; set; }
}

public class Issuetype
{
    public required string name { get; set; }
}