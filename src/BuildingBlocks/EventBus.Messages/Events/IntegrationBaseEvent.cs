namespace EventBus.Messages.Events;

public abstract class IntegrationBaseEvent
{
    public Guid Id { get; private set; }
    public DateTime CreationDate { get; private set; }

    public IntegrationBaseEvent()
    {
        this.Id = Guid.NewGuid();
        this.CreationDate = DateTime.UtcNow;
    }


    public IntegrationBaseEvent(Guid id, DateTime creationDate)
    {
        this.Id = id;
        this.CreationDate = creationDate;
    }
}