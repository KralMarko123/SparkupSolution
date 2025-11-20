namespace SparkUpSolution.Application.Helpers
{
    public interface IJwtHelper
    {
        Task<string> GenerateJwtToken(string username);
    }
}
