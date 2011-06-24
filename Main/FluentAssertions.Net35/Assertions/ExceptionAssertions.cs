﻿using System;
using System.Diagnostics;
using System.Linq.Expressions;

using FluentAssertions.Common;

namespace FluentAssertions.Assertions
{
    [DebuggerNonUserCode]
    public class ExceptionAssertions<TException> : ReferenceTypeAssertions<Exception, ExceptionAssertions<TException>>
        where TException : Exception
    {
        protected internal ExceptionAssertions(TException exception)
        {
            Subject = exception;
        }

        /// <summary>
        ///   Gets the exception object of the exception thrown.
        /// </summary>
        public TException And
        {
            get { return (TException) Subject; }
        }

        /// <summary>
        ///   Asserts that the thrown exception has a message matching the <paramref name = "expectedMessage" />.
        /// </summary>
        /// <param name = "expectedMessage">The expected message of the exception.</param>
        public ExceptionAssertions<TException> WithMessage(string expectedMessage)
        {
            return WithMessage(expectedMessage, null, null);
        }

        /// <summary>
        ///   Asserts that the thrown exception has a message matching the <paramref name = "expectedMessage" />.
        /// </summary>
        /// <param name = "expectedMessage">The expected message of the exception.</param>
        /// <param name = "reason">
        ///   The reason why the message of the exception should match the <paramref name = "expectedMessage" />.
        /// </param>
        /// <param name = "reasonArgs">The parameters used when formatting the <paramref name = "reason" />.</param>
        public virtual ExceptionAssertions<TException> WithMessage(string expectedMessage, string reason,
            params object[] reasonArgs)
        {
            Verification verification = Execute.Verification.BecauseOf(reason, reasonArgs).UsingLineBreaks;

            verification.ForCondition(Subject != null).FailWith(
                "Expected exception with message {0}{reason}, but no exception was thrown.", expectedMessage);

            string message = Subject.Message;

            verification.ForCondition(!string.IsNullOrEmpty(message)).FailWith(
                "Expected exception with message {0}{reason}, but message was empty.", expectedMessage);

            verification.ForCondition(message.Length >= expectedMessage.Length).FailWith(
                "Expected exception with message {0}{reason}, but {1} is too short.", expectedMessage, message);

            int index = message.IndexOfFirstMismatch(expectedMessage);
            if (index != -1)
            {
                verification.FailWith(
                    "Expected exception with message {0}{reason}, but {1} differs near " + message.IndexedSegmentAt(index) +
                        ".",
                    expectedMessage, message);
            }

            return this;
        }

        /// <summary>
        ///   Asserts that the thrown exception contains an inner exception of type <typeparamref name = "TInnerException" />.
        /// </summary>
        /// <typeparam name = "TInnerException">The expected type of the inner exception.</typeparam>
        /// <returns>An <see cref = "AndConstraint" /> which can be used to chain assertions.</returns>
        public ExceptionAssertions<TException> WithInnerException<TInnerException>()
        {
            return WithInnerException<TInnerException>(null, null);
        }

        /// <summary>
        ///   Asserts that the thrown exception contains an inner exception of type <typeparamref name = "TInnerException" />.
        /// </summary>
        /// <typeparam name = "TInnerException">The expected type of the inner exception.</typeparam>
        /// <param name = "reason">The reason why the inner exception should be of the supplied type.</param>
        /// <param name = "reasonArgs">The parameters used when formatting the <paramref name = "reason" />.</param>
        /// <returns>An <see cref = "AndConstraint" /> which can be used to chain assertions.</returns>
        public virtual ExceptionAssertions<TException> WithInnerException<TInnerException>(string reason,
            params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject != null)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected inner {0}{reason}, but no exception was thrown.", typeof(TInnerException));

            Execute.Verification
                .ForCondition(Subject.InnerException != null)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected inner {0}{reason}, but the thrown exception has no inner exception.",
                    typeof (TInnerException));

            Execute.Verification
                .ForCondition(Subject.InnerException != null)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected inner {0}{reason}, but the thrown exception has no inner exception.",
                    typeof (TInnerException));

            Execute.Verification
                .ForCondition(Subject.InnerException is TInnerException)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected inner {0}{reason}, but found {1}.", typeof(TInnerException), Subject.InnerException);

            return this;
        }

        /// <summary>
        ///   Asserts that the thrown exception contains an inner exception with the <paramref name = "expectedInnerMessage" />.
        /// </summary>
        /// <param name = "expectedInnerMessage">The expected message of the inner exception.</param>
        /// <returns>An <see cref = "AndConstraint" /> which can be used to chain assertions.</returns>
        public ExceptionAssertions<TException> WithInnerMessage(string expectedInnerMessage)
        {
            return WithInnerMessage(expectedInnerMessage, null, null);
        }

        /// <summary>
        ///   Asserts that the thrown exception contains an inner exception with the <paramref name = "expectedInnerMessage" />.
        /// </summary>
        /// <param name = "expectedInnerMessage">The expected message of the inner exception.</param>
        /// <param name = "reason">
        ///   The reason why the message of the inner exception should match <paramref name = "expectedInnerMessage" />.
        /// </param>
        /// <param name = "reasonArgs">The parameters used when formatting the <paramref name = "reason" />.</param>
        public virtual ExceptionAssertions<TException> WithInnerMessage(string expectedInnerMessage, string reason,
            params object[] reasonArgs)
        {
            Verification verification = Execute.Verification.BecauseOf(reason, reasonArgs).UsingLineBreaks;

            verification.ForCondition(Subject != null).FailWith(
                "Expected exception {reason}, but no exception was thrown.");

            verification.ForCondition(Subject.InnerException != null).FailWith(
                "Expected exception{reason}, but the thrown exception has no inner exception.");

            string innerMessage = Subject.InnerException.Message;

            int index = innerMessage.IndexOfFirstMismatch(expectedInnerMessage);
            if (index != -1)
            {
                verification.FailWith(
                    "Expected inner exception with message {0}{reason}, but {1} differs near " +
                        innerMessage.IndexedSegmentAt(index) + ".",
                    expectedInnerMessage, innerMessage);
            }

            return this;
        }

        /// <summary>
        ///   Asserts that the exception matches a particular condition.
        /// </summary>
        /// <param name = "exceptionExpression">
        ///   The condition that the exception must match.
        /// </param>
        public ExceptionAssertions<TException> Where(Expression<Func<TException, bool>> exceptionExpression)
        {
            return Where(exceptionExpression, string.Empty);
        }

        /// <summary>
        ///   Asserts that the exception matches a particular condition.
        /// </summary>
        /// <param name = "exceptionExpression">
        ///   The condition that the exception must match.
        /// </param>
        /// <param name = "reason">
        ///   A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        ///   start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name = "reasonArgs">
        ///   Zero or more values to use for filling in any <see cref = "string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public ExceptionAssertions<TException> Where(Expression<Func<TException, bool>> exceptionExpression,
            string reason,
            params object[] reasonArgs)
        {
            Func<TException, bool> condition = exceptionExpression.Compile();
            Execute.Verification
                .ForCondition(condition((TException)Subject))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected exception where {0}{reason}, but the condition was not met by:\r\n\r\n{1}", exceptionExpression.Body, Subject);
            
            return this;
        }
    }
}