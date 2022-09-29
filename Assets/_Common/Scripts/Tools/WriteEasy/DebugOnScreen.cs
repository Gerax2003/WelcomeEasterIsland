///-----------------------------------------------------------------
/// Author : Adrien Lemaire
/// Date : 11/08/2022 22:10
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Isartdigital.EasterIsland.Common.Tools.WriteEasy {

    public class DebugOnScreen : MonoBehaviour
	{
		private const string CANVAS_NAME = "DebugOnScreen";
		private const string CONTAINER_NAME = "Container";

		private const string GO_TO_LINE = "\n";

		private const float MATCH = 0.5f;

		private static Vector2 screenResolution = new Vector2(1920, 1080);

		private static Vector2 minAnchor = new Vector2(0.02f, 0.5f);
		private static Vector2 maxAnchor = new Vector2(0.4f, 1f);

		private static List<MethodParam> methodList =		new List<MethodParam>();
		private static List<PropertyParam> propertyList =	new List<PropertyParam>();
		private static List<FieldParam> fieldList =			new List<FieldParam>();

		private static GameObject container = default;
		private static TextMeshProUGUI text = default;

		private static Action DoAction = DoActionVoid;

		public static DebugOnScreen CreateWindow(bool playOnAwake = true, Color colorText = default, bool sizeAuto = true)
		{
			GameObject objectCanvas = new GameObject();

			GameObject objectDebug = Instantiate(objectCanvas, objectCanvas.transform);

			Canvas canvas = (Canvas)objectCanvas.AddComponent(typeof(Canvas));
			CanvasScaler canvasScaler = (CanvasScaler)objectCanvas.AddComponent(typeof(CanvasScaler));

			DebugOnScreen debugScript = (DebugOnScreen)objectDebug.AddComponent(typeof(DebugOnScreen));
			RectTransform debugRect = (RectTransform)objectDebug.AddComponent(typeof(RectTransform));
			TextMeshProUGUI debugText = (TextMeshProUGUI)objectDebug.AddComponent(typeof(TextMeshProUGUI));

			objectCanvas.name = CANVAS_NAME;
			objectDebug.name = CONTAINER_NAME;
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			canvasScaler.referenceResolution = screenResolution;
			canvasScaler.matchWidthOrHeight = MATCH;

			debugRect.anchorMin = minAnchor;
			debugRect.anchorMax = maxAnchor;
			debugRect.offsetMin = Vector2.zero;
			debugRect.offsetMax = Vector2.zero;

			debugText.color = new Color(colorText.r, colorText.g, colorText.b, 1);

			container = objectCanvas;
			text = debugText;

			if (playOnAwake) SetModeWrite();
			else SetModeVoid();

			return debugScript;
		}
		
		#region Unity Methods
		private void Update()
        {
			DoAction();
		}
		#endregion

		#region State Machine
		public static void SetModeVoid()
		{
			DoAction = DoActionVoid;
		}

		public static void SetModeWrite()
        {
			DoAction = DoActionWrite;
        }

		private static void DoActionVoid() { }

		private static void DoActionWrite()
        {
			WriteOnce();
		}
		#endregion

		public static void WriteOnce()
		{
			MethodParam methodParam;
			PropertyParam propertyParam;
			FieldParam fieldParam;
			string debug = "";

			for (int i = 0; i < methodList.Count; i++)
			{
				methodParam = methodList[i];

				debug += GO_TO_LINE + methodParam.Description + " : " + methodParam.MethodInfo?.Invoke(methodParam.Instance, methodParam.Params);
			}

			for (int j = 0; j < propertyList.Count; j++)
			{
				propertyParam = propertyList[j];

				debug += GO_TO_LINE + propertyParam.Description + " : " + propertyParam.PropertyInfo?.GetValue(propertyParam.Instance);
			}

			for (int k = 0; k < fieldList.Count; k++)
			{
				fieldParam = fieldList[k];

				debug += GO_TO_LINE + fieldParam.Description + " : " + fieldParam.FieldInfo?.GetValue(fieldParam.Instance);
			}

			text.text = debug;
		}

		public static void Stop()
		{
			SetModeVoid();

			text.text = string.Empty;
			Destroy(container);
		}

		#region Lists
		public static void AddMethod(Type classType, string methodName, string description, UnityEngine.Object instance = null, Type[] paramType = null, object[] _params = null)
		{
			MethodInfo methodInfo;

			if (paramType != null) methodInfo = classType.GetMethod(methodName, paramType);
			else					methodInfo = classType.GetMethod(methodName);

			MethodParam methodParam = new MethodParam()
			{
				MethodInfo = methodInfo,
				Params = _params,
				Description = description,
				Instance = instance
			};

			methodList.Add(methodParam);
		}

		public static void AddProperty(Type classType, string propertyName, string description, UnityEngine.Object instance = null)
		{
			PropertyInfo propertyInfo = classType.GetProperty(propertyName);

			PropertyParam propertyParam = new PropertyParam()
			{
				PropertyInfo = propertyInfo,
				Description = description,
				Instance = instance
			};

			propertyList.Add(propertyParam);
		}

		public static void AddField(Type classType, string fieldName, string description, UnityEngine.Object instance = null)
		{
			FieldInfo fieldInfo = classType.GetField(fieldName);

			FieldParam fieldParam = new FieldParam()
			{
				FieldInfo = fieldInfo,
				Description = description,
				Instance = instance
			};

			fieldList.Add(fieldParam);
		}

		public static void RemoveAllMethods()
        {
            for (int i = methodList.Count - 1; i >= 0; i--)
            {
				methodList.RemoveAt(i);
            }
		}

		public static void RemoveAllProperties()
		{
			for (int i = propertyList.Count - 1; i >= 0; i--)
			{
				propertyList.RemoveAt(i);
			}
		}

		public static void RemoveAllFields()
		{
			for (int i = fieldList.Count - 1; i >= 0; i--)
			{
				fieldList.RemoveAt(i);
			}
		}
		#endregion
	}

	public struct MethodParam
    {
		public MethodInfo MethodInfo { get; set; }
		public object[] Params { get; set; }
		public string Description { get; set; }
		public UnityEngine.Object Instance { get; set; }
	}

	public struct PropertyParam
	{
		public PropertyInfo PropertyInfo { get; set; }
		public string Description { get; set; }
		public UnityEngine.Object Instance { get; set; }
	}

	public struct FieldParam
	{
		public FieldInfo FieldInfo { get; set; }
		public string Description { get; set; }
		public UnityEngine.Object Instance { get; set; }
	}
}

