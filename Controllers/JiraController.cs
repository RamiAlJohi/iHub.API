using iHub.API.Modals;
using Microsoft.AspNetCore.Mvc;

namespace iHub.API.Controllers;

[ApiController]
[Route("[controller]")]
public class JiraController : ControllerBase
{
    private IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<JiraController> _logger;

    public JiraController(IConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger<JiraController> logger)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    [HttpPost]
    [RequestSizeLimit(50000000)]
    public async Task<IActionResult> Post([FromForm] ProposalDto proposalDto)
    {
        var jiraDto = mapToJiraDto(proposalDto);
        var url = _configuration.GetValue<string>("JiraIhub:CreateIssueUrl");
        var httpClient = _httpClientFactory.CreateClient("JiraIhub");

        var response = await httpClient.PostAsJsonAsync(url, jiraDto);
        if (!response.IsSuccessStatusCode)
            return BadRequest();

        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = await response.Content.ReadFromJsonAsync<JiraResponse>();

        if (proposalDto.file != null && proposalDto.file.Length > 0 && string.IsNullOrEmpty(responseObject.id))
        {
            bool isUploadSuccess = await UploadFile(responseObject.id, proposalDto.file);
            if (!isUploadSuccess)
                return BadRequest();
        }

        return Ok(responseString);
    }

    private async Task<bool> UploadFile(string id, IFormFile file)
    {
        var url = _configuration.GetValue<string>("JiraIhub:AddAttechmentByIssueIdUrl");
        url = string.Format(url, id);
        var httpClient = _httpClientFactory.CreateClient("JiraIhub");

        var multipartContent = new MultipartFormDataContent();
        using (var ms = new MemoryStream())
        {
            await file.CopyToAsync(ms);
            var fileBytes = new ByteArrayContent(ms.ToArray());
            multipartContent.Add(fileBytes, file.Name, file.FileName);
        }
        
        var response = await httpClient.PostAsync(url, multipartContent);
        return response.IsSuccessStatusCode;
    }

    private JiraDto mapToJiraDto(ProposalDto proposalDto)
    {
        var projectKey = _configuration.GetValue<string>("JiraIhub:projectKey");
        var issuetypeName = _configuration.GetValue<string>("JiraIhub:issuetypeName");

        return new JiraDto()
        {
            fields = new()
            {
                project = new()
                {
                    key = projectKey
                },
                issuetype = new()
                {
                    name = issuetypeName
                },

                summary = proposalDto.summary,
                description = proposalDto.description,
                Address = proposalDto.Address,
                Company = proposalDto.Company,
                Email = proposalDto.Email,
                Name = proposalDto.Name,
                Phone = proposalDto.Phone,
                Website = proposalDto.Website,
                Challange = new { value = proposalDto.Challange },
                Rating = new { value = "0" }
            }
        };
    }
}
