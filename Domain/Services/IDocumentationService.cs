using Domain.Channels;
using Domain.DTOs;
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
        Task UploadFile(DocumentFormDto document, CancellationToken cancellationToken);

    }
}
