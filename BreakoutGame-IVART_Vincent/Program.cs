using OpenTK;
using OpenTK.Graphics;
using System;

namespace BreakoutGame_IVART_Vincent {
    internal class Program {
        static void Main(string[] args) {
            #region Attributs
            int largeurFenetre = 1200;
            int hauteurFenetre = 600;
            string titreFenetre = "BreakoutGame-IVART Vincent";
            #endregion //Attributs

            #region Code
            DisplayDevice moniteur = DisplayDevice.Default;
            if (DisplayDevice.Default == DisplayDevice.GetDisplay(DisplayIndex.Second)) {
                moniteur = DisplayDevice.GetDisplay(DisplayIndex.First);
            }
            GameWindow window = new GameWindow(largeurFenetre, hauteurFenetre, GraphicsMode.Default, titreFenetre, GameWindowFlags.Default, moniteur);
            centrerFenetre(window, moniteur);
            GestionJeu fenetrePrincipale = new GestionJeu(window);
            #endregion //Code
        }
        static void centrerFenetre(GameWindow fenetreJeu, DisplayDevice moniteur) {
            int hauteurBarreDeTache = 100;
            fenetreJeu.X = (moniteur.Width - fenetreJeu.Width) / 2;
            fenetreJeu.Y = (moniteur.Height - fenetreJeu.Height - hauteurBarreDeTache) / 2;
        }
    }
}
