using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class PropertyRefs
    {
        public static readonly int RotationIndexPropertyHash = Animator.StringToHash("Rotation Index");
        public static readonly int IsMovingPropertyHash = Animator.StringToHash("Is Moving");
        public static readonly string PlayerTag = "Player";
    }
}