using OpenTK;
using System;
using OpenTK.Graphics.OpenGL;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.Remoting.Messaging;

namespace BreakoutGame_IVART_Vincent {
    class Brique : BasePourObjets {
        #region Attribut
        int positionLigne;
        int positionColonne;
        int pointsDeVie;
        bool estIndestructible = false;
        int nbrFrame = 0;
        bool enDestruction = false;
        bool aDetruire = false;
        bool estDynamique = false;
        const int maxPV = 15;
        #endregion

        #region ConstructeursInitialisation
        public Brique(Vector2 pointA, Vector2 pointB, Vector2 pointC, Vector2 pointD, int posLigne, int posColonne) :
            base("../../images/Brique1.bmp", pointA, pointB, pointC, pointD) {
            positionLigne = posLigne;
            positionColonne = posColonne;
            Random random = new Random((int)DateTime.Now.Ticks); ;
            pointsDeVie = random.Next(1, 4);
            int typeBrique = random.Next(10);
            switch (typeBrique) {
                case 1:
                    estDynamique=true;
                    pointsDeVie = 1;
                    break;
                case 2:
                    estIndestructible = true;
                    break;
                default:
                    break;
            }
            getTextureBrique();
        }
        #endregion

        #region MethodesCLasseParent
        public override void update() {
            if (enDestruction) {
                animationDestruction();
            }
        }
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
            if (estIndestructible) {
                nomTexture = "../../images/BriqueIndestructible.bmp";
            } else if (estDynamique) {
                nomTexture = "../../images/BriqueDynamique" + pointsDeVie + ".bmp";
            } else {
                nomTexture = "../../images/Brique" + pointsDeVie + ".bmp";
            }
            chargerTexture();
        }
        public int getPV() {
            return pointsDeVie;
        }
        public bool getEstIndestructible() {
            return estIndestructible;
        }
        public bool estEnDestruction() {
            return enDestruction;
        }
        public void reducPV() {
            pointsDeVie--;
        }
        public void ajoutPV(int ajout) {
            pointsDeVie += ajout;
            if (pointsDeVie > maxPV) { pointsDeVie = maxPV; }
        }
        public bool estADetruire() {
            return aDetruire;
        }

        public bool getEstDynamique() {
            return estDynamique;
        }
        public void activeEnDestruction() { enDestruction = true; }

        public void animationDestruction() {
            nbrFrame++;
            if (nbrFrame <= 20) {
                nomTexture = estDynamique ? "../../images/BriqueDynamiqueDestruction1.bmp" : "../../images/BriqueDestruction1.bmp";
            } else if (nbrFrame <= 40) {
                nomTexture = estDynamique ? "../../images/BriqueDynamiqueDestruction2.bmp" : "../../images/BriqueDestruction2.bmp";
            } else if (nbrFrame <= 60) {
                nomTexture = estDynamique ? "../../images/BriqueDynamiqueDestruction3.bmp" : "../../images/BriqueDestruction3.bmp";
            } else {
                aDetruire = true;
            }
            chargerTexture();
        }
    }
}
