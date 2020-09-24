using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    [SerializeField] private RectTransform playerItem, enemyItem;
    [SerializeField] private RectTransform obstacleItemPrefab;
    [SerializeField] private RectTransform miniMapRoot;
    [SerializeField] private Transform player, enemy;
    [SerializeField] private Transform[] obstacles;
    [SerializeField] private Vector2 mapSize = new Vector2(50, 50);
    [SerializeField] private Vector2 mapCenter = new Vector2(0, 0);
    private float arrowSize;
    private Vector2 miniMapSize;
    private float xRate, yRate;
    // Start is called before the first frame update
    void Start()
    {
        arrowSize = enemyItem.sizeDelta.y;
        miniMapSize = miniMapRoot.sizeDelta;
        xRate = mapSize.x / miniMapSize.x;
        yRate = mapSize.y / miniMapSize.y;

        CreateObstacleItems();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMiniMapAngle();
        CalculateMiniMapPivot();
        CalculateEnemyPosition();
    }

    private void CreateObstacleItems()
    {
        for (int i = 0; i < obstacles.Length; i++)
        {
            Transform obstacle = obstacles[i];
            Transform obstacleItem = GameObject.Instantiate(obstacleItemPrefab, miniMapRoot).transform;
            obstacleItem.gameObject.SetActive(true);
            Vector3 position = new Vector3((obstacle.position.x - mapCenter.x) * miniMapSize.x / mapSize.x, (obstacle.position.z - mapCenter.y) * miniMapSize.y / mapSize.y, 0);
            obstacleItem.localPosition = position;
        }
    }

    private void CalculateMiniMapAngle()
    {
        float rotateY = player.transform.eulerAngles.y;
        miniMapRoot.localEulerAngles = new Vector3(0, 0, rotateY);
    }

    private void CalculateMiniMapPivot()
    {
        float miniMapX = (player.position.x - mapCenter.x) / xRate;
        float miniMapY = (player.position.z - mapCenter.y) / yRate;
        miniMapX = -miniMapX;
        miniMapY = -miniMapY;
        float pivotX = 0.5f - miniMapX / (miniMapSize.x * 0.5f) * 0.5f;
        float pivotY = 0.5f - miniMapY / (miniMapSize.y * 0.5f) * 0.5f;
        miniMapRoot.pivot = new Vector2(pivotX, pivotY);
        miniMapRoot.anchoredPosition = Vector2.zero;
    }

    private void CalculateEnemyPosition()
    {
        Vector2 position = new Vector2((enemy.position.x - mapCenter.x) * miniMapSize.x / mapSize.x, (enemy.position.z - mapCenter.y) * miniMapSize.y / mapSize.y);
        enemyItem.anchoredPosition = position;
    }

    private void SetInMiniMap()
    {
        float x = Mathf.Clamp(enemyItem.position.x , miniMapSize.x * -0.5f, miniMapSize.x * 0.5f) + playerItem.position.x;
        float y = Mathf.Clamp(enemyItem.position.y, miniMapSize.y * -0.5f, miniMapSize.y * 0.5f) + playerItem.position.y;
        Debug.LogError(enemyItem.localPosition);
        enemyItem.localPosition = new Vector2(x, y);
    }

}
