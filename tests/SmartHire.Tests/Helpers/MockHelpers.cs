using Moq;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Interfaces.Repositories;

namespace SmartHire.Tests.Helpers
{
    public static class MockHelpers
    {
        public static Mock<IUnitOfWork> CreateMockUnitOfWork()
        {
            var mock = new Mock<IUnitOfWork>();

            var mockUsers = new Mock<IUserRepository>();
            mock.Setup(u => u.Users).Returns(mockUsers.Object);

            var mockJobs = new Mock<IJobRepository>();
            mock.Setup(u => u.Jobs).Returns(mockJobs.Object);

            var mockCompanies = new Mock<ICompanyRepository>();
            mock.Setup(u => u.Companies).Returns(mockCompanies.Object);

            var mockCandidateProfiles = new Mock<ICandidateProfileRepository>();
            mock.Setup(u => u.CandidateProfiles).Returns(mockCandidateProfiles.Object);

            return mock;
        }
    }
}
