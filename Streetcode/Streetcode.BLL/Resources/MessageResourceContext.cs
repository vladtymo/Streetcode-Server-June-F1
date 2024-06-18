using System.Globalization;
using System.Resources;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Streetcode.BLL.Resources
{
    public class MessageResourceContext
    {
        private static readonly ResourceManager _resourceManager = new ResourceManager("Streetcode.BLL.Resources.ErrorMessages", typeof(ErrorMessages).Assembly);
        private static string? _entityId;
        private static string? _entityName;
        public static string GetMessage(string error, params object[] formatValue)
        {
            string requestType = formatValue[0].GetType().ToString();
            _entityName = EntityFilter(requestType);

            if(formatValue.Length > 1)
            {
                _entityId = formatValue[1].ToString();
            }

            if (_entityName != null)
            {
                try
                {
                   return string.Format(error, _entityName, _entityId);
                }
                catch
                {
                    return "An unknown error occurred.";
                }
            }

            return "An unknown error occurred.";
        }

        private static string EntityFilter(string input)
        {
            // Define a regex pattern to match the entity name
            var regex = new Regex(@"\.MediatR(?:\.AdditionalContent)?\.(\w+)\.");

            // Match the pattern
            var match = regex.Match(input);

            // Check if the match is successful
            if (match.Success)
            {
                // Capture the group that matched
                return match.Groups[1].Value;
            }
            else
            {
                return null!;
            }
        }
    }
}