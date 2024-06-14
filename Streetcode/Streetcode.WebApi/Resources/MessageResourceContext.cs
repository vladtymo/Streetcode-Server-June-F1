using System.Globalization;
using System.Resources;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Streetcode.WebApi.Resources
{
    public class MessageResourceContext
    {
        private static ResourceManager _resourceManager = new ResourceManager("Streetcode.WebApi.Resources.ErrorMessages", typeof(ErrorMessages).Assembly);
        private static string _entityId;
        private static string _entityName;
        public static string GetMessage(string name)
        {
            var typeOfError = ErrorFilter(name);
            string message = _resourceManager.GetString(typeOfError, CultureInfo.CurrentCulture);
           
            if (message != null)
            {
                try
                {
                    return string.Format(message, _entityName, _entityId);
                }
                catch
                {
                    return "An unknown error occurred.";
                }
            }

            return "An unknown error occurred.";
        }

        private static string ErrorFilter(string error)
        {
            _entityName = error.Split(' ')[3];
            if (error.Contains("any"))
            {
                _entityName = error.Split(' ').Last();
                return "EntityNotFound";
            }

            if(error.Contains("Failed to create an"))
            {
                return "FailToCreateAn";
            }

            if (error.Contains("Failed to create a"))
            {
                return "FailToCreateA";
            }

            _entityId = error.Split(':')[1].Trim();

            if(_entityId != null)
            {
                return "EntityWithIdNotFound";
            }

            return error;
        }
    }
}