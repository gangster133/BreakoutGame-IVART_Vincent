using System;
using System.CodeDom;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace BreakoutGame_IVART_Vincent {
    enum CoteObjets { NULL, NORD, SUD, EST, OUEST }
    enum TableauBrique { Full, CouronneInverse, PyramideInverse, PyramideNegative }
    class GestionJeu {

        #region Attributs
        GameWindow window;
        Balle balle;
        Raquette raquette;
        List<Brique> brique;
        //bool isGameOver;
        bool jeuActif;
        bool jeuPause;
        int nbrBalle;
        #endregion

        #region ConstructeurInitialisation
        public GestionJeu(GameWindow window) {
            this.window = window;
            //isGameOver = false;
            jeuActif = false;
            jeuPause = false;
            nbrBalle = 3;
            start();
        }

        private void start() {
            double nbrImagesParSeconde = 60.0f;
            double dureeAffichageChaqueImage = 1.0f / nbrImagesParSeconde;

            window.Load += chargement;
            window.Resize += redimensionner;
            //window.KeyPress += actionKeyPress;
            window.KeyDown += actionKeyDown;
            window.KeyUp += actionKeyUp;
            window.UpdateFrame += update;
            window.RenderFrame += rendu;
            window.Run(dureeAffichageChaqueImage);
        }

        private void chargement(object sender, EventArgs arg) {
            GL.ClearColor(0.196f, 0.196f, 0.196f, 1.0f);
            GL.Enable(EnableCap.Texture2D);

            nouvelleBalle();

            nouvelleRaquette();

            brique = UsineDeBrique.getListeBrique(TableauBrique.PyramideInverse);
        }
        #endregion

        #region GestionAffichage
        private void redimensionner(object sender, EventArgs arg) {
            GL.Viewport(0, 0, window.Width, window.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-300.0, 300.0, -150.0, 150.0, -1.0, 1.0);
            GL.MatrixMode(MatrixMode.Modelview);
        }
        private void update(object sender, FrameEventArgs e) {
            if (jeuActif && !jeuPause) {
                balle.update();
                raquette.update();
                detectionCollision();
                if (balle.estSortie()) {
                    balleSortie();
                }
            }
        }

        private void rendu(object sender, FrameEventArgs arg) {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            raquette.dessiner();
            if (brique != null) {
                foreach (Brique brique in brique) {
                    brique.dessiner();
                }
            }
            if (balle != null) {
                balle.dessiner();
            }
            window.SwapBuffers();
        }
        #endregion

        #region GestionEntrees
        private void actionKeyDown(object sender, KeyboardKeyEventArgs arg) {
            if (arg.IsRepeat) return; // Ignorer les événements de répétition

            if (arg.Key == Key.A || arg.Key == Key.Left) {
                raquette.deplacer(arg.Key, true); // Touche enfoncée
            } else if (arg.Key == Key.D || arg.Key == Key.Right) {
                raquette.deplacer(arg.Key, true); // Touche enfoncée
            } else if (arg.Key == Key.P && jeuActif) {
                jeuPause = !jeuPause;
            } else if (arg.Key == Key.Space) {
                jeuActif = true;
            } else if (arg.Key == Key.Escape) {
                window.Exit();
            }
        }

        private void actionKeyUp(object sender, KeyboardKeyEventArgs arg) {
            if (arg.Key == Key.A || arg.Key == Key.Left) {
                raquette.deplacer(arg.Key, false); // Touche libérée
            } else if (arg.Key == Key.D || arg.Key == Key.Right) {
                raquette.deplacer(arg.Key, false); // Touche libérée
            }
        }
        #endregion

        #region GestionCollisions
        private void detectionCollision() {
            List<Brique> copieCaisse = new List<Brique>(brique);
            bool tableauVide = true;

            foreach (Brique copieBrique in copieCaisse) {
                if (balle.siCollision(copieBrique)) {
                    if (copieBrique.getPV() > 1) {
                        copieBrique.reducPV();
                        copieBrique.getTextureBrique();
                    } else {
                        if(!copieBrique.estIndestructible()) {
                            brique.Remove(copieBrique);
                        }
                    }
                    balle.inverserDirectionSelonCollision();
                }
                if (!copieBrique.estIndestructible()) {
                    tableauVide = false;
                }
            }
            balle.siCollision(raquette);
            if (tableauVide) {
                changerTableau();
            }
        }
        #endregion // GestionCollisions

        private void changerTableau() {
            jeuActif = false;
            List<Brique> copieCaisse = new List<Brique>(brique);
            foreach (Brique copieBrique in copieCaisse) {
                brique.Remove(copieBrique);
            }
            Random random = new Random();
            TableauBrique tableau = (TableauBrique)random.Next(4);
            brique = UsineDeBrique.getListeBrique(tableau);

            nouvelleBalle();
        }

        public void nouvelleBalle() {
            Vector2 pointA = new Vector2(-5.0f, -5.0f);
            Vector2 pointB = new Vector2(5.0f, -5.0f);
            Vector2 pointC = new Vector2(5.0f, 5.0f);
            Vector2 pointD = new Vector2(-5.0f, 5.0f);
            balle = new Balle(pointA, pointB, pointC, pointD);
        }

        public void nouvelleRaquette() {
            Vector2 pointA = new Vector2(-30.0f, -135.0f);
            Vector2 pointB = new Vector2(30.0f, -135.0f);
            Vector2 pointC = new Vector2(30.0f, -120.0f);
            Vector2 pointD = new Vector2(-30.0f, -120.0f);
            raquette = new Raquette(pointA, pointB, pointC, pointD);
        }
        public void balleSortie() {
            jeuActif = false;
            nbrBalle--;
            if (nbrBalle > 0) {
                nouvelleBalle();
                nouvelleRaquette();
            }
        }
    }
}
