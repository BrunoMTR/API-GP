using Domain.Channels;
using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Infrastructure.Notifications.Email
{
    public class EmailChannel : IEmailChannel
    {
        private readonly Channel<NotificationDto> _channel;
        public EmailChannel()
        {
            var options = new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = false
            };
            _channel = Channel.CreateUnbounded<NotificationDto>(options);
        }

        public IAsyncEnumerable<NotificationDto> ReadAllAsync(CancellationToken cancellationToken)
         => _channel.Reader.ReadAllAsync(cancellationToken);

        public async Task<bool> SubmitAsync(NotificationDto message, CancellationToken cancellationToken)
        {
            while (await _channel.Writer.WaitToWriteAsync(cancellationToken)
                   && !cancellationToken.IsCancellationRequested)
            {
                if (_channel.Writer.TryWrite(message))
                    return true;
            }
            return false;
        }
    }
}
