using MediatR;
using SmartHire.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartHire.Application.Features.SavedJobs.Commands.UnsaveJob
{
    public class UnsaveJobCommand : IRequest<Result>
    {
        public Guid CandidateId { get; set; }
        public Guid JobId { get; set; }
    }
}
