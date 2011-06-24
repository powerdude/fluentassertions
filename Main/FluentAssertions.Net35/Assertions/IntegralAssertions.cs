﻿using System;

namespace FluentAssertions.Assertions
{
    public class IntegralAssertions<T> : NumericAssertions<T>
    {
        protected internal IntegralAssertions(T value) : base(value)
        {
        }

        public AndConstraint<NumericAssertions<T>> Be(T expected)
        {
            return Be(expected, String.Empty);
        }

        public AndConstraint<NumericAssertions<T>> Be(T expected, string reason, params object [] reasonArgs)
        {
            Execute.Verification
                .ForCondition(ReferenceEquals(Subject, expected) || (Subject.CompareTo(expected) == 0))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {0}{reason}, but found {1}.", expected, Subject);

            return new AndConstraint<NumericAssertions<T>>(this);
        }

        public AndConstraint<NumericAssertions<T>> NotBe(T expected)
        {
            return NotBe(expected, String.Empty);
        }

        public AndConstraint<NumericAssertions<T>> NotBe(T expected, string reason, params object [] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.CompareTo(expected) != 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect {0}{reason}.", expected);

            return new AndConstraint<NumericAssertions<T>>(this);
        }
    }
}