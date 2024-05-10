using System;
using System.Collections.Generic;
using System.Drawing;
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
        GestionAudio audio;
        bool jeuActif;
        bool jeuPause;
        int nbrBalle;
        string stringNbrBalles;
        Texte texteNbrBalles;
        int nbrPoints;
        string stringNbrPoints;
        Texte texteNbrPoints;
        int nbrTableau;
        string stringNbrTableaux;
        Texte texteNbrTableaux;
        int nbrRebondRaquette;
        bool strategie2Activee;
        Random random = new Random((int)DateTime.Now.Ticks);
        #endregion

        #region ConstructeurInitialisation
        public GestionJeu(GameWindow window) {
            this.window = window;
            jeuActif = false;
            jeuPause = false;
            start();
        }
        private void start() {
            double nbrImagesParSeconde = 60.0f;
            double dureeAffichageChaqueImage = 1.0f / nbrImagesParSeconde;
            strategie2Activee = false;
            nbrBalle = 3;
            stringNbrBalles = "Balles : ";
            nbrPoints = 0;
            stringNbrPoints = "Points : ";
            nbrTableau = 0;
            stringNbrTableaux = "Tableaux :";
            window.Load += chargement;
            window.Resize += redimensionner;
            window.KeyDown += actionKeyDown;
            window.KeyUp += actionKeyUp;
            window.UpdateFrame += update;
            window.RenderFrame += rendu;
            window.Run(dureeAffichageChaqueImage);
        }
        private void chargement(object sender, EventArgs arg) {
            GL.ClearColor(0.196f, 0.196f, 0.196f, 1.0f);
            GL.Enable(EnableCap.Texture2D);

            affichageConsole();

            audio = new GestionAudio();
            nouvelleBalle();
            nouvelleRaquette();
            nouveauTableau();

            int hauteurZoneTexte = 15;
            int largeurZoneTexte = 200;
            Vector2 coinInferieurGauche = new Vector2(-300.0f, -150.0f);
            texteNbrTableaux = new Texte(coinInferieurGauche, largeurZoneTexte, hauteurZoneTexte);
            texteNbrTableaux.setTexte(getTxtCompletNbrTableaux());
            texteNbrTableaux.setCouleurTexte(Color.White);
            texteNbrTableaux.setCouleurFond(Color.Black);
            texteNbrTableaux.setPoliceNormal();

            largeurZoneTexte = 200;
            coinInferieurGauche = new Vector2(-100.0f, -150.0f);
            texteNbrPoints = new Texte(coinInferieurGauche, largeurZoneTexte, hauteurZoneTexte);
            texteNbrPoints.setTexte(getTxtCompletNbrPoints());
            texteNbrPoints.setCouleurTexte(Color.Goldenrod);
            texteNbrPoints.setCouleurFond(Color.Purple);
            texteNbrPoints.setPoliceNormal();

            largeurZoneTexte = 200;
            coinInferieurGauche = new Vector2(100.0f, -150.0f);
            texteNbrBalles = new Texte(coinInferieurGauche, largeurZoneTexte, hauteurZoneTexte);
            texteNbrBalles.setTexte(getTxtCompletNbrBalle());
            texteNbrBalles.setCouleurTexte(Color.Orange);
            texteNbrBalles.setCouleurFond(Color.BlueViolet);
            texteNbrBalles.setPoliceNormal();
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
                List<Brique> copieCaisse = new List<Brique>(brique);
                foreach (Brique copieBrique in copieCaisse) {
                    copieBrique.update();
                    if (copieBrique.estADetruire()) {
                        brique.Remove(copieBrique);
                    }
                }
                texteNbrBalles.setTexte(getTxtCompletNbrBalle());
                texteNbrPoints.setTexte(getTxtCompletNbrPoints());
                texteNbrTableaux.setTexte(getTxtCompletNbrTableaux());
            }
        }
        private void rendu(object sender, FrameEventArgs arg) {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            texteNbrBalles.dessiner();
            texteNbrPoints.dessiner();
            texteNbrTableaux.dessiner();
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

        public void affichageConsole() {
            Console.SetWindowSize(60, 13);
            Console.WriteLine(" espace\t: lance la partie/active la balle");
            Console.WriteLine(" A ou <-: Décale la raquette a gauche");
            Console.WriteLine(" D ou ->: Décale la raquette a droite");
            Console.WriteLine(" P\t: Met en pause le jeu");
            Console.WriteLine("");
            Console.WriteLine(" Stratégie 1 : Les briques dynamique gagnent 1 PV\n a chaque fois que la balle touche 10 fois la raquette\n");
            Console.WriteLine(" Stratégie 2 : Les briques dynamique gagnent 5 PV\n si il ne reste qu'elles et les briques indestructible\n");
            Console.WriteLine(" Stratégie 3 : Les briques dynamique gagnent\n 2 PV lorsque la balle sort");
            Console.SetCursorPosition(0, 0);
        }
        #endregion

        #region GestionEntrees
        private void actionKeyDown(object sender, KeyboardKeyEventArgs arg) {
            if (arg.IsRepeat) return;

            if (arg.Key == Key.A || arg.Key == Key.Left) {
                raquette.deplacer(arg.Key, true);
            } else if (arg.Key == Key.D || arg.Key == Key.Right) {
                raquette.deplacer(arg.Key, true);
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
                raquette.deplacer(arg.Key, false);
            } else if (arg.Key == Key.D || arg.Key == Key.Right) {
                raquette.deplacer(arg.Key, false);
            }
        }
        #endregion

        #region GestionCollisions
        private void detectionCollision() {
            List<Brique> copieCaisse = new List<Brique>(brique);
            bool tableauVide = true;
            bool tableauQueDynamique = true;

            foreach (Brique copieBrique in copieCaisse) {
                if (balle.siCollision(copieBrique)) {
                    if (copieBrique.getPV() > 1) {
                        copieBrique.reducPV();
                        copieBrique.getTextureBrique();
                        audio.jouerSonBounce();
                    } else {
                        if (!copieBrique.getEstIndestructible()) {
                            copieBrique.activeEnDestruction();
                            audio.jouerSonBrick();
                        } else { audio.jouerSonBounce(); }
                    }
                    balle.inverserDirectionSelonCollision();
                    nbrPoints += copieBrique.getEstIndestructible() ? 0 : 5;
                }
                if (!copieBrique.getEstIndestructible()) {
                    tableauVide = false;
                    if (!copieBrique.getEstDynamique()) {
                        tableauQueDynamique = false;
                    }
                }

            }
            if (balle.siCollision(raquette)) {
                audio.jouerSonRaquette();
                nbrRebondRaquette++;
                strategie1BriqueDynamique();
            }
            if (balle.aCollisionneBordure()) {
                audio.jouerSonBounce();
            }
            if (tableauQueDynamique) {
                strategie2BriqueDynamique();
                if (tableauVide) {
                    changerTableau();
                }
            }
        }
        #endregion // GestionCollisions

        #region GestionTexte
        private string getTxtCompletNbrBalle() {
            return stringNbrBalles + nbrBalle;
        }
        private string getTxtCompletNbrPoints() {
            return stringNbrPoints + nbrPoints;
        }
        private string getTxtCompletNbrTableaux() {
            return stringNbrTableaux + nbrTableau;
        }
        #endregion // GestionTexte

        private void changerTableau() {
            jeuActif = false;
            nbrBalle += 2;
            nbrPoints += 50;
            nbrTableau++;
            List<Brique> copieCaisse = new List<Brique>(brique);
            foreach (Brique copieBrique in copieCaisse) {
                brique.Remove(copieBrique);
            }
            nouvelleBalle();
            nouvelleRaquette();
            nouveauTableau();
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
        public void nouveauTableau() {
            nbrTableau++;
            nbrRebondRaquette = 0;
            strategie2Activee = false;
            TableauBrique tableau = (TableauBrique)random.Next(4);
            brique = UsineDeBrique.getListeBrique(tableau);
        }
        public void balleSortie() {
            jeuActif = false;
            nbrBalle--;
            audio.jouerSonFail();
            if (nbrBalle > 0) {
                nouvelleBalle();
                nouvelleRaquette();
            }
            strategie3BriqueDynamique();
        }

        //Stratégie 1 -> Toutes les briques dynamique gagnent 1 PV a chaque fois que la balle touche 10 fois la raquette
        public void strategie1BriqueDynamique() {
            if (nbrRebondRaquette % 10 == 0) {
                List<Brique> copieCaisse = new List<Brique>(brique);
                foreach (Brique copieBrique in copieCaisse) {
                    if (copieBrique.getEstDynamique() && !copieBrique.estEnDestruction()) {
                        copieBrique.ajoutPV(1);
                        copieBrique.getTextureBrique();
                    }
                }
            }
        }

        //Stratégie 2 -> Si il ne reste que des briques indestructible et dynamique, toutes les briques dynamique gagnent 5 PV
        public void strategie2BriqueDynamique() {
            if (!strategie2Activee) {
                strategie2Activee = true;
                List<Brique> copieCaisse = new List<Brique>(brique);
                foreach (Brique copieBrique in copieCaisse) {
                    if (copieBrique.getEstDynamique() && !copieBrique.estEnDestruction()) {
                        copieBrique.ajoutPV(5);
                        copieBrique.getTextureBrique();
                    }
                }
            }
        }

        //Stratégie 3 -> Toutes les briques dynamique gagnent 2 PV si la balle sort
        public void strategie3BriqueDynamique() {
            List<Brique> copieCaisse = new List<Brique>(brique);
            foreach (Brique copieBrique in copieCaisse) {
                if (copieBrique.getEstDynamique() && !copieBrique.estEnDestruction()) {
                    copieBrique.ajoutPV(2);
                    copieBrique.getTextureBrique();
                }
            }
        }
    }
}