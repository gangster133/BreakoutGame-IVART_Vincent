using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace BreakoutGame_IVART_Vincent {
    internal class RectangleCollision {

        #region Attributs
        Vector3 couleurTEST = new Vector3(1.0f, 0.0f, 0.0f);
        Vector3 couleurBlanc = new Vector3(1.0f, 1.0f, 1.0f);

        protected Vector2 pointInferieurGauche;
        protected Vector2 pointInferieurDroite;
        protected Vector2 pointSuperieurDroite;
        protected Vector2 pointSuperieurGauche;
        #endregion

        #region ConstructeursInitialisation
        public RectangleCollision(Vector2 pointInfGauche, float largeur, float hauteur) {
            pointInferieurGauche = pointInfGauche;
            pointInferieurDroite = new Vector2(pointInfGauche.X + largeur, pointInfGauche.Y);
            pointSuperieurDroite = new Vector2(pointInfGauche.X + largeur, pointInfGauche.Y + hauteur);
            pointSuperieurGauche = new Vector2(pointInfGauche.X, pointInfGauche.Y + hauteur);
        }
        #endregion

        #region GestionCollisions
        private bool siCollisionPotentielle_AxeY(Vector2[] segmentBalle, CoteObjets coteVerificationBalle) {
            bool estCollisionPotentielle = false;
            switch (coteVerificationBalle) {
                case CoteObjets.NORD:
                case CoteObjets.SUD:
                    if (segmentBalle[0].Y > this.pointInferieurGauche.Y
                       && segmentBalle[0].Y < this.pointSuperieurGauche.Y) {
                        estCollisionPotentielle = true;
                    }
                    break;
                case CoteObjets.EST:
                case CoteObjets.OUEST:
                    if ((segmentBalle[0].Y > this.pointInferieurGauche.Y
                        && segmentBalle[0].Y < this.pointSuperieurGauche.Y)
                        ||
                        (segmentBalle[1].Y > this.pointInferieurGauche.Y
                        && segmentBalle[1].Y < this.pointSuperieurGauche.Y)
                        ) {
                        estCollisionPotentielle = true;
                    }
                    break;
                default:
                    break;
            }
            return estCollisionPotentielle;
        }
        private bool siCollisionPotentielle_AxeX(Vector2[] segmentBalle, CoteObjets coteVerificationBalle) {
            bool estCollisionPotentielle = false;
            switch (coteVerificationBalle) {
                case CoteObjets.NORD:
                case CoteObjets.SUD:
                    if ((segmentBalle[0].X > this.pointInferieurGauche.X && segmentBalle[0].X < this.pointInferieurDroite.X)
                        || (segmentBalle[1].X > this.pointInferieurGauche.X && segmentBalle[1].X < this.pointInferieurDroite.X)) {
                        estCollisionPotentielle = true;
                    }
                    break;
                case CoteObjets.EST:
                case CoteObjets.OUEST:
                    if (segmentBalle[0].X > this.pointInferieurGauche.X && segmentBalle[0].X < this.pointInferieurDroite.X) {
                        estCollisionPotentielle = true;
                    }
                    break;
                default:
                    break;
            }
            return estCollisionPotentielle;
        }
        public bool verifCollision(Vector2[] segmentBalle, CoteObjets coteVerificationBalle) {
            bool estCollision = false;
            if (siCollisionPotentielle_AxeY(segmentBalle, coteVerificationBalle)
                && siCollisionPotentielle_AxeX(segmentBalle, coteVerificationBalle)) {
                estCollision = true;
            }
            return estCollision;
        }
        #endregion

        #region TEST
        public void dessinerTEST() {
            GL.PushMatrix();
            GL.Color3(color: couleurTEST);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(pointInferieurGauche.X, pointInferieurGauche.Y);
            GL.Vertex2(pointInferieurDroite.X, pointInferieurDroite.Y);
            GL.Vertex2(pointSuperieurDroite.X, pointSuperieurDroite.Y);
            GL.Vertex2(pointSuperieurGauche.X, pointSuperieurGauche.Y);
            GL.End();
            GL.Color3(color: couleurBlanc);
            GL.PopMatrix();
        }
        #endregion
    }
}
