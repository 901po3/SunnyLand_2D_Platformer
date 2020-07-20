using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoolJoystick
{
	public class SpeedometerArrow : MonoBehaviour
	{

		private RectTransform _rt;   // Reference to self rect transform
		private float         _zRot; // Default rotation of z axis

		public float    StartZRot , FinalZRot; // From which to which rotation angle we need rotate arrow
		public Joystick Joystick;              // Reference to Joystick
		public float    InterpolateSpeed = 5;  // Interpolate speed

		// Use this for initialization
		private void Start ( )
		{
			_rt = GetComponent < RectTransform > ( ); // Getting self rect transform
		}

		// Update is called once per frame
		private void Update ( )
		{
			if ( !Joystick || !_rt ) return; // if we don't setup joystick or rect transform , not going to next step 
			// Getting alpha value based on magnitude of joystick direction 
			var alpha = new Vector2 ( Mathf.Abs ( Joystick.Direction.x ) , Mathf.Abs ( Joystick.Direction.y ) )
				.magnitude;
			// Do 2 step interpolation, 1 - between start and final rot by alpha value and 2 - by time to get smoother rotation result
			_zRot = Mathf.Lerp ( _zRot , Mathf.Lerp ( StartZRot , FinalZRot , alpha ) ,
								 Time.deltaTime * InterpolateSpeed );
			// Then set this rotation value to object rotation
			_rt.rotation = Quaternion.Euler ( 0 , 0 , _zRot );
		}
	}
}
