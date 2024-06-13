using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Streetcode.DAL.Entities.Feedback;

public class Response
{
    public int Id { get; set; }

    public string? Name { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    public string? Description { get; set; }
}