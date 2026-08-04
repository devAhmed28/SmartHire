using System;
using System.Collections.Generic;
using System.Text;

namespace SmartHire.Application.DTOs.Applications
{
    public class ApplyForJobRequest
    {
        public Guid JobId { get; set; }
        public string? CoverLetter { get; set; }
    }
}
