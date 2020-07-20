using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoolJoystick
{
	public class JoystickRotationEffect : MonoBehaviour
	{

		private Quaternion    _startRot;                   // Default rotation value
		private RectTransform _rt;                         // Reference to self rect transform
		public  Vector2       RotationRange = Vector2.one; // Range by which rotation will be multiplied
		public  Joystick      Joystick;                    // Reference to Joystick

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
			// Rotating object by joystick direction and RotationRange property, by this we adding fake 3d rotation effect
			_rt.rotation = _startRot * Quaternion.Euler ( Joystick.Direction.y * RotationRange.x ,
														  Joystick.Direction.x * RotationRange.y , 1 );
		}
	}
}
