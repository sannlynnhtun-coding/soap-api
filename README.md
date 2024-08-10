# SOAP API with ASP.NET Core

This project demonstrates how to create a SOAP API using ASP.NET Core.

Command for NuGet Package Manager Console:

```
PM> Install-Package SoapCore
```

The command for dotnet CLI (Command Line Interface):

```
dotnet add package SoapCore
```

### Step 1: Define Data Contracts

Define the `Blog` class with data members:

```csharps
using System.Runtime.Serialization;

namespace SoapApi.Features;

[DataContract]
public class Blog
{
    [DataMember]
    public int Id { get; set; }

    [DataMember]
    public string Title { get; set; }

    [DataMember]
    public string Content { get; set; }

    [DataMember]
    public DateTime CreatedDate { get; set; }
}
```

### Step 2: Define Service Contracts

Create the `IBlogService` interface with service operations:

```csharp
using System.ServiceModel;

namespace SoapApi.Features;

[ServiceContract]
public interface IBlogService
{
    [OperationContract]
    void AddBlog(Blog blog);

    [OperationContract]
    Blog GetBlog(int id);

    [OperationContract]
    List<Blog> GetAllBlogs();

    [OperationContract]
    void UpdateBlog(Blog blog);

    [OperationContract]
    void DeleteBlog(int id);
}
```

### Step 3: Implement the Service

Implement the `IBlogService` interface in `BlogService` class:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

namespace SoapApi.Features;

public class BlogService : IBlogService
{
    private static List<Blog> blogs = new List<Blog>();

    public void AddBlog(Blog blog)
    {
        blog.Id = blogs.Count + 1;
        blog.CreatedDate = DateTime.Now;
        blogs.Add(blog);
    }

    public Blog GetBlog(int id)
    {
        return blogs.FirstOrDefault(b => b.Id == id)!;
    }

    public List<Blog> GetAllBlogs()
    {
        return blogs;
    }

    public void UpdateBlog(Blog blog)
    {
        var existingBlog = blogs.FirstOrDefault(b => b.Id == blog.Id);
        if (existingBlog != null)
        {
            existingBlog.Title = blog.Title;
            existingBlog.Content = blog.Content;
        }
    }

    public void DeleteBlog(int id)
    {
        var blog = blogs.FirstOrDefault(b => b.Id == id);
        if (blog != null)
        {
            blogs.Remove(blog);
        }
    }
}
```

### Step 4: Configure SOAP Endpoint

Register and configure the SOAP endpoint in `Program.cs`:

```csharp
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
```

### Step 5: Run the Service

Build and run the project:

```sh
dotnet run
```

### Step 6: Test the Service

Navigate to `http://localhost:5000/Blog.asmx` to access the SOAP API.

### Step 7: Add Service Reference

To add a service reference named `BlogServiceReference` for an `.asmx` service in Visual Studio, follow these steps:

1. **Open Solution Explorer**: Right-click on your project name.
2. **Add Service Reference**:
   - Select "Add" > "Service Reference..."
3. **Enter Service URL**:
   - In the Address field, enter the URL of your `.asmx` service (e.g., `http://localhost:5000/Blog.asmx`) and click "Go".
4. **Configure Namespace**:
   - In the Namespace field, enter `BlogServiceReference`.
5. **Finish**:
   - Click "OK" to add the service reference.

Now, you can use `BlogServiceReference` to interact with the SOAP service in your code.

## References

- [Creating SOAP Services with ASP.NET Core](https://positiwise.com/blog/how-to-create-soap-services-using-asp-net-core#What_is_SOAP_Based_Web_Service)