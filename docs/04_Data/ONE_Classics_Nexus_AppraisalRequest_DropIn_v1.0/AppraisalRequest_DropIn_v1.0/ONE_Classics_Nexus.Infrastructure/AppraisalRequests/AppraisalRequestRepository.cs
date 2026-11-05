using Dapper;
using ONE_Classics_Nexus.Application.AppraisalRequests;
using ONE_Classics_Nexus.Domain.AppraisalRequests;

namespace ONE_Classics_Nexus.Infrastructure.AppraisalRequests;

public sealed class AppraisalRequestRepository(IAppraisalRequestConnectionFactory factory) : IAppraisalRequestRepository
{
    private const string SelectColumns=@"appraisal_request_id AppraisalRequestId, request_number RequestNumber, client_id ClientId,
client_contact_id ClientContactId, vehicle_id VehicleId, converted_appraisal_id ConvertedAppraisalId, request_status RequestStatus,
request_source RequestSource, requested_appraisal_type RequestedAppraisalType, requested_purpose RequestedPurpose,
requested_intended_use RequestedIntendedUse, requested_value_type RequestedValueType, requested_effective_date RequestedEffectiveDate,
requested_completion_date RequestedCompletionDate, inspection_preference InspectionPreference, inspection_location InspectionLocation,
subject_year SubjectYear, subject_make SubjectMake, subject_model SubjectModel, subject_vin SubjectVin, subject_description SubjectDescription,
client_comments ClientComments, internal_notes InternalNotes, requester_acknowledged_verification RequesterAcknowledgedVerification,
qualification_status QualificationStatus, qualification_notes QualificationNotes, assigned_to_user_id AssignedToUserId,
qualified_by_user_id QualifiedByUserId, received_at_utc ReceivedAtUtc, reviewed_at_utc ReviewedAtUtc, qualified_at_utc QualifiedAtUtc,
converted_at_utc ConvertedAtUtc, closed_at_utc ClosedAtUtc, row_version RowVersion, created_at_utc CreatedAtUtc, updated_at_utc UpdatedAtUtc";

    public async Task<AppraisalRequest?> GetByIdAsync(Guid id,CancellationToken ct)
    { await using var c=await factory.OpenAsync(ct); var row=await c.QuerySingleOrDefaultAsync<Row>(new CommandDefinition($"SELECT {SelectColumns} FROM appraisal_requests WHERE appraisal_request_id=@Id",new{Id=id},cancellationToken:ct)); return row?.ToEntity(); }
    public async Task<AppraisalRequest?> GetByRequestNumberAsync(string n,CancellationToken ct)
    { await using var c=await factory.OpenAsync(ct); var row=await c.QuerySingleOrDefaultAsync<Row>(new CommandDefinition($"SELECT {SelectColumns} FROM appraisal_requests WHERE request_number=@N",new{N=n},cancellationToken:ct)); return row?.ToEntity(); }

    public async Task<PagedResult<AppraisalRequestSummary>> SearchAsync(AppraisalRequestSearchCriteria q,CancellationToken ct)
    {
        var page=Math.Max(1,q.Page); var size=Math.Clamp(q.PageSize,1,200); var offset=(page-1)*size;
        var where=new List<string>{"1=1"}; var p=new DynamicParameters();
        void Add(string sql,string name,object? value){if(value is not null){where.Add(sql);p.Add(name,value);}}
        Add("request_status=@Status","Status",q.Status); Add("client_id=@ClientId","ClientId",q.ClientId); Add("assigned_to_user_id=@Assigned","Assigned",q.AssignedToUserId);
        Add("received_at_utc>=@From","From",q.ReceivedFrom); Add("received_at_utc<@To","To",q.ReceivedTo); Add("request_number LIKE CONCAT('%',@Number,'%')","Number",q.RequestNumber);
        Add("subject_vin LIKE CONCAT('%',@Vin,'%')","Vin",q.Vin); Add("subject_make LIKE CONCAT('%',@Make,'%')","Make",q.Make); Add("subject_model LIKE CONCAT('%',@Model,'%')","Model",q.Model);
        var w=string.Join(" AND ",where); p.Add("Offset",offset); p.Add("Size",size);
        await using var c=await factory.OpenAsync(ct);
        var total=await c.ExecuteScalarAsync<long>(new CommandDefinition($"SELECT COUNT(*) FROM appraisal_requests WHERE {w}",p,cancellationToken:ct));
        var sql=$@"SELECT appraisal_request_id AppraisalRequestId,request_number RequestNumber,client_id ClientId,request_status RequestStatus,
qualification_status QualificationStatus,assigned_to_user_id AssignedToUserId,received_at_utc ReceivedAtUtc,requested_completion_date RequestedCompletionDate,
subject_year SubjectYear,subject_make SubjectMake,subject_model SubjectModel,subject_vin SubjectVin,requested_appraisal_type RequestedAppraisalType,
requested_purpose RequestedPurpose,row_version RowVersion FROM appraisal_requests WHERE {w} ORDER BY received_at_utc DESC LIMIT @Size OFFSET @Offset";
        var items=(await c.QueryAsync<AppraisalRequestSummary>(new CommandDefinition(sql,p,cancellationToken:ct))).AsList(); return new(items,page,size,total);
    }

