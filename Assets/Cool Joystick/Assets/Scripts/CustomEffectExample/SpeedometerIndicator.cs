using UnityEngine;
using UnityEngine.UI;

namespace CoolJoystick
{
	public class SpeedometerIndicator : MonoBehaviour
	{

		private Image    _img;                 // Reference to self image component
		private Color    _defaultCol;          // Reference to default color
		public  Joystick Joystick;             // Reference to Joystick
		public  float    InterpolateSpeed = 5; // Interpolate speed

		public float
			MinMagnitude ,
			MaxMagnitude; // Min and Max values of joystick direction magnitude needed for calculation of alpha

		// Use this for initialization
		private void Start ( )
		{
			_img        = GetComponent < Image > ( ); // Getting self image component
			_defaultCol = _img.color;
		}

		// Update is called once per frame
		private void Update ( )
		{
			if ( !Joystick || !_img ) return; // if we don't setup joystick or image component , not going to next step 
			//Getting alpha value based on magnitude of joystick direction 
			var magnitude = new Vector2 ( Mathf.Abs ( Joystick.Direction.x ) , Mathf.Abs ( Joystick.Direction.y ) )
				.magnitude;
			var alpha = ( Mathf.Clamp ( magnitude , MinMagnitude , MaxMagnitude ) - MinMagnitude ) /
						( MaxMagnitude                                            - MinMagnitude );
			//Change image color based on alpha value and Pressed(?) property
			_img.color = Color.Lerp ( _img.color , new Color ( _defaultCol.r , _defaultCol.g , _defaultCol.b , alpha ) ,
									  InterpolateSpeed * Time.deltaTime );
		}
	}
}
