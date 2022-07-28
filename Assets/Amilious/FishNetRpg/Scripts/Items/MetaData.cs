using System;
using System.Collections.Generic;
using System.Globalization;
using Amilious.Core.Extensions;

namespace Amilious.FishNetRpg.Items {
    
    [System.Serializable]
    public abstract class Metadata {

        public string Creator;

        public string CreatedOn;

        public void SetCreatedTime() {
            CreatedOn = DateTime.UtcNow.ToString("O",CultureInfo.InvariantCulture);
        }
        
        public DateTime GetCreatedTime() => 
            DateTime.Parse(CreatedOn, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        public DateTime GetCreatedTimeLocal() => GetCreatedTime().ToLocalTime();

    }
}