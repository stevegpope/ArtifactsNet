using System;
using System.Collections.Generic;
using System.Text;

namespace Artifacts
{
    // Represents a task to collect a number of items/kill monsters/etc.
    internal class Job
    {
        public string Id { get; set; }

        // The owner is the character that needs the job done
        public string Owner { get; set; }

        // The type of job, e.g., "items", "monsters", etc.
        public string JobType { get; set; }

        // The code for the job, which can be an item or a monster
        public string Code { get; set; }

        public int Quantity { get; set; }

        public DateTime Expiry { get; set; }
    }
}
