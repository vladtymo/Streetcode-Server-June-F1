using mk.profanity;
using Edi.WordFilter;

namespace Streetcode.BLL.Util.Comments
{
    public class BadWordsVerification
    {
        private const string ApiUrl = "http://api.languagetool.org/v2/check";
        public static bool NotContainBadWords(string text)
        {
            ProfanityFilter profanityFilter = new ProfanityFilter();

            return !profanityFilter.ContainsProfanity(text);
        }
    }
}
