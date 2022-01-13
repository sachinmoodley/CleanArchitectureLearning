using Microsoft.AspNetCore.Mvc;

namespace Web.Presenters
{
    public class RestPresenter : IActionResultPresenter
    {
        private IActionResult _actionResult;
        private string _error = "No response recorded";

        public void Success<TResponse>(TResponse response)
        {
            _actionResult = new OkObjectResult(response);
        }

        public void Error(string error)
        {
            _error = error;
        }

        public IActionResult Render()
        {
            if (_actionResult != null)
            {
                return new OkObjectResult(_actionResult);
            }

            return new BadRequestObjectResult(_error);
        }
    }
}