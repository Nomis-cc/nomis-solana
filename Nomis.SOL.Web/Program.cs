using System.Reflection;
using Microsoft.OpenApi.Models;
using Nomis.SOL.Web.Client;
using Nomis.SOL.Web.Services;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressMapClientErrors = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Nomis Score API",
        Description = "An API to get Nomis Score for Solana wallets.",
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    options.EnableAnnotations();
});

// Add services to the container.
builder.Services
    .AddRazorPages()
    .AddRazorRuntimeCompilation();

builder.Services.AddTransient(x => new SolscanClient(new(builder.Configuration["SolScanApiBaseUrl"])));
builder.Services.AddTransient(x => new MagicEdenClient(new(builder.Configuration["MagicEdenApiBaseUrl"])));
builder.Services.AddTransient<ScoreCalcService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("v1/swagger.json", "Nomis Score API V1");
    options.DefaultModelsExpandDepth(0);
    options.InjectStylesheet("/css/swagger.css");
    options.DocExpansion(DocExpansion.Full);
});


app.Run();