using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class SurroundingBalls : MonoBehaviour
{
	[SerializeField] int ballsCount;
	[SerializeField] BallSurrounding ballPrefab;

	private void Awake()
	{
		float circleAngle = 360 / ballPrefab.circleRotationSpeed / ballsCount;
		for (int i = 0; i < ballsCount; i++)
		{
			BallSurrounding ball = Instantiate(ballPrefab);
			ball.owner = PlayerState.Instance.Transform;
			ball.SetInitialCircleAngle(i * circleAngle);
			if (i % 2 == 0) ball.SetInitialUpDownAngle(45f / 2f);
			else ball.SetInitialUpDownAngle(0);
		}
	}
}
