using MyHub.Enums;

namespace MyHub.Shared.Models
{
    public record Error(string Id, ErrorType Type, string Description);
}
