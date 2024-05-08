using OpenTK;
using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace BreakoutGame_IVART_Vincent {
    internal class Texte {
        #region  Attributs
        Vector2 pointA;
        Vector2 pointB;
        Vector2 pointC;
        Vector2 pointD;
        string texte;
        int largeurZoneTexte;
        int hauteurZoneTexte;
        Color couleurDeFond;
        SolidBrush pinceau;
        PointF position;
        Font policeSansSerif;
        Font policeSansSerifGras;
        Font policeAffichage;
        int textureID;
        #endregion // Atributs

        #region ConstructeursInitialisation
        public Texte(Vector2 coinInfGauche, int largeur, int hauteur) {
            pointA = coinInfGauche;
            pointB = new Vector2(coinInfGauche.X + largeur, coinInfGauche.Y);
            pointC = new Vector2(coinInfGauche.X + largeur, coinInfGauche.Y + hauteur);
            pointD = new Vector2(coinInfGauche.X, coinInfGauche.Y + hauteur);
            texte = "";
            couleurDeFond = Color.LightGray;
            pinceau = new SolidBrush(Color.Blue);
            position = new PointF(60.0f, 1.0f);
            policeSansSerif = new Font(FontFamily.GenericSansSerif, 10);
            policeSansSerifGras = new Font(this.policeSansSerif, FontStyle.Bold);
            policeAffichage = policeSansSerif;
            largeurZoneTexte = largeur;
            hauteurZoneTexte = hauteur;
            textureID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, largeur, hauteur, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
        }
        #endregion

        #region Methodes
        public void dessiner() {
            GL.PushMatrix();
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex2(pointA.X, pointA.Y);
            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex2(pointB.X, pointB.Y);
            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex2(pointC.X, pointC.Y);
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex2(pointD.X, pointD.Y);
            GL.End();
            GL.PopMatrix();
        }
        public void setTexte(string txt) {
            texte = txt;
            chargerTexte();
        }
        private void chargerTexte() {
            Bitmap bmpTxt = new Bitmap(largeurZoneTexte, hauteurZoneTexte, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics graphique = Graphics.FromImage(bmpTxt);
            graphique.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            graphique.Clear(couleurDeFond);
            graphique.DrawString(texte, policeAffichage, pinceau, position);
            Rectangle zoneTexte = new Rectangle(0, 0, largeurZoneTexte, hauteurZoneTexte);
            System.Drawing.Imaging.BitmapData dataTxt = bmpTxt.LockBits(zoneTexte, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, largeurZoneTexte, hauteurZoneTexte, PixelFormat.Bgra, PixelType.UnsignedByte, dataTxt.Scan0);
            bmpTxt.UnlockBits(dataTxt);
        }
        public void setCouleurFond(Color couleur) {
            couleurDeFond = couleur;
            chargerTexte();
        }
        public void setCouleurTexte(Color couleur) {
            pinceau.Color = couleur;
            chargerTexte();
        }
        public void setPoliceNormal() {
            policeAffichage = policeSansSerif;
            chargerTexte();
        }
        public void setPoliceGras() {
            policeAffichage = policeSansSerifGras;
            chargerTexte();
        }
        #endregion
    }
}
