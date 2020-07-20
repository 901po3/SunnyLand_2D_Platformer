using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoolJoystick
{
	/// <summary>
	/// Class which controls how we will pick up item
	/// </summary>
	public class PickUpItem_2DExample3 : MonoBehaviour
	{

		public float         PickUpRadius = 100; // Radius how closer we needs be to item for pick up
		public float         RotateSpeed  = 30;  // Rotation speed of item
		public RectTransform Player;             // Reference to player transform

		public Transform[] SpawnPoints; // Reference to item spawn points

		private Player2DExample3 _playerController; // Player controller script
		private RectTransform    _rt;               // Reference to self rect transform
		private int              _rndPoint;

		private void Start ( )
		{
			_rt = GetComponent < RectTransform > ( ); // Getting self rect transform
			if ( Player )
				_playerController =
					Player.GetComponent < Player2DExample3 > ( ); // And then getting player controller script
		}

		// Update is called once per frame
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
			if ( !_playerController ) return; // if we don't have player controller we not going to next step
			_playerController.ItemCount += 1; // Add new item to player controller for count
			//Do getting new spawn point index which will be not same as previous
			var rndPoint = Random.Range ( 0 , SpawnPoints.Length );
			while ( rndPoint == _rndPoint )
				rndPoint = Random.Range ( 0 , SpawnPoints.Length );
			_rndPoint = rndPoint;
			//And then changing position of item for new pick up
			transform.position = SpawnPoints[rndPoint].position;
		}
	}
}
