using UnityEngine;
using Spine.Unity;

public class Archer : MonoBehaviour
{
    [SerializeField] private Transform bowPosition;
    [SerializeField] private SkeletonAnimation skeleton; 
    [SerializeField] private ArrowPool arrowPool;
    [SerializeField] private Trajectory trajectory;
    
    private Camera mainCamera;
    private Vector3 direction;
    
    private void Start()
    {
        mainCamera = Camera.main;
        trajectory.InitializeDots();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartAiming();
        }

        if (Input.GetMouseButton(0))
        {
            UpdateAiming();
        }

        if (Input.GetMouseButtonUp(0))
        {
            ReleaseArrow();
        }
    }

    private void StartAiming()
    {
        trajectory.Show();
        PlayAnimations("attack_start", "attack_target");
    }

    private void UpdateAiming()
    {
        var startPosition = bowPosition.position;
        var currentMousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        direction = (startPosition - currentMousePosition);
        direction.z = 0f;
        bowPosition.rotation = Quaternion.FromToRotation(bowPosition.right, direction.normalized);
        trajectory.UpdateTrajectory(direction);
        ApplyManualRotation();
    }

    private void ReleaseArrow()
    {
        PlayAnimations("attack_finish", "idle");
        trajectory.Hide();
        Fire();
    }

    private void ApplyManualRotation()
    {
        var boneBody = skeleton.Skeleton.FindBone("body");
        var boneGun = skeleton.Skeleton.FindBone("gun");

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        boneBody.Rotation = angle;
        boneGun.Rotation = angle;
    }

    private void Fire()
    {
        var arrow = arrowPool.Pool.Get();
        arrow.transform.position = bowPosition.position;
        arrow.GetComponent<Rigidbody2D>().velocity = direction;
    }

    private void PlayAnimations(string startAnim, string nextAnim)
    {
        skeleton.AnimationState.SetAnimation(0, startAnim, false);
        skeleton.AnimationState.AddAnimation(0, nextAnim, true, 0f);
    }
}