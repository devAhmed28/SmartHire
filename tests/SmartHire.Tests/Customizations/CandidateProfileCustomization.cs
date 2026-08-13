using AutoFixture;
using AutoFixture.Kernel;
using SmartHire.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartHire.Tests.Customizations
{
    public class CandidateProfileCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<CandidateProfile>(composer => composer
                .FromFactory(new MethodInvoker(new CandidateProfileConstructorQuery()))
                .OmitAutoProperties()
            );
        }

        private class CandidateProfileConstructorQuery : IMethodQuery
        {
            public IEnumerable<IMethod> SelectMethods(Type type)
            {
                var constructor = type.GetConstructor(new[]
                {
                    typeof(Guid),      // userId
                    typeof(string),    // bio
                    typeof(string),    // currentPosition
                    typeof(string),    // currentLocation
                    typeof(int),       // yearsOfExperience
                    typeof(decimal)    // expectedSalary
                });

                if (constructor != null)
                {
                    yield return new ConstructorMethod(constructor);
                }
            }
        }
    }
}
