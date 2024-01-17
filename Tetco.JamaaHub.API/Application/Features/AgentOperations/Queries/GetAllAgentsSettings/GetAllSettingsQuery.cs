using Application.Common.Interfaces.AsasLandingzoneDb;
using Domain.Common.Patterns;

namespace Application.Features.AgentOperations.Queries.GetAllSettings
{

    public sealed record GetAllSettingsRes(int nextPage, int totalPages, bool hasNextPage, object records);
    public sealed class GetAllSettingsQuery : IRequest<Result<GetAllSettingsRes>>
    {
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 10;
    }
    public sealed class GetAllSettingsQueryHandler : IRequestHandler<GetAllSettingsQuery, Result<GetAllSettingsRes>>
    {

        private readonly IAsasLandZoneDb _db;

        public GetAllSettingsQueryHandler(IAsasLandZoneDb db)
        {
            _db = db;
        }

        public async Task<Result<GetAllSettingsRes>> Handle(GetAllSettingsQuery request, CancellationToken cancellationToken)
        {
            //var totalRecords = await _db.Settings.CountAsync ( );
            //int totalPages = ( int ) Math.Ceiling ( ( double ) totalRecords / request.pageSize );

            //var data = await _db.Settings
            //    .Select ( x => new
            //       {
            //       x.Id ,
            //       x.SettingKey ,
            //       x.SettingValue ,
            //       } )
            //    .Skip ( ( request.pageNumber - 1 ) * request.pageSize )
            //    .Take ( request.pageSize )
            //    .ToListAsync ( );
            //return Result<GetAllSettingsRes>.Success ( "data retreived successfully" )
            //    .WithData (
            //       new (
            //           totalPages > request.pageNumber ? request.pageNumber + 1 : request.pageNumber ,
            //           totalPages ,
            //           request.pageNumber < totalPages ,
            //           data ) );

            return Result<GetAllSettingsRes>.Success("data retreived successfully");
        }
    }
}
