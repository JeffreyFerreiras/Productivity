﻿using System.Collections.Generic;
using System.Linq;

//NOTE TO SELF - Prefix trees don't need generic implementations... it defeats their purpose..

namespace Productivity.DataStructures
{
    /// <summary>
    /// Useful Trie data structure for finding partial word matching with provided words.
    /// </summary>
    public class PrefixTree
    {
        private Node _root;

        protected class Node
        {
            public bool IsWord { get; set; } = false;
            public string Word { get; set; }

            public Dictionary<char, Node> Children { get; set; } = new Dictionary<char, Node>();

            public Node(string word)
            {
                Word = word;
            }
        }

        public PrefixTree()
        {
            _root = new Node(string.Empty);
        }

        public PrefixTree(IEnumerable<string> dictionary) : this()
        {
            foreach (string word in dictionary.Distinct())
            {
                Add(word);
            }
        }

        public void Add(string word)
        {
            AddRecursive(_root, word, string.Empty);
        }

        private void AddRecursive(Node node, string remaining, string current)
        {
            if (string.IsNullOrWhiteSpace(remaining))
            {
                return;
            }

            char prefix = remaining[0];
            remaining = remaining.Substring(1);

            if (!node.Children.ContainsKey(prefix))
            {
                node.Children.Add(prefix, new Node(current + prefix));
            }

            if (remaining.Length == 0)
            {
                node.Children[prefix].IsWord = true;
            }
            else
            {
                AddRecursive(node.Children[prefix], remaining, current + prefix);
            }
        }

        public IEnumerable<string> Search(string searchString)
        {
            Node node = _root;

            foreach (char search in searchString)
            {
                if (!node.Children.ContainsKey(search))
                {
                    return new string[0];
                }

                node = node.Children[search];
            }

            return FindAllWords(node);
        }

        private IEnumerable<string> FindAllWords(Node node)
        {
            if (node.IsWord)
            {
                yield return node.Word;
            }

            foreach (var childPair in node.Children)
            {
                foreach (string result in FindAllWords(childPair.Value))
                {
                    yield return result;
                };
            }
        }
    }
}