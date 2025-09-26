using Domain.Channels;
using Domain.DTOs;
using Domain.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IDocumentationService
    {
        Task<Response<bool>> UploadAsync(DocumentFormDto document, CancellationToken cancellationToken);

    }
}
