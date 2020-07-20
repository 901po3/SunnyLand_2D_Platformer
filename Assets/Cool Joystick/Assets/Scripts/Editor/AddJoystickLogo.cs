using UnityEditor;
using UnityEngine;

namespace CoolJoystick
{
	/// <summary>
	/// Class for adding joystick logo to scripts
	/// </summary>
	public class AddJoystickLogo : Editor
	{

		private Texture _texture;

		/// <summary>
		/// Getting texture from resources folders
		/// </summary>
		private void OnEnable ( ) { _texture = Resources.Load < Texture2D > ( "JoystickLogo" ); }

		/// <summary>
		/// Drawing the logo
		/// </summary>
		public override void OnInspectorGUI ( )
		{
			//If we have logo texture from resources -> we can draw the logo
			if ( _texture )
			{
				var rt = EditorGUILayout.GetControlRect ( );
				var r1 = new Rect ( rt );
				r1.height   =  128;
				r1.width    += 30;
				r1.position =  new Vector2 ( rt.position.x - 15 , rt.position.y );
				EditorGUI.DrawRect ( r1 , new Color ( 33 / 255f , 33 / 255f , 33 / 255f ) );

				var r3 = new Rect ( rt );
				r3.height   =  5;
				r3.width    += 30;
				r3.position =  new Vector2 ( rt.position.x - 15 , rt.position.y + 128 );
				EditorGUI.DrawRect ( r3 , new Color ( 1 , 1 , 1 ) );

				var r2 = new Rect ( rt );
				r2.position = new Vector2 ( rt.width / 2 - 128 , r2.position.y );
				r2.width    = 256;
				r2.height   = 128;
				EditorGUI.DrawPreviewTexture ( r2 , _texture );

				GUILayoutUtility.GetRect ( r2.position.x , r2.position.y , 120 , 0 );
			}

			// After logo drawing other properties
			DrawDefaultInspector ( );
		}
	}
}
