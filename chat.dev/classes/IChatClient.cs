namespace chat.dev.classes;

public interface IChatClient
{
    Task ReceiveMessage(string user, string message);
}
