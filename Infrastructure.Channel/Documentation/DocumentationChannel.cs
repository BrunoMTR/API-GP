using Domain.Channels;
using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Infrastructure.Channel.Documentation
{
    public class DocumentationChannel : IDocumentationChannel
    {
        private readonly Channel<DocumentMessageDto> _channel;

        public DocumentationChannel()
        {
            var options = new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = false
            };
            _channel = System.Threading.Channels.Channel.CreateUnbounded<DocumentMessageDto>(options); // Fully qualify the namespace
        }

        public IAsyncEnumerable<DocumentMessageDto> ReadAllAsync(CancellationToken cancellationToken)
            => _channel.Reader.ReadAllAsync(cancellationToken);

        public async Task<bool> SubmitAsync(DocumentMessageDto message, CancellationToken cancellationToken)
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
