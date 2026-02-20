using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(AreaLocation))]
public class AreaLocationEditor : Editor
{
    private AreaLocation _area;

    private void OnEnable()
    {
        _area = (AreaLocation)target;
    }

    private void OnSceneGUI()
    {
        if (_area == null || _area.transform == null)
            return;

        Transform t = _area.transform;

        for (int i = 0; i < _area.PointCount; i++)
        {
            Vector2 local2D = _area.GetLocalPoint(i);
            Vector3 local3D = VectorUtils.Vec2ToVec3(local2D);
            Vector3 worldPos = t.TransformPoint(local3D);

            Handles.color = Color.yellow;

            EditorGUI.BeginChangeCheck();
            Vector3 newWorldPos = Handles.PositionHandle(worldPos, Quaternion.identity);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_area, "Move Area Point");

                Vector3 newLocal = t.InverseTransformPoint(newWorldPos);
                Vector2 newLocal2D = VectorUtils.Vec3ToVec2(newLocal);

                _area.SetLocalPoint(i, newLocal2D);

                EditorUtility.SetDirty(_area);
            }
        }
    }
}
