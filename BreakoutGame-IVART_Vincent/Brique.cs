using OpenTK;
using System;
using OpenTK.Graphics.OpenGL;
using System.Drawing.Imaging;
using System.Drawing;

namespace BreakoutGame_IVART_Vincent {
    class Brique : BasePourObjets {
        #region Attribut
        int positionLigne;
        int positionColonne;
        int pointsDeVie;
        bool indestructible = false;
        #endregion

        #region ConstructeursInitialisation
        public Brique(Vector2 pointA, Vector2 pointB, Vector2 pointC, Vector2 pointD, int posLigne, int posColonne) :
            base("../../images/Brique1.bmp", pointA, pointB, pointC, pointD) {
            positionLigne = posLigne;
            positionColonne = posColonne;
            Random random = new Random();
            pointsDeVie = random.Next(1, 4);
            if (random.Next(10) == 3) {
                indestructible = true;
            }
            getTextureBrique();
        }
        #endregion

        #region MethodesCLasseParent
        public override void update() { }

        public void dessiner() {
            GL.PushMatrix();
            base.dessiner(PrimitiveType.Quads);
            GL.PopMatrix();
        }
        #endregion

        #region GestionCollisions
        public RectangleCollision getRectangleCollision(CoteObjets coteBalle, float padding) {
            CoteObjets coteCaisse = getCoteInverse(coteBalle);

            Vector2 pointInferieurGauche;
            float largeur;
            float hauteur;

            switch (coteCaisse) {
                case CoteObjets.NORD:
                    hauteur = padding;
                    largeur = listePoints[2].X - listePoints[3].X;
                    pointInferieurGauche = new Vector2(listePoints[3].X, listePoints[3].Y - hauteur);
                    break;
                case CoteObjets.SUD:
                    hauteur = padding;
                    largeur = listePoints[1].X - listePoints[0].X;
                    pointInferieurGauche = new Vector2(listePoints[0].X, listePoints[0].Y);
                    break;
                case CoteObjets.EST:
                    largeur = padding;
                    hauteur = listePoints[2].Y - listePoints[1].Y;
                    pointInferieurGauche = new Vector2(listePoints[1].X - largeur, listePoints[1].Y);
                    break;
                case CoteObjets.OUEST:
                    largeur = padding;
                    hauteur = listePoints[3].Y - listePoints[0].Y;
                    pointInferieurGauche = new Vector2(listePoints[0].X, listePoints[0].Y);
                    break;
                default:
                    pointInferieurGauche = new Vector2();
                    largeur = 0.0f;
                    hauteur = 0.0f;
                    break;
            }

            return new RectangleCollision(pointInferieurGauche, largeur, hauteur);
        }
        private CoteObjets getCoteInverse(CoteObjets coteBallse) {
            CoteObjets coteBrique;

            switch (coteBallse) {
                case CoteObjets.NORD:
                    coteBrique = CoteObjets.SUD;
                    break;
                case CoteObjets.SUD:
                    coteBrique = CoteObjets.NORD;
                    break;
                case CoteObjets.EST:
                    coteBrique = CoteObjets.OUEST;
                    break;
                case CoteObjets.OUEST:
                    coteBrique = CoteObjets.EST;
                    break;
                default:
                    coteBrique = CoteObjets.NULL;
                    break;
            }
            return coteBrique;
        }
        #endregion
        public void getTextureBrique() {
            if (indestructible) {
                nomTexture = "../../images/BriqueIndestructible.bmp";
            } else {
                nomTexture = "../../images/Brique" + pointsDeVie + ".bmp";
            }

            chargerTexture();
        }

        public int getPV() {
            return pointsDeVie;
        }

        public bool estIndestructible() {
            return indestructible;
        }

        public void reducPV() { pointsDeVie--; }
    }
}
