using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CoolJoystick
{
	/// <summary>
	/// Class which controls how player moving and collecting items
	/// </summary>
	public class Player3DExample2 : MonoBehaviour
	{

		public float    MovementSpeed = 10f; // Move speed
		public Joystick Joystick;            // Reference to Joystick

		public  Text ItemCountText; // Reference to items count text
		private int  _itemCount;

		// Property for changing items count
		public int ItemCount
		{
			get { return _itemCount; }
			set
			{
				_itemCount = value;
				if ( ItemCountText ) ItemCountText.text = "GemsCollected: " + _itemCount;
			}
		}

		private void Update ( )
		{
			// Getting move direction based on joystick direction and custom rotation angle
			var moveDir = Quaternion.AngleAxis ( -45 , Vector3.up ) *
						  ( Vector3.right * Joystick.Horizontal + Vector3.forward * Joystick.Vertical );

			if ( moveDir == Vector3.zero ) return; // if move direction not zero
			// Do look rotation to move direction
			transform.rotation = Quaternion.LookRotation ( moveDir );
			// And moving player by this move direction with needed speed
			transform.Translate ( moveDir * MovementSpeed * Time.deltaTime , Space.World );
		}
	}
}
