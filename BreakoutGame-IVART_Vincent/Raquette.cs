using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace BreakoutGame_IVART_Vincent {
    class Raquette : BasePourObjets {
        #region Attributs
        float deplacementHorizontal;
        float incrementHorizontal;
        bool toucheGauchePressee = false;
        bool toucheDroitePressee = false;
        #endregion //Attributs

        #region ConstructeursInitialisation
        public Raquette(Vector2 pointA, Vector2 pointB, Vector2 pointC, Vector2 pointD) : base("../../images/Raquette.bmp", pointA, pointB, pointC, pointD) {
            incrementHorizontal = 2.0f;
        }
        #endregion //ConstructeursInitialisation

        #region MethodesClasseParent
        public override void update() {
            deplacementHorizontal = 0.0f;
            if (toucheGauchePressee && deplacementHorizontal - incrementHorizontal >= -300.0f - listePoints[0].X) {
                deplacementHorizontal -= incrementHorizontal;
            } else if (toucheDroitePressee && deplacementHorizontal + incrementHorizontal <= 300.0f - listePoints[2].X) {
                deplacementHorizontal += incrementHorizontal;
            }
            for (int i = 0;i < 4;i++) {
                listePoints[i].X += deplacementHorizontal;
            }
        }
        public void dessiner() {
            GL.PushMatrix();
            base.dessiner(PrimitiveType.Quads);
            GL.PopMatrix();
        }
        public void deplacer(Key touche, bool pressee) {
            if (touche == Key.A || touche == Key.Left) {
                toucheGauchePressee = pressee;
            } else if (touche == Key.D || touche == Key.Right) {
                toucheDroitePressee = pressee;
            }
        }

        public float getDeplacementHorizontal() {
            return deplacementHorizontal;
        }
        #endregion // MethodesClasseParent

        #region GestionCollisions
        public RectangleCollision getRectangleCollision(CoteObjets coteBalle, float padding) {
            CoteObjets coteRaquette = getCoteInverse(coteBalle);
            Vector2 pointInferieurGauche;
            float largeur;
            float hauteur;
            switch (coteRaquette) {
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
        private CoteObjets getCoteInverse(CoteObjets coteBalle) {
            CoteObjets coteRaquette;

            switch (coteBalle) {
                case CoteObjets.NORD:
                    coteRaquette = CoteObjets.SUD;
                    break;
                case CoteObjets.SUD:
                    coteRaquette = CoteObjets.NORD;
                    break;
                case CoteObjets.EST:
                    coteRaquette = CoteObjets.OUEST;
                    break;
                case CoteObjets.OUEST:
                    coteRaquette = CoteObjets.EST;
                    break;
                default:
                    coteRaquette = CoteObjets.NULL;
                    break;
            }
            return coteRaquette;
        }
        #endregion
    }
}
