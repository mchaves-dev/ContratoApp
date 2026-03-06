using ContratoApp.Dominio.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ContratoApp.Aplicacao;

/// <summary>
/// Implementation of IEventPublisher that resolves all handlers for an event and executes them in parallel
/// </summary>
public class EventPublisher(IServiceProvider serviceProvider, ILogger<EventPublisher> logger)
    : IEventPublisher
{
    public async Task PublishAsync<TEvent>(
        TEvent @event,
        CancellationToken cancellationToken)
        where TEvent : IEvent
    {
        var eventType = @event.GetType();
        logger.LogDebug("Publishing event {EventType}", eventType.Name);

        try
        {
            var handlers = serviceProvider.GetServices<IEventHandler<TEvent>>().ToArray();

            if (handlers.Length == 0)
            {
                logger.LogDebug("No handlers registered for event {EventType}", eventType.Name);
                return;
            }

            logger.LogDebug("Found {HandlerCount} handlers for event {EventType}", handlers.Length, eventType.Name);

            var handlerTasks = handlers
                .Select(handler => ExecuteHandlerAsync(handler, @event, cancellationToken))
                .ToList();

            await Task.WhenAll(handlerTasks);

            var exceptions = handlerTasks
                .Select(t => t.Result)
                .Where(ex => ex is not null)
                .ToList();

            if (exceptions.Count > 0)
            {
                logger.LogError("One or more handlers threw exceptions while processing event {EventType}", eventType.Name);
                throw new AggregateException(
                    $"One or more handlers threw exceptions while processing event {eventType.Name}",
                    exceptions!);
            }

            logger.LogDebug("Successfully published event {EventType}", eventType.Name);
        }
        catch (AggregateException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error publishing event {EventType}", eventType.Name);
            throw;
        }
    }

    private async Task<Exception?> ExecuteHandlerAsync<TEvent>(
        IEventHandler<TEvent> handler,
        TEvent @event,
        CancellationToken cancellationToken) where TEvent : IEvent
    {
        try
        {
            await handler.HandleAsync(@event, cancellationToken);
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error handling event {EventType} with handler {HandlerType}",
                @event.GetType().Name, handler.GetType().Name);
            return ex;
        }
    }
}
