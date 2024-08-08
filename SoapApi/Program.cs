using SoapApi.Features;
using SoapCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseSoapEndpoint<IBlogService>("/Blog.asmx", new SoapEncoderOptions());
app.UseAuthorization();
app.MapControllers();

app.Run();