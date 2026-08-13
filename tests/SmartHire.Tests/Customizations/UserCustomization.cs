using AutoFixture;
using AutoFixture.Kernel;
using SmartHire.Domain.Entities;
using SmartHire.Domain.Enums;

namespace SmartHire.Tests.Customizations
{
    public class UserCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<User>(composer => composer
                .FromFactory(new MethodInvoker(new UserConstructorQuery()))
                .OmitAutoProperties()
            );
        }

        private class UserConstructorQuery : IMethodQuery
        {
            public IEnumerable<IMethod> SelectMethods(Type type)
            {
                var constructor = type.GetConstructor(new[]
                {
                typeof(string), // firstName
                typeof(string), // lastName
                typeof(string), // email
                typeof(string), // passwordHash
                typeof(string), // phoneNumber
                typeof(UserRole) // role
            });

                if (constructor != null)
                {
                    yield return new ConstructorMethod(constructor);
                }
            }
        }
    }
}
