using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;

namespace BreakoutGame_IVART_Vincent {
    abstract class BasePourObjets {
        #region Attributs
        protected Vector2[] listePoints;
        protected Vector2[] coordonneesTextures;
        protected int textureID;
        protected string nomTexture;
        #endregion //Attributs

        #region ConstructeursInitialisation
        public BasePourObjets(string nomTexture, Vector2 a, Vector2 b, Vector2 c, Vector2 d) {
            listePoints = new Vector2[4];
            listePoints[0] = a;
            listePoints[1] = b;
            listePoints[2] = c;
            listePoints[3] = d;
            init(nomTexture);
        }
        private void init(string nomTexture) {
            this.nomTexture = nomTexture;
            setCoordonneesTextureCarre();
            chargerTexture();
        }
        #endregion // ConstructeursInitialisation

        #region GestionTexture
        protected void chargerTexture() {
            GL.GenTextures(1, out textureID);
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            BitmapData textureData = chargerImage(nomTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, textureData.Width, textureData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, textureData.Scan0);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }
        private BitmapData chargerImage(string nomImage) {
            Bitmap bmpImage = new Bitmap(nomImage);
            Rectangle rectangle = new Rectangle(0, 0, bmpImage.Width, bmpImage.Height);
            BitmapData bmpData = bmpImage.LockBits(rectangle, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            bmpImage.UnlockBits(bmpData);
            return bmpData;
        }
        private void setCoordonneesTextureCarre() {
            coordonneesTextures = new Vector2[]
            {
                new Vector2(0.0f, 1.0f),
                new Vector2(1.0f, 1.0f),
                new Vector2(1.0f, 0.0f),
                new Vector2(0.0f, 0.0f)
            };
        }
        #endregion // GestionTexture

        #region GestionAffichage
        abstract public void update();
        public void dessiner(PrimitiveType typeDessin) {
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.Begin(typeDessin);
            for (int i = 0; i < listePoints.Length; i++) {
                GL.TexCoord2(coordonneesTextures[i]);
                GL.Vertex2(listePoints[i].X, listePoints[i].Y);
            }
            GL.End();
        }
        #endregion
    }
}
