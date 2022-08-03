using HelloNet.Interfaces;
using HelloNet.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IMyService,MyService>();
var app = builder.Build();

app.MapGet("/", () => "Welcome to Hello .NET");
app.MapGet("/hello", () => "Hello World!");
app.MapGet("/json", () => new { example=123, myString="This is a string", source=nameof(app) });
app.MapGet("/custom", MyCustomEndpoint);
app.MapGet("/services", (IMyService service) => service.GetData());
//app.MapGet("/services", ServicesEndpoint);

IResult MyCustomEndpoint()
{
	var myDictionary = new Dictionary<string,dynamic>();
	myDictionary.Add("anInteger", 123);
	myDictionary.Add("aString", "Example string");
	myDictionary.Add("now", DateTime.UtcNow);
	myDictionary.Add("nowString", DateTime.UtcNow.ToString("yyyy-MM-dd mm:HH:ss"));
    myDictionary.Add("compositeString", $"Now={DateTime.UtcNow:yyyy-MM-dd}, Count={myDictionary.Keys.Count}");
    return Results.Ok(myDictionary);
}

// IResult ServicesEndpoint(IMyService service) => service.GetData();

app.Run();
