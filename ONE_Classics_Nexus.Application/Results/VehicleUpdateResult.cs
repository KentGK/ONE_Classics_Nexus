using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONE_Classics_Nexus.Application.Results
{
    public class VehicleUpdateResult
    {
        public bool IsNotFound { get; private set; }
        public bool IsConcurrencyConflict { get; private set; }
        public long? CurrentRowVersion { get; private set; }

        private VehicleUpdateResult() { }

        public static VehicleUpdateResult NotFound()
        {
            return new VehicleUpdateResult { IsNotFound = true };
        }

        public static VehicleUpdateResult ConcurrencyConflict(long currentRowVersion)
        {
            return new VehicleUpdateResult
            {
                IsConcurrencyConflict = true,
                CurrentRowVersion = currentRowVersion
            };
        }
    }
}
