using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CoolJoystick
{
	/// <summary>
	/// Class which controls how player moving and collecting items
	/// </summary>
	public class Player3DExample3 : MonoBehaviour
	{

		public float    FlySpeed    = 10f; // Flying speed
		public float    RotateSpeed = 10f; // Fly Rotation speed
		public Joystick JoystickForMove;   // Reference to Joystick of fly magnitude
		public Joystick JoystickForRotate; // Reference to Joystick of fly direction

		public  Text    ItemCountText; // Reference to items count text
		private int     _itemCount;
		private Vector2 _rotation;

		// Property for changing items count
		public int ItemCount
		{
			get { return _itemCount; }
			set
			{
				_itemCount = value;
				if ( ItemCountText ) ItemCountText.text = "TriggersEntered: " + _itemCount;
			}
		}

		private void Update ( )
		{
			// Getting move direction based on direction of first joystick
			var moveDir = transform.right * JoystickForMove.Horizontal + transform.forward * JoystickForMove.Vertical;
			// Do look rotation to direction of second joystick
			_rotation          += JoystickForRotate.Direction * Time.deltaTime * RotateSpeed;
			transform.rotation =  Quaternion.Euler ( -_rotation.y , _rotation.x , 0 );
			// And moving player by this move direction with needed speed
			transform.Translate ( moveDir * FlySpeed * Time.deltaTime , Space.World );
		}
	}
}
