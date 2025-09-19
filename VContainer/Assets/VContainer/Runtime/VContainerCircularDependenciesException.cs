using System;
using System.Collections.Generic;
using System.Linq;

namespace VContainer
{
    public sealed class VContainerCircularDependenciesException : Exception
    {
        public readonly Registration InvalidRegistration;
        public readonly Stack<Registration> RegistrationStack;

        public VContainerCircularDependenciesException(Registration invalidRegistration, Stack<Registration> registrationStack)
            : base(GetMessage(invalidRegistration, registrationStack))
        {
            InvalidRegistration = invalidRegistration;
            RegistrationStack = registrationStack;
        }

        public static string GetMessage(Registration invalidRegistration, Stack<Registration> registrationStack)
        {
            string targetType = invalidRegistration.ImplementationType.FullName;
            var stack= registrationStack.Aggregate(targetType, (acc, c) => $"{acc} -> {c.ImplementationType.FullName}");
            return $"Circular dependency detected! {stack}";
        }
    }
}