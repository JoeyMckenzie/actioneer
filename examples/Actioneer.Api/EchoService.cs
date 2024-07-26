namespace Actioneer.Api;

public class EchoService(ILogger<EchoService> logger)
{
    public string Echo(string phrase)
    {
        logger.LogInformation("Received echo request");

        return phrase;
    }
}
