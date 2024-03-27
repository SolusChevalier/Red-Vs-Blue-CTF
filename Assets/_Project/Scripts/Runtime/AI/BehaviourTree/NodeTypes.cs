using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CTF
{
    public class Selector : NodeAbs
    {
        private List<NodeAbs> nodes = new List<NodeAbs>();

        public Selector(List<NodeAbs> nodes) => this.nodes = nodes;

        public override bool Eval()
        {
            foreach (var node in nodes)
                if (node.Eval())
                    return true;
            return false;
        }
    }

    public class ActionNode : NodeAbs
    {
        public delegate bool ActionNodeDelegate();

        private ActionNodeDelegate action;

        public ActionNode(ActionNodeDelegate action) => this.action = action;

        public override bool Eval() => action();
    }

    public class Sequence : NodeAbs
    {
        private List<NodeAbs> nodes = new List<NodeAbs>();

        public Sequence(List<NodeAbs> nodes) => this.nodes = nodes;

        public override bool Eval()
        {
            foreach (var node in nodes)
                if (!node.Eval())
                    return false;
            return true;
        }
    }

    public class DecoratorNode : NodeAbs
    {
        private NodeAbs childNode;
        private Func<bool> condition;

        public DecoratorNode(NodeAbs childNode, Func<bool> condition)
        {
            this.childNode = childNode;
            this.condition = condition;
        }

        public override bool Eval()
        {
            if (condition != null && condition())
            {
                return childNode.Eval();
            }
            return false;
        }
    }

    public class ConditionNode : NodeAbs
    {
        private Func<bool> condition;

        public ConditionNode(Func<bool> condition)
        {
            this.condition = condition;
        }

        public override bool Eval()
        {
            return condition();
        }
    }
}