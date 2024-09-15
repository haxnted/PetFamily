﻿using System.Threading.Channels;
using PetFamily.Application.Messaging;

namespace PetFamily.Infrastructure.MessageQueues;

public class InMemoryMessageQueues<TMessage> : IMessageQueue<TMessage>
{
    private readonly Channel<TMessage> _channel = Channel.CreateUnbounded<TMessage>();

    public async Task WriteAsync(TMessage paths, CancellationToken token = default) =>
        await _channel.Writer.WriteAsync(paths, token);

    public async Task<TMessage> ReadAsync(CancellationToken token = default) =>
        await _channel.Reader.ReadAsync(token);
}