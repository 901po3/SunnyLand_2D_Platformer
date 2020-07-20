using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoolJoystick
{
	/// <summary>
	/// Class which controls how we will pick up item
	/// </summary>
	public class PickUpItem_3DExample : MonoBehaviour
	{

		public float     PickUpRadius = 100; // Radius how closer we needs be to item for pick up
		public Transform Player;             // Reference to player transform

		private Player3DExample _playerController; // Player controller script

		private void Start ( )
		{
			if ( Player )
				_playerController = Player.GetComponent < Player3DExample > ( ); // Getting player controller script
		}

		// Update is called once per frame
		private void Update ( )
		{
			var dstToPlayer =
				Vector3.Distance ( Player.position , transform.position ); // Checking distance from player to item
			if ( dstToPlayer <= PickUpRadius )
				PickUpItem ( ); // And if distance less or equal to radius we can do pick up item
		}

		private void PickUpItem ( )
		{
			if ( !_playerController ) return; // if we don't have player controller we not going to next step
			_playerController.ItemCount += 1; // Add new item to player controller for count
			//And then changing position of item for new pick up
			transform.position =
				new Vector3 ( Random.Range ( -7 , 7 ) , transform.position.y , Random.Range ( -7 , 7 ) );
		}
	}
}
