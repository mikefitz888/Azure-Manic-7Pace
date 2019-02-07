using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTrackingService.Internal
{
    public enum ActivityType
    {
        NotSet = 0,
        Development,
        Internal,
        OutOfOffice,
        Planning,
        PreSales,
        QA,
        Support
    }
}
