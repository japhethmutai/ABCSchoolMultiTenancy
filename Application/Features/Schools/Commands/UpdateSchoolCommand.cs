using Application.Models.Wrapper;
using Application.Pipelines;
using MediatR;

namespace Application.Features.Schools.Commands
{
    public class UpdateSchoolCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public UpdateSchoolRequest SchoolRequest { get; set; }
    }

    public class UpdateSchoolCommandHandler(ISchoolService schoolService) : IRequestHandler<UpdateSchoolCommand, IResponseWrapper>
    {
        private readonly ISchoolService _schoolService = schoolService;

        public async Task<IResponseWrapper> Handle(UpdateSchoolCommand request, CancellationToken cancellationToken)
        {
            var schoolInDb = await _schoolService.GetSchoolByIdAsync(request.SchoolRequest.Id);

            schoolInDb.Name = request.SchoolRequest.Name;
            schoolInDb.EstablishedOn = request.SchoolRequest.EstablishedOn;

            var updatedSchoolId = await _schoolService.UpdateSchoolAsync(schoolInDb);

            return await ResponseWrapper<int>.SuccessAsync(data: updatedSchoolId, message: "School updated successfully.");
        }
    }
}
