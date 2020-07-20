using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoolJoystick
{
	/// <summary>
	/// Class which controls how gun will shoot
	/// </summary>
	public class Gun_2DExample2 : MonoBehaviour
	{
		public GameObject[] BulletsPool; // Pool of bullets

		private void Start ( ) { StartCoroutine ( Shoot ( ) ); } // Start recursive shoot coroutine

		private IEnumerator Shoot ( )
		{
			yield return new WaitForSeconds ( 0.5f ); // Waiting half of second
			for ( var i = 0; i < BulletsPool.Length; i ++ )
			{
				if ( BulletsPool[i].activeInHierarchy == false ) // Then choosing disabled bullet from pool
				{
					BulletsPool[i].transform.position = transform.position; // Move to shoot position
					BulletsPool[i].SetActive ( true );                      // And enable bullet
					break;
				}
			}

			StartCoroutine ( Shoot ( ) ); // Start Coroutine again
		}
	}
}
