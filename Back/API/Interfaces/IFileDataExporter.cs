using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace API.Interfaces
{
    public interface IFileDataExporter
    {
        Task<List<KeyValuePair<string, List<string>>>> GetData(IFormFile file);
    }
}
