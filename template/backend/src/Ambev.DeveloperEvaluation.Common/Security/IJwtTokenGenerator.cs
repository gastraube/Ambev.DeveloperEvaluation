namespace Ambev.DeveloperEvaluation.Common.Security
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Guid userId, string username, IEnumerable<string>? roles = null);
    }
}
