namespace PBGame.Rulesets.Scoring
{
	/// <summary>
	/// Types of score rankings.
	/// </summary>
	public enum RankType {

		F = 0,
		D,
		C,
		B,
		A,
		S,
		SH, // S plus
		X,
		XH // SS Plus
	}

    public static class RankTypeExtension
    {
		/// <summary>
		/// Returns the string value of RankType seend by the end user.
		/// </summary>
        public static string ToDisplayedString(this RankType context)
        {
            if (context == RankType.XH || context == RankType.X)
                return "SS";
			if(context == RankType.SH)
                return "S";
            return context.ToString();
        }
    }
}