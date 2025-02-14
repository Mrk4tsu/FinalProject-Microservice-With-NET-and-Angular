using System.Text;

namespace FN.Utilities
{
    public class StringHelperBuilder
    {
       
    }
    
    public interface IStringHelperBuilder
    {
        void SetDomain(string domain);
        void SetToken(string token);
        void SetUserId(int userId);

    }
}
