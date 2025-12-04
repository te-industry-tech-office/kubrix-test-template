using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", async context =>
{
    var now = DateTime.Now.ToString("F");
    var html = $@"<!doctype html>
<html>
  <head>
    <meta charset=""utf-8"" />
    <title>Minimal WebApp</title>
    <style>
      :root {{
        --bg1: #071029;
        --bg2: #0b2545;
        --accent1: #8b5cf6;
        --accent2: #06b6d4;
        --card: rgba(255,255,255,0.03);
        --text: #e6eef8;
      }}
      html,body{{height:100%;margin:0}}
      body{{font-family:Inter,Arial,Helvetica,sans-serif;background:linear-gradient(135deg,var(--bg1),var(--bg2));display:flex;align-items:center;justify-content:center;padding:40px;color:var(--text)}}
      .container{{max-width:820px;background:linear-gradient(180deg,rgba(255,255,255,0.02),rgba(255,255,255,0.01));padding:28px;border-radius:14px;box-shadow:0 10px 40px rgba(2,6,23,0.6);backdrop-filter: blur(6px);}}
      h1{{font-size:1.9rem;margin:0 0 10px;background:linear-gradient(90deg,var(--accent1),var(--accent2));-webkit-background-clip:text;background-clip:text;color:transparent}}
      .time{{font-family:Menlo,monospace;background:rgba(255,255,255,0.02);padding:6px 10px;border-radius:8px;display:inline-block;margin-left:10px;color:var(--text);}}
      p{{margin:0}}
      .small{{opacity:0.92;font-size:1rem}}
    </style>
  </head>
  <body>
    <div class=""container"">
      <h1>Containerized .NET Core application</h1>
      <p class=""small"">This is a tiny ASP.NET Core app running in a container.
        <span class=""time"">Current time: {now}</span>
      </p>
    </div>
  </body>
</html>";

    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync(html);
});

app.Run();
