namespace PetFamily.Application.Messaging;

public interface IMessageQueue<TMessage>
{
    public Task WriteAsync(TMessage paths, CancellationToken token = default);

    public Task<TMessage> ReadAsync(CancellationToken token = default);
}