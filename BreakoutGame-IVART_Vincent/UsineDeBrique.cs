using OpenTK;
using System;
using System.Collections.Generic;

namespace BreakoutGame_IVART_Vincent {
    internal class UsineDeBrique {
        public static List<Brique> getListeBrique(TableauBrique choixListe) {
            Vector2 pointA;
            Vector2 pointB;
            Vector2 pointC;
            Vector2 pointD;
            float positionX;
            float positionY;
            float espacement = 5.0f;
            float Original_X = -300.0f;
            int qtyRows = 4;
            int qtyColumn = 9;
            float largeurCotes = (600.0f - (qtyColumn - 1) * espacement) / qtyColumn;
            float hauteurCotes = 20.0f;
            List<Brique> listeCaisses = new List<Brique>();

            positionY = 150.0f - hauteurCotes;

            switch (choixListe) {
                case TableauBrique.Full:
                    for (int row = 0; row < qtyRows; row++) {
                        positionX = Original_X;

                        for (int column = 0; column < qtyColumn; column++) {
                            pointA = new Vector2(positionX, positionY);
                            pointB = new Vector2(positionX + largeurCotes, positionY);
                            pointC = new Vector2(positionX + largeurCotes, positionY + hauteurCotes);
                            pointD = new Vector2(positionX, positionY + hauteurCotes);
                            listeCaisses.Add(new Brique(pointA, pointB, pointC, pointD, row, column));
                            positionX += largeurCotes + espacement;
                        }
                        positionY -= hauteurCotes + espacement;
                    }
                    break;
                case TableauBrique.PyramideNegative:

                    for (int row = 1; row <= qtyColumn / 2; row++) {
                        positionX = Original_X;

                        for (int column = 1; column <= qtyColumn / 2 + 1 - row; column++) {
                            pointA = new Vector2(positionX, positionY);
                            pointB = new Vector2(positionX + largeurCotes, positionY);
                            pointC = new Vector2(positionX + largeurCotes, positionY + hauteurCotes);
                            pointD = new Vector2(positionX, positionY + hauteurCotes);
                            listeCaisses.Add(new Brique(pointA, pointB, pointC, pointD, row, column));
                            positionX += largeurCotes + espacement;
                        }

                        positionX = (positionX - espacement) * -1.0f;
                        for (int column = 1; column <= qtyColumn / 2 + 1 - row; column++) {
                            pointA = new Vector2(positionX, positionY);
                            pointB = new Vector2(positionX + largeurCotes, positionY);
                            pointC = new Vector2(positionX + largeurCotes, positionY + hauteurCotes);
                            pointD = new Vector2(positionX, positionY + hauteurCotes);
                            listeCaisses.Add(new Brique(pointA, pointB, pointC, pointD, row, column));
                            positionX += largeurCotes + espacement;
                        }
                        positionY -= hauteurCotes + espacement;
                    }
                    break;
                case TableauBrique.CouronneInverse:

                    for (int row = 0; row < 3; row++) {
                        positionX = Original_X;

                        for (int column = 0; column < qtyColumn; column++) {
                            switch (row) {
                                case 1:
                                    if (column != 0 && column != 4 && column != 8 && column != 12 && column != 16) {
                                        pointA = new Vector2(positionX, positionY);
                                        pointB = new Vector2(positionX + largeurCotes, positionY);
                                        pointC = new Vector2(positionX + largeurCotes, positionY + hauteurCotes);
                                        pointD = new Vector2(positionX, positionY + hauteurCotes);
                                        listeCaisses.Add(new Brique(pointA, pointB, pointC, pointD, row, column));
                                    }
                                    break;
                                case 2:
                                    if (column == 2 || column == 6 || column == 10 || column == 14) {
                                        pointA = new Vector2(positionX, positionY);
                                        pointB = new Vector2(positionX + largeurCotes, positionY);
                                        pointC = new Vector2(positionX + largeurCotes, positionY + hauteurCotes);
                                        pointD = new Vector2(positionX, positionY + hauteurCotes);
                                        listeCaisses.Add(new Brique(pointA, pointB, pointC, pointD, row, column));
                                    }
                                    break;
                                default:
                                    pointA = new Vector2(positionX, positionY);
                                    pointB = new Vector2(positionX + largeurCotes, positionY);
                                    pointC = new Vector2(positionX + largeurCotes, positionY + hauteurCotes);
                                    pointD = new Vector2(positionX, positionY + hauteurCotes);
                                    listeCaisses.Add(new Brique(pointA, pointB, pointC, pointD, row, column));
                                    break;
                            }
                            positionX += largeurCotes + espacement;
                        }
                        positionY -= hauteurCotes + espacement;
                    }
                    break;
                case TableauBrique.PyramideInverse:
                    int skipColumn = 0;

                    int qtyColumnPair = (qtyColumn % 2 != 0) ? qtyColumn + 1 : qtyColumn;

                    for (int row = 0; row < qtyColumnPair / 2; row++) {
                        float startX = Original_X + row * (largeurCotes + espacement);

                        for (int column = 0; column < qtyColumn - skipColumn; column++) {
                            float currentX = startX + column * (largeurCotes + espacement);

                            pointA = new Vector2(currentX, positionY);
                            pointB = new Vector2(currentX + largeurCotes, positionY);
                            pointC = new Vector2(currentX + largeurCotes, positionY + hauteurCotes);
                            pointD = new Vector2(currentX, positionY + hauteurCotes);
                            listeCaisses.Add(new Brique(pointA, pointB, pointC, pointD, row, column));
                        }
                        positionY -= hauteurCotes + espacement;
                        skipColumn += 2;
                    }
                    break;

                default:
                    break;
            }

            return listeCaisses;
        }

    }
}
