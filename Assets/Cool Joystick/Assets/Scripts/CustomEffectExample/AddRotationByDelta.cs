using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoolJoystick
{
	public class AddRotationByDelta : MonoBehaviour
	{

		private RectTransform _rt; // Reference to self rect transform
		private float         _zRot;

		public Joystick Joystick;             // Reference to Joystick
		public float    RotationSpeed    = 5; // Rotation speed
		public float    InterpolateSpeed = 5; // Interpolate speed

		// Use this for initialization
		private void Start ( )
		{
			_rt   = GetComponent < RectTransform > ( ); // Getting self rect transform
			_zRot = _rt.rotation.eulerAngles.z;         // Saving current object rotation by z axis
		}

		// Update is called once per frame
		private void Update ( )
		{
			if ( !Joystick || !_rt ) return; // if we don't setup joystick or rect transform , not going to next step 
			// Adding rotation by z axis based on magnitude of Joystick delta
			_zRot += Joystick.Delta.normalized.magnitude * Time.deltaTime * RotationSpeed;
			// Set this rotation to object rotation with interpolation
			_rt.rotation = Quaternion.Lerp ( _rt.rotation , Quaternion.Euler ( 0 , 0 , _zRot ) ,
											 Time.deltaTime * InterpolateSpeed );
		}
	}
}
