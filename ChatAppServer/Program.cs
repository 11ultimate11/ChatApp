using Blazored.LocalStorage;
using ChatAppServer.Data;
using ChatAppServer.SRhub;
using GhostLibrary.Services.Interfaces;
using GhostLibrary.Services.Processor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Text.Json.Serialization;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddTransient<IApiProcessor, ApiProcessor>();
builder.Services.AddHttpContextAccessor();
//builder.Services.AddBlazoredLocalStorage(config =>
//{
//    config.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
//    config.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
//    config.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
//    config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
//    config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
//    config.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
//    config.JsonSerializerOptions.WriteIndented = false;
//});
builder.Services.AddBlazoredLocalStorage();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapHub<ChatHub>("chatHub");
app.MapFallbackToPage("/_Host");

app.Run();
