namespace FamilyAssistant.Interfaces.Polling;

public interface IReceiverService
{
    Task ReceiveAsync(CancellationToken token);
}
