using System.Resources;

namespace Streetcode.WebApi.Resources
{
    public static class ErrorMessages
    {
        private static ResourceManager _resourceManager = new ResourceManager("Streetcode.WebApi.Resources.ErrorMessages", typeof(ErrorMessages).Assembly);

        public static string GetErrorMessage(string key)
        {
            return _resourceManager.GetString(key);
        }
    }
}
