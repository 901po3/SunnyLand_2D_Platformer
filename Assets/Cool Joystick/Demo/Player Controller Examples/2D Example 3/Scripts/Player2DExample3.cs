using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CoolJoystick
{
	/// <summary>
	/// Class which controls how player moving and collecting items
	/// </summary>
	public class Player2DExample3 : MonoBehaviour
	{

		public float    MovementSpeed = 10f; // Move speed
		public float    JumpForce     = 5f;  // Jump force
		public Joystick Joystick;            // Reference to Joystick

		public  Text        ItemCountText; // Reference to items count text
		private int         _itemCount;
		private Rigidbody2D _rb; // Reference to self Rigidbody

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

		// Getting self Rigidbody
		private void Start ( ) { _rb = GetComponent < Rigidbody2D > ( ); }

		public void Jump ( )
		{
			// Do Jump by adding jump force to self Rigidbody
			_rb.AddForce ( new Vector2 ( 0 , JumpForce ) * 100 , ForceMode2D.Impulse );
		}

		private void Update ( )
		{
			// Moving player by direction based on joystick horizontal axis and self Rigidbody
			_rb.AddForce ( new Vector2 ( Joystick.Horizontal * MovementSpeed , Physics.gravity.y ) * 100 ,
						   ForceMode2D.Force );
		}
	}
}
