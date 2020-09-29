using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectColoni
{
    public class HideOnPlay : MonoBehaviour
    {
        void Start () {
            gameObject.SetActive (false);
        }
    }
}
