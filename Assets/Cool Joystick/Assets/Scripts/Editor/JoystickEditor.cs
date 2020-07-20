using UnityEditor;
using UnityEngine;

namespace CoolJoystick
{
	/// <summary>
	/// Class for adding joystick logo to Joystick script
	/// </summary>
	[CustomEditor ( typeof ( Joystick ) )]
	[CanEditMultipleObjects]
	public class JoystickEditor : AddJoystickLogo
	{
	}
}
