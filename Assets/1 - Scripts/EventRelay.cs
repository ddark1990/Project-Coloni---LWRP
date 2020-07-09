using System;
using System.Collections;
using System.Collections.Generic;
using ProjectColoni;
using UnityEngine;

namespace ProjectColoni
{
    public static class EventRelay
    {
        public static Action<Selectable> ObjectSelected { get; set; }
        public static Action<Selectable> ObjectDeSelected { get; set; }
    }
}
