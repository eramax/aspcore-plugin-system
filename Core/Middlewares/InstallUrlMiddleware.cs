using Microsoft.AspNetCore.Http;
using SharedKernel.Engines;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Core.Middlewares
{
    /// <summary>
    /// Represents middleware that checks whether database is installed and redirects to installation URL in otherwise
    /// </summary>
    public class InstallUrlMiddleware
    {
        #region Fields

        private readonly RequestDelegate _next;
        private readonly IEngine _engine;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly string InstallUrl = "/install";

        #endregion

        #region Ctor

        public InstallUrlMiddleware(RequestDelegate next, IEngine engine, IActionContextAccessor actionContextAccessor)
        {
            _next = next;
            _engine = engine;
            _actionContextAccessor = actionContextAccessor;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invoke middleware actions
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <returns>Task</returns>
        public async Task Invoke(HttpContext context)
        {
            var xx = _actionContextAccessor?.ActionContext;
            string x = context.Request.Path.Value;
            //whether database is installed
            if (!_engine.SystemInstalled && x != InstallUrl)
            {
                context.Response.Redirect("/install",false);
                return;
            }

            if (x == InstallUrl && _engine.SystemInstalled)
            {
                context.Response.Redirect("/", false);
                return;
            }

            //or call the next middleware in the request pipeline
            await _next(context);
        }

        #endregion
    }
}
