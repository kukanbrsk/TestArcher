using UnityEngine;
using UnityEngine.Pool;

public class ArrowPool : MonoBehaviour
{
    [SerializeField] private Arrow arrowPrefab;

    private ObjectPool<Arrow> pool;
    public ObjectPool<Arrow> Pool => pool;

    private void Start()
    {
        pool = new ObjectPool<Arrow>(
            CreateArrow,
            OnGetArrow,
            OnReleaseArrow,
            OnDestroyArrow,
            false,
            10,
            20);
    }

    private Arrow CreateArrow()
    {
        var arrow = Instantiate(arrowPrefab, transform);
        arrow.OnReturnToPool += ReleaseArrow;
        arrow.gameObject.SetActive(false);
        return arrow;
    }

    private void OnGetArrow(Arrow obj)
    {
        obj.gameObject.SetActive(true);
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        obj.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
    
    private void ReleaseArrow(Arrow obj)
    {
       pool.Release(obj);
    }
    private void OnReleaseArrow(Arrow obj)
    {
        obj.gameObject.SetActive(false);
    }

    private void OnDestroyArrow(Arrow obj)
    {
        Destroy(obj.gameObject);
    }
}