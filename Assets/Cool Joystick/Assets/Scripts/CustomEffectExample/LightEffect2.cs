using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CoolJoystick
{
	public class LightEffect2 : MonoBehaviour
	{

		private RectTransform _rt;  // Reference to self rect transform
		private Image         _img; // Reference to self image component

		public Joystick Joystick;                  // Reference to Joystick
		public float    InterpolateColorSpeed = 5; // Interpolate speed of color change

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

		}
	}
}
