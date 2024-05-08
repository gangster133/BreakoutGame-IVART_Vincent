using OpenTK;
using System;
using OpenTK.Graphics.OpenGL;

namespace BreakoutGame_IVART_Vincent {
    class Balle : BasePourObjets {
        #region Attributs
        GestionAudio audio = new GestionAudio();
        float deplacementVertical;
        float incrementVertical;
        float deplacementHorizontal;
        float incrementHorizontal;
        const float maxIncrementVertical = 3.0f;
        const float maxIncrementHorizontal = 2.0f;
        bool estSotie = false;

        CoteObjets coteCollision;
        #endregion //Attributs

        #region ConstructeursInitialisation
        public Balle(Vector2 pointA, Vector2 pointB, Vector2 pointC, Vector2 pointD) : base("../../images/Balle.bmp", pointA, pointB, pointC, pointD) {
            deplacementVertical = 0.0f;
            incrementVertical = -1.0f;
            deplacementHorizontal = 0.0f;
            incrementHorizontal = 0.0f;
            coteCollision = CoteObjets.NULL;
        }
        #endregion //ConstructeursInitialisation

        #region MethodesClasseParent
        public override void update() {
            if (deplacementVertical + incrementVertical >= 150.0f - listePoints[3].Y) {
                incrementVertical *= -1.0f;
                audio.jouerSonBounce();
            }
            if (deplacementHorizontal + incrementHorizontal >= 300.0f - listePoints[2].X || deplacementHorizontal + incrementHorizontal <= -300.0f - listePoints[0].X) {
                incrementHorizontal *= -1.0f;
                audio.jouerSonBounce();
            }
            if (deplacementVertical + incrementVertical <= -150.0f - listePoints[0].Y) {
                estSotie = true;
            }
            for (int i = 0; i < 4; i++) {
                listePoints[i].Y += incrementVertical;
                listePoints[i].X += incrementHorizontal;
            }
        }
        public void dessiner() {
            GL.PushMatrix();
            base.dessiner(PrimitiveType.Quads);
            GL.PopMatrix();
        }
        #endregion // MethodesClasseParent

        #region MethodesAlteration
        public void inverserDirection(CoteObjets coteCollision) {
            incrementVertical *= -1.0f;
            incrementHorizontal *= -1.0f;
        }
        public void inverserDirectionSelonCollision() {
            regressionPreventive();
            switch (this.coteCollision) {
                case CoteObjets.NORD:
                case CoteObjets.SUD:
                    inverserDirectionVerticale();
                    break;
                case CoteObjets.EST:
                case CoteObjets.OUEST:
                    inverserDirectionHorizontale();
                    break;
                default:
                    break;
            }
            coteCollision = CoteObjets.NULL;
        }
        private void regressionPreventive() {
            for (int i = 0; i < 4; i++) {
                listePoints[i].X -= incrementHorizontal;
                listePoints[i].Y -= incrementVertical;
            }
        }
        private void inverserDirectionVerticale() {
            incrementVertical *= -1.0f;
        }
        private void inverserDirectionHorizontale() {
            incrementHorizontal *= -1.0f;
        }
        #endregion

        #region GestionCollisions
        public bool siCollision(Brique brique) {
            bool siCollision = false;

            float marge;
            Vector2[] segmentBalle;
            RectangleCollision rectangleColisionBrique;

            foreach (CoteObjets coteVerificationBalle in Enum.GetValues(typeof(CoteObjets))) {
                if (coteVerificationBalle != CoteObjets.NULL) {
                    marge = getMargePourCollision(coteVerificationBalle);
                    segmentBalle = getSegment(coteVerificationBalle);
                    rectangleColisionBrique = brique.getRectangleCollision(coteVerificationBalle, marge);

                    siCollision = rectangleColisionBrique.verifCollision(segmentBalle, coteVerificationBalle);

                    if (siCollision) {
                        siCollision = true;
                        this.coteCollision = coteVerificationBalle;
                        Console.WriteLine("Brique collisioné");
                        break;
                    }
                }
            }
            return siCollision;
        }
        public bool siCollision(Raquette raquette) {
            bool siCollision = false;

            float marge;
            Vector2[] segmentBalle;
            RectangleCollision rectangleColisionRaquette;

            foreach (CoteObjets coteVerificationBalle in Enum.GetValues(typeof(CoteObjets))) {
                if (coteVerificationBalle != CoteObjets.NULL) {
                    marge = getMargePourCollision(coteVerificationBalle);
                    segmentBalle = getSegment(coteVerificationBalle);
                    rectangleColisionRaquette = raquette.getRectangleCollision(coteVerificationBalle, marge);

                    siCollision = rectangleColisionRaquette.verifCollision(segmentBalle, coteVerificationBalle);

                    if (siCollision) {
                        siCollision = true;
                        coteCollision = coteVerificationBalle;

                        float deltaHorizontal = raquette.getDeplacementHorizontal() - this.deplacementHorizontal;

                        float signeDelta = (deltaHorizontal < 0) ? -1f : 1f;
                        float valeurDelta = (deltaHorizontal < 0) ? -deltaHorizontal : deltaHorizontal;
                        float minDelta = (valeurDelta < maxIncrementHorizontal * 0.25f) ? valeurDelta : maxIncrementHorizontal * 0.25f;
                        incrementHorizontal += signeDelta * minDelta;

                        inverserDirectionVerticale();
                        incrementVertical = incrementVertical < maxIncrementVertical ? incrementVertical + 0.2f : maxIncrementVertical;
                        audio.jouerSonRaquette();
                        Console.WriteLine("Raquette Collisioné");
                        break;
                    }
                }
            }
            return siCollision;
        }


        private float getMargePourCollision(CoteObjets coteVerificationBalle) {
            float padding;

            switch (coteVerificationBalle) {
                case CoteObjets.NORD:
                case CoteObjets.SUD:
                    padding = maxIncrementVertical*2;
                    break;
                case CoteObjets.EST:
                case CoteObjets.OUEST:
                    padding = maxIncrementHorizontal*2;
                    break;
                default:
                    padding = 0.0f;
                    break;
            }
            return padding;
        }
        private Vector2[] getSegment(CoteObjets coteRectangle) {
            Vector2[] segment = new Vector2[2];

            switch (coteRectangle) {
                case CoteObjets.NORD:
                    segment[0] = listePoints[3];
                    segment[1] = listePoints[2];
                    break;
                case CoteObjets.SUD:
                    segment[0] = listePoints[0];
                    segment[1] = listePoints[1];
                    break;
                case CoteObjets.EST:
                    segment[0] = listePoints[1];
                    segment[1] = listePoints[2];
                    break;
                case CoteObjets.OUEST:
                    segment[0] = listePoints[0];
                    segment[1] = listePoints[3];
                    break;
                default:
                    break;
            }
            return segment;
        }
        #endregion

        public bool estSortie() {
            return estSotie;
        }
    }
}
