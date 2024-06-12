using Application.Features.Schools;
using Application.Features.Schools.Commands;
using Application.Features.Schools.Queries;
using Infrastructure.Identity.Auth;
using Infrastructure.Identity.Constants;
using Infrastructure.OpenApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolsController : BaseApiController
    {
        [HttpPost("add")]
        //[ShouldHavePermission(SchoolAction.Create, SchoolFeature.Schools)]
        [AllowAnonymous]
        [TenantHeader]
        public async Task<IActionResult> CreateSchoolAsync([FromBody] CreateSchoolRequest createSchool)
        {
            var response = await Sender.Send(new CreateSchoolCommand { SchoolRequest = createSchool });

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("update")]
        [ShouldHavePermission(SchoolAction.Update, SchoolFeature.Schools)]
        public async Task<IActionResult> UpdateSchoolAsync([FromBody] UpdateSchoolRequest updateSchool)
        {
            var response = await Sender.Send(new UpdateSchoolCommand { SchoolRequest = updateSchool });

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("{schoolId}")]
        [ShouldHavePermission(SchoolAction.Delete, SchoolFeature.Schools)]
        public async Task<IActionResult> DeleteSchoolAsync(int schoolId)
        {
            var response = await Sender.Send(new DeleteSchoolCommand { SchoolId = schoolId });

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("by-id/{schoolId}")]
        [ShouldHavePermission(SchoolAction.View, SchoolFeature.Schools)]
        public async Task<IActionResult> GetSchoolByIdAsync(int schoolId)
        {
            var response = await Sender.Send(new GetSchoolByIdQuery { SchoolId = schoolId });

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("by-name/{name}")]
        [ShouldHavePermission(SchoolAction.View, SchoolFeature.Schools)]
        public async Task<IActionResult> GetSchoolByNameAsync(string name)
        {
            var response = await Sender.Send(new GetSchoolByNameQuery { Name = name });

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("all")]
        //[ShouldHavePermission(SchoolAction.View, SchoolFeature.Schools)]
        [AllowAnonymous]
        [TenantHeader]
        public async Task<IActionResult> GetAllSchoolsAsync()
        {
            var response = await Sender.Send(new GetAllSchoolsQuery());

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
    }
}
