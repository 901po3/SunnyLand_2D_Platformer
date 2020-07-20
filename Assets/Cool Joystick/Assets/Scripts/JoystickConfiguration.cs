using System;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Object = UnityEngine.Object;

namespace CoolJoystick
{
	[CreateAssetMenu ( fileName = "JoystickConfiguration" , menuName = "Joystick Configuration File" , order = 1 )]
	public class JoystickConfiguration : ScriptableObject
	{

		public enum JoystickMode
		{
			AllAxis ,
			Horizontal ,
			Vertical
		}

		public enum JoystickMoveType
		{
			Fixed ,
			Dynamic ,
			Follow
		}

		[Serializable]
		public class JoustickVisualSetup
		{
			public Gradient       TouchedColor;
			public Gradient       NotTouchedColor;
			public AnimationCurve FingerDownEffect;
			public AnimationCurve FingerUpEffect;
		}

		public JoustickVisualSetup ForegroundImageCustomization;
		public JoustickVisualSetup BackgroundImageCustomization;

		public bool  InterpolateBackground;
		public float InterpolateBackgroundSpeed = 15;
		public bool  InterpolateForeground;
		public float InterpolateForegroundSpeed = 15;
		public bool  ClampMovementMagnitude;
		public float ClampMagnitudeRange = 5;

		public JoystickMode     Axis     = JoystickMode.AllAxis;
		public JoystickMoveType MoveType = JoystickMoveType.Fixed;

		[Range ( 0f , 2f )] public float MovementRange = 1f;
	}

	#if UNITY_EDITOR
	public class MakeScriptableObject
	{

		//Method for getting path of selected object or folder in project window
		private static string GetPath ( Object obj )
		{
			string path;
			if ( obj == null ) return "";

			//Getting path of the selected object
			path = AssetDatabase.GetAssetPath ( obj.GetInstanceID ( ) );
			if ( path.Length > 0 )
			{
				return Directory.Exists ( path ) ? path : AssetDatabase.GetAssetPath ( obj );
			}

			return "";
		}

		//Method for creating scriptable object - config of joystick
		[MenuItem ( "Assets/Create/Joystick Configuration File" )]
		public static void CreateMyAsset ( )
		{

			//Creating instance of config file
			var asset = ScriptableObject.CreateInstance < JoystickConfiguration > ( );
			var path  = GetPath ( Selection.activeObject );

			//Saving config to project
			AssetDatabase.CreateAsset ( asset , path + "/NewJoystickConfig.asset" );
			AssetDatabase.SaveAssets ( );

			// Do focusing on created config in project window
			EditorUtility.FocusProjectWindow ( );
			// Selecting him
			Selection.activeObject = asset;
		}
	}
	#endif
}
