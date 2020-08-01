using System;
using System.Collections;
using System.Collections.Generic;
using ProjectColoni;
using UnityEngine;

namespace ProjectColoni
{
    public static class EventRelay
    {
        public static Action ObjectSelected { get; set; }
        public static Action ObjectDeSelected { get; set; }
    }
}
