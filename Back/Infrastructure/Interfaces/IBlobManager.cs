using System.Threading.Tasks;
using Infrastructure.Enums;

namespace Infrastructure.Interfaces
{
    public interface IBlobManager
    {
        Task<string> UploadFile(byte[] file, string name, AudioFormat format);
    }
}