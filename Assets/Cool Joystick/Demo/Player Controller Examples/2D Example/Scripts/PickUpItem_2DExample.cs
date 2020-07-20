using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoolJoystick
{
	/// <summary>
	/// Class which controls how we will pick up item
	/// </summary>
	public class PickUpItem_2DExample : MonoBehaviour
	{

		public float         PickUpRadius = 100; // Radius how closer we needs be to item for pick up
		public float         RotateSpeed  = 30;  // Rotation speed of item
		public RectTransform Player;             // Reference to player transform

		private Player2DExample _playerController; // Player controller script
		private RectTransform   _rt;               // Reference to self rect transform

		private void Start ( )
		{
			_rt = GetComponent < RectTransform > ( ); // Getting self rect transform
			if ( Player )
				_playerController =
					Player.GetComponent < Player2DExample > ( ); // And then getting player controller script
		}

		private void Update ( )
		{
			transform.Rotate ( 0 , 0 , RotateSpeed * Time.deltaTime ); // Rotate item
			if ( !_rt )
				return; // if we dont have self rect transform we will not go to next step
			var dstToPlayer =
				Vector2.Distance ( Player.anchoredPosition ,
								   _rt.anchoredPosition ); // Checking distance from player to item
			if ( dstToPlayer <= PickUpRadius )
				PickUpItem ( ); // And if distance less or equal to radius we can do pick up item
		}

		private void PickUpItem ( )
		{
			if ( !_playerController || !_rt )
				return;                       // if we don't have player controller or self rect transform we not going to next step
			_playerController.ItemCount += 1; // Add new item to player controller for count
			//And then changing position of item for new pick up
			_rt.anchoredPosition = new Vector2 ( Random.Range ( Player.sizeDelta.x , 1920 - Player.sizeDelta.x ) / 2 ,
												 Random.Range ( Player.sizeDelta.x , 1080 - Player.sizeDelta.x ) / 2 );
		}
	}
}
