using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpriteManager 
{

    public class TileMap
    {
        int[] adjacentIndices = { 1, 3, 4, 6 };
        int[] cornerIndices   = { 0, 2, 5, 7 };

        Sprite fill;
        Sprite wall;
        Sprite wallOneCorner;
        Sprite wallOneCornerAlt;
        Sprite wallBothCorners;
        Sprite wallCorner;
        Sprite wallCornerDiagonal;
        Sprite alley;
        Sprite end;
        Sprite box;
        Sprite corner;
        Sprite sameCorners;
        Sprite oppositeCorners;
        Sprite threeCorners;
        Sprite allCorners;

        Sprite test;

        public struct SpriteInfo
        {
            public Sprite sprite;
            public float rotation;
        };

        public struct TileInfo
        {
            public Tile tile;
            public float rotation;
        };


        public TileMap(string mapName)
        {
            // FOR TESTING!!!
            if (mapName == "Water" || mapName == "Mountain") 
            {
                if (mapName == "Water") { test = Resources.Load<Sprite>("TileMaps/Test"); }
                if (mapName == "Mountain") { test = Resources.Load<Sprite>("TileMaps/Test2"); }
                fill = test;
                wall = test;
                wallOneCorner = test;
                wallOneCornerAlt = test;
                wallBothCorners = test;
                wallCorner = test;
                wallCornerDiagonal = test;
                alley = test;
                end = test;
                box = test;
                corner = test;
                sameCorners = test;
                oppositeCorners = test;
                threeCorners = test;
                allCorners = test;
                return;
            }

            string prefix = "TileMaps/" + mapName;

            fill               = Resources.Load<Sprite>(prefix + "/Fill");
            wall               = Resources.Load<Sprite>(prefix + "/Wall");
            wallOneCorner      = Resources.Load<Sprite>(prefix + "/WallOneCorner");
            wallOneCornerAlt   = Resources.Load<Sprite>(prefix + "/WallOneCornerAlt");
            wallBothCorners    = Resources.Load<Sprite>(prefix + "/WallBothCorners");
            wallCorner         = Resources.Load<Sprite>(prefix + "/WallCorner");
            wallCornerDiagonal = Resources.Load<Sprite>(prefix + "/WallCornerDiagonal");
            alley              = Resources.Load<Sprite>(prefix + "/Alley");
            end                = Resources.Load<Sprite>(prefix + "/End");
            box                = Resources.Load<Sprite>(prefix + "/Box");
            corner             = Resources.Load<Sprite>(prefix + "/Corner");
            sameCorners        = Resources.Load<Sprite>(prefix + "/SameCorner");
            oppositeCorners    = Resources.Load<Sprite>(prefix + "/OppositeCorner");
            threeCorners       = Resources.Load<Sprite>(prefix + "/ThreeCorners");
            allCorners         = Resources.Load<Sprite>(prefix + "/AllCorners");
            test               = Resources.Load<Sprite>(prefix + "/Test");
        }

        // Given the level array and coord (x, y), returns sprite of what should be placed
        public TileInfo GetTileSprite(Tile tile, string[,] map, Tilemap tm, int x, int y)
        {
            // Get neighbors of the tiles
            string[] neighbors = GetNeighbors(map, x, y);
            int[] similarNeighbors = GetSimilarNeighbors(neighbors, map[x, y]);

            TileInfo result = new TileInfo();
            result.tile = new Tile();
            SpriteInfo si = GetSprite(similarNeighbors);
            result.tile.sprite = si.sprite;
            result.rotation = si.rotation;
            return result;
        }

        public Sprite GetFill()
        {
            return fill;
        }

        private SpriteInfo GetSprite(int[] neighbors)
        {
            SpriteInfo result = new SpriteInfo();
            result.rotation = 0f;

            if (neighbors == null) { result.sprite = box; return result; }

            int n = neighbors.Length;
            if (n == 0) { result.sprite = box; }
            else if (n == 1)
            {
                return OneNeighbor(neighbors);
            }
            else if (n == 2)
            {
                return TwoNeighbors(neighbors);
            }
            else if (n == 3)
            {
                return ThreeNeighbors(neighbors);
            }
            else if (n == 4)
            {
                return FourNeighbors(neighbors);
            }
            else if (n == 5)
            {
                return FiveNeighbors(neighbors);
            }
            else if (n == 6)
            {
                return SixNeighbors(neighbors);
            }
            else if (n == 7)
            {
                return SevenNeighbors(neighbors);
            }
            else if (n == 8)
            {
                result.sprite = fill;
            }
            else
            {
                result.sprite = test;
            }

            return result;
        }


        // Gets the sprite given there is one neighbor
        private SpriteInfo OneNeighbor(int[] neighbors)
        {
            SpriteInfo result = new SpriteInfo();
            result.rotation = 0f;

            int index = neighbors[0];
            // Adjacent
            if (IsInIntArray(adjacentIndices, index))
            {
                result.sprite = end;
                if (index == 6) { result.rotation = 180f; }
                else if (index == 3) { result.rotation = -90f; }
                else if (index == 4) { result.rotation = 90; }
            }
            else // Diagonal; no adjacent boxes to connect to
            {
                result.sprite = box;
            }

            return result;
        }


        // Gets the sprite given there is two neighbors
        private SpriteInfo TwoNeighbors(int[] neighbors)
        {
            SpriteInfo result = new SpriteInfo();
            result.rotation = 0f;

            // Will give constant ordering for following comparisons
            Array.Sort(neighbors);
            int index1 = neighbors[0];
            int index2 = neighbors[1];
            int adjacentNeighbors = NumInIntArray(adjacentIndices, neighbors);

            //Both indices are adjacent
            if (adjacentNeighbors == 2)
            {
                // If they are opposite it is an alley
                if ((index1 == 1 && index2 == 6) || (index1 == 3 && index2 == 4))
                {
                    result.sprite = alley;
                    if (index1 == 3) { result.rotation = 90f; }
                }
                else// If they are together it is a wall corner
                {
                    result.sprite = wallCornerDiagonal;
                    if (index1 == 4 && index2 == 6) { result.rotation = 90f; }
                    else if (index1 == 1 && index2 == 3) { result.rotation = -90f; }
                    else if (index1 == 3 && index2 == 6) { result.rotation = 180f; }
                }
            }
            // Only one index is adjacent
            else if (adjacentNeighbors == 1)
            {
                result.sprite = end;

                int i;
                if (IsInIntArray(adjacentIndices, index1)) { i = index1; }
                else { i = index2; }

                if (i == 4) { result.rotation = 90f; }
                else if (i == 3) { result.rotation = -90f; }
                else if (i == 6) { result.rotation = 180f; }
            }
            else // Adjacent
            {
                result.sprite = box;
            }

            return result;
        }



        private SpriteInfo ThreeNeighbors(int[] neighbors)
        {
            SpriteInfo result = new SpriteInfo();
            result.rotation = 0f;

            // Will give constant ordering for following comparisons
            Array.Sort(neighbors);
            int index1 = neighbors[0];
            int index2 = neighbors[1];
            int index3 = neighbors[2];
            int numAdjacentNeighbors = NumInIntArray(adjacentIndices, neighbors);


            if (numAdjacentNeighbors == 3)
            {
                result.sprite = wallBothCorners;
                if (index1 == 1 && index2 == 3 && index3 == 4) { result.rotation = -90f;}
                else if (index1 == 1 && index2 == 3 && index3 == 6) { result.rotation = 180f;}
                else if (index1 == 3 && index2 == 4 && index3 == 6) { result.rotation = 90f; }
            }
            else if (numAdjacentNeighbors == 2)
            {
                //Get adjacent indices
                int[] adjacentNeighbors = GetIntInArray(adjacentIndices, neighbors);
                Array.Sort(adjacentNeighbors);
                int n1 = adjacentNeighbors[0];
                int n2 = adjacentNeighbors[1];

                if ((n1 == 1 && n2 == 6) || (n1 == 3 && n2 == 4))
                {
                    result.sprite = alley;
                    if (n1 == 3) { result.rotation = 90f; }
                }
                else 
                {
                    int c = GetMissingInt(neighbors, adjacentNeighbors);

                    result.sprite = wallCorner;
                    if (n1 == 4 && n2 == 6) 
                    { 
                        result.rotation = 90f;
                        if (c != 7) { result.sprite = wallCornerDiagonal; }
                    }
                    else if (n1 == 1 && n2 == 3) 
                    { 
                        result.rotation = -90f;
                        if (c != 0) { result.sprite = wallCornerDiagonal; }
                    }
                    else if (n1 == 3 && n2 == 6) 
                    { 
                        result.rotation = 180f;
                        if (c != 5) { result.sprite = wallCornerDiagonal; }
                    }
                    else
                    {
                        if (c != 2) { result.sprite = wallCornerDiagonal; }
                    }
                }
            }
            else if (numAdjacentNeighbors == 1)
            {
                int[] adjacentNeighbors = GetIntInArray(adjacentIndices, neighbors);
                result.sprite = end; //end

                if (adjacentNeighbors.Length == 1)
                {
                    int neighbor = adjacentNeighbors[0];
                    if (neighbor == 6) { result.rotation = 180f; }
                    else if (neighbor == 3) { result.rotation = -90f; }
                    else if (neighbor == 4) { result.rotation = 90; }
                }
                else { result.sprite = box; }
            }
            else
            {
                result.sprite = box;
            }


            return result;
        }



        private SpriteInfo FourNeighbors(int[] neighbors)
        {
            SpriteInfo result = new SpriteInfo();
            result.rotation = 0f;

            // Will give constant ordering for following comparisons
            Array.Sort(neighbors);
            int index1 = neighbors[0];
            int index2 = neighbors[1];
            int index3 = neighbors[2];
            int index4 = neighbors[3];
            int numAdjacentNeighbors = NumInIntArray(adjacentIndices, neighbors);

            if (numAdjacentNeighbors == 4)
            {
                result.sprite = allCorners;
            }
            else if (numAdjacentNeighbors == 3)
            {
                //Get adjacent indices
                int[] adjacentNeighbors = GetIntInArray(adjacentIndices, neighbors);
                Array.Sort(adjacentNeighbors);
                int n1 = adjacentNeighbors[0];
                int n2 = adjacentNeighbors[1];
                int n3 = adjacentNeighbors[2];

                int[] cs = GetMissingInts(neighbors, adjacentNeighbors);

                result.sprite = wall;
                if (n1 == 1 && n2 == 3 && n3 == 4)
                {
                    if (!IsInIntArray(cs, 0)) { result.sprite = wallOneCornerAlt; }
                    else if (!IsInIntArray(cs, 2)) { result.sprite = wallOneCorner; }
                    result.rotation = -90f; 
                }
                else if (n1 == 1 && n2 == 3 && n3 == 6) 
                {
                    if (!IsInIntArray(cs, 5)) { result.sprite = wallOneCornerAlt; }
                    else if (!IsInIntArray(cs, 7)) { result.sprite = wallOneCorner; }
                    result.rotation = 180f; 
                }
                else if (n1 == 3 && n2 == 4 && n3 == 6) 
                {
                    if (!IsInIntArray(cs, 5)) { result.sprite = wallOneCorner; }
                    else if (!IsInIntArray(cs, 0)) { result.sprite = wallOneCornerAlt; }
                    result.rotation = 90f; 
                }
                else
                {
                    if (!IsInIntArray(cs, 2)) { result.sprite = wallOneCornerAlt; }
                    else if (!IsInIntArray(cs, 7)) { result.sprite = wallOneCorner; }
                }
            }
            else if (numAdjacentNeighbors == 2)
            {
                //Get adjacent indices
                int[] adjacentNeighbors = GetIntInArray(adjacentIndices, neighbors);
                Array.Sort(adjacentNeighbors);
                int n1 = adjacentNeighbors[0];
                int n2 = adjacentNeighbors[1];

                if ((n1 == 1 && n2 == 6) || (n1 == 3 && n2 == 4))
                {
                    result.sprite = alley;
                    if (n1 == 3) { result.rotation = 90f; }
                }
                else
                {
                    int[] cs = GetMissingInts(neighbors, adjacentNeighbors);

                    result.sprite = wallCorner;
                    if (n1 == 4 && n2 == 6)
                    {
                        result.rotation = 90f;
                        if (!IsInIntArray(cs, 7)) { result.sprite = wallCornerDiagonal; }
                    }
                    else if (n1 == 1 && n2 == 3)
                    {
                        result.rotation = -90f;
                        if (!IsInIntArray(cs, 0)) { result.sprite = wallCornerDiagonal; }
                    }
                    else if (n1 == 3 && n2 == 6)
                    {
                        result.rotation = 180f;
                        if (!IsInIntArray(cs, 5)) { result.sprite = wallCornerDiagonal; }
                    }
                    else
                    {
                        if (!IsInIntArray(cs, 2)) { result.sprite = wallCornerDiagonal; }
                    }
                }
            }
            else if (numAdjacentNeighbors == 1)
            {
                int[] adjacentNeighbors = GetIntInArray(adjacentIndices, neighbors);
                result.sprite = end; //end

                if (adjacentNeighbors.Length == 1)
                {
                    int neighbor = adjacentNeighbors[0];
                    if (neighbor == 6) { result.rotation = 180f; }
                    else if (neighbor == 3) { result.rotation = -90f; }
                    else if (neighbor == 4) { result.rotation = 90; }
                }
                else { result.sprite = box; }
            }
            else { result.sprite = test; }

            return result;
        }


        private SpriteInfo FiveNeighbors(int[] neighbors)
        {
            SpriteInfo result = new SpriteInfo();
            result.rotation = 0f;

            // Will give constant ordering for following comparisons
            Array.Sort(neighbors);
            int index1 = neighbors[0];
            int index2 = neighbors[1];
            int index3 = neighbors[2];
            int index4 = neighbors[3];
            int index5 = neighbors[4];
            int numAdjacentNeighbors = NumInIntArray(adjacentIndices, neighbors);
            
            if (numAdjacentNeighbors == 4)
            {

                // Means that there is one corner that is the same
                result.sprite = threeCorners;
                int[] c = GetIntInArray(cornerIndices, neighbors);
                int n = c[0];

                if (n == 2) { result.rotation = 90f; }
                else if (n == 5) { result.rotation = -90f; }
                else if (n == 7) { result.rotation = 180f; }
            }
            else if (numAdjacentNeighbors == 3)
            {
                //Get adjacent indices
                int[] adjacentNeighbors = GetIntInArray(adjacentIndices, neighbors);
                Array.Sort(adjacentNeighbors);
                int n1 = adjacentNeighbors[0];
                int n2 = adjacentNeighbors[1];
                int n3 = adjacentNeighbors[2];

                int[] cs = GetMissingInts(neighbors, adjacentNeighbors);

                result.sprite = wall;
                if (n1 == 1 && n2 == 3 && n3 == 4)
                {
                    if (!IsInIntArray(cs, 0) && !IsInIntArray(cs, 2)) { result.sprite = wallBothCorners; }
                    else if (!IsInIntArray(cs, 0)) { result.sprite = wallOneCornerAlt; }
                    else if (!IsInIntArray(cs, 2)) { result.sprite = wallOneCorner; }
                    result.rotation = -90f;
                }
                else if (n1 == 1 && n2 == 3 && n3 == 6)
                {
                    if (!IsInIntArray(cs, 0) && !IsInIntArray(cs, 5)) { result.sprite = wallBothCorners; }
                    else if (!IsInIntArray(cs, 0)) { result.sprite = wallOneCorner; }
                    else if (!IsInIntArray(cs, 5)) { result.sprite = wallOneCornerAlt; }
                    result.rotation = 180f;
                }
                else if (n1 == 3 && n2 == 4 && n3 == 6)
                {
                    if (!IsInIntArray(cs, 5) && !IsInIntArray(cs, 7)) { result.sprite = wallBothCorners; }
                    else if (!IsInIntArray(cs, 5)) { result.sprite = wallOneCorner; }
                    else if (!IsInIntArray(cs, 7)) { result.sprite = wallOneCornerAlt; }
                    result.rotation = 90f;
                }
                else
                {
                    if (!IsInIntArray(cs, 2) && !IsInIntArray(cs, 7)) { result.sprite = wallBothCorners; }
                    else if (!IsInIntArray(cs, 2)) { result.sprite = wallOneCornerAlt; }
                    else if (!IsInIntArray(cs, 7)) { result.sprite = wallOneCorner; }
                }
            }
            else if (numAdjacentNeighbors == 2)
            {
                //Get adjacent indices
                int[] adjacentNeighbors = GetIntInArray(adjacentIndices, neighbors);
                Array.Sort(adjacentNeighbors);
                int n1 = adjacentNeighbors[0];
                int n2 = adjacentNeighbors[1];

                if ((n1 == 1 && n2 == 6) || (n1 == 3 && n2 == 4))
                {
                    result.sprite = alley;
                    if (n1 == 3) { result.rotation = 90f; }
                }
                else
                {
                    int[] cs = GetMissingInts(neighbors, adjacentNeighbors);

                    result.sprite = wallCorner;
                    if (n1 == 4 && n2 == 6)
                    {
                        result.rotation = 90f;
                        if (!IsInIntArray(cs, 7)) { result.sprite = wallCornerDiagonal; }
                    }
                    else if (n1 == 1 && n2 == 3)
                    {
                        result.rotation = -90f;
                        if (!IsInIntArray(cs, 0)) { result.sprite = wallCornerDiagonal; }
                    }
                    else if (n1 == 3 && n2 == 6)
                    {
                        result.rotation = 180f;
                        if (!IsInIntArray(cs, 5)) { result.sprite = wallCornerDiagonal; }
                    }
                    else
                    {
                        if (!IsInIntArray(cs, 2)) { result.sprite = wallCornerDiagonal; }
                    }
                }
            }
            else if (numAdjacentNeighbors == 1)
            {
                int[] adjacentNeighbors = GetIntInArray(adjacentIndices, neighbors);
                result.sprite = end; //end

                if (adjacentNeighbors.Length == 1)
                {
                    int neighbor = adjacentNeighbors[0];
                    if (neighbor == 6) { result.rotation = 180f; }
                    else if (neighbor == 3) { result.rotation = -90f; }
                    else if (neighbor == 4) { result.rotation = 90; }
                }
                else { result.sprite = box; }
            }
            else { result.sprite = test; }



            return result;
        }


        private SpriteInfo SixNeighbors(int[] neighbors)
        {
            SpriteInfo result = new SpriteInfo();
            result.rotation = 0f;

            // Will give constant ordering for following comparisons
            Array.Sort(neighbors);
            int index1 = neighbors[0];
            int index2 = neighbors[1];
            int index3 = neighbors[2];
            int index4 = neighbors[3];
            int index5 = neighbors[4];
            int index6 = neighbors[5];
            int numAdjacentNeighbors = NumInIntArray(adjacentIndices, neighbors);


            if (numAdjacentNeighbors == 4)
            {
                // Means that there are two corners that are the same
                int[] c = GetIntInArray(cornerIndices, neighbors);
                int n1 = c[0];
                int n2 = c[1];

                // Diagonal
                if ((n1 == 0 && n2 == 7) || (n1 == 2 && n2 == 5))
                {
                    result.sprite = oppositeCorners;
                    if (n1 == 2) { result.rotation = 90f; }
                }
                else
                {
                    result.sprite = sameCorners;
                    if (n1 == 5 && n2 == 7) { result.rotation = 180f; }
                    else if (n1 == 2 && n2 == 7) { result.rotation = 90f; }
                    else if (n1 == 0 && n2 == 5) { result.rotation = -90f; }
                }

            }
            else if (numAdjacentNeighbors == 3)
            {
                //Get adjacent indices
                int[] adjacentNeighbors = GetIntInArray(adjacentIndices, neighbors);
                Array.Sort(adjacentNeighbors);
                int n1 = adjacentNeighbors[0];
                int n2 = adjacentNeighbors[1];
                int n3 = adjacentNeighbors[2];

                int[] cs = GetMissingInts(neighbors, adjacentNeighbors);

                result.sprite = wall;
                if (n1 == 1 && n2 == 3 && n3 == 4)
                {
                    if (!IsInIntArray(cs, 0) && !IsInIntArray(cs, 2)) { result.sprite = wallBothCorners; }
                    else if (!IsInIntArray(cs, 0)) { result.sprite = wallOneCornerAlt; }
                    else if (!IsInIntArray(cs, 2)) { result.sprite = wallOneCorner; }
                    result.rotation = -90f;
                }
                else if (n1 == 1 && n2 == 3 && n3 == 6)
                {
                    if (!IsInIntArray(cs, 0) && !IsInIntArray(cs, 5)) { result.sprite = wallBothCorners; }
                    else if (!IsInIntArray(cs, 0)) { result.sprite = wallOneCorner; }
                    else if (!IsInIntArray(cs, 5)) { result.sprite = wallOneCornerAlt; }
                    result.rotation = 180f;
                }
                else if (n1 == 3 && n2 == 4 && n3 == 6)
                {
                    if (!IsInIntArray(cs, 5) && !IsInIntArray(cs, 7)) { result.sprite = wallBothCorners; }
                    else if (!IsInIntArray(cs, 5)) { result.sprite = wallOneCorner; }
                    else if (!IsInIntArray(cs, 7)) { result.sprite = wallOneCornerAlt; }
                    result.rotation = 90f;
                }
                else
                {
                    if (!IsInIntArray(cs, 2) && !IsInIntArray(cs, 7)) { result.sprite = wallBothCorners; }
                    else if (!IsInIntArray(cs, 2)) { result.sprite = wallOneCornerAlt; }
                    else if (!IsInIntArray(cs, 7)) { result.sprite = wallOneCorner; }
                }
            }
            else if (numAdjacentNeighbors == 2)
            {
                //Get adjacent indices
                int[] adjacentNeighbors = GetIntInArray(adjacentIndices, neighbors);
                Array.Sort(adjacentNeighbors);
                int n1 = adjacentNeighbors[0];
                int n2 = adjacentNeighbors[1];

                if ((n1 == 1 && n2 == 6) || (n1 == 3 && n2 == 4))
                {
                    result.sprite = alley;
                    if (n1 == 3) { result.rotation = 90f; }
                }
                else
                {
                    int[] cs = GetMissingInts(neighbors, adjacentNeighbors);

                    result.sprite = wallCorner;
                    if (n1 == 4 && n2 == 6)
                    {
                        result.rotation = 90f;
                        if (!IsInIntArray(cs, 7)) { result.sprite = wallCornerDiagonal; }
                    }
                    else if (n1 == 1 && n2 == 3)
                    {
                        result.rotation = -90f;
                        if (!IsInIntArray(cs, 0)) { result.sprite = wallCornerDiagonal; }
                    }
                    else if (n1 == 3 && n2 == 6)
                    {
                        result.rotation = 180f;
                        if (!IsInIntArray(cs, 5)) { result.sprite = wallCornerDiagonal; }
                    }
                    else
                    {
                        if (!IsInIntArray(cs, 2)) { result.sprite = wallCornerDiagonal; }
                    }
                }
            }
            else if (numAdjacentNeighbors == 1)
            {
                int[] adjacentNeighbors = GetIntInArray(adjacentIndices, neighbors);
                result.sprite = end; //end

                if (adjacentNeighbors.Length == 1)
                {
                    int neighbor = adjacentNeighbors[0];
                    if (neighbor == 6) { result.rotation = 180f; }
                    else if (neighbor == 3) { result.rotation = -90f; }
                    else if (neighbor == 4) { result.rotation = 90; }
                }
                else { result.sprite = box; }
            }
            else { result.sprite = test; }



            return result;
        }


        private SpriteInfo SevenNeighbors(int[] neighbors)
        {
            SpriteInfo result = new SpriteInfo();
            result.rotation = 0f;

            // Will give constant ordering for following comparisons
            Array.Sort(neighbors);

            int[] neighborIndices = {0, 1, 2, 3, 4, 5, 6, 7};
            int n = GetMissingInt(neighborIndices, neighbors);

            if (IsInIntArray(adjacentIndices, n))
            {
                result.sprite = wall;
                if (n == 1) { result.rotation = 90f; }
                else if (n == 4) { result.rotation = 180f; }
                else if (n == 6) { result.rotation = -90f; }
            }
            else
            {
                result.sprite = corner;
                if (n == 0) { result.rotation = 90f; }
                else if (n == 2) { result.rotation = 180f; }
                else if (n == 7) { result.rotation = -90f; }
            }

            return result;
        }


            public void RotateTile(Tilemap tilemap, Vector3Int pos, float rot)
        {
            Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0.0f, 0.0f, rot), Vector3.one);
            tilemap.SetTransformMatrix(pos, m);
        }

        private float GetSpriteRotation(int i)
        {
            // Corners
            if (i == 0) { return 0; }
            if (i == 2) { return 90.0f; }
            if (i == 5) { return 180.0f; }
            if (i == 7) { return 270.0f; }

            // Walls
            if (i == 1) { return 0; }
            if (i == 3) { return 90.0f; }
            if (i == 4) { return 180.0f; }
            if (i == 6) { return 270.0f; }

            return 0.0f;
        }


        private int[] GetSimilarNeighbors(string[] neighbors, string current)
        {
            if (neighbors.Length == 0){
                return null;
            }

            List<int> resultList = new List<int>();

            for (int i = 0; i < neighbors.Length; i++)
            {
                if (neighbors[i] == current)
                {
                    resultList.Add(i);
                }
            }


            if (resultList.Count == 0)
                return null;

            int[] result = new int[resultList.Count];
            for (int i = 0; i < resultList.Count; i++) { result[i] = resultList[i]; }
            return result;
        }


        private bool IsInIntArray(int[] a, int x)
        {
            foreach (int i in a){ if (i == x) { return true; } }
            return false;
        }

        private int NumInIntArray(int[] a, int[] x)
        {
            int result = 0;
            foreach (int xi in x) { if (IsInIntArray(a, xi)) { result++; } }
            return result;
        }


        private int[] GetIntInArray(int[] a, int[] x){

            List<int> temp = new List<int>();

            foreach (int xi in x)
            {
                foreach (int ai in a)
                {
                    if (ai == xi)
                    {
                        temp.Add(xi);
                        break;
                    }
                }
            }

            int[] result = new int[temp.Count];
            for (int i = 0; i < result.Length; i++) { result[i] = temp[i]; }
            return result;

        }


        // Gets first int in a that is not in x
        private int GetMissingInt(int[] a, int[] x)
        {
            int result = 0;
            foreach(int ai in a)
            {
                bool isPresent = IsInIntArray(x, ai);
                if (!isPresent) { result = ai; break; }
            }
            return result;
        }



        // Gets all ints in a that are not in x
        private int[] GetMissingInts(int[] a, int[] x)
        {

            List<int> temp = new List<int>();

            foreach (int ai in a)
            {
                bool isPresent = IsInIntArray(x, ai);
                if (!isPresent) { temp.Add(ai); }
            }

            int[] result = new int[temp.Count];
            for (int i = 0; i < temp.Count; i++) { result[i] = temp[i]; }
            return result;
        }


        // Get neighbors of a space on the tilemap
        private string[] GetNeighbors(string[,] map, int x, int y)
        {
            string[] result = new string[8];

            int i = 0;
            for (int yi = -1; yi < 2; yi++)
            {
                for (int xi = -1; xi < 2; xi++)
                {
                    int xt = x + xi;
                    int yt = y + yi;

                    if(!IsValidLocation(xt, yt, map.GetUpperBound(0), map.GetUpperBound(1)))
                    {
                        result[i] = null;
                        i++;
                    }
                    else if (!(xi == 0 && yi == 0))
                    {
                        result[i] = map[xt, yt];
                        i++;
                    }
                }
            }

            //string test = "";
            //foreach (string s in result)
            //{
            //    test += s + ",";
            //}
            //Debug.Log(test);



            return result;
        }

        private bool IsValidLocation(int x, int y, int w, int h)
        {
            return x >= 0 && x < w && y >= 0 && y < h;
        }
    }



    private Sprite[] resourceSprites;
    private TileMap[] tileMaps;


    public SpriteManager()
    {
        LoadTileMaps();
    }

    // Get the tilemap for the given square
    public TileMap.TileInfo GetTileSprite(Tile tile, string[,] map, Tilemap tm, int x, int y)
    {
        if (x < 0 || y < 0 || x > map.GetUpperBound(0) || y > map.GetUpperBound(1))
        {
            TileMap.TileInfo result = new TileMap.TileInfo();
            result.tile = new Tile();
            result.tile.sprite = tileMaps[3].GetFill();
            return result;
        }

        return GetTileMap(map[x, y]).GetTileSprite(tile, map, tm, x, y);
    }

    private TileMap GetTileMap(string mapName)
    {
        if (mapName == "Grass")
        {
            return tileMaps[0];
        }
        else if (mapName == "Dirt")
        {
            return tileMaps[1];
        }
        else if (mapName == "Water")
        {
            return tileMaps[2];
        }

        return tileMaps[3];
    }


    private void LoadTileMaps()
    {
        // Load Sprites from resources folders
        TextAsset txt = (TextAsset)Resources.Load("TileMaps/loadTileMaps", typeof(TextAsset));
        string[] lines = Regex.Split(txt.text, "\n|\r|\r\n");

        tileMaps = new TileMap[lines.Length+1];
        for (int i = 0; i < lines.Length; i++)
        {
            tileMaps[i] = new TileMap(lines[i]);
        }

        // FOR TESTING!!!
        tileMaps[2] = new TileMap("Water");
        tileMaps[3] = new TileMap("Mountain");
    }
}
