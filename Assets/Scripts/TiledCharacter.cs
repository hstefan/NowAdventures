using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TiledCharacter : MonoBehaviour {
	[SerializeField]
	private Transform mapOrigin;
	public int TileX;
	public int TileY;
	[SerializeField]
	private Vector3 BaseOffset;

#if UNITY_EDITOR
	void Update()
	{
		if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
		{
			enabled = false;
		}
		else
		{
			transform.position = getPositionForTile(TileX, TileY);
		}
	}
#endif

	private Vector3 getRealPosition()
	{
		return getPositionForTile(TileX, TileY);
	}

	private Vector3 getPositionForTile(int x, int y)
	{
		return mapOrigin.transform.position + BaseOffset + new Vector3(x, y, 0f);
	}

	public void MoveToTile(int x, int y, float duration)
	{
		StartCoroutine(MoveToTileCo(x, y, duration));
	}

	public IEnumerator MoveToTileCo(int x, int y, float duration)
	{
		TileX = x;
		TileY = y;

		Vector3 prev_pos = transform.position;
		Vector3 new_pos = getPositionForTile(x, y);

		for (float t = 0; t <= duration; t += Time.deltaTime)
		{
			transform.position = Vector3.Lerp(prev_pos, new_pos, t / duration);
			yield return null;
		}
		rigidbody2D.MovePosition(new_pos);
	}

	public bool CollideWithBlocker(Vector2 direction) {
		var hits = Physics2D.RaycastAll(getRealPosition(), direction, 1.0f);
		for (int i = 0; i < hits.Length; ++i) {
			if (hits[i].collider.gameObject.layer == 9) {
				return true;
			}
		}
		return false;
	}
}