using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Channels
{
    public interface IEmailChannel
    {
        Task<bool> SubmitAsync(NotificationDto message, CancellationToken cancellationToken);
        IAsyncEnumerable<NotificationDto> ReadAllAsync(CancellationToken cancellationToken);
    }
}
