using UnityEngine;

public class CameraPresenter
{
    private const float MAX_VERTICAL_ANGLE_VARIATION = 45 * Mathf.Deg2Rad;
    private const float MIN_VERTICAL_ANGLE_VARIATION = 40 * Mathf.Deg2Rad;
    private const float SMOOTH_VELOCITY = 2f;

    private CameraView _view;
    private float _angleHorizontal;
    private float _angleVertical;
    private Transform _pivotPoint;
    private float _initialVerticalAngle;
    private float _initialDistanceToPlayer;
    private bool _isSmoothing;
    

    public CameraPresenter(CameraView view)
    {
        _view = view;
        _pivotPoint = _view.GetCameraPivotPoint();
        _angleVertical = 135 * Mathf.Deg2Rad;
        _initialVerticalAngle = _angleVertical;
        _initialDistanceToPlayer = Vector3.Distance(_pivotPoint.position, _view.transform.position);
    }


    public void VerticalOrbit()
    {
        var dirVectorA = Vector3.up;
        var dirVectorB = new Vector3(_view.transform.forward.x, 0, _view.transform.forward.z).normalized;

        _angleVertical += Input.GetAxis("Mouse Y") * _view.HorizontalSensitivity;
        _angleVertical = Mathf.Clamp(_angleVertical, _initialVerticalAngle - MIN_VERTICAL_ANGLE_VARIATION,
            _initialVerticalAngle + MAX_VERTICAL_ANGLE_VARIATION);

        var finalDistance = CalculateSmoothDistance();

        _view.transform.position = _pivotPoint.position +
                                   (dirVectorA * Mathf.Sin(_angleVertical) + dirVectorB * Mathf.Cos(_angleVertical)) *
                                   finalDistance;
        LookAtPlayer();
    }

    public void HorizontalOrbit()
    {
        _angleHorizontal += Input.GetAxis("Mouse X") * _view.HorizontalSensitivity;

        var finalDistance = CalculateSmoothDistance();

        _view.transform.position = _pivotPoint.position +
                                   new Vector3(Mathf.Sin(_angleHorizontal), 0, Mathf.Cos(_angleHorizontal)) *
                                   finalDistance;
        LookAtPlayer();
    }

    public void AvoidObstacles()
    {
        RaycastHit avoidHit;
        if (Physics.Linecast(_pivotPoint.position, _pivotPoint.position + (-_view.transform.forward) * _initialDistanceToPlayer, out avoidHit, _view.ObstaclesLayer))
        {
            if(avoidHit.collider.gameObject.GetComponent<NotClipingCamera>() == null)
            {
            _view.transform.position = avoidHit.point + avoidHit.normal * _view.OffsetFromColliders;
            _isSmoothing = true;
            LookAtPlayer();
            }
        }
    }

    private float CalculateSmoothDistance()
    {
        if (!_isSmoothing) return _initialDistanceToPlayer;
        var currentDistance = Vector3.Distance(_pivotPoint.position, _view.transform.position);
        var finalDistance = Mathf.Lerp(currentDistance, _initialDistanceToPlayer, Time.deltaTime * SMOOTH_VELOCITY);
        finalDistance = Mathf.Clamp(finalDistance, 0, _initialDistanceToPlayer);
        if (finalDistance >= _initialDistanceToPlayer - 0.01f) _isSmoothing = false;
        return finalDistance;
    }

    private void LookAtPlayer()
    {
        _view.transform.forward = (_pivotPoint.position - _view.transform.position).normalized;
    }
}