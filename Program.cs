using LR_5;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder();

builder.Logging.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));


var app = builder.Build();



app.MapGet("/", async (context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.SendFileAsync("resources/index.html");
});

app.MapPost("/", async (context) =>
{
    string value = context.Request.Form["value"];
    string datetime = context.Request.Form["datetime"];

    context.Response.Cookies.Append(value, datetime);
    context.Response.Redirect("/cookies");
});

app.MapGet("/cookies", async (context) =>
{


    DateTime now = DateTime.Now;
    String resp = "";
    foreach (var cookie in context.Request.Cookies) {
        DateTime parsedDateTime = DateTime.Parse(cookie.Value);
        
        if (parsedDateTime < now)
        {
            context.Response.Cookies.Delete(cookie.Key);
        }
        else {
            resp += "Key: " + cookie.Key + "\nExpiration date: " + cookie.Value + "\n";
        }

        
    }
    await context.Response.WriteAsync(resp);

});

app.Run();