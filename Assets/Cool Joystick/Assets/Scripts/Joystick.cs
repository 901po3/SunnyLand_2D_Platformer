using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CoolJoystick
{

	public class Joystick : MonoBehaviour , IPointerDownHandler , IPointerUpHandler , IDragHandler
	{
		public RectTransform JoystickBackground , JoystickForeground; // Main transforms of joystick needed for working

		public JoystickConfiguration
			Config; // Config of Joystick where stored all setup values (like movement range ,etc.)

		//Getters and Setters for quick access to joystick values
		public float
			Horizontal { get { return _axis.x; } set { _axis.x = value; } } // For horizontal direction movement

		public float   Vertical  { get { return _axis.y; } set { _axis.y = value; } } // For vertical direction movement
		public Vector2 Direction { get { return _axis; }   set { _axis   = value; } } // For all directions movement
		public Vector2 Delta     { get { return _delta; }  set { _delta  = value; } } // For delta of directions

		public bool
			Pressed
		{
			get;
			set;
		} // For understand , joystick moving now or not

		private     CanvasScaler  _canvasScaler;
		private     RectTransform _foregroundParent , _backgroundParent , _canvasScalerRT;
		private new RectTransform transform;

		private Vector2 _newForegroundPos ,
						_newBackgroundPos ,
						_newForegroundPosLerp ,
						_newBackgroundPosLerp ,
						_startJoystickFolderPosition ,
						_refAxis ,
						_lastAxis ,
						_axis ,
						_delta;

		private Image     _backgroundImage , _foregroundImage;
		private Coroutine _currentEffect;
		private float     _nonDragTime;

		// Method for initialization all values
		private void Start ( )
		{
			transform     = GetComponent < RectTransform > ( );        // Getting touch surface transform
			_canvasScaler = GetComponentInParent < CanvasScaler > ( ); // Getting CanvasScaler of joystics canvas
			_canvasScalerRT =
				GetComponentInParent < CanvasScaler > ( )
					.GetComponent < RectTransform > ( ); // Then getting his transform

			// Setup background part
			// Setup default size from curve
			JoystickBackground.localScale =
				Vector3.one * Config.BackgroundImageCustomization.FingerDownEffect.Evaluate ( 0 );
			_backgroundImage =
				JoystickBackground.GetComponent < Image > ( ); // Getting image component of Background part
			if ( _foregroundImage )                            // if we have image component
				_backgroundImage.color =
					Config.BackgroundImageCustomization.TouchedColor
						  .Evaluate ( 0 ); // Setup default color from gradient

			_backgroundParent =
				JoystickBackground.parent.GetComponent < RectTransform > ( );   // Then getting his parent transform
			_startJoystickFolderPosition = JoystickBackground.anchoredPosition; // Saving his default position
			_newBackgroundPos            = _startJoystickFolderPosition;
			// Setup foreground part
			// Setup default size from curve
			JoystickForeground.localScale =
				Vector3.one * Config.ForegroundImageCustomization.FingerDownEffect.Evaluate ( 0 );
			_foregroundImage =
				JoystickForeground
					.GetComponent < Image
					> ( );          // Then do almost the same for Foreground part - getting image component
			if ( _foregroundImage ) // if we have image component
				_foregroundImage.color =
					Config.ForegroundImageCustomization.TouchedColor
						  .Evaluate ( 0 ); // Setup default color from gradient

			_foregroundParent = JoystickForeground.parent.GetComponent < RectTransform > ( ); // Getting transform
			_newForegroundPos = JoystickForeground.anchoredPosition;
		}

		// Method for updating position of our joystick
		private void Update ( )
		{

			if ( Pressed ) // If we pressing/moving joystick
			{
				// Do interpolate for position of foreground part
				_newForegroundPosLerp = Vector2.Lerp ( _newForegroundPosLerp , _newForegroundPos ,
													   Time.deltaTime * Config.InterpolateForegroundSpeed );
				// Then setup this interpolated value to foreground part transform position
				JoystickForeground.anchoredPosition =
					Config.InterpolateForeground ? _newForegroundPosLerp : _newForegroundPos;

				// Do interpolate for position of background part
				_newBackgroundPosLerp = Vector2.Lerp ( _newBackgroundPosLerp , _newBackgroundPos ,
													   Time.deltaTime * Config.InterpolateBackgroundSpeed );
				// Then setup this interpolated value to background part transform position
				JoystickBackground.anchoredPosition =
					Config.InterpolateBackground ? _newBackgroundPosLerp : _newBackgroundPos;

				_nonDragTime += Time.deltaTime; //Checking non drag time
				if ( _nonDragTime > Time.deltaTime )
					_delta = Vector2.zero; // And set delta to zero if non drag time greater than frame time
			}

			var joystickLimit = GetForegroundMovementRange ( ); // Getting limit for foreground part movement
			JoystickForeground.anchoredPosition =
				Vector2.ClampMagnitude ( JoystickForeground.anchoredPosition ,
										 joystickLimit ); // Clamping position of foreground part to this limit
			_axis = Pressed
						? JoystickForeground.anchoredPosition / joystickLimit
						: Vector2.zero; // Setup axis value based on position of foreground part (range[-1,1] for boh directions)
			//ClampBackgroundPosition ( ); // Clamping position of background part for follow joystick mode *WIP*
		}

		// Method for setup joystick for all resolutions and ratios
		private Vector2 GetPointInCanvasSpace ( Vector2 pos )
		{
			var referenceRes = _canvasScalerRT.sizeDelta;                    // Getting size of canvas
			var screenRes    = new Vector2 ( Screen.width , Screen.height ); // Getting current screen resolution
			var finalRes =
				new Vector2 ( screenRes.x / referenceRes.x ,
							  screenRes.y /
							  referenceRes
								  .y ); // Then divide current screen resolution by reference resolution (1920 x 1080 as default)
			var finalPos =
				new Vector2 ( pos.x / finalRes.x ,
							  pos.y / finalRes.y ); // Then divide received position of joystick by final  resolution
			return finalPos;                        // Returning the result
		}

		// Coroutine of finger down effect
		private IEnumerator OnPointerDownEffect ( )
		{
			var t       = Time.time;     // Get current time
			var endTime = Time.time + 1; // Setup final time
			while ( t < endTime )        // Doing while to end of setup time
			{
				t = Time.time;                   // Always rewriting current time
				var alpha = 1 - ( endTime - t ); // Calculate alpha value based on time
				// Then setup gradient color by alpha to foreground and background parts
				_foregroundImage.color = Config.ForegroundImageCustomization.TouchedColor.Evaluate ( alpha );
				_backgroundImage.color = Config.BackgroundImageCustomization.TouchedColor.Evaluate ( alpha );
				// Then setup scale from curve by alpha to foreground and background parts
				JoystickForeground.localScale =
					Vector3.one * Config.ForegroundImageCustomization.FingerDownEffect.Evaluate ( alpha );
				JoystickBackground.localScale =
					Vector3.one * Config.BackgroundImageCustomization.FingerDownEffect.Evaluate ( alpha );
				yield return null; // Returning null for continue coroutine during needed time
			}
		}

		// Coroutine of finger up effect
		private IEnumerator OnPointerUpEffect ( )
		{
			var t       = Time.time;     // Get current time
			var endTime = Time.time + 1; // Setup final time
			while ( t < endTime )        // Doing while to end of setup time
			{
				t = Time.time;                   // Always rewriting current time
				var alpha = 1 - ( endTime - t ); // Calculate alpha value based on time
				// Then setup gradient color by alpha to foreground and background parts
				_foregroundImage.color = Config.ForegroundImageCustomization.NotTouchedColor.Evaluate ( alpha );
				_backgroundImage.color = Config.BackgroundImageCustomization.NotTouchedColor.Evaluate ( alpha );
				// Then setup scale from curve by alpha to foreground and background parts
				JoystickForeground.localScale =
					Vector3.one * Config.ForegroundImageCustomization.FingerUpEffect.Evaluate ( alpha );
				JoystickBackground.localScale =
					Vector3.one * Config.BackgroundImageCustomization.FingerUpEffect.Evaluate ( alpha );
				yield return null; // Returning null for continue coroutine during needed time
			}
		}

		// Method which implement interface for handle finger down event
		public void OnPointerDown ( PointerEventData eventData )
		{
			if ( Pressed ) return; // If we moving joystick now we not need execute this method , only for begin of tap
			//If we not using moveType as Fixed , we need move joystick into tap position

			var xAlpha = Mathf.Lerp ( 1 , 2 , JoystickBackground.anchorMin.x ); // Checking anchor value by X axis

			// Fixing position for all resolutions and all aspect rations and all touch zone size and position and all anchors
			if ( transform && Config.MoveType != JoystickConfiguration.JoystickMoveType.Fixed )
				_newBackgroundPos = GetPointInCanvasSpace ( eventData.position ) -
									new
										Vector2 ( transform.rect.width * xAlpha + transform.offsetMin.x + transform.rect.x * 2 ,
												  transform.rect.height * JoystickBackground.anchorMin.y +
												  transform.offsetMin.y );

			Pressed = true; // Then we enabling moving of joystick
			// And stopping current joystick effect and start new
			if ( _currentEffect != null ) StopCoroutine ( _currentEffect );
			_currentEffect = StartCoroutine ( OnPointerDownEffect ( ) );
		}

		// Method which implement interface for handle finger up event
		//Resetting all temp values to default state
		public void OnPointerUp ( PointerEventData eventData )
		{
			// We stopping current joystick effect and start new
			if ( _currentEffect != null ) StopCoroutine ( _currentEffect );
			_currentEffect = StartCoroutine ( OnPointerUpEffect ( ) );
			//Then reset all temp values and variables to default state and positions
			Pressed               = false;
			_newForegroundPos     = Vector2.zero;
			_newForegroundPosLerp = Vector2.zero;
			_newBackgroundPos     = _startJoystickFolderPosition;
			_newBackgroundPosLerp = _startJoystickFolderPosition;
			_delta                = Vector2.zero;
			if ( JoystickForeground ) JoystickForeground.anchoredPosition = _newForegroundPos;
			if ( JoystickBackground ) JoystickBackground.anchoredPosition = _newBackgroundPos;
		}

		// Method for getting movement range of joystick
		public float GetForegroundMovementRange ( )
		{
			var divider = _foregroundParent.sizeDelta.x         / JoystickForeground.sizeDelta.x * 2;
			var limit   = _foregroundParent.sizeDelta.magnitude * Config.MovementRange           / divider;
			return limit;
		}

		// Method which implement interface for handle finger drag event
		public void OnDrag ( PointerEventData eventData )
		{
			if ( !Pressed )
				return;          // If we not moving joystick now we not need execute this method , only for begin of tap
			_nonDragTime = 0.0f; // set non drag time to zero because we do drag
			_delta = eventData.delta /
					 _canvasScaler
						 .scaleFactor; // Getting delta of movement from drag event with adapting it to current resolution
			if ( Config.ClampMovementMagnitude )
				_delta =
					Vector2.ClampMagnitude ( _delta , Config.ClampMagnitudeRange ); // Then clamping delta if we needed
			// Then setup ingoring not needed axis by setup it to zero
			if ( Config.Axis == JoystickConfiguration.JoystickMode.Horizontal ) _delta.y = 0;
			if ( Config.Axis == JoystickConfiguration.JoystickMode.Vertical ) _delta.x   = 0;

			// Then doing *WIP*  type of movement
			// Getting drag direction
			var followEndPoint      = JoystickForeground.anchoredPosition + _delta;
			var dstToFollowEndPoint = Vector2.Distance ( followEndPoint , Vector2.zero );
			var limit               = GetForegroundMovementRange ( ); // getting movement limit of foreground part
			// Then checking if we dragging joystick further limit of joystick movement
			var followReason = Vector2.Distance ( Vector2.zero , _newForegroundPos ) > limit &&
							   dstToFollowEndPoint                                   > limit &&
							   Config.MoveType ==
							   JoystickConfiguration.JoystickMoveType.Follow;

			if ( followReason )
				_newBackgroundPos  += _delta; // If YES, we moving background part with foreground part by delta
			else _newForegroundPos += _delta; // If NO , we moving only foreground part
		}

		/// <summary>
		/// WIP
		/// </summary>
		// Method for clamping position of background part in touch surface bounds
		private void ClampBackgroundPosition ( )
		{
			// Getting current position and current parent size of background part
			var x    = JoystickBackground.anchoredPosition.x;
			var y    = JoystickBackground.anchoredPosition.y;
			var step = JoystickBackground.sizeDelta.x / 2;
			var size = new Vector2 ( _backgroundParent.rect.width , _backgroundParent.rect.height );

			// Clamping position of background part to touch surface bounds
			if ( x - step < 0 ) JoystickBackground.anchoredPosition      = new Vector2 ( 0      + step , y );
			if ( x + step > size.x ) JoystickBackground.anchoredPosition = new Vector2 ( size.x - step , y );
			if ( y - step < 0 )
				JoystickBackground.anchoredPosition = new Vector2 ( x , 0 + step );
			if ( y + step > size.y )
				JoystickBackground.anchoredPosition = new Vector2 ( x , size.y - step );
		}
	}

}
