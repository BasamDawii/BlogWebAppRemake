using System.ComponentModel.DataAnnotations;

namespace Service.CommandQuerryModels.CommandModels;

public class CreateBlogCommand
{
    [Required]
    public int UserId { get; set; }
    [Required, MinLength(2), MaxLength(20)]
    public string Title { get; set; }
    [Required, MinLength(2), MaxLength(2400)]
    public string Content { get; set; }
    [Required]
    public int CategoryId { get; set; }
    public string? FeaturedImage { get; set; }
}

public class UpdateBlogCommand
{
    [Required]
    public int Id { get; set; }
    
    [Required]
    public int UserId { get; set; }
    [Required, MinLength(2), MaxLength(20)]
    public string Title { get; set; }
    [Required, MinLength(2), MaxLength(2400)]
    public string Content { get; set; }
    [Required]
    public int CategoryId { get; set; }
    public string? FeaturedImage { get; set; }
}

public class CreateCategoryCommand
{
    [Required, MinLength(2), MaxLength(40)]
    public string Name { get; set; }
}

public class UpdateCategoryCommand
{
    [Required]
    public int Id { get; set; }
    [Required, MinLength(2), MaxLength(20)]
    public string Name { get; set; }
}

public class CreateCommentCommand
{
    [Required]
    public int UserId { get; set; }
    [Required]
    public int BlogId { get; set; }
    [Required, MinLength(1), MaxLength(70)]
    public string Text { get; set; }
    [Required]
    public DateTime PublicationDate { get; set; }
}

public class UpdateCommentCommand
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int UserId { get; set; }
    [Required, MinLength(1), MaxLength(70)]
    public string Text { get; set; }
    [Required]
    public DateTime PublicationDate { get; set; }
}