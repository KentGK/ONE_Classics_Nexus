using System.Collections.Generic;

namespace ONE_Classics_Nexus.Domain.Constants
{
    public static class InspectionSystems
    {
        public const string ExteriorBodyPaint = "exterior_body_paint";
        public const string GlassTrimLighting = "glass_trim_lighting";
        public const string InteriorCabin = "interior_cabin";
        public const string DashboardControls = "dashboard_controls";
        public const string EngineCompartment = "engine_compartment";
        public const string FuelExhaustCooling = "fuel_exhaust_cooling";
        public const string TransmissionDriveline = "transmission_driveline";
        public const string SuspensionSteeringBrakes = "suspension_steering_brakes";
        public const string ChassisUndercarriage = "chassis_undercarriage";
        public const string WheelsTires = "wheels_tires";
        public const string ElectricalAccessories = "electrical_accessories";
        public const string DocumentationAuthenticity = "documentation_authenticity";

        public static IReadOnlyList<InspectionSystemDefinition> All { get; } =
            new List<InspectionSystemDefinition>
            {
                new InspectionSystemDefinition(1, ExteriorBodyPaint, 1.00m),
                new InspectionSystemDefinition(2, GlassTrimLighting, 0.70m),
                new InspectionSystemDefinition(3, InteriorCabin, 0.90m),
                new InspectionSystemDefinition(4, DashboardControls, 0.60m),
                new InspectionSystemDefinition(5, EngineCompartment, 1.00m),
                new InspectionSystemDefinition(6, FuelExhaustCooling, 0.75m),
                new InspectionSystemDefinition(7, TransmissionDriveline, 0.95m),
                new InspectionSystemDefinition(8, SuspensionSteeringBrakes, 0.95m),
                new InspectionSystemDefinition(9, ChassisUndercarriage, 0.90m),
                new InspectionSystemDefinition(10, WheelsTires, 0.55m),
                new InspectionSystemDefinition(11, ElectricalAccessories, 0.65m),
                new InspectionSystemDefinition(12, DocumentationAuthenticity, 0.85m)
            };
    }

    public sealed class InspectionSystemDefinition
    {
        public InspectionSystemDefinition(int systemNumber, string systemKey, decimal weight)
        {
            SystemNumber = systemNumber;
            SystemKey = systemKey;
            Weight = weight;
        }

        public int SystemNumber { get; }
        public string SystemKey { get; }
        public decimal Weight { get; }
    }
}