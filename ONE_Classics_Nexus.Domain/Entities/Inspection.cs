using System;
using System.Collections.Generic;

namespace ONE_Classics_Nexus.Domain.Entities
{
    public sealed class Inspection
    {
        public Guid InspectionId { get; set; }
        public Guid AppraisalId { get; set; }
        public string InspectionType { get; set; } = "Standard";
        public DateTime CreatedAtUtc { get; set; }
        public DateTime UpdatedAtUtc { get; set; }
        public List<ConditionScore> ConditionScores { get; set; } = new List<ConditionScore>();

        public static Inspection Create(Guid appraisalId, string? inspectionType = null)
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

            ConditionScores = new List<ConditionScore>(scores);
            UpdatedAtUtc = DateTime.UtcNow;
        }
    }
}