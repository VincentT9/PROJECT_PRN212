using SchoolMedicalManagementSystem.Enum;

namespace BusinessObjects;

public partial class Blog
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public Guid? AuthorId { get; set; }

    public DateTime PublishedDate { get; set; }

    public BlogStatus Status { get; set; }

    public string? Images { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual User? Author { get; set; }
}
