using Application.Models.Wrapper;
using Application.Pipelines;
using MediatR;

namespace Application.Features.Schools.Commands
{
    public class DeleteSchoolCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public int SchoolId { get; set; }
    }

    public class DeleteSchoolCommandHandler(ISchoolService schoolService) : IRequestHandler<DeleteSchoolCommand, IResponseWrapper>
    {
        private readonly ISchoolService _schoolService = schoolService;

        public async Task<IResponseWrapper> Handle(DeleteSchoolCommand request, CancellationToken cancellationToken)
        {
            var schoolInDb = await _schoolService.GetSchoolByIdAsync(request.SchoolId);

            var deletedSchoolId = await _schoolService.DeleteSchoolAsync(schoolInDb);

            return await ResponseWrapper<int>.SuccessAsync(data: deletedSchoolId, message: "School deleted successfully.");
        }
    }
}
