using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Web.Interfaces.Base;
using System.Linq.Expressions;
using System.Security.Claims;

namespace SnowaTec.Test.Web.Pages.Admin
{
    [Authorize]
    public class DashboardModel : PageModel
    {
        [Inject]
        private ICallApiService _service { get; }

        [Inject]
        private IMapper _mapper { get; }

        public DashboardModel(ICallApiService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
    }
}
