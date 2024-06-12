using Application.Features.Teachers.Commands;
using Application.Features.Teachers;
using Infrastructure.OpenApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : BaseApiController
    {
        [HttpPost("add")]
        public async Task<IActionResult> CreateTeacherAsync([FromBody] CreateTeacherRequest createTeacher)
        {
            var response = await Sender.Send(new CreateTeacherCommand { TeacherRequest = createTeacher });

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
