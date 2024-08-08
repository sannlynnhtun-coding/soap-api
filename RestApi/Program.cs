using BlogServiceReference;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/blogs", async () =>
{
    BlogServiceClient client = new BlogServiceClient();
    return await client.GetAllBlogsAsync();
})
.WithName("GetBlogs")
.WithOpenApi();

app.MapPost("/blogs", async (Blog blog) =>
{
    BlogServiceClient client = new BlogServiceClient();
    await client.AddBlogAsync(blog);
    return Results.Created();
})
.WithName("CreateBlog")
.WithOpenApi();

app.Run();
