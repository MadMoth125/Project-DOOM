using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Doom.DeadScripts
{
    public struct CharacterInputs
    {
        public float MoveAxisForward;
        public float MoveAxisRight;
        public Quaternion CameraRotation;
        public bool JumpDown;
        public bool CrouchDown;
        public bool CrouchUp;
    }
}
