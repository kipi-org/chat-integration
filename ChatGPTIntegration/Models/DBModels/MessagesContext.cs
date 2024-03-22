using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

public class MessagesContext : DbContext
{
    public MessagesContext(DbContextOptions<MessagesContext> options)
        : base(options)
    { }

    public DbSet<Message> Messages { get; set; }
    public object Message { get; internal set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}

[Table("messages")]
public class Message
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("role")]
    [JsonPropertyName("role")]
    public string Role { get; set; }

    [Column("content")]
    [JsonPropertyName("content")]
    public string Content { get; set; }

    [Column("date")]
    public DateTime Date { get; set; }
}
