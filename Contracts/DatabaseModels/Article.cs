namespace Contracts.DatabaseModels;

public class Article
{
    public long Id { get; set; } // Primary key
    public string Title { get; set; } // Index
    public virtual ICollection<Author> Author { get; set; } // Many-To-Many
    public virtual Site Site { get; set; } // One-To-Many
}