    public async Task InsertAsync(AppraisalRequest x,CancellationToken ct)
    {
        const string sql=@"INSERT INTO appraisal_requests(appraisal_request_id,request_number,client_id,client_contact_id,vehicle_id,converted_appraisal_id,
request_status,request_source,requested_appraisal_type,requested_purpose,requested_intended_use,requested_value_type,requested_effective_date,
requested_completion_date,inspection_preference,inspection_location,subject_year,subject_make,subject_model,subject_vin,subject_description,
client_comments,internal_notes,requester_acknowledged_verification,qualification_status,qualification_notes,assigned_to_user_id,qualified_by_user_id,
received_at_utc,reviewed_at_utc,qualified_at_utc,converted_at_utc,closed_at_utc,row_version,created_at_utc,updated_at_utc)
VALUES(@AppraisalRequestId,@RequestNumber,@ClientId,@ClientContactId,@VehicleId,@ConvertedAppraisalId,@RequestStatus,@RequestSource,@RequestedAppraisalType,
@RequestedPurpose,@RequestedIntendedUse,@RequestedValueType,@RequestedEffectiveDate,@RequestedCompletionDate,@InspectionPreference,@InspectionLocation,
@SubjectYear,@SubjectMake,@SubjectModel,@SubjectVin,@SubjectDescription,@ClientComments,@InternalNotes,@RequesterAcknowledgedVerification,
@QualificationStatus,@QualificationNotes,@AssignedToUserId,@QualifiedByUserId,@ReceivedAtUtc,@ReviewedAtUtc,@QualifiedAtUtc,@ConvertedAtUtc,@ClosedAtUtc,
@RowVersion,@CreatedAtUtc,@UpdatedAtUtc)";
        await using var c=await factory.OpenAsync(ct); await c.ExecuteAsync(new CommandDefinition(sql,Params(x),cancellationToken:ct));
    }

    public async Task<bool> UpdateAsync(AppraisalRequest x,long expected,CancellationToken ct)
    {
        const string sql=@"UPDATE appraisal_requests SET client_id=@ClientId,client_contact_id=@ClientContactId,vehicle_id=@VehicleId,
converted_appraisal_id=@ConvertedAppraisalId,request_status=@RequestStatus,request_source=@RequestSource,requested_appraisal_type=@RequestedAppraisalType,
requested_purpose=@RequestedPurpose,requested_intended_use=@RequestedIntendedUse,requested_value_type=@RequestedValueType,
requested_effective_date=@RequestedEffectiveDate,requested_completion_date=@RequestedCompletionDate,inspection_preference=@InspectionPreference,
inspection_location=@InspectionLocation,subject_year=@SubjectYear,subject_make=@SubjectMake,subject_model=@SubjectModel,subject_vin=@SubjectVin,
subject_description=@SubjectDescription,client_comments=@ClientComments,internal_notes=@InternalNotes,requester_acknowledged_verification=@RequesterAcknowledgedVerification,
qualification_status=@QualificationStatus,qualification_notes=@QualificationNotes,assigned_to_user_id=@AssignedToUserId,qualified_by_user_id=@QualifiedByUserId,
reviewed_at_utc=@ReviewedAtUtc,qualified_at_utc=@QualifiedAtUtc,converted_at_utc=@ConvertedAtUtc,closed_at_utc=@ClosedAtUtc,
row_version=@RowVersion,updated_at_utc=@UpdatedAtUtc WHERE appraisal_request_id=@AppraisalRequestId AND row_version=@Expected";
        var p=new DynamicParameters(Params(x)); p.Add("Expected", expected); await using var c=await factory.OpenAsync(ct); return await c.ExecuteAsync(new CommandDefinition(sql,p,cancellationToken:ct))==1;
    }

    private static dynamic Params(AppraisalRequest x)=>new {
        x.AppraisalRequestId,x.RequestNumber,x.ClientId,x.ClientContactId,x.VehicleId,x.ConvertedAppraisalId,RequestStatus=x.RequestStatus.ToString(),x.RequestSource,
        x.RequestedAppraisalType,x.RequestedPurpose,x.RequestedIntendedUse,x.RequestedValueType,x.RequestedEffectiveDate,x.RequestedCompletionDate,x.InspectionPreference,
        x.InspectionLocation,x.SubjectYear,x.SubjectMake,x.SubjectModel,x.SubjectVin,x.SubjectDescription,x.ClientComments,x.InternalNotes,x.RequesterAcknowledgedVerification,
        QualificationStatus=x.QualificationStatus.ToString(),x.QualificationNotes,x.AssignedToUserId,x.QualifiedByUserId,x.ReceivedAtUtc,x.ReviewedAtUtc,x.QualifiedAtUtc,
        x.ConvertedAtUtc,x.ClosedAtUtc,x.RowVersion,x.CreatedAtUtc,x.UpdatedAtUtc };

    private sealed class Row
    {
        public Guid AppraisalRequestId{get;set;} public string RequestNumber{get;set;}=""; public Guid ClientId{get;set;} public Guid? ClientContactId{get;set;}
        public Guid? VehicleId{get;set;} public Guid? ConvertedAppraisalId{get;set;} public string RequestStatus{get;set;}=""; public string? RequestSource{get;set;}
        public string RequestedAppraisalType{get;set;}=""; public string RequestedPurpose{get;set;}=""; public string? RequestedIntendedUse{get;set;} public string? RequestedValueType{get;set;}
        public DateOnly? RequestedEffectiveDate{get;set;} public DateOnly? RequestedCompletionDate{get;set;} public string? InspectionPreference{get;set;} public string? InspectionLocation{get;set;}
        public int? SubjectYear{get;set;} public string? SubjectMake{get;set;} public string? SubjectModel{get;set;} public string? SubjectVin{get;set;} public string? SubjectDescription{get;set;}
        public string? ClientComments{get;set;} public string? InternalNotes{get;set;} public bool RequesterAcknowledgedVerification{get;set;} public string QualificationStatus{get;set;}="";
        public string? QualificationNotes{get;set;} public Guid? AssignedToUserId{get;set;} public Guid? QualifiedByUserId{get;set;} public DateTime ReceivedAtUtc{get;set;}
        public DateTime? ReviewedAtUtc{get;set;} public DateTime? QualifiedAtUtc{get;set;} public DateTime? ConvertedAtUtc{get;set;} public DateTime? ClosedAtUtc{get;set;}
        public long RowVersion{get;set;} public DateTime CreatedAtUtc{get;set;} public DateTime UpdatedAtUtc{get;set;}
        public AppraisalRequest ToEntity()=>AppraisalRequest.Rehydrate(new(AppraisalRequestId,RequestNumber,ClientId,ClientContactId,VehicleId,ConvertedAppraisalId,
            Enum.Parse<AppraisalRequestStatus>(RequestStatus),RequestSource,RequestedAppraisalType,RequestedPurpose,RequestedIntendedUse,RequestedValueType,
            RequestedEffectiveDate,RequestedCompletionDate,InspectionPreference,InspectionLocation,SubjectYear,SubjectMake,SubjectModel,SubjectVin,SubjectDescription,
            ClientComments,InternalNotes,RequesterAcknowledgedVerification,Enum.Parse<AppraisalRequestQualificationStatus>(QualificationStatus),QualificationNotes,
            AssignedToUserId,QualifiedByUserId,ReceivedAtUtc,ReviewedAtUtc,QualifiedAtUtc,ConvertedAtUtc,ClosedAtUtc,RowVersion,CreatedAtUtc,UpdatedAtUtc));
    }
}
