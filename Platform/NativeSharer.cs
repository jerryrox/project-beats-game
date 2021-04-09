using System.IO;
using System.Collections;
using PBGame.Rulesets.Maps;
using PBGame.Data.Records;
using PBFramework.Utils;
using PBFramework.Threading;
using UnityEngine;

namespace PBGame.Platform
{
    public class NativeSharer {

        private string tempPath;


        public NativeSharer(string tempPath)
        {
            this.tempPath = tempPath;
        }

        /// <summary>
        /// Shares the specified result, assuming the user is in the result screen.
        /// </summary>
        public void ShareResult(IMap map, IRecord record)
        {
            string subject = $"{map.Metadata.Artist} - {map.Metadata.Title}\n[Rank: {record.Rank}] [Acc: {record.Accuracy.ToString("P2")}] [Score: {record.Score.ToString("N0")}]";
            UnityThread.StartCoroutine(ShareScreenshotInternal(subject));
        }

        /// <summary>
        /// Internally processes the screenshot sharing routine.
        /// </summary>
        private IEnumerator ShareScreenshotInternal(string subject)
        {
            yield return new WaitForEndOfFrame();

            Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            screenshot.Apply();

            long timestamp = DateUtils.GetTimestamp(System.DateTime.Now);
            string path = Path.Combine(tempPath, $"share_{timestamp}.png");
            File.WriteAllBytes(path, screenshot.EncodeToPNG());

            Object.Destroy(screenshot);

            NativeShare sharer = new NativeShare();
            sharer.AddFile(path);
            sharer.SetText(subject);
            sharer.Share();
        }
    }
}