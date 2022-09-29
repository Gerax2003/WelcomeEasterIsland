///-----------------------------------------------------------------
/// Author : Adrien Lemaire
/// Date : #DATE#
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Com.Isartdigital.EasterIsland.Common.Tools {
    public static class UsefullTools
	{
        #region Numbers
        public static bool IsIntPlusHalf(this float number)
            => GetRemainder(number) == 0.5f;

        public static float GetRemainder(this float number, int indexStart = 0)
        {
            string numberString = number.ToString();

            if (!numberString.Contains(",")) return 0;

            return float.Parse("0," + numberString.Substring(numberString.IndexOf(",") + 1 + indexStart));
        }

        public static int BaseNToDecimal(this string baseN, int n)
        {
            int _decimal = Int32.Parse(baseN.Substring(0, 1));

            for (int i = 0; i < baseN.Length - 1; i++)
            {
                _decimal = _decimal * n + Int32.Parse(baseN.Substring(i + 1, 1));
            }

            return _decimal;
        }

        public static string ToBaseN(this int _decimal, int n)
            => _ToBaseN(_decimal, string.Empty, n);

        private static string _ToBaseN(int _decimal, string ternary, int n)
        {
            float division = (float)_decimal / n;
            float remain = division.GetRemainder();

            int euclidianDivision = Mathf.RoundToInt(division - remain);

            ternary = Mathf.RoundToInt(remain * n).ToString() + ternary;

            if (euclidianDivision == 0) return ternary;
            else return _ToBaseN(euclidianDivision, ternary, n);
        }
        #endregion

        #region Vector
        public static Vector2Int GetInverse(this Vector2Int vector)
            => new Vector2Int(vector.y, vector.x);

        public static Vector2 GetInverse(this Vector2 vector)
            => new Vector2(vector.y, vector.x);
        #endregion

        #region List
        public static List<T> AddObjectsToListByPercentage<T>(this List<int> percentages, List<T> objectList, int numberObject)
        {
            if (percentages.Count != objectList.Count) return null;

            List<T> newObjectList = new List<T>();
            float nObjects;
            float currentRemainder;
            float remainderAdditioner = 0;

            for (int i = 0; i < percentages.Count; i++)
            {
                nObjects = numberObject * percentages[i] / 100f;

                currentRemainder = GetRemainder(nObjects);

                remainderAdditioner += currentRemainder;

                nObjects -= currentRemainder;

                if (remainderAdditioner >= 1)
                {
                    remainderAdditioner--;
                    nObjects++;
                }

                for (int j = 0; j < nObjects; j++)
                {
                    newObjectList.Add(objectList[i]);
                }
            }

            return newObjectList;
        }

        public static List<T> RandomiseList<T>(this List<T> objectList)
        {
            List<T> tempList = objectList;
            List<T> newList = new List<T>();
            int random;

            for (int i = tempList.Count - 1; i >= 0; i--)
            {
                random = UnityEngine.Random.Range(0, tempList.Count - 1);
                newList.Add(tempList[random]);
                tempList.RemoveAt(random);
            }

            return newList;
        }

        public static List<T> RandomiseList<T>(this List<T> objectList, int nObjectsInList)
        {
            List<T> tempList = new List<T>();
            List<T> newList = new List<T>();

            int random;

            for (int i = 0; i < nObjectsInList; i++)
            {
                if (tempList.Count == 0)
                {
                    for (int j = 0; j < objectList.Count; j++)
                    {
                        tempList.Add(objectList[j]);
                    }
                }

                random = UnityEngine.Random.Range(0, tempList.Count - 1);
                newList.Add(tempList[random]);
                tempList.RemoveAt(random);
            }

            return newList;
        }

        public static List<T> ToList<T>(this T[] objectList)
        {
            List<T> newObjectList = new List<T>();

            for (int i = 0; i < objectList.Length; i++)
            {
                newObjectList.Add(objectList[i]);
            }

            return newObjectList;
        }

        public static List<T> CopyToList<T>(this List<T> fromList)
        {
            List<T> newObjectList = new List<T>();

            for (int i = 0; i < fromList.Count; i++)
            {
                newObjectList.Add(fromList[i]);
            }

            return newObjectList;
        }

        public static List<List<T>> SortAllPossibilities<T>(List<T> listToSort)
        {
            List<List<T>> allPossibleSortedLists = new List<List<T>>();
            allPossibleSortedLists._SortAllPossibilities(listToSort, new List<T>());
            return allPossibleSortedLists;
        }

        private static void _SortAllPossibilities<T>(this List<List<T>> allPossibleSortedLists, List<T> remainingPossibilities, List<T> possibleSortedList)
        {
            if (remainingPossibilities.Count == 1)
            {
                possibleSortedList.Add(remainingPossibilities[0]);
                allPossibleSortedLists.Add(possibleSortedList);
            }
            else
            {
                List<T> _possibleSortedList;
                List<T> _remainingPossibilities;
                T possibility;

                for (int i = 0; i < remainingPossibilities.Count; i++)
                {
                    possibility = remainingPossibilities[i];

                    _possibleSortedList = possibleSortedList.CopyToList();
                    _possibleSortedList.Add(possibility);

                    _remainingPossibilities = remainingPossibilities.CopyToList();
                    _remainingPossibilities.Remove(possibility);

                    allPossibleSortedLists._SortAllPossibilities(_remainingPossibilities, _possibleSortedList);
                }
            }
        }
        #endregion

        #region Camera
        //public Rect GetCameraGameSpace2D(Camera camera, Vector3 pointPosition)
        //{
        //    Rect gameSpace = new Rect();

        //    gameSpace.x = camera.transform.position.x - pointPosition.x;
        //    gameSpace.y = camera.transform.position.y - pointPosition.y;

        //    gameSpace.height = 2 * camera.orthographicSize;
        //    gameSpace.width = gameSpace.height * camera.aspect;

        //    return gameSpace;
        //}
        #endregion
    }
}

