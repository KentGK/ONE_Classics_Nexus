using System;
using System.Collections.Generic;

namespace ONE_Classics_Nexus.Domain.Entities
{
    public sealed class Inspection
    {
        public Guid InspectionId { get; set; }
        public Guid AppraisalId { get; set; }
        public Guid InspectionTemplateId { get; set; }
        public Guid? InspectedByUserId { get; set; }
        public string InspectionType { get; set; } = "Standard";
        public string Status { get; set; } = string.Empty;
        public string? InspectionLocation { get; set; }
        public DateTime? InspectedOnUtc { get; set; }
        public decimal? CompletionPercentage { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime UpdatedAtUtc { get; set; }
        public List<ConditionScore> ConditionScores { get; set; } = new List<ConditionScore>();

        public static Inspection Create(Guid appraisalId, string inspectionType = null)
        {
            if (appraisalId == Guid.Empty)
                throw new ArgumentException("AppraisalId is required.", nameof(appraisalId));

            var now = DateTime.UtcNow;

            return new Inspection
            {
                InspectionId = Guid.NewGuid(),
                AppraisalId = appraisalId,
                InspectionType = string.IsNullOrWhiteSpace(inspectionType) ? "Standard" : inspectionType.Trim(),
                CreatedAtUtc = now,
                UpdatedAtUtc = now,
                ConditionScores = new List<ConditionScore>()
            };
        }

        public void ReplaceScores(IEnumerable<ConditionScore> scores)
        {
            if (scores is null)
                throw new ArgumentNullException(nameof(scores));

            // Avoid LINQ dependency by constructing a List directly
            ConditionScores = new List<ConditionScore>(scores);
            UpdatedAtUtc = DateTime.UtcNow;
        }
    }
}