using System;
using System.Collections;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;
using System.ComponentModel;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
/*
/// <summary>
/// Abstract class for making reload-proof singletons out of ScriptableObjects
/// Returns the asset created on the editor, or null if there is none
/// Based on https://www.youtube.com/watch?v=VBA1QCoEAX4
/// </summary>
/// <typeparam name="T">Singleton type</typeparam>

//PlayerSettings�� PreLoadedAssets�� ��ϵǾ� ���� ������ �۵� ����.
public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    static T _instance = null;
    public static T Instance
    {
        get
        {
            if (!_instance)
            {

                //�����ͻ󿡼� preloaded Assets�� Player Settings�� Ŭ���ϱ� ������ �۵� ���Ѵ�.. �䷸�� ���� �־��ָ� �ذ�
#if UNITY_EDITOR
                PlayerSettings.GetPreloadedAssets();
#endif
                _instance = (T)Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
            }


            return _instance;
        }
    }
}
*/
namespace MorningBird
{

    public static class CoreMB
    {
        public static float GetVolumeOfColl(BoxCollider coll)
        {
            return coll.size.x * coll.size.y * coll.size.z;
        }
        public static float GetVolumeOfColl(SphereCollider coll)
        {
            return coll.radius * coll.radius * coll.radius * Mathf.PI * 1.333f; // ���� ���� ���ϴ� ����, 4/3 * PI * r^3
        }
        public static float GetVolumeOfColl(CapsuleCollider coll)
        {
            return coll.height * (coll.radius * coll.radius * Mathf.PI) + (coll.radius * coll.radius * coll.radius * Mathf.PI * 1.333f);
        }
        /// <summary>
        /// !!!Get Bound of Meshcollider!!!
        /// </summary>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static float GetVolumeOfColl(MeshCollider coll)
        {
            return coll.bounds.size.x * coll.bounds.size.y * coll.bounds.size.z;
        }
        /// <summary>
        /// Get Volume of mesh. Mesh must be closed! And submesh will not calculate correctly. And volume of tree such as an chunk with lot of leafs, will not be correct.
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        /// If you wanna more information, http://chenlab.ece.cornell.edu/Publication/Cha/icip01_Cha.pdf by Cha Zhang 
        public static float GetVolumeOfMesh(Mesh mesh)
        {

            return VolumeOfMesh(mesh);

            float VolumeOfMesh(Mesh mesh)
            {
                float volume = 0;

                float3[] vertices = new float3[mesh.vertices.Length];
                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] = mesh.vertices[i];
                }

                int[] triangles = mesh.triangles;

                for (int i = 0; i < triangles.Length; i += 3)
                {
                    float3 p1 = vertices[triangles[i + 0]];
                    float3 p2 = vertices[triangles[i + 1]];
                    float3 p3 = vertices[triangles[i + 2]];
                    volume += SignedVolumeOfTriangle(p1, p2, p3);
                }
                return Math.Abs(volume);
            }

