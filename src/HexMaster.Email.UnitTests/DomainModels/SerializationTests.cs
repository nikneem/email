﻿using System.Collections.Generic;
using System.Text.Json;
using HexMaster.Email.DomainModels;
using Xunit;

namespace HexMaster.Email.UnitTests.DomainModels;

public class SerializationTests
{
    [Fact]
    public void WhenEmailMessageIsSerialized_ItCanBeRecreated()
    {
        var substitutions = new Dictionary<string, string> { { "x", "y" } };
        var recipient = Recipient.Create("info@hexmaster.nl", substitutions: substitutions);
        var sender = new Sender("sender@domain.com");
        var body = new Body("default", "Hi there");
        var mailMessage = new Message(sender,  recipient , "Subject", body);

        var serialized = mailMessage.SerializeToJson();
        var restoredObject = Message.FromJson(serialized);

        Assert.Equal(mailMessage.Subject, restoredObject.Subject);

    }

    [Fact]
    public void WhenSubstitutionIsSerialized_ItCanBeDeserialized()
    {
        var originalSubstitution = new Substitution("key", "value");
        var json = JsonSerializer.Serialize(originalSubstitution);
        var restoredSubstitution = JsonSerializer.Deserialize<Substitution>(json);
        Assert.NotNull(restoredSubstitution);
        if (restoredSubstitution != null)
        {
            Assert.Equal(originalSubstitution.Key, restoredSubstitution.Key);
            Assert.Equal(originalSubstitution.Value, restoredSubstitution.Value);
        }
    }

    [Fact]
    public void WhenRecipient()
    {
        var opts = new JsonSerializerOptions();
        opts.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        opts.PropertyNameCaseInsensitive = true;
        var recipient = Recipient.Create("my-email@domain.com", "Foo Bar",
            new Dictionary<string, string> {{"Firstname", "Foo"}, {"Lastname", "Bar"}});
        var json = JsonSerializer.Serialize(recipient);

        var restoredSubstitution = JsonSerializer.Deserialize<Recipient>(json);
        Assert.NotNull(restoredSubstitution);
        if (restoredSubstitution != null)
        {
            Assert.Equal(recipient.EmailAddress, restoredSubstitution.EmailAddress);
        }
    }

}


