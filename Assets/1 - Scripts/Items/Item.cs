﻿using System;
using UnityEngine;

namespace ProjectColoni
{
    public class Item : Selectable
    {
        public int itemCount;
        public ItemType itemType;

        private void Start()
        {
            InitializeSelectable();
        }
    }
}
