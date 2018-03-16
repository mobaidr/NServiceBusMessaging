using NServiceBus;
using System;

public class MyMessage :
    IMessage
{
    public Guid MessageId { get; set; }
}