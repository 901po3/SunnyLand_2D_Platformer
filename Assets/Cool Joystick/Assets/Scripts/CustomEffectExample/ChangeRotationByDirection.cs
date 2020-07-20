using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoolJoystick
{
	public class ChangeRotationByDirection : MonoBehaviour
	{

		private RectTransform _rt;       // Reference to self rect transform
		private Quaternion    _startRot; // Default rotation value

		public Joystick Joystick;                   // Reference to Joystick
		public float    RotationAngle;              // Angle to which object will be rotated
		public float    InterpolateRotateSpeed = 5; // Interpolate rotation speed

		// Use this for initialization
		private void Start ( )
		{
			_rt       = GetComponent < RectTransform > ( ); // Getting self rect transform
			_startRot = _rt.rotation;                       // Saving default rotation
		}

		// Update is called once per frame
		private void Update ( )
		{
			if ( !Joystick || !_rt ) return; // if we don't setup joystick or rect transform , not going to next step 
			// Getting alpha value based on magnitude of joystick direction 
			var alpha = new Vector2 ( Mathf.Abs ( Joystick.Direction.x ) , Mathf.Abs ( Joystick.Direction.y ) )
				.magnitude;
			// Setup rotation value based RotationAngle property and Angle from joystick direction
			// And interpolate between default rotation and new rotation value based on alpha value 
			var alphaRot =
				Quaternion.Slerp ( _startRot ,
								   Quaternion.Euler ( 0 , 0 , RotationAngle - Angle ( Joystick.Direction ) ) , alpha );
			// THen again interpolate but by time and InterpolateRotateSpeed property
			_rt.rotation = Quaternion.Slerp ( _rt.rotation , alphaRot , Time.deltaTime * InterpolateRotateSpeed );
		}

		// Method for converting Vector2 direction to angle
		public static float Angle ( Vector2 direction )
		{
			if ( direction.x < 0 ) return 360 - Mathf.Atan2 ( direction.x , direction.y ) * Mathf.Rad2Deg * -1;
			return Mathf.Atan2 ( direction.x , direction.y ) * Mathf.Rad2Deg;
		}
	}
}
