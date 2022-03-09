using System.Threading.Tasks;
using HexMaster.Email.DomainModels;

namespace HexMaster.Email.Abstractions.Services
{
    public interface IMailService
    {
        Task SendAsync(Message mail);
    }
}