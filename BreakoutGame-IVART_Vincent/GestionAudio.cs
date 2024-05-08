﻿using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using System;

namespace BreakoutGame_IVART_Vincent {
    internal class GestionAudio {
        #region Attributs
        AudioContext audioContex;
        float volumeMusique;

        int bufferBounce;
        int sourceBounce;
        FichierWAV fichierBounce;

        int bufferBrick;
        int sourceBrick;
        FichierWAV fichierBrick;

        int bufferRaquette;
        int sourceRaquette;
        FichierWAV fichierRaquette;

        int bufferFail;
        int sourceFail;
        FichierWAV fichierFail;
        #endregion // Attributs

        #region ConstructeursInitialisation
        public GestionAudio() {
            audioContex = new AudioContext();
            fichierBounce = new FichierWAV("../../audios/Bounce.wav");
            fichierBrick = new FichierWAV("../../audios/Brique.wav");
            fichierRaquette = new FichierWAV("../../audios/Raquette.wav");
            fichierFail = new FichierWAV("../../audios/fail.wav");
            volumeMusique = 0.1f;
            AL.Listener(ALListenerf.Gain, volumeMusique);
            init();
        }
        private void init() {
            bufferBounce = AL.GenBuffer();
            sourceBounce = AL.GenSource();
            AL.BufferData(bufferBounce, fichierBounce.getFormatSonAL(),
                fichierBounce.getDonneesSonores(), fichierBounce.getQteDonneesSonores(),
                fichierBounce.getFrequence());
            AL.Source(sourceBounce, ALSourcei.Buffer, bufferBounce);
            AL.Source(sourceBounce, ALSourceb.Looping, false);

            bufferBrick = AL.GenBuffer();
            sourceBrick = AL.GenSource();
            AL.BufferData(bufferBrick, fichierBrick.getFormatSonAL(),
                fichierBrick.getDonneesSonores(), fichierBrick.getQteDonneesSonores(),
                fichierBrick.getFrequence());
            AL.Source(sourceBrick, ALSourcei.Buffer, bufferBrick);
            AL.Source(sourceBrick, ALSourceb.Looping, false);

            bufferRaquette = AL.GenBuffer();
            sourceRaquette = AL.GenSource();
            AL.BufferData(bufferRaquette, fichierRaquette.getFormatSonAL(),
                fichierRaquette.getDonneesSonores(), fichierRaquette.getQteDonneesSonores(),
                fichierRaquette.getFrequence());
            AL.Source(sourceRaquette, ALSourcei.Buffer, bufferRaquette);
            AL.Source(sourceRaquette, ALSourceb.Looping, false);

            bufferFail = AL.GenBuffer();
            sourceFail = AL.GenSource();
            AL.BufferData(bufferFail, fichierFail.getFormatSonAL(),
                fichierFail.getDonneesSonores(), fichierFail.getQteDonneesSonores(),
                fichierFail.getFrequence());
            AL.Source(sourceFail, ALSourcei.Buffer, bufferFail);
            AL.Source(sourceFail, ALSourceb.Looping, false);
        }
        #endregion // ConstructeursInitialisation

        #region Destructeur
        ~GestionAudio() {
            AL.SourceStop(sourceBounce);
            AL.DeleteSource(sourceBounce);
            AL.DeleteBuffer(bufferBounce);

            AL.SourceStop(sourceBrick);
            AL.DeleteSource(sourceBrick);
            AL.DeleteBuffer(bufferBrick);

            AL.SourceStop(sourceRaquette);
            AL.DeleteSource(sourceRaquette);
            AL.DeleteBuffer(bufferRaquette);

            AL.SourceStop(sourceFail);
            AL.DeleteSource(sourceFail);
            AL.DeleteBuffer(bufferFail);
        }
        #endregion // Destructeur

        #region Methodes
        public void jouerSonBounce() {
            AL.SourcePlay(sourceBounce);
        }
        public void jouerSonBrick() {
            AL.SourcePlay(sourceBrick);
        }
        public void jouerSonRaquette() {
            AL.SourcePlay(sourceRaquette);
        }
        public void jouerSonFail() {
            AL.SourcePlay(sourceFail);
        }
        public bool effetSonoreEntainDeJouer() {
            ALSourceState etatSon;
            etatSon = AL.GetSourceState(sourceBounce);
            if (etatSon == ALSourceState.Playing) {
                return true;
            }
            etatSon = AL.GetSourceState(sourceBrick);
            if (etatSon == ALSourceState.Playing) {
                return true;
            }
            etatSon = AL.GetSourceState(sourceRaquette);
            if (etatSon == ALSourceState.Playing) {
                return true;
            }
            etatSon = AL.GetSourceState(sourceFail);
            if (etatSon == ALSourceState.Playing) {
                return true;
            }
            return false;
        }
        #endregion // Methodes
    }
}
