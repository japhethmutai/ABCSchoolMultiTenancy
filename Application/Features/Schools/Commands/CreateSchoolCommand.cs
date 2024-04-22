using Application.Models.Wrapper;
using Application.Pipelines;
using Domain.Entities;
using Mapster;
using MediatR;

namespace Application.Features.Schools.Commands
{
    public class CreateSchoolCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public CreateSchoolRequest SchoolRequest { get; set; }
    }

    public class CreateSchoolCommandHandler(ISchoolService schoolService) : IRequestHandler<CreateSchoolCommand, IResponseWrapper>
    {
        private readonly ISchoolService _schoolService = schoolService;

        public async Task<IResponseWrapper> Handle(CreateSchoolCommand request, CancellationToken cancellationToken)
        {
            var SchoolDomain = request.SchoolRequest.Adapt<School>();

            var schoolId = await _schoolService.CreateSchoolAsync(SchoolDomain);

            return await ResponseWrapper<int>.SuccessAsync(data: schoolId, message: "School created successfully.");
        }
    }
}
