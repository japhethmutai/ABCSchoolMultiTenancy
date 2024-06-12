using Application.Features.Teachers;
using Application.Models.Wrapper;
using Application.Pipelines;
using Domain.Entities;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Teachers.Commands
{
    public class CreateTeacherCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public CreateTeacherRequest TeacherRequest { get; set; }
    }

    public class CreateTeacherCommandHandler(ITeacherService teacherService) : IRequestHandler<CreateTeacherCommand, IResponseWrapper>
    {
        private readonly ITeacherService _teacherService = teacherService;

        public async Task<IResponseWrapper> Handle(CreateTeacherCommand request, CancellationToken cancellationToken)
        {
            var TeacherDomain = request.TeacherRequest.Adapt<Teacher>();

            var teacherId = await _teacherService.CreateTeacherAsync(TeacherDomain);

            return await ResponseWrapper<int>.SuccessAsync(data: teacherId, message: "Teacher created successfully.");
        }
    }
}
