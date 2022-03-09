using HexMaster.Email.DomainModels;
using HexMaster.Email.Exceptions;
using Xunit;

namespace HexMaster.Email.UnitTests.DomainModels;

public class ValidEmailAddressTests
{
    [Theory]
    [InlineData("info@bol.com")]
    [InlineData("info@hexmaster.nl")]
    [InlineData("info+spam@hexmaster.nl")]
    [InlineData("info+spam@hex.master.nl")]
    [InlineData("info.spam@hexmaster.nl")]
    [InlineData("info.spam+test@hexmaster.nl")]
    public void WhenEmailAddressIsValid_TheEmailAddressCanBeSet(string emailAddress)
    {
        var recipient = Recipient.Create(emailAddress);
        Assert.Equal(recipient.EmailAddress, emailAddress);

    }

    [Theory]
    [InlineData("info@bol com")]
    [InlineData("info@hexmaster nl")]
    [InlineData("info@hex master.nl")]
    [InlineData("info@ hexmaster.nl")]
    [InlineData("info @hexmaster.nl")]
    [InlineData("info spam@hexmaster.nl")]
    public void WhenEmailAddressIsInvalid_ItThrowsInvalidEmailAddressException(string emailAddress)
    {
        var exception = Assert.Throws<InvalidEmailAddressException>(() => Recipient.Create(emailAddress));
         Assert.True(exception.Message.Contains(emailAddress));

    }
}