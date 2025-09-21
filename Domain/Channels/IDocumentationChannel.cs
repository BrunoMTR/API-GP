using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Channels
{
    public interface IDocumentationChannel
    {
        Task<bool> SubmitAsync(DocumentMessageDto message, CancellationToken cancellationToken);
        IAsyncEnumerable<DocumentMessageDto> ReadAllAsync(CancellationToken cancellationToken);
    }
}
