using LMS.Features.ActivityManagement.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace LMS.Features.ActivityManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ActivityController
    {
        private readonly ActivityService _activityService;
        public ActivityController( ActivityService activityService)
        {
            _activityService = activityService;
        }
    }


}
