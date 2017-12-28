﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace GhostGen
{
    public class GeneralEvent
    {
        public string type;
        public object target;
        public object currentTarget;
        public Hashtable data;
        public bool isBubbling;        
    }
}