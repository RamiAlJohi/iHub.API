using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("JiraIhub", httpClient => {
    var baseUrl = builder.Configuration.GetSection("JiraIhub:BaseUrl").Value;
    var username = builder.Configuration.GetSection("JiraIhub:Username").Value;
    var password = builder.Configuration.GetSection("JiraIhub:Password").Value;

    httpClient.BaseAddress = new Uri(baseUrl);
    string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
    httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + encoded);
    httpClient.DefaultRequestHeaders.Add("X-Atlassian-Token", "no-check");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
