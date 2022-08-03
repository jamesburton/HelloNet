Hello .NET!
===========

This project is designed to be repeated not cloned or deployed from here ... please following the instructions below and use this site as a comparison for the final output.

## Requirements
* Install .NET 6.0 SDK
	* See https://dotnet.microsoft.com/en-us/download/dotnet/6.0
* Install VSCode
	* See https://code.visualstudio.com/Download
## Create Project
* Go to your projects/development folder
	* Such as `cd c:\Development\`
* Create first project and open in VSCode
`
dotnet new web -n HelloNet
cd HelloNet
code .
`
## Running the project
* If loading .NET code for the first time you may be prompted to install OmniSharp and other extensions, please click through to install recommendations to complete this tutorial.
* Run project
	* Open Terminal (`Ctrl+Shift+'` or `Menu->Terminal->New Terminal`)
	* dotnet run
* Check project output and view the site root (e.g. `http://localhost:5094/`)
* Open main code file `Program.cs`
## First Modifications
* Modify default "/" method to return "Welcome to Hello .NET"
* Add additional handler for "/hello" to return "Hello World!"
* Stop and run project e.g. `ctrl+c` in terminal then `dotnet run` again
* View updated site
	* Check /
	* Check /hello
	You should see the updated responses
* Add a method to return an example object
```
app.MapGet("/json", () => new { example=123, myString="This is a string", source=nameof(app) });
```
* Restart the server again and check "/json" endpoint
## Using separate functions and using the IResult return type
* Below app.MapGet(...) calls and above app.Run() add the following function
```
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
```
* Add another endpoint (in existing MapGet block):
```
app.MapGet("/custom", MyCustomEndpoint);
```
* Restart and test `/custom` endpoint
## Defining an interface
* Create a folder called "Interfaces"
* Right click the folder and select "New C# Interface"
	* Call it `IMyService`
* Shorten namespace declaration and add example GetDataMethod by replacing it's body/content with this:
```
namespace HelloNet.Interfaces;
public interface IMyService
{
    string GetData();
}
```
## Impement the interface via a service
* Create a `Services` folder, right click and select "New C# Class"
	* Call it `MyService`
* Amend `MyService` to the following:
```
namespace HelloNet.Services;
public class MyService : IMyService
{
}
```
* You should see that IMyService has a red squiggly line underneath it showing an issue
	* Move the cursor to `IMyService` and press `ctrl+.` then select `using HelloNet.Interfaces`
		* This inserts the required `using HelloNet.Interfaces;` line at the top, which you could have entered manually but useful to know this technique too.
* You should now see the colour of IMyService has changed to match other names, but that it still has a red squiggly line showing another issue
	* Again select and use `ctrl+.`, this time selecting `Implement interface`
* That will have generated out a template method with a `NotImplementedException`, so we will replace that with this short lambda version:
```
public string GetData() => $"Called {nameof(GetData)} in instance of {nameof(MyService)}";
```
## Using the service
* Add the following endpoint to the current MapGet block:
```
app.MapGet("/services", (IMyService service) => service.GetData());
```
* Again you'll have a red squiggly line, use `ctrl+.`->`using ...` or add `using HelloNet.Services;` to the top of `Program.cs` to resolve the missing reference.
* Restart project and you'll find it complains with
```
Unhandled exception. System.InvalidOperationException: Body was inferred but the method does not allow inferred body parameters.
Below is the list of parameters that we found:

Parameter           | Source
---------------------------------------------------------------------------------
service             | Body (Inferred)


Did you mean to register the "Body (Inferred)" parameter(s) as a Service or apply the [FromService] or [FromBody] attribute?
```
This is because we have no registered anything of type IMyService so it isn't connecting this to out implementation...
* After `var builder = WebAppllication.CreateBuilder(args);` and before `var app = builer.Build();` add the following
builder.Services.AddSingleton<IMyService,MyService>();
* Resolve the missing dependency with `ctrl+.` or by adding `using HelloNet.Services`
* Restart and view `/services`
## Using configuration and child dependencies
* Add the `myData` property to appsettings.json e.g.
```
{
	...
	"myData": "This is from appsettings.json"
	...
}
```
* Update `MyService` to use this value by replacing the `GetData` method with the following:
```
    protected IConfiguration Configuration;
    public MyService(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public string GetData() => Configuration.GetValue<string>("myData");
```
* Restart and refresh `/services`
* Set `myData` in `appsettings.Development.json` to `"This is from appsettings.Development.json"`
* Restart and refresh `/services` ... you will see the new value, as when running in development environment this file takes precedence over base values from `appsetings.json`
