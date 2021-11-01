using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroing : MonoBehaviour
{
   private float secondsTolive = 2f;

   private void Start()
   {
      StartCoroutine(SelfDestruction());
   }

   private IEnumerator SelfDestruction()
   {
      yield return new WaitForSeconds(secondsTolive);
      Destroy(gameObject);
   }
}
