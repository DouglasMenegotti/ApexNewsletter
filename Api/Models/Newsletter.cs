using System.ComponentModel.DataAnnotations;

public class Newletter
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(250)]
    public string SubTitle { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

     public DateTime CreatedAt { get; set; } = DateTime.Now;
}