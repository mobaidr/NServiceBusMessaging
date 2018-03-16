using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using System;

[Route("api/[controller]")]
public class SendMessageController :
    Controller
{
    IMessageSession messageSession;

    #region MessageSessionInjection
    public SendMessageController(IMessageSession messageSession)
    {
        this.messageSession = messageSession;
    }
    #endregion


    #region MessageSessionUsage
    [HttpGet]
    public async Task<string> Get()
    {
        //if (messageSession == null)
        //    return "MessageSession is NULL";

        //return "Hello from SendMessageController";
        var message = new MyMessage { MessageId = Guid.NewGuid() };
        try
        {
            await messageSession.Send(message)
               .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }

        return $"Message sent to endpoint : {message.MessageId}";
    }
    #endregion
}
