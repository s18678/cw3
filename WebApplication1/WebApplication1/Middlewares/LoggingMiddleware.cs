using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        private static readonly string logpath = "log.txt";
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();
            if (!File.Exists(logpath)) {
                File.Create(logpath);
            }
            

            var meth = httpContext.Request.Scheme.ToString();
            var path = httpContext.Request.Path.ToString();
            var body = string.Empty;
            using (var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true)) {
                body = await reader.ReadToEndAsync();
            }
            var query = httpContext.Request.QueryString.ToString();

            using (var writer = new StreamWriter(File.Open(logpath, FileMode.Append)))
            {
                writer.Write(DateTime.Now.ToString() + "\n" + meth + "\n" + path + "\n" + body + "\n" + query + "\n\n\n");
                writer.Flush();
            }

            httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
            await _next(httpContext);
        }
    }
}