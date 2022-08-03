using HelloNet.Interfaces;

namespace HelloNet.Services;
public class MyService : IMyService
{
    // public string GetData() => $"Called {nameof(GetData)} in instance of {nameof(MyService)}";
    protected IConfiguration Configuration;
    public MyService(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public string GetData() => Configuration.GetValue<string>("myData");
}