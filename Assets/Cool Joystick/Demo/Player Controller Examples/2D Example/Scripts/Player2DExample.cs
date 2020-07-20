using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CoolJoystick
{
	/// <summary>
	/// Class which controls how player moving and collecting items
	/// </summary>
	public class Player2DExample : MonoBehaviour
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
				if ( ItemCountText ) ItemCountText.text = "ItemCount: " + _itemCount;
			}
		}

		private void Update ( )
		{

			// Getting move direction based on joystick direction
			var moveDir = Vector3.right * Joystick.Horizontal + Vector3.up * Joystick.Vertical;

			if ( moveDir == Vector3.zero ) return; // if move direction not zero
			// Do look rotation to move direction
			transform.rotation = Quaternion.LookRotation ( Vector3.forward , moveDir );
			// And moving player by this move direction with needed speed
			transform.Translate ( moveDir * MovementSpeed * Time.deltaTime , Space.World );
		}
	}
}
