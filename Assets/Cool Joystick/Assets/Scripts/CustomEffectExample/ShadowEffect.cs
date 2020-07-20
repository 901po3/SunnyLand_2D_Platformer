using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoolJoystick
{
	public class ShadowEffect : MonoBehaviour
	{

		private Vector2       _startPos , _startScale;   // Default position and scale values
		private RectTransform _rt;                       // Reference to self rect transform
		public  Vector2       MoveRange  = Vector2.one;  // Range by which move will be multiplied
		public  Vector2       ScaleRange = Vector2.zero; // Range by which scale will be multiplied
		public  Joystick      Joystick;                  // Reference to Joystick
		public  bool          KeepJoystickPos;           // Keep Joystick position(?)
		public  float         InterpolatePosSpeed = 50;  // Interpolate position speed

		// Use this for initialization
		private void Start ( )
		{
			_rt         = GetComponent < RectTransform > ( );                  // Getting self rect transform
			_startPos   = _rt.anchoredPosition;                                // Saving default position value
			_startScale = new Vector2 ( _rt.localScale.x , _rt.localScale.y ); // Saving default scale value
		}

		// Update is called once per frame
		private void Update ( )
		{
			if ( !Joystick || !_rt ) return; // if we don't setup joystick or rect transform , not going to next step 
			// Change object position based on joystick direction and MoveRange property with KeepJoystickPos property and interpolation
			_rt.anchoredPosition = Vector2.Lerp ( _rt.anchoredPosition ,
												  (
													  KeepJoystickPos // if we use this option shadow offset will have also joystick position in additive
														  ? Joystick.JoystickForeground.anchoredPosition + _startPos
														  : _startPos ) +
												  // Adding offset based on joystick direction and MoveRange property
												  new Vector2 ( Joystick.Direction.x * MoveRange.x ,
																Joystick.Direction.y * MoveRange.y ) ,
												  Time.deltaTime * InterpolatePosSpeed );

			// Getting alpha value based on magnitude of joystick direction 
			var alpha = new Vector2 ( Mathf.Abs ( Joystick.Direction.x ) , Mathf.Abs ( Joystick.Direction.y ) )
				.magnitude;
			// Change object scale based on alpha and ScaleRange property
			_rt.localScale = new Vector3 ( _startScale.x ,        _startScale.y ,        1 ) +
							 new Vector3 ( alpha * ScaleRange.x , alpha * ScaleRange.y , 0 );
		}
	}
}
