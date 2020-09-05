using UnityEngine;

public class PathTile : Tile
{
    public float slowDown = 0f;

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.black);
        }
    }
}
