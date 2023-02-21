using OrgamaxSharp;
using OrgamaxSharp.Sample.Web;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseOrgamax<SampleOrgamaxIntegration>("Panda", "Panda2019*");

app.Run();