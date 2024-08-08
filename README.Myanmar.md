# ASP.NET Core အသုံးပြုပြီး SOAP API ဖန်တီးခြင်း

ဒီ project က ASP.NET Core အသုံးပြုပြီး SOAP API တစ်ခု ဖန်တီးပုံကို ပြသပေးမှာ ဖြစ်ပါတယ်။

### ပထမအဆင့်: Data Contracts သတ်မှတ်ခြင်း

`Blog` class ကို အောက်ပါအတိုင်း သတ်မှတ်ပါ:

```csharp
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

### ဒုတိယအဆင့်: Service Contracts သတ်မှတ်ခြင်း

`IBlogService` interface ကို အောက်ပါအတိုင်း ဖန်တီးပါ:

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

### တတိယအဆင့်: Service ကို Implement လုပ်ခြင်း

`IBlogService` interface ကို အောက်ပါအတိုင်း `BlogService` class တွင် Implement လုပ်ပါ:

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

### စတုတ္ထအဆင့်: SOAP Endpoint ကို Configure လုပ်ခြင်း

`Program.cs` တွင် SOAP endpoint ကို အောက်ပါအတိုင်း Register နှင့် Configure လုပ်ပါ:

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

### ပဉ္စမအဆင့်: Service ကို Run လုပ်ခြင်း

Project ကို build နှင့် run လုပ်ပါ:

```sh
dotnet run
```

### ဆဌမအဆင့်: Service ကို စမ်းသပ်ခြင်း

`http://localhost:5000/Blog.asmx` တွင် SOAP API ကို အသုံးပြုနိုင်ပါပြီ။

### ဆဋ္ဌမအဆင့်: Service Reference ထည့်သွင်းခြင်း

Visual Studio တွင် `.asmx` service အတွက် `BlogServiceReference` အမည်ဖြင့် service reference ထည့်သွင်းရန် အဆင့်များမှာ -

1. **Solution Explorer** တွင် project အမည်ကို Right-click လုပ်ပါ။
2. **Add Service Reference** ကို ရွေးပါ:
   - "Add" > "Service Reference..." ကို ရွေးပါ။
3. **Service URL ထည့်ပါ**:
   - Address ခရက်ထဲတွင် `.asmx` service ၏ URL (ဥပမာ - `http://localhost:5000/Blog.asmx`) ကို ထည့်ပြီး "Go" ကိုနှိပ်ပါ။
4. **Namespace ကို Configure လုပ်ပါ**:
   - Namespace ခရက်ထဲတွင် `BlogServiceReference` ကို ထည့်ပါ။
5. **ပြီးဆုံးရန်**:
   - "OK" ကိုနှိပ်ပြီး service reference ကို ထည့်ပါ။

အခုဆိုရင် `BlogServiceReference` ကို အသုံးပြုပြီး SOAP service နှင့် အပြန်အလှန်လုပ်ဆောင်နိုင်ပါပြီ။

## References

- [Creating SOAP Services with ASP.NET Core](https://positiwise.com/blog/how-to-create-soap-services-using-asp-net-core#What_is_SOAP_Based_Web_Service)
