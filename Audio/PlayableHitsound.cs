// using System;
// using System.Collections.Generic;
// using PBGame.Assets;
// using PBGame.Rulesets.Maps.ControlPoints;
// using PBFramework.Audio;
// using UnityEngine;

// namespace PBGame.Audio
// {
//     /// <summary>
//     /// Hitsound information in its most playable form.
//     /// </summary>
//     public class PlayableHitsound
//     {
//         private MapAsset asset;
//         private List<SoundInfo> samples;
//         private SoundPooler sfxController;

//         private float volume;


//         public PlayableHitsound(MapAsset asset, SampleControlPoint samplePoint, List<SoundInfo> samples,
//             IEffectController sfxController)
//         {
//             this.asset = asset;
//             this.samples = samples;
//             this.sfxController = sfxController;

//             volume = samplePoint.Volume;
//         }

//         /// <summary>
//         /// Plays the hitsounds included in this object.
//         /// </summary>
//         public void Play()
//         {
//             sfxController.Play(asset.GetHitsoundClips(samples), volume);
//         }
//     }
// }

