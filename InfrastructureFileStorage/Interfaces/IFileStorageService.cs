using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureFileStorage.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveTempFileAsync(IFormFile file, CancellationToken cancellationToken);
    }
}
