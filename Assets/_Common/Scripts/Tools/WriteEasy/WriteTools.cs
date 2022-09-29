///-----------------------------------------------------------------
/// Author : Adrien Lemaire
/// Date : 01/08/2022 21:10
///-----------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Isartdigital.EasterIsland.Common.Tools.WriteEasy {
	public static class WriteTools
	{
        public static void WriteList<T>(this List<T> list)
        {
            string toWrite = list[0].ToString();

            for (int i = 1; i < list.Count; i++)
            {
                toWrite += " : " + list[i];
            }

            Debug.Log(toWrite);
        }

        public static void WriteList<T>(this List<List<T>> list)
        {
            List<T> listList;
            string toWrite;

            for (int i = 0; i < list.Count; i++)
            {
                listList = list[i];

                toWrite = listList[0].ToString();

                for (int j = 1; j < listList.Count; j++)
                {
                    toWrite += " : " + listList[j];
                }

                Debug.Log(toWrite);
            }
        }

        public static void WriteList<T>(this List<List<List<T>>> list)
        {
            List<T> listList;
            string toWrite;

            Debug.Log("///");

            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list[i].Count; j++)
                {
                    listList = list[i][j];

                    toWrite = listList[0].ToString();

                    for (int k = 1; k < listList.Count; k++)
                    {
                        toWrite += " : " + listList[k];
                    }

                    Debug.Log(toWrite);
                }

                Debug.Log("///");
            }
        }
    }
}

