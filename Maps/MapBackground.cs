using UnityEngine;

namespace PBGame.Maps
{
	public class MapBackground : IMapBackground {

		/// <summary>
		/// Map background with empth image preset.
		/// </summary>
		public static readonly MapBackground Empty = new MapBackground(null);


		public Texture2D Image { get; private set; }

		public Vector2 Size { get { return Image == null ? Vector2.zero : new Vector2(Image.width, Image.height); } }

		public Color GradientTop { get; private set; }

		public Color GradientBottom { get; private set; }

		public Color Highlight { get; private set; }


        // TODO: Receive a color preset interface
		public MapBackground(Texture2D image)
		{
			Image = image;
			if(image == null)
			{
                // TODO: Apply color using color preset interface.
				GradientTop = new Color(0.25f, 0.25f, 0.25f);
				GradientBottom = new Color(0f, 0f, 0f);
				Highlight = new Color(0.125f, 0.125f, 0.125f);
			}
			else
			{
				GradientTop = SampleColors(5, 2, image.height / 5, image.height / 5);
				GradientBottom = SampleColors(5, 2, image.height / 5, image.height / -5);
				Highlight = SampleColors(5, 1, image.height / 2);

			}
		}

		public void Dispose()
		{
			if(Image != null)
				Object.Destroy(Image);
            Image = null;
        }

		/// <summary>
		/// Extracts average pixel color from current image using specified options.
		/// </summary>
		private Color SampleColors(int colCount, int rowCount, int startY, int stepY = 0)
		{
			colCount ++;

			Texture2D image = Image;
			Color color = new Color(0f, 0f, 0f, 1f);

			for(int r=0; r<rowCount; r++)
			{
				int y = startY + stepY * r;

				for(int c=1; c<colCount; c++)
					color += image.GetPixel(image.width * c / colCount, y);
			}

			return color / (rowCount * (colCount - 1));
		}
	}
}

