using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoolJoystick
{
	/// <summary>
	/// Class which controls what bullet needs to do
	/// </summary>
	public class Bullet_2DExample2 : MonoBehaviour
	{
		public float            Speed = 100; // Speed of bullet moving
		public Player2DExample2 Player;      // Reference to player controller script

		private RectTransform _rt , _canvas; // Reference to self rect transform and canvas rect transform

		private void Start ( )
		{
			// Getting self and canvas rect transforms
			_rt     = GetComponent < RectTransform > ( );
			_canvas = GetComponentInParent < Canvas > ( ).GetComponent < RectTransform > ( );
		}

		// Update is called once per frame
		private void Update ( )
		{
			// Moving bullet to need direction with setup speed
			transform.Translate ( Vector3.up * Speed * Time.deltaTime , Space.World );

			// If bullet higher then screen height disable her
			if ( _rt && _rt.anchoredPosition.y > _canvas.sizeDelta.y / 2 )
				gameObject.SetActive ( false );
		}

		// Method for handle trigger enter
		private void OnTriggerEnter2D ( Collider2D other )
		{
			if ( other.isTrigger ) return;        // if entered collider not a trigger also
			if ( Player ) Player.ItemCount ++;    // Giving to player plus 1 destroyed item
			other.gameObject.SetActive ( false ); // Disabling destroyed item
			gameObject.SetActive ( false );       // Disabling bullet
		}
	}
}
