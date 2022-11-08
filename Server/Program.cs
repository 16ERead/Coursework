namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the web application
            #region Services
            builder.Services.AddAuthorization();

            // Swagger is an endpoint-testing tool to be used in development
            builder.Services.AddEndpointsApiExplorer()
                            .AddSwaggerGen();
            #endregion

            WebApplication app = builder.Build();

            // If testing, use swagger (sets the application into endpoint testing)
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Redirects HTTP calls to HTTPS calls, the same as how accessing http://classroom.google.com will redirect to https://classroom.google.com
            app.UseHttpsRedirection();

            app.UseAuthorization();

            // Runs the application in a loop and blocks this thread
            app.Run();
        }
    }
}