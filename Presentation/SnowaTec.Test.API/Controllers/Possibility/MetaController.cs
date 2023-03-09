using Microsoft.AspNetCore.Mvc;
using SnowaTec.Test.API;
using System.Diagnostics;

namespace SnowaTec.Test.API.Controllers.Possibility
{
    public class MetaController : ControllerBase
    {
        [HttpGet("/info")]
        public ActionResult<string> Info()
        {
            var assembly = typeof(Startup).Assembly;

            var lastUpdate = System.IO.File.GetLastWriteTime(assembly.Location);
            var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

            return Ok($"Version: {version}, Last Updated: {lastUpdate}");
        }
    }
}
