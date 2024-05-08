using System;
using System.IO;
using OpenTK.Audio.OpenAL;

namespace BreakoutGame_IVART_Vincent {
    internal class FichierWAV {
        string nomFichier;
        int nbrCanaux;
        int frequence;
        int nbrBits;
        int qteDonneesSonores;
        byte[] donneesSonores;

        public FichierWAV(string nomFichier) {
            this.nomFichier = nomFichier;
            Stream fichierAudio = File.Open(nomFichier, FileMode.Open);
            chargerFichier(fichierAudio);
            fichierAudio.Close();
        }
        private void chargerFichier(Stream stream) {
            if (stream == null) {
                throw new ArgumentNullException("stream");
            }

            int nbrOctets;
            string donneesFichier;

            BinaryReader reader = new BinaryReader(stream);

            nbrOctets = 4;
            donneesFichier = new string(reader.ReadChars(nbrOctets));
            if (donneesFichier != "RIFF") {
                throw new NotSupportedException("N'est pas un fichier multimédia");
            }
            donneesFichier = reader.ReadInt32().ToString();
            donneesFichier = new string(reader.ReadChars(nbrOctets));
            if (donneesFichier != "WAVE") {
                throw new NotSupportedException("Le fichier audio n'est pas au format WAVE");
            }
            donneesFichier = new string(reader.ReadChars(nbrOctets));
            if (donneesFichier != "fmt ") {
                throw new NotSupportedException("Fichier WAVE non supporté (fmt).");
            }
            nbrOctets = 6;
            donneesFichier = new string(reader.ReadChars(nbrOctets));
            nbrCanaux = reader.ReadInt16();
            frequence = reader.ReadInt32();
            nbrOctets = 6;
            donneesFichier = new string(reader.ReadChars(nbrOctets));
            nbrBits = reader.ReadInt16();
            nbrOctets = 4;
            donneesFichier = new string(reader.ReadChars(nbrOctets));
            if (donneesFichier != "data") {
                throw new NotSupportedException("Fichier WAVE non supporté (data).");
            }
            qteDonneesSonores = reader.ReadInt32();
            donneesSonores = reader.ReadBytes(qteDonneesSonores);
        }
        public ALFormat getFormatSonAL() {
            ALFormat format;
            switch (nbrCanaux) {
                case 1: format = (nbrBits == 8 ? ALFormat.Mono8 : ALFormat.Mono16); break;
                case 2: format = (nbrBits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16); break;
                default: throw new NotSupportedException("Format non supporté.");
            }
            return format;
        }
        public byte[] getDonneesSonores() {
            return donneesSonores;
        }
        public int getQteDonneesSonores() {
            return qteDonneesSonores;
        }
        public int getFrequence() {
            return frequence;
        }
    }
}
