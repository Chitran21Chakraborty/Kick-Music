﻿using Sourav.Engine.Core.DebugRelated;
using Sourav.Engine.Core.NotificationRelated;
using Sourav.Engine.Editable.NotificationRelated;
using UnityEngine;

namespace Sourav.Engine.Core.ControllerRelated
{
	public class HapticFeedbackController : Controller
	{
		#region ANDROID
		public class HapticFeedbackManagerAndroid
		{
			#if UNITY_ANDROID && !UNITY_EDITOR
                private int hapticFeedbackConstantsKey;
                private AndroidJavaObject UnityPlayer;
			#endif

			public HapticFeedbackManagerAndroid()
			{
				#if UNITY_ANDROID && !UNITY_EDITOR
					hapticFeedbackConstantsKey =
					new AndroidJavaClass("android.view.HapticFeedbackConstants").GetStatic<int>("VIRTUAL_KEY");
					UnityPlayer =
					new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer");
				#endif
			}

			public bool Execute()
			{
				#if UNITY_ANDROID && !UNITY_EDITOR
					return UnityPlayer.Call<bool>("performHapticFeedback", hapticFeedbackConstantsKey);
				#endif
				return false;
			}
		}
		private static HapticFeedbackManagerAndroid _mHapticFeedbackManager;

		public static bool HapticFeedback()
		{
			if (_mHapticFeedbackManager == null)
			{
				_mHapticFeedbackManager = new HapticFeedbackManagerAndroid();
			}

			return _mHapticFeedbackManager.Execute();
		}
		#endregion

		[SerializeField] private iOSHapticFeedback.iOSFeedbackType feedbackTypeSuccess;
		[SerializeField] private iOSHapticFeedback.iOSFeedbackType feedbackTypeFailure;
		[SerializeField] private iOSHapticFeedback.iOSFeedbackType feedbackTypeMild;
		[SerializeField] private iOSHapticFeedback.iOSFeedbackType feedbackTypeLight;

		[Space(10)] [Header("Android Haptic Feedback")] [SerializeField]
		private AndroidHapticManager hapticFeedback;
		public override void OnNotificationReceived(Notification notification, NotificationParam param = null)
		{
			if (!App.GetLevelData().IsVibrationOn)
			{
				return;
			}
			
			switch (notification)
			{
				case Notification.HapticSuccess:
					HapticForSubmitButtonSuccess();
					break;
				
				case Notification.HapticFailure:
					HapticForSubmitButtonFailure();
					break;
				
				case Notification.HapticMild:
					HapticForMediumImpact();
					break;
				
				case Notification.HapticLight:
					HapticForLightImpact();
					break;
			}
		}

		private void HapticForSubmitButtonFailure()
		{
			D.Log("Haptic FAILURE");
			#if UNITY_IOS && !UNITY_EDITOR
			if (iOSHapticFeedback.Instance.IsSupported())
			{
				iOSHapticFeedback.Instance.Trigger(feedbackTypeFailure);
			}
			#elif UNITY_ANDROID && !UNITY_EDITOR
			HapticFeedback();
			#endif
		}

		private void HapticForSubmitButtonSuccess()
		{
			D.Log("Haptic SUCCESS");
			#if UNITY_IOS && !UNITY_EDITOR
			if (iOSHapticFeedback.Instance.IsSupported())
			{
				iOSHapticFeedback.Instance.Trigger(feedbackTypeSuccess);
			}
			#elif UNITY_ANDROID && !UNITY_EDITOR
			HapticFeedback();
			#endif
		}
		
		private void HapticForMediumImpact()
		{
			D.Log("Haptic Mild");
			#if UNITY_IOS && !UNITY_EDITOR
			if (iOSHapticFeedback.Instance.IsSupported())
			{
				iOSHapticFeedback.Instance.Trigger(feedbackTypeMild);
			}
			#elif UNITY_ANDROID && !UNITY_EDITOR
			HapticFeedback();
			#endif
		}
		
		private void HapticForLightImpact()
		{
			D.Log("Haptic Light");
			#if UNITY_IOS && !UNITY_EDITOR
			if (iOSHapticFeedback.Instance.IsSupported())
			{
				iOSHapticFeedback.Instance.Trigger(feedbackTypeLight);
			}
			#elif UNITY_ANDROID && !UNITY_EDITOR
			HapticFeedback();
			#endif
		}
	}
}
