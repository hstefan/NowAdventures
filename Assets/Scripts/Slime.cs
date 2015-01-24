using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TiledCharacter))]
public class Slime : MonoBehaviour
{
	private TiledCharacter tiled;

	void Awake()
	{
		tiled = GetComponent<TiledCharacter>();
	}
}
