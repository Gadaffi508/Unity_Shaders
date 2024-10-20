using UnityEngine;

public interface IWallMaterialUpdater
{
    void UpdateWallMaterial(Vector3 position);
    void SetSize(float size);
}

public class WallMaterialUpdater : IWallMaterialUpdater
{
    private Material _wallMaterial;
    private int _sizeId;
    private int _posId;

    public WallMaterialUpdater(Material wallMaterial)
    {
        _wallMaterial = wallMaterial;
        _sizeId = Shader.PropertyToID("_Size");
        _posId = Shader.PropertyToID("_Player_Position");
    }

    public void UpdateWallMaterial(Vector3 position)
    {
        var view = Camera.main.WorldToViewportPoint(position);
        _wallMaterial.SetVector(_posId, view);
    }

    public void SetSize(float size) // Erişim seviyesini public yaptık
    {
        _wallMaterial.SetFloat(_sizeId, size);
    }
}

public interface IVisibilityChecker
{
    bool IsVisible(Vector3 position, LayerMask mask);
}

public class RaycastVisibilityChecker : IVisibilityChecker
{
    private Camera _camera;

    public RaycastVisibilityChecker(Camera camera)
    {
        _camera = camera;
    }

    public bool IsVisible(Vector3 position, LayerMask mask)
    {
        var dir = _camera.transform.position - position;
        var ray = new Ray(position, dir.normalized);
        return Physics.Raycast(ray, 3000, mask);
    }
}

public class CircleSync : MonoBehaviour
{
    [SerializeField] private Material wallMaterial;
    [SerializeField] private Camera camera;
    [SerializeField] private LayerMask mask;

    private IWallMaterialUpdater _wallMaterialUpdater;
    private IVisibilityChecker _visibilityChecker;

    private void Awake()
    {
        _wallMaterialUpdater = new WallMaterialUpdater(wallMaterial);
        _visibilityChecker = new RaycastVisibilityChecker(camera);
    }

    private void Update()
    {
        bool isVisible = _visibilityChecker.IsVisible(transform.position, mask);
        _wallMaterialUpdater.SetSize(isVisible ? 1f : 0f);
        _wallMaterialUpdater.UpdateWallMaterial(transform.position);
    }
}
