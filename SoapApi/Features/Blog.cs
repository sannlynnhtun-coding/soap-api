using System.Runtime.Serialization;
using System.ServiceModel;

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