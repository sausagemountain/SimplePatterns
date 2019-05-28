using System;
using System.Collections;
using System.Collections.Generic;

namespace Interpreter
{
    public class Node<T>:IEnumerable<Node<T>>
    {
        private IEnumerable<Node<T>> _enumerable => new []{ Left, this, Right };
        public Node<T> Left { get; set; }
        public Node<T> Right { get; set; }
        public T Value { get; set; }

        public bool Add(T child)
        {
            return Add(new Node<T>(child));
        }
        public bool Add(Node<T> child)
        {
            if (Left == default(Node<T>)) {
                Left = child;
                return true;
            }

            if (Right == default(Node<T>)) {
                Right = child;
                return true;
            }

            return false;
        }

        public bool IsLeaf => Left == default(Node<T>) && Right == default(Node<T>);


        public Node()
        { }
        public Node(T value):this()
        {
            Value = value;
        }
        public Node(Node<T> left, T value, Node<T> right) : this(value)
        {
            Left = left;
            Right = right;
        }

        public string InfixString => IsLeaf ? Value.ToString() : $"{Left.InfixString} {Value} {Right.InfixString}";
        public string PostfixString => IsLeaf ? Value.ToString() : $"{Left.PostfixString} {Right.PostfixString} {Value}";
        public string PrefixString => IsLeaf ? Value.ToString() : $"{Value} {Left.PrefixString} {Right.PrefixString}";

        public IEnumerator<Node<T>> GetEnumerator()
        {
            return _enumerable.GetEnumerator();
        }

        public override string ToString()
        {
            return PostfixString;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _enumerable).GetEnumerator();
        }
    }
}