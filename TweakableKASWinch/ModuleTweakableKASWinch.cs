using System;
using KAS;
using UnityEngine;
using KSP;

namespace PhaserArray.TweakableKASWinch
{
    public class ModuleTweakableKASWinch : PartModule
    {
	    protected KASModuleWinch WinchModule;

		[KSPField(isPersistant = true, guiName = "Winch Length", guiUnits = "m", guiFormat = "F2", guiActiveEditor = true)]
	    [UI_FloatRange(scene = UI_Scene.Editor, minValue = 10f, maxValue = 2000f, stepIncrement = 10f)]
		public float MaxLength;

	    private float _lastMaxLength;

		public override void OnStart(StartState state)
	    {
		    base.OnStart(state);

			// Find the winch module.
			foreach (var m in part.Modules)
		    {
			    if (m is KASModuleWinch winch)
			    {
				    WinchModule = winch;
			    }
		    }
		    if (WinchModule == null) return;

			// If the length slider has not been used (is at the default value of 0, e.g. right after placing the winch), set the starting value to be the default winch length.
			// If the value is not the default starting value, set the winch length to what the value is.
			if (Math.Abs(MaxLength - default(float)) < 0.01f)
			{
				MaxLength = WinchModule.maxLenght;
			}
		    else
		    {
			    WinchModule.maxLenght = MaxLength;
			}
		    _lastMaxLength = MaxLength;
		}

	    public void LateUpdate()
		{
			// Slider value can only change in the editor, and we also need the winch module to be present.
			if (!HighLogic.LoadedSceneIsEditor) return;
			if (WinchModule == null) return;
			// Update winch's max length if _lastMaxLength does not equal the current MaxLength on the slider.
			if (Math.Abs(_lastMaxLength - MaxLength) < 0.01f) return;
			WinchModule.maxLenght = MaxLength;
			_lastMaxLength = MaxLength;
		}
    }
}
