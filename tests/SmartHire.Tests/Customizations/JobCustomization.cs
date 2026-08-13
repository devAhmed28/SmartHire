using AutoFixture;
using AutoFixture.Kernel;
using SmartHire.Domain.Entities;
using SmartHire.Domain.Enums;

namespace SmartHire.Tests.Customizations
{
    public class JobCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<Job>(composer => composer
                .FromFactory(new MethodInvoker(new JobConstructorQuery()))
                .OmitAutoProperties()
            );
        }

        private class JobConstructorQuery : IMethodQuery
        {
            public IEnumerable<IMethod> SelectMethods(Type type)
            {
                var constructor = type.GetConstructor(new[]
                {
                    typeof(Guid),      // companyId
                    typeof(string),    // title
                    typeof(string),    // description
                    typeof(string),    // responsibilities
                    typeof(string),    // requirements
                    typeof(decimal),   // salaryMin
                    typeof(decimal),   // salaryMax
                    typeof(string),    // location
                    typeof(int),       // vacancies
                    typeof(Currency),  // currency
                    typeof(JobType),   // jobType
                    typeof(WorkMode),  // workMode
                    typeof(DateTime)   // expirationDate
                });

                if (constructor != null)
                {
                    yield return new ConstructorMethod(constructor);
                }
            }
        }
    }
}
