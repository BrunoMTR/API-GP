using InfrastructureFileStorage.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureFileStorage.Services
{
    public class FileStorageService: IFileStorageService
    {
        public async Task<string> SaveTempFileAsync(IFormFile file, CancellationToken cancellationToken)
        {
            var tempFolder = Path.Combine(Directory.GetCurrentDirectory(), "tempUploads");
            Directory.CreateDirectory(tempFolder);

            var tempFilePath = Path.Combine(tempFolder, $"{Guid.NewGuid()}_{file.FileName}");

            await using (var fs = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 81920, useAsync: true))
            {
                await file.CopyToAsync(fs, cancellationToken);
            }

            return tempFilePath;
        }
    }
}
