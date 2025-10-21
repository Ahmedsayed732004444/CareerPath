namespace CareerPath.Services.Abstraction
{
    public interface IEmailServices
    {
        Task sendEmailAsync(string mailTo, string Subject, string body, IList<IFormFile> attatcments = null);
    }
}
