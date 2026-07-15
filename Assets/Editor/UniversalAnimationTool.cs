using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.IO;

public class UniversalAnimationTool
{
    [MenuItem("Tools/1. Create Unified Animations")]
    public static void CreateAnimations()
    {
        string dir = "Assets/Animations";
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        // 1. Create Controller
        string controllerPath = "Assets/Animations/UniversalPlanetController.controller";
        AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
        controller.AddParameter("PlayAction", AnimatorControllerParameterType.Trigger);

        // 2. Create Idle Clip (Float and Rotate)
        AnimationClip idleClip = new AnimationClip();
        idleClip.name = "Planet_Idle";
        idleClip.wrapMode = WrapMode.Loop;
        
        // Rotation (Y axis) 0 to 360 over 10 seconds
        AnimationCurve rotCurve = AnimationCurve.Linear(0, 0, 10f, 360f);
        idleClip.SetCurve("", typeof(Transform), "localEulerAnglesRaw.y", rotCurve);
        
        // Float (Y axis) slight up and down
        AnimationCurve posYCurve = new AnimationCurve();
        posYCurve.AddKey(new Keyframe(0f, 0f, 0, 0));
        posYCurve.AddKey(new Keyframe(2.5f, 0.5f, 0, 0));
        posYCurve.AddKey(new Keyframe(5f, 0f, 0, 0));
        posYCurve.AddKey(new Keyframe(7.5f, -0.5f, 0, 0));
        posYCurve.AddKey(new Keyframe(10f, 0f, 0, 0));
        idleClip.SetCurve("", typeof(Transform), "localPosition.y", posYCurve);

        AnimationClipSettings idleSettings = AnimationUtility.GetAnimationClipSettings(idleClip);
        idleSettings.loopTime = true;
        AnimationUtility.SetAnimationClipSettings(idleClip, idleSettings);
        AssetDatabase.CreateAsset(idleClip, "Assets/Animations/Planet_Idle.anim");

        // 3. Create Action Clip (Pulse and Fast Spin)
        AnimationClip actionClip = new AnimationClip();
        actionClip.name = "Planet_Action";
        
        // Fast rotation
        AnimationCurve fastRotCurve = AnimationCurve.EaseInOut(0, 0, 2f, 720f);
        actionClip.SetCurve("", typeof(Transform), "localEulerAnglesRaw.y", fastRotCurve);
        
        // Pulse scale
        AnimationCurve scaleCurve = new AnimationCurve();
        scaleCurve.AddKey(new Keyframe(0f, 1f));
        scaleCurve.AddKey(new Keyframe(1f, 1.3f));
        scaleCurve.AddKey(new Keyframe(2f, 1f));
        actionClip.SetCurve("", typeof(Transform), "localScale.x", scaleCurve);
        actionClip.SetCurve("", typeof(Transform), "localScale.y", scaleCurve);
        actionClip.SetCurve("", typeof(Transform), "localScale.z", scaleCurve);
        
        AssetDatabase.CreateAsset(actionClip, "Assets/Animations/Planet_Action.anim");

        // 4. Set up States
        AnimatorStateMachine rootStateMachine = controller.layers[0].stateMachine;
        AnimatorState idleState = rootStateMachine.AddState("Idle");
        idleState.motion = idleClip;
        rootStateMachine.defaultState = idleState;

        AnimatorState actionState = rootStateMachine.AddState("Action");
        actionState.motion = actionClip;

        // 5. Transitions
        AnimatorStateTransition toAction = idleState.AddTransition(actionState);
        toAction.AddCondition(AnimatorConditionMode.If, 0, "PlayAction");
        toAction.hasExitTime = false;
        toAction.duration = 0.25f;

        AnimatorStateTransition backToIdle = actionState.AddTransition(idleState);
        backToIdle.hasExitTime = true;
        backToIdle.exitTime = 1f; // Wait for full action clip to finish
        backToIdle.duration = 0.25f;

        // 6. Apply to all planets
        ApplyControllerToPlanets(controller);

        AssetDatabase.SaveAssets();
        Debug.Log("Animations created and applied to all planets.");
    }

    private static void ApplyControllerToPlanets(AnimatorController controller)
    {
        PlanetInteractable[] planets = Resources.FindObjectsOfTypeAll<PlanetInteractable>();
        foreach (var p in planets)
        {
            if (p.gameObject.scene.isLoaded && p.animator != null)
            {
                p.animator.runtimeAnimatorController = controller;
                Debug.Log($"Applied Universal Controller to {p.gameObject.name}");
            }
        }
    }
}
