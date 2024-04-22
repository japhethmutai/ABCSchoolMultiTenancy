using Application.Models.Wrapper;
using Mapster;
using MediatR;

namespace Application.Features.Schools.Queries
{
    public class GetAllSchoolsQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetAllSchoolsQueryHandler(ISchoolService schoolService) : IRequestHandler<GetAllSchoolsQuery, IResponseWrapper>
    {
        private readonly ISchoolService _schoolService = schoolService;

        public async Task<IResponseWrapper> Handle(GetAllSchoolsQuery request, CancellationToken cancellationToken)
        {
            var schools = await _schoolService.GetAllSchoolsAsync();
            
            if (schools is not null)
            {
                return await ResponseWrapper<List<SchoolResponse>>.SuccessAsync(data: schools.Adapt<List<SchoolResponse>>());
            }
            return await ResponseWrapper<string>.FailAsync(message: "No schools found.");
        }
    }
}
