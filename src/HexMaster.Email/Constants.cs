namespace HexMaster.Email
{
    public class Constants
    {
    
    }

    public static class RegularExpression
    {
        public const string Email = "^[\\w-\\+\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$";
        public const string Html = "<(\"[^\"]*\"|'[^']*'|[^'\">])*>";
    }
}