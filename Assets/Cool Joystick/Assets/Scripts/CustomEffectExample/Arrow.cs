using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CoolJoystick
{
	public class Arrow : MonoBehaviour
	{

		public RectTransform JoystickForeground;          // Reference to joystick foreground rect transform
		public float         ContactDistance;             //By what distance will be used color gradient evaluate 
		public Gradient      MainColor;                   // Main color gradient
		public float         InterpolateColorSpeed = 500; // Interpolate color speed


		private Image         _img; // Reference to self image component
		private RectTransform _rt;  // Reference to self rect transform
		private float         _prevAlpha;

		// Use this for initialization
		private void Start ( )
		{
			_rt  = GetComponent < RectTransform > ( ); // Getting self rect transform
			_img = GetComponent < Image > ( );         // Getting self image component
		}

		// Update is called once per frame
		private void Update ( )
		{
			if ( !_rt || !_img || !JoystickForeground )
				return; // If we don't setup self rect transform or self image component
			// or joystick foreground rect transform , we not going to next step
			// Checking distance from joystick foreground position to object self position
			var dst = Vector2.Distance ( JoystickForeground.anchoredPosition , _rt.anchoredPosition );
			dst = Mathf.Clamp ( dst , 0 , ContactDistance ); // Clamping distance by ContactDistance
			var alpha = ( ContactDistance - dst ) /
						ContactDistance;                              // Then calculate alpha (0-1 range) value based on distance
			var interpoalte = InterpolateColorSpeed * Time.deltaTime; // setup interpolate speed
			if ( Math.Abs ( _prevAlpha - alpha ) > 0.1f
			) // If prevAlpha not equal current alpha do next steps
			{
				if ( alpha > _prevAlpha )
					interpoalte *=
						10;         // If cur alpha greater than prevAlpha change interpolate speed to faster by 10x times
				_prevAlpha = alpha; //  Set current alpha to prevAlpha
			}

			_img.color =
				Color.Lerp ( _img.color , MainColor.Evaluate ( alpha ) ,
							 interpoalte ); // Change image color by color gradient and alpha value
		}
	}
}
