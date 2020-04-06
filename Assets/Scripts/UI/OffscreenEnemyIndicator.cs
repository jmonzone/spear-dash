using UnityEngine;

public class OffscreenEnemyIndicator : MonoBehaviour
{
    [SerializeField] private Transform indicator;

    private Player  player;
    private Camera cam;
    private Vector2 range;

    private void Awake()
    {
        cam = Camera.main;

        range.x = Screen.width - (Screen.width / 6);
        range.y = Screen.height - (Screen.height / 7);
        range /= 2f;

        //PlayerManager.OnPlayerSpawned += (player) =>
        //{
        //    if (!player.IsMine) return;
        //    this.player = player;
        //    this.player.IsOnScreen += () => Display(false);
        //    this.player.IsOffScreen += () => Display();
        //};
    }

    private void Update()
    {
        var direction = player.transform.position - cam.transform.position;
        direction.z = 0;
        indicator.up = -direction;

        Vector2 indPos = new Vector2(range.x * direction.x, range.y * direction.y) / 11;
        indPos = new Vector2((Screen.width / 2) + indPos.x, Screen.height / 2 + indPos.y);
        var pos = cam.ScreenToWorldPoint(indPos);
        pos.z = 0;

        //var pos = cam.transform.position + direction / 2.0f;
        //pos.z = 0;
        indicator.position = pos;
    }

    private void Display(bool display = true)
    {
        indicator.gameObject.SetActive(display);
    }
}
