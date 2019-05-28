namespace Interpreter
{
    public static partial class LibRpn
    {
        #region Result

        private static readonly System.Text.StringBuilder result = new System.Text.StringBuilder();

        #endregion

        #region InfixToRpn method

        /** 
        <summary>
            Transforms the provided string from infix to Reverse Polish Notation (as best it can).
            The terms and operators in the resultant string will be separated by SPACE characters.
            Supported operators are:
            <list type="table">
                <item>
                    <term>
                        +
                    </term>
                    <description>
                        Binary and unary addition, unary is treated as if it were (0+x)
                    </description>
                </item>
                <item>
                    <term>
                        -
                    </term>
                    <description>
                        Binary and unary subtraction, unary is treated as if it were (0-x)
                    </description>
                </item>
                <item>
                    <term>
                        *
                    </term>
                    <description>
                        Multiplication
                    </description>
                </item>
                <item>
                    <term>
                        /
                    </term>
                    <description>
                        Division
                    </description>
                </item>
                <item>
                    <term>
                        %
                    </term>
                    <description>
                        Modulus
                    </description>
                </item>
                <item>
                    <term>
                        ^
                    </term>
                    <description>
                        Exponentiation
                    </description>
                </item>
                <item>
                    <term>
                        ` '
                    </term>
                    <description>
                        Specifies a span of verbatim text, can be used to enclose terms in scientific notation
                    </description>
                </item>
            </list>
        </summary>
        <remarks>
            The developer is not responsible for what you put in. 
            Strictly Garbage-In-Garbage-Out -- actually I can't even guarantee that!
        </remarks>
        <param name="Subject">
            The string to transform.
        </param>
        <param name="Options">
            Bitmapped RpnOptions to control the transformation.
        </param>
        <returns>
            A string containing a Reverse Polish Notation expression.
        </returns>
        <example>
            <code>
                string rpn = PIEBALD.Lib.LibRpn.InfixToRpn 
                ( 
                    infix
                ,
                    PIEBALD.Lib.LibRpn.RpnOptions.FailOnMalformedExpression |
                    PIEBALD.Lib.LibRpn.RpnOptions.FailOnMismatchedParenthesis 
                )
            </code>
        </example>
        <exception cref="System.ArgumentException">
            Will be thrown if Subject is null or empty.
        </exception>
        <exception cref="InfixToRpnFormatException">
            Will be thrown for certain eventualities when requested to do so through the Options parameter.
        </exception>
        */
        public static string InfixToRpn(string Subject, RpnOptions Options)
        {
            if (string.IsNullOrEmpty(Subject)) throw new System.ArgumentException("Subject is null or empty", "Subject");

            #region Local variables

            /* Prime the OperatorStackStack with an OperatorStack */
            var ops = new OperatorStackStack(new OperatorStack(0));
            /* Make helper-booleans to simplify testing for options */
            var retainwhitespace = (Options & RpnOptions.RetainWhiteSpace) == RpnOptions.RetainWhiteSpace;
            var failonmismatchedparenthesis = (Options & RpnOptions.FailOnMismatchedParenthesis) == RpnOptions.FailOnMismatchedParenthesis;
            var failonmalformedexpression = (Options & RpnOptions.FailOnMalformedExpression) == RpnOptions.FailOnMalformedExpression;
            /* Used to pass the position of a character to an InfixToRpnFormatException */
            var index = 0;
            /* Helps track whether or not the expression is malformed */
            var validatorstate = ValidatorState.OnlyOneOpAllowed;

            #endregion

            result.Length = 0;
            result.EnsureCapacity(Subject.Length * 2);
            foreach (var thischar in Subject) {
                /* Index will be one-based rather than zero-based. But this is really */
                /* due to assuming a "virtual left parenthesis" at position zero.     */
                index++;

                #region Handle Verbatim Text Characters 

                /* Seems like a clunky implementation, but what the hell it works */
                if (validatorstate == ValidatorState.VerbatimTextBegun) {
                    switch (thischar) {
                        case '`': {
                            ops.Push(new OperatorStack(index * -1));
                            break;
                        }
                        case '\'': {
                            ops.Pop();
                            /* If all verbatim text has been terminated then exit verbatim text mode */
                            if (ops.Peek().Position >= 0) validatorstate = ValidatorState.TermEnded;
                            break;
                        }
                        default: {
                            result.Append(thischar);
                            break;
                        }
                    }

                    continue;
                }

                #endregion

                #region Handle WhiteSpace

                if (thischar == char.MinValue ||
                    char.IsWhiteSpace(thischar) ||
                    char.IsControl(thischar)) {
                    if (retainwhitespace) result.Append(thischar);
                    if (validatorstate == ValidatorState.TermBegun) validatorstate = ValidatorState.TermEnded;
                    continue;
                }

                #endregion

                switch (thischar) {
                    #region Handle Left-Parenthesis

                    case '(': {
                        #region Error Handling

                        if (failonmalformedexpression)
                            if (validatorstate == ValidatorState.TermBegun ||
                                validatorstate == ValidatorState.TermEnded ||
                                validatorstate == ValidatorState.BinaryOpAllowed)
                                throw new InfixToRpnFormatException("No operator preceding left-parenthesis", index);

                        #endregion

                        ops.Push(new OperatorStack(index));
                        validatorstate = ValidatorState.OnlyOneOpAllowed;
                        break;
                    }

                    #endregion

                    #region Handle Right-Parenthesis

                    case ')': {
                        #region Error Handling

                        if (failonmalformedexpression) {
                            if (validatorstate == ValidatorState.UnaryOpAllowed ||
                                validatorstate == ValidatorState.NoMoreOpsAllowed)
                                throw new InfixToRpnFormatException("Operator followed by right-parenthesis", index);
                            if (validatorstate == ValidatorState.OnlyOneOpAllowed) throw new InfixToRpnFormatException("Empty parentheses", index);
                        }

                        #endregion

                        /* If we don't have a matching left-parenthesis */
                        if (ops.Count == 1) {
                            #region Error Handling

                            if (failonmismatchedparenthesis) throw new InfixToRpnFormatException("Mismatched right-parenthesis", index);

                            #endregion

                            ops.Peek().Clear();
                        }
                        else {
                            ops.Pop().Clear();
                        }

                        validatorstate = ValidatorState.BinaryOpAllowed;
                        break;
                    }

                    #endregion

                    #region Handle Addition and Subtraction, both binary and unary

                    case '-':
                    case '+': {
                        #region Error Handling

                        if (failonmalformedexpression)
                            if (validatorstate == ValidatorState.NoMoreOpsAllowed)
                                throw new InfixToRpnFormatException("Too many operators in a row", index);

                        #endregion

                        if (validatorstate == ValidatorState.UnaryOpAllowed ||
                            validatorstate == ValidatorState.OnlyOneOpAllowed) {
                            /* Unary */
                            ops.Peek().Push(new Operator(thischar, OperatorPrecedence.Highest));
                            validatorstate = ValidatorState.NoMoreOpsAllowed;
                        }
                        else {
                            /* Binary */
                            ops.Peek().Push(new Operator(thischar, OperatorPrecedence.Low));
                            validatorstate = ValidatorState.UnaryOpAllowed;
                        }

                        break;
                    }

                    #endregion

                    #region Handle Multiplication, Division, and Modulus

                    case '*':
                    case '/':
                    case '%': {
                        #region Error Handling

                        if (failonmalformedexpression)
                            if (validatorstate == ValidatorState.UnaryOpAllowed ||
                                validatorstate == ValidatorState.NoMoreOpsAllowed)
                                throw new InfixToRpnFormatException("Too many operators in a row", index);

                        #endregion

                        ops.Peek().Push(new Operator(thischar, OperatorPrecedence.Medium));
                        validatorstate = ValidatorState.UnaryOpAllowed;
                        break;
                    }

                    #endregion

                    #region Handle Exponentiation

                    case '^': {
                        #region Error Handling

                        if (failonmalformedexpression)
                            if (validatorstate == ValidatorState.UnaryOpAllowed ||
                                validatorstate == ValidatorState.NoMoreOpsAllowed)
                                throw new InfixToRpnFormatException("Too many operators in a row", index);

                        #endregion

                        ops.Peek().Push(new Operator(thischar, OperatorPrecedence.High));
                        validatorstate = ValidatorState.UnaryOpAllowed;
                        break;
                    }

                    #endregion

                    #region Handle Verbatim Text Beginning

                    case '`': {
                        #region Error Handling

                        if (failonmalformedexpression) {
                            if (validatorstate == ValidatorState.BinaryOpAllowed)
                                throw new InfixToRpnFormatException("No operator following right-parenthesis", index);
                            if (validatorstate == ValidatorState.TermEnded)
                                throw new InfixToRpnFormatException("Second consecutive term detected", index);
                        }

                        #endregion

                        ops.Push(new OperatorStack(index * -1));
                        validatorstate = ValidatorState.VerbatimTextBegun;
                        break;
                    }

                    #endregion

                    #region Handle Everything else

                    default: {
                        #region Error Handling

                        if (failonmalformedexpression) {
                            if (validatorstate == ValidatorState.BinaryOpAllowed)
                                throw new InfixToRpnFormatException("No operator preceding term", index);
                            if (validatorstate == ValidatorState.TermEnded)
                                throw new InfixToRpnFormatException("Second consecutive term detected", index);
                        }

                        #endregion

                        result.Append(thischar);
                        validatorstate = ValidatorState.TermBegun;
                        break;
                    }

                    #endregion
                }
            }

            #region Handle the end of the string

            #region Error Handling

            if (failonmismatchedparenthesis) {
                if (validatorstate == ValidatorState.VerbatimTextBegun)
                    throw new InfixToRpnFormatException("No terminator found for verbatim text", ops.Pop().Position);
                if (validatorstate == ValidatorState.UnaryOpAllowed ||
                    validatorstate == ValidatorState.NoMoreOpsAllowed)
                    throw new InfixToRpnFormatException("Dangling operator", index);
                if (ops.Count > 1) throw new InfixToRpnFormatException("Mismatched left-parenthesis", ops.Pop().Position);
            }

            #endregion

            while (ops.Count > 0) ops.Pop().Clear();

            #endregion

            return result.ToString();
        }

        #endregion

        #region RpnOptions enumeration

        /** 
        <summary>
            Options for controlling certain features of the transformation.
        </summary>
        <remarks>
            Has the Flags attribute, so values can be combined.
        </remarks>
        */
        [System.FlagsAttribute()]
        public enum RpnOptions
        {
            /** 
            <summary>
                No options selected.
            </summary>
            */
            None = 0,

            /** 
            <summary>
                Retain the whitespace, control characters, and null characters from the source string.
            </summary>
            <remarks>
                The default behaviour is to insert SPACEs where operators and parentheses had been in the 
                source string, but otherwise ignore the whitespace. 
                With this option selected the whitespace will be echoed to the resulting RPN string.
            </remarks>
            */
            RetainWhiteSpace = 1,

            /** 
            <summary>
                Throw an InfixToRpnFormatException if a mismatched parenthesis is detected.
            </summary>
            <remarks>
                The default behaviour is to ignore this because the resultant string won't contain them and
                it's easy enough to behave as if the missing parenthesis were at the appropriate end.
                With this option selected, a mismatched parenthesis will cause an InfixToRpnFormatException
                with the position of the unmatched parenthesis to be thrown.
            </remarks>
            */
            FailOnMismatchedParenthesis = 2,

            /** 
            <summary>
                Throw an InfixToRpnFormatException when various other situations occur.
            </summary>
            <remarks>
                The default behaviour is to ignore these because the user may pass in an expression in which
                these things are not errors.
                With this option selected, the following situations will be checked and violations will cause 
                an InfixToRpnFormatException with the position of the first errant character to be thrown.
                <list type="bullet">
                    <item>
                        <description>
                            Empty parentheses is not allowed --()
                        </description>
                    </item>
                    <item>
                        <description>
                            Back-to-back parentheses is not allowed -- )(
                        </description>
                    </item>
                    <item>
                        <description>
                            Two consecutive operators are only allowed if the second is a (unary) plus or minus
                        </description>
                    </item>
                    <item>
                        <description>
                            Plus and minus are the only allowed unary operators
                        </description>
                    </item>
                    <item>
                        <description>
                            Two consecutive terms are not allowed
                        </description>
                    </item>
                </list>
            </remarks>
            */
            FailOnMalformedExpression = 4,

            /** 
            <summary>
                What part of "All" don't you understand?
            </summary>
            */
            All = 1 + 2 + 4
        };

        #endregion

        #region InfixToRpnFormatException class

        /** 
        <summary>
            Implements a specialized FormatException for the InfixToRpn method.
        </summary>
        <remarks>
            Tracks the position of the character that caused the exception.
        </remarks>
        */
        public class InfixToRpnFormatException: System.FormatException
        {
            /** 
            <summary>
                Holds the position of the character that caused the exception. 
                Readonly.
            </summary>
            */
            public readonly int Position;

            /** 
            <summary>
                Constructor for InfixToRpnFormatException class.
            </summary>
            <param name="Message">
                What the problem was.
            </param>
            <param name="Position">
                Position of the offending character.
            </param>
            */
            internal InfixToRpnFormatException(string Message, int Position): base(Message)
            {
                this.Position = Position;
                return;
            }

            /** 
            <summary>
                Retrieves the value of the Message field.
            </summary>
            */
            public override string Message => ToString();

            /** 
            <summary>
                Formats a string from the contents.
            </summary>
            <returns>
                The Message and Position in a string.
            </returns>
            */
            public override string ToString()
            {
                return string.Format("{0} at position {1}", base.Message, Position);
            }
        }

        #endregion

        #region OperatorPrecedence enumeration

        /** 
        <summary>
            Precedence values for Operators.
        </summary>
        */
        private enum OperatorPrecedence
        {
            /** 
            <summary>
                Binary addition and subtraction.
            </summary>
            */
            Low,

            /** 
            <summary>
                Multiplication and division.
            </summary>
            */
            Medium,

            /** 
            <summary>
                Exponentionation.
            </summary>
            */
            High,

            /** 
            <summary>
                Unary plus and minus.
            </summary>
            */
            Highest
        };

        #endregion

        #region Operator class

        /** 
        <summary>
            Holder for an operator character and its precedence.
        </summary>
        */
        private class Operator
        {
            /** 
            <summary>
                Holds the operator's character value.
            </summary>
            */
            public readonly char Character;

            /** 
            <summary>
                Holds the operator's precedence level.
                Readonly.
            </summary>
            */
            public readonly OperatorPrecedence Precedence;

            /** 
            <summary>
                Constructor for the Operator class.
            </summary>
            <param name="Character">
                Which operator it is.
            </param>
            <param name="Precedence">
                What the precedence is.
            </param>
            */
            public Operator(char Character, OperatorPrecedence Precedence)
            {
                this.Character = Character;
                this.Precedence = Precedence;
                return;
            }

            /** 
            <summary>
                Formats a string from the contents.
            </summary>
            <returns>
                The operator character as a string.
            </returns>
            */
            public override string ToString()
            {
                return Character.ToString();
            }
        }

        #endregion

        #region OperatorStack class

        /** 
        <summary>
            Holder for a stack of operators.
        </summary>
        <remarks>
            Each OperatorStack defines a parenthesis-level.
            When a left-parenthesis is encountered a new OperatorStack should be created.
            When a right-parenthesis is encountered the top OperatorStack should be removed and emptied.
        </remarks>
        */
        private class OperatorStack: System.Collections.Generic.Stack<Operator>
        {
            /** 
            <summary>
                Holds the position of the left-parenthesis that prompted the new level.
            </summary>
            */
            public readonly int Position;

            /** 
            <summary>
                Constructor for the OperatorStack class.
            </summary>
            <param name="Position">
                The position of the left-parenthesis that prompted the new level.
            </param>
            */
            internal OperatorStack(int Position)
            {
                this.Position = Position;
                return;
            }

            /** 
            <summary>
                Pushes an Operator onto the stack after popping off any higher or equal precedence operators.
            </summary>
            <param name="Op">
                The Operator to push.
            </param>
            */
            internal new void Push(Operator Op)
            {
                /* Highest precedence means it's a unary operator so insert a zero */
                if (Op.Precedence == OperatorPrecedence.Highest) result.Append("0");
                /* Always replace the operator with a SPACE */
                result.Append(" ");
                while (Count > 0 && Peek().Precedence >= Op.Precedence) {
                    result.Append(Pop());
                    result.Append(" ");
                }

                base.Push(Op);
                return;
            }

            /** 
            <summary>
                Pops all the Operators from the stack.
            </summary>
            */
            internal new void Clear()
            {
                while (Count > 0) {
                    result.Append(" ");
                    result.Append(Pop());
                }

                return;
            }
        }

        #endregion

        #region OperatorStackStack class

        /** 
        <summary>
            Holder for a stack of stacks of operators.
        </summary>
        <remarks>
            Defines nothing new, simply limits itself to holding OperatorStacks.
            When a left-parenthesis is encountered a new OperatorStack should be pushed on.
            When a right-parenthesis is encountered the top OperatorStack should be popped and cleared.
        </remarks>
        */
        private class OperatorStackStack: System.Collections.Generic.Stack<OperatorStack>
        {
            /** 
            <summary>
                Constructor for the OperatorStackStack class.
            </summary>
            <param name="Stack">
                An initial stack to push.
            </param>
            */
            internal OperatorStackStack(OperatorStack Stack)
            {
                Push(Stack);
                return;
            }
        }

        #endregion

        #region ValidatorState enumeration

        /** 
        <summary>
            State of the (very simple) validator.
        </summary>
        */
        private enum ValidatorState
        {
            /** 
            <summary>
                We had a second operator.
            </summary>
            */
            NoMoreOpsAllowed,

            /** 
            <summary>
                We had a left-parenthesis.
            </summary>
            */
            OnlyOneOpAllowed,

            /** 
            <summary>
                We had a first operator.
            </summary>
            */
            UnaryOpAllowed,

            /** 
            <summary>
                We had a right-parenthesis.
            </summary>
            */
            BinaryOpAllowed,

            /** 
            <summary>
                We had a term character.
            </summary>
            */
            TermBegun,

            /** 
            <summary>
                We had a term-ending whitespace operator.
            </summary>
            */
            TermEnded,

            /** 
            <summary>
                In verbatim text mode.
            </summary>
            */
            VerbatimTextBegun
        };

        #endregion
    }
}