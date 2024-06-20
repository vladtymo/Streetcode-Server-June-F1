namespace Streetcode.DAL.Shared;

public interface IValidationResult
{
    public static Error ValidationError = new("Validation Error", "A validation error occurred.");
    Error[] Errors { get;  }
}