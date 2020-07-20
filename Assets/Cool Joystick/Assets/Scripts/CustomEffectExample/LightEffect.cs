using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CoolJoystick
{
	public class LightEffect : MonoBehaviour
	{

		private RectTransform _rt;  // Reference to self rect transform
		private Image         _img; // Reference to self image component

		public Joystick Joystick; // Reference to Joystick

		public float
			InterpolateColorSpeed = 5 , InterpolateRotateSpeed = 5; // Interpolate speed of rotation and color change

		// Use this for initialization
		private void Start ( )
		{
			_img = GetComponent < Image > ( );         // Getting self image component
			_rt  = GetComponent < RectTransform > ( ); // Getting self rect transform
		}

		// Update is called once per frame
		private void Update ( )
		{
			if ( !Joystick || !_img || !_rt )
				return; // if we don't setup joystick or self rect transform or image component , not going to next step 
			// Getting alpha value based on magnitude of joystick direction 
			var alpha = new Vector2 ( Mathf.Abs ( Joystick.Direction.x ) , Mathf.Abs ( Joystick.Direction.y ) )
				.magnitude;
			// Changing image color alpha based on new alpha value with interpolation
			_img.color = Color.Lerp ( _img.color , new Color ( _img.color.r , _img.color.g , _img.color.b , alpha ) ,
									  InterpolateColorSpeed * Time.deltaTime );
			if ( Joystick.Pressed && alpha > 0.25f
				) // Then change rotation based on joystick direction converted to angle only if joystick pressed
				// and new alpha value greater than 0.25 with interpolation
				_rt.rotation = Quaternion.Lerp ( _rt.rotation ,
												 Quaternion.Euler ( 0 , 0 , 360 - Angle ( Joystick.Direction ) ) ,
												 Time.deltaTime * InterpolateRotateSpeed );
		}

		// Method for converting Vector2 direction to angle
		public static float Angle ( Vector2 direction )
		{
			if ( direction.x < 0 ) return 360 - Mathf.Atan2 ( direction.x , direction.y ) * Mathf.Rad2Deg * -1;

			return Mathf.Atan2 ( direction.x , direction.y ) * Mathf.Rad2Deg;
		}
	}
}
