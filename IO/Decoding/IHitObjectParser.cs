using PBGame.Rulesets.Objects;

namespace PBGame.IO.Decoding
{
    public interface IHitObjectParser {

        /// <summary>
        /// Parses the specified line of text into a hit object.
        /// </summary>
        HitObject Parse(string data);
    }
}