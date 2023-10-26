using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MinaPechuex.BehaviorTree
{
    public abstract class NodeTree : MonoBehaviour
    {

        private Node _root = null;

        protected void Start()
        {
            _root = SetupTree();
        }

        private void Update()
        {
            if (_root != null)
                _root.Evaluate();
        }

        protected abstract Node SetupTree();

    }

}
