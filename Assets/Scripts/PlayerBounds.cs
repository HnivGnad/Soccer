using UnityEngine;

public class PlayerBounds : MonoBehaviour
{
    // Xác định biên sân bóng
    public float minX = -14f;
    public float maxX = 14f;
    public float minZ = -10f;
    public float maxZ = 10f;

    void Update()
    {
        // Lấy vị trí hiện tại
        Vector3 pos = transform.position;

        // Giới hạn vị trí trong khung hình chữ nhật
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);

        // Áp dụng vị trí đã giới hạn
        transform.position = pos;
    }
}
