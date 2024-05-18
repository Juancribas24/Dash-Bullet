using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [System.Serializable]
    public class BulletType
    {
        public string name;
        public GameObject prefab;
        public int poolSize;
    }

    public BulletType[] bulletTypes;
    private Dictionary<string, List<GameObject>> pools;

    void Awake()
    {
        pools = new Dictionary<string, List<GameObject>>();

        foreach (var bulletType in bulletTypes)
        {
            List<GameObject> pool = new List<GameObject>();

            for (int i = 0; i < bulletType.poolSize; i++)
            {
                GameObject bullet = Instantiate(bulletType.prefab);
                bullet.SetActive(false);
                pool.Add(bullet);
            }

            pools.Add(bulletType.name, pool);
        }
    }

    public GameObject GetBullet(string bulletType)
    {
        foreach (GameObject bullet in pools[bulletType])
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                return bullet;
            }
        }

        GameObject newBullet = Instantiate(pools[bulletType][0]);
        newBullet.SetActive(false);
        pools[bulletType].Add(newBullet);
        return newBullet;
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
    }
}
