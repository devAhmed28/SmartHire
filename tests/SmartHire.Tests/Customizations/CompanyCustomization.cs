using AutoFixture;
using AutoFixture.Kernel;
using SmartHire.Domain.Entities;
using SmartHire.Domain.Enums;

namespace SmartHire.Tests.Customizations
{
    public class CompanyCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<Company>(composer => composer
                .FromFactory(new MethodInvoker(new CompanyConstructorQuery()))
                .OmitAutoProperties()
            );
        }

        private class CompanyConstructorQuery : IMethodQuery
        {
            public IEnumerable<IMethod> SelectMethods(Type type)
            {
                var constructor = type.GetConstructor(new[]
                {
                typeof(Guid), // userId
                typeof(string), // companyName
                typeof(string), // description
                typeof(string), // industry
                typeof(CompanySize) // companySize
            });

                if (constructor != null)
                {
                    yield return new ConstructorMethod(constructor);
                }
            }
        }
    }
}