            float SignedVolumeOfTriangle(float3 p1, float3 p2, float3 p3)
            {
                float v321 = p3.x * p2.y * p1.z;
                float v231 = p2.x * p3.y * p1.z;
                float v312 = p3.x * p1.y * p2.z;
                float v132 = p1.x * p3.y * p2.z;
                float v213 = p2.x * p1.y * p3.z;
                float v123 = p1.x * p2.y * p3.z;

                return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
            }
        }

        public static float2 GetAppScrenResolution => new float2(Screen.width, Screen.height);

        public static float2 GetScreenRatio()
        {
            float2 screenResolution = GetAppScrenResolution;
            return new float2(1, screenResolution.y / screenResolution.x);
        }
    }

    public static class MathMB
    {
        public static float3 ConstForward = new float3(0f, 0f, 1f);
        public static float3 ConstLeft = new float3(-1f, 0f, 0f);


        #region Quaternion Translation

        public static Quaternion Float3ToQuaternion(float3 float3)
        {

            return Float3ToQuaternion(float3.y, float3.x, float3.z);
        }

        public static Quaternion Float3ToQuaternion(float yaw, float pitch, float roll)
        {
            yaw *= Mathf.Deg2Rad;
            pitch *= Mathf.Deg2Rad;
            roll *= Mathf.Deg2Rad;
            float rollOver2 = roll * 0.5f;
            float sinRollOver2 = (float)Math.Sin((double)rollOver2);
            float cosRollOver2 = (float)Math.Cos((double)rollOver2);
            float pitchOver2 = pitch * 0.5f;
            float sinPitchOver2 = (float)Math.Sin((double)pitchOver2);
            float cosPitchOver2 = (float)Math.Cos((double)pitchOver2);
            float yawOver2 = yaw * 0.5f;
            float sinYawOver2 = (float)Math.Sin((double)yawOver2);
            float cosYawOver2 = (float)Math.Cos((double)yawOver2);
            Quaternion result;
            result.w = cosYawOver2 * cosPitchOver2 * cosRollOver2 + sinYawOver2 * sinPitchOver2 * sinRollOver2;
            result.x = cosYawOver2 * sinPitchOver2 * cosRollOver2 + sinYawOver2 * cosPitchOver2 * sinRollOver2;
            result.y = sinYawOver2 * cosPitchOver2 * cosRollOver2 - cosYawOver2 * sinPitchOver2 * sinRollOver2;
            result.z = cosYawOver2 * cosPitchOver2 * sinRollOver2 - sinYawOver2 * sinPitchOver2 * cosRollOver2;

            return result;
        }

        public static float3 QuaternionToFloat3(Quaternion quaternion)
        {
            float sqw = quaternion.w * quaternion.w;
            float sqx = quaternion.x * quaternion.x;
            float sqy = quaternion.y * quaternion.y;
            float sqz = quaternion.z * quaternion.z;
            float unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
            float test = quaternion.x * quaternion.w - quaternion.y * quaternion.z;
            float3 v;

            if (test > 0.4995f * unit)
            { // singularity at north pole
                v.y = 2f * Mathf.Atan2(quaternion.y, quaternion.x);
                v.x = Mathf.PI / 2;
                v.z = 0;
                return NormalizeAngles(v * Mathf.Rad2Deg);
            }

            if (test < -0.4995f * unit)
            { // singularity at south pole
                v.y = -2f * Mathf.Atan2(quaternion.y, quaternion.x);
                v.x = -Mathf.PI / 2;
                v.z = 0;
                return NormalizeAngles(v * Mathf.Rad2Deg);
            }

            Quaternion q = new Quaternion(quaternion.w, quaternion.z, quaternion.x, quaternion.y);
            v.y = (float)Math.Atan2(2f * q.x * q.w + 2f * q.y * q.z, 1 - 2f * (q.z * q.z + q.w * q.w));     // Yaw
            v.x = (float)Math.Asin(2f * (q.x * q.z - q.w * q.y));                             // Pitch
            v.z = (float)Math.Atan2(2f * q.x * q.y + 2f * q.z * q.w, 1 - 2f * (q.y * q.y + q.z * q.z));      // Roll
            return NormalizeAngles(v * Mathf.Rad2Deg);

        }

        static float3 NormalizeAngles(float3 angles)
        {
            angles.x = NormalizeAngle(angles.x);
            angles.y = NormalizeAngle(angles.y);
            angles.z = NormalizeAngle(angles.z);
            return angles;
        }

        static float NormalizeAngle(float angle)
        {
            while (angle > 360)
                angle -= 360;
            while (angle < 0)
                angle += 360;
            return angle;
        }

        #endregion

        #region Separate Mul Of

        #region Return float, float x float

        public static float2 SeparateMulOf(float2 child, float2 mother)
        {
            return new float2(child.x * mother.x, child.y * mother.y);
        }

        public static float2 SeparateMulOf(float3 child, float2 mother)
        {
            return new float2(child.x * mother.x, child.y * mother.y);
        }

        public static float3 SeparateMulOf(float2 child, float3 mother)
        {
            return new float3(child.x * mother.x, child.y * mother.y, mother.z);
        }

        public static float3 SeparateMulOf(float3 child, float3 mother)
        {
            return new float3(child.x * mother.x, child.y * mother.y, child.z * mother.z);
        }

        #endregion

        #region Return float, vector x float

        public static float2 SeparateMulOf(Vector2 child, float2 mother)
        {
            return new float2(child.x * mother.x, child.y * mother.y);
        }

        public static float2 SeparateMulOf(Vector3 child, float2 mother)
        {
            return new float2(child.x * mother.x, child.y * mother.y);
        }

        public static float3 SeparateMulOf(Vector2 child, float3 mother)
        {
            return new float3(child.x * mother.x, child.y * mother.y, mother.z);
        }

        public static float3 SeparateMulOf(Vector3 child, float3 mother)
        {
            return new float3(child.x * mother.x, child.y * mother.y, child.z * mother.z);
        }

        #endregion

        #region Return Vector, float x vector

        public static Vector2 SeparateMulOf(float2 child, Vector2 mother)
        {
            return new Vector2(child.x * mother.x, child.y * mother.y);
        }

        public static Vector2 SeparateMulOf(float3 child, Vector2 mother)
        {
            return new Vector2(child.x * mother.x, child.y * mother.y);
        }

        public static Vector3 SeparateMulOf(float2 child, Vector3 mother)
        {
            return new Vector3(child.x * mother.x, child.y * mother.y, mother.z);
        }

        public static Vector3 SeparateMulOf(float3 child, Vector3 mother)
        {
            return new Vector3(child.x * mother.x, child.y * mother.y, child.z * mother.z);
        }

        #endregion

        #region Return Vector, vector x vector

        public static Vector2 SeparateMulOf(Vector2 child, Vector2 mother)
        {
            return new Vector2(child.x * mother.x, child.y * mother.y);
        }

        public static Vector2 SeparateMulOf(Vector3 child, Vector2 mother)
        {
            return new Vector2(child.x * mother.x, child.y * mother.y);
        }

        public static Vector3 SeparateMulOf(Vector2 child, Vector3 mother)
        {
            return new Vector3(child.x * mother.x, child.y * mother.y, mother.z);
        }

        public static Vector3 SeparateMulOf(Vector3 child, Vector3 mother)
        {
            return new Vector3(child.x * mother.x, child.y * mother.y, child.z * mother.z);
        }

        #endregion

        #endregion

        /// <summary>
        /// Devide each value. Must same size need for deviding.
        /// child deivde by mother (child / mother)
        /// </summary>
        /// <returns></returns>
        public static float2 SeparateDevideOfFloat2(float2 child, float2 mother)
        {
            return new float2(child.x / mother.x, child.y / mother.y);
        }

        public static Vector2 SeparateDevideOfFloat2(Vector2 child, Vector2 mother)
        {
            return new Vector2(child.x / mother.x, child.y / mother.y);
        }

        /// <summary>
        /// Round value to number position
        /// </summary>
        /// <param name="value"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static float MathRoundTo(float value, int number)
        {
            for (int intA = 0; intA < number; intA++)
            {
                value *= 10;
            }

            value = (float)Math.Round(value);

            for (int intA = 0; intA < number; intA++)
            {
                value *= 0.1f;
            }
            return value;
        }

        /// <summary>
        /// �ּҰ��� �ִ밪�� Ȯ���ؼ�, Value�� ������ ������ ��Ű���� �մϴ�. value�� minRange�� MaxRange ���̿����� ������� ���� ��, �̸� ��ȯ�մϴ�.
        /// �� �Լ��� Value�� ������ ���ϰ�, �� ������ 0.5�� �������� 0~1.0 ���̿��� �����մϴ�. ������ ������ ���� "0.2��� 0.8��", "0.7�̶��, 0.3��" ��ȯ�մϴ�.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minRange"></param>
        /// <param name="maxRange"></param>
        /// <returns></returns>
        public static float GetRatioReflectWithValue(float value, float minMotherValue, float maxMotherValue)
        {
            // Set Retrun Checker before start to calculate for defending error occure
            if (value < minMotherValue || value > maxMotherValue || minMotherValue > maxMotherValue)
                return float.NaN;

            // Value�� ������� ���մϴ�.
            float tempFloatRatioOfValue = GetRatioValue(value - minMotherValue, maxMotherValue - minMotherValue);

            // �̸� �����մϴ�.
            if (tempFloatRatioOfValue > 0.5f)
                return 1f - tempFloatRatioOfValue;
            else if (tempFloatRatioOfValue < 0.5f)
                return Math.Abs(tempFloatRatioOfValue - 1f);
            else
                return tempFloatRatioOfValue;
        }

        /// <summary>
        /// ���� ���� �����մϴ�. ���� ���� 1.0 �ʰ� Ȥ�� 0.0 �̸��̶�� float.NaN�� ��ȯ�մϴ�.
        /// �� ���� 0.5�� �������� 0~1.0 ���̿��� �����մϴ�. ������ ���� "0.2��� 0.8��", "0.7�̶��, 0.3��" ��ȯ�մϴ�.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float GetRatioReflect(float value)
        {
            if (value < 0f || value > 1f)
                return float.NaN;

            if (value > 0.5f)
                return 1f - value;
            else if (value < 0.5f)
                return Math.Abs(value - 1f);
            else
                return value;
        }

        /// <summary>
        /// �ּҰ��� �ִ밪 �ȿ��� targetValue�� ������ ���մϴ�. �� �Լ��� 3�� 7 ���̿� 6�� ���ۼ�Ʈ�ΰ�? �� ���Ҷ� �����մϴ�.
        /// </summary>
        /// <param name="targetValue"></param>
        /// <param name="minimumValue"></param>
        /// <param name="maximumValue"></param>
        /// <returns></returns>
        public static float GetRatioValueWithMinimum(float targetValue, float minimumValue, float maximumValue)
        {
            return GetRatioValue(targetValue - minimumValue, maximumValue - minimumValue);
        }

        /// <summary>
        /// Get Ratio(����) of convertValue. motherValue will divide convertValue. It will return ratio of where convertValue depending on motherValue.
        /// </summary>
        /// <param name="convertValue">It will be divided by motherValue. Which meaning motherValue show where of value.</param>
        /// <param name="motherValue">It will divide convertValue. Whice meaning motherValue show position of ratio.</param>
        /// <returns></returns>
        public static float GetRatioValue(float convertValue, float motherValue)
        {
            return convertValue / motherValue;
        }

        #region GetVectorTotal

        /// <summary>
        /// Get vector elements total. All of elemets use absolute value only.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float GetAbsVectorTotal(Vector2 value)
        {
            return Math.Abs(value.x) + Math.Abs(value.y);
        }

        /// <summary>
        /// Get vector elements total. All of elemets use absolute value only.
        /// </summary>
        /// <param name="value"> Converting to 1 </param>
        /// <returns></returns>
        public static float GetAbsVectorTotal(Vector3 value)
        {
            return Math.Abs(value.x) + Math.Abs(value.y) + Math.Abs(value.z);
        }

        /// <summary>
        /// Get vector elements total. All of elemets use absolute value only.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float GetAbsVectorTotal(float2 value)
        {
            return Math.Abs(value.x) + Math.Abs(value.y);
        }

        /// <summary>
        /// Get vector elements total. All of elemets use absolute value only.
        /// </summary>
        /// <param name="value"> Converting to 1 </param>
        /// <returns></returns>
        public static float GetAbsVectorTotal(float3 value)
        {
            return Math.Abs(value.x) + Math.Abs(value.y) + Math.Abs(value.z);
        }

        /// <summary>
        /// Get vector elements total. If you want absolute value, use instead of GetVectorTotalWithAbsolute. This function is just add all vector elements to an float value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float GetVectorTotal(Vector2 value)
        {
            return value.x + value.y;
        }

        /// <summary>
        /// Get vector elements total. If you want absolute value, use instead of GetVectorTotalWithAbsolute. This function is just add all vector elements to an float value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float GetVectorTotal(Vector3 value)
        {
            return value.x + value.y + value.z;
        }

        #endregion


        /// <summary>
        /// Return Cube Root of value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float Cbrt(float value)
        {
            return (float)(Math.Pow(value, (double)1 / 3));
        }
        /// <summary>
        /// Return Cube Root of value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double Cbrt(double value)
        {
            return Math.Pow(value, (double)1 / 3);
        }

        /// <summary>
        /// sign�� ��ȣ�� target�� �����մϴ�. ���� ��� sign�� �������, target�� ������ �˴ϴ�. sign�� ������ target�� ����� �˴ϴ�. 
        /// sign�� 0�� ��쿡�� ����� ��ȯ�մϴ�. ��ȣ�ִ� 0�� ������� �ʽ��ϴ�.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public static float CopySign(float target, float sign)
        {
            if(target >= 0f)
            {
                if(sign >= 0f)
                { 
                    return target; 
                }
                else if(sign < 0f)
                {
                    return target * -1f;
                }
            }
            else
            {
                if (sign >= 0f)
                {
                    return target * -1f;
                }
                else if (sign < 0f)
                {
                    return target;
                }
            }
            return target;
        }

        public static float3 GetEulerRotation(quaternion quaternion)
        {
            // 1. ���Ϸ������� ȣ�������� �ٲٰ�, 57.29578f�� ���Ѵ�.
            // 2. �װ��� MakePositive�� �Ѵ�. �̸� �����Ѵ�.
            // �� �̷��� �ϴ����� �𸣰�����, ����Ƽ���� ����ϴ� �����̴�.
            // ����Ƽ�� ���ʹϿ��� ���Ϸ� ���۷�, �׸��� ȣ��������, 57.29578f�� ���ϰ�, Positive�� �ٲ۴�.
            // �ͱ�? ������ �ϴ� �̷��� �Ѵ�.
            return Internal_MakePositive(ToEulerAngles(quaternion)/* * 57.29578f*/);
        }

        /// <summary>
        /// ����Ƽ���� ���������� Ȱ���ϴ� ���´� GetEulerRotaion�̴�. ���� ��Ȯ�� ������ ���ٸ� GetEulerRotation�� �������.
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public static float3 ToEulerAngles(Quaternion q)
        {
            float3 angles;

            // roll (x-axis rotation)
            float sinr_cosp = 2 * (q.w * q.x + q.y * q.z);
            float cosr_cosp = 1 - 2 * (q.x * q.x + q.y * q.y);
            angles.x = math.atan2(sinr_cosp, cosr_cosp);

            // pitch (y-axis rotation)
            float sinp = 2 * (q.w * q.y - q.z * q.x);
            if (math.abs(sinp) >= 1)
                angles.y = CopySign(math.PI * 0.5f, sinp); // use 90 degrees if out of range
            else
                angles.y = math.asin(sinp);

            // yaw (z-axis rotation)
            float siny_cosp = 2 * (q.w * q.z + q.x * q.y);
            float cosy_cosp = 1 - 2 * (q.y * q.y + q.z * q.z);
            angles.z = math.atan2(siny_cosp, cosy_cosp);

            return angles;
        }

        public static float3 QuaternionToDirection(quaternion q)
        {
            return math.mul(q, new float3(0f, 0f, 1f));
        }

        public static float3 QuaternionToDirection(quaternion q, float3 standard)
        {
            return math.mul(q, standard);
        }

        private static float3 Internal_MakePositive(float3 euler)
        {
            float num = -0.005729578f;
            float num2 = 360f + num;
            if (euler.x < num)
            {
                euler.x += 360f;
            }
            else if (euler.x > num2)
            {
                euler.x -= 360f;
            }

            if (euler.y < num)
            {
                euler.y += 360f;
            }
            else if (euler.y > num2)
            {
                euler.y -= 360f;
            }

            if (euler.z < num)
            {
                euler.z += 360f;
            }
            else if (euler.z > num2)
            {
                euler.z -= 360f;
            }

            return euler;
        }

        public static float3 EulerToRad(float3 euler)
        {
            return math.radians(euler);
        }
    }
}
