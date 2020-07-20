using UnityEditor;
using UnityEngine;

namespace CoolJoystick
{
	/// <summary>
	/// Class for adding joystick logo to Joystick Configuration files
	/// </summary>
	[CustomEditor ( typeof ( JoystickConfiguration ) )]
	[CanEditMultipleObjects]
	public class JoystickConfigurationEditor : AddJoystickLogo
	{
	}

}
