using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test123 : MonoBehaviour
{
    public int duplicates = 0;
    
    private int[] _array1 = {2,2,3,3,4};
    public int _hasZero = 0;
    private int _temp = 0;


    private void Start()
    {
        var a = GetDuplicateAmountFromArray(_array1);
        Debug.Log(a);
    }

    private int GetDuplicateAmountFromArray(int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            var a = array[i];
            if (a == 0) _hasZero = 1;
            if (_temp == a) duplicates++;
            _temp = a;
        } return array.Length - duplicates + _hasZero;
    }
    
    public int removeDuplicates(int[] nums) {
        if (nums.Length == 0) return 0;
        int i = 0;
        for (int j = 1; j < nums.Length; j++) {
            if (nums[j] != nums[i]) {
                i++;
                nums[i] = nums[j];
            }
        }
        return i + 1;
    }
}
