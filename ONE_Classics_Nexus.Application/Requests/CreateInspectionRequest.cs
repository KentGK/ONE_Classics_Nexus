using System;

namespace ONE_Classics_Nexus.Application.Requests;

public sealed class CreateInspectionRequest
{
    public Guid AppraisalId { get; set; }
    public string InspectionType { get; set; } = "Standard";
}