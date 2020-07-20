using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CoolJoystick
{
	public class ChangeOutlineColorByDirection : MonoBehaviour
	{

		private Image _img; // Reference to self image component

		public Color    DefaultColor = Color.white , PressedColor = Color.white , MovingColor = Color.white;
		public Joystick Joystick;             // Reference to Joystick
		public float    InterpolateSpeed = 5; // Interpolate speed

		// Use this for initialization
		private void Start ( )
		{
			_img = GetComponent < Image > ( ); // Getting self image component
		}

		// Update is called once per frame
		private void Update ( )
		{
			if ( !Joystick || !_img ) return; // if we don't setup joystick or image component , not going to next step 
			//Getting alpha value based on magnitude of joystick direction 
			var alpha = new Vector2 ( Mathf.Abs ( Joystick.Direction.x ) , Mathf.Abs ( Joystick.Direction.y ) )
				.magnitude;
			//Change image color based on alpha value and Pressed(?) property
			_img.color = Color.Lerp ( _img.color , /*Joystick.Pressed ? */
									  Color.Lerp ( PressedColor , MovingColor , alpha ) /* : DefaultColor*/ ,
									  InterpolateSpeed * Time.deltaTime );
		}
	}
}
