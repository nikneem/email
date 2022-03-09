using HexMaster.Email.DomainModels;
using Xunit;

namespace HexMaster.Email.UnitTests.DomainModels;

public class SenderTests
{
    [Fact]
    public void WhenSenderWithoutNameCreated_EmailAndNameAreEqual()
    {
        var emailAddress = "email@domain.com";
        var sender = new Sender(emailAddress);
        Assert.Equal(emailAddress, sender.EmailAddress);
        Assert.Equal(emailAddress, sender.Name);
    }
}