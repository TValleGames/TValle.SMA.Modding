using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.Memory
{
    /// <summary>
    /// Save and Load data
    /// </summary>
    public interface IContextMemory
    {


        //strings
        void AddData(string id, string data, bool replace = true);
        string FindData(string id, string defaultValue);

        //bool
        void AddData(string id, bool data, bool replace = true);
        bool FindDataBool(string id, bool defaultValue);

        //int
        void AddData(string id, int data, bool replace = true);
        int FindDataInt(string id, int defaultValue);
        void AddData(int id, int data, bool replace = true);
        int FindDataInt(int id, int defaultValue);
        bool TryFindDataInt(string id, out int value);
        bool TryFindDataInt(int id, out int value);

        //float
        void AddData(string id, float data, bool replace = true);
        void AddData(int id, float data, bool replace = true);
        float FindDataFloat(string id, float defaultValue);
        float FindDataFloat(int id, float defaultValue);
        bool TryFindDataFloat(int id, out float value);
        bool TryFindDataFloat(string id, out float value);

        //arrays
        void AddData<T>(string id, List<T> data, bool replace = true);
        bool TryFindDataArrayEmpty<T>(string id, out List<T> value);
        bool TryFindDataArrayNull<T>(string id, out List<T> value);
        void AddData<T>(string id, T[] data, bool replace = true);
        bool TryFindDataArrayEmpty<T>(string id, out T[] value);
        bool TryFindDataArrayNull<T>(string id, out T[] value);

        //images
        void AddData(string id, Texture2D data, bool replace = true);
        Texture2D FindDataImage(string id);
        bool TryFindDataImage(string id, ref Texture2D result);

        //objects
        void AddDataObject<TKey, TValue>(TKey id, TValue data, bool replace = true);
        bool TryFindDataObject<TKey, TValue>(string id, out TKey key, out TValue value, TKey defaultKey, TValue defaultValue);

        void AddDataObject<T>(string id, T data, bool replace = true);
        bool TryFindDataObject<T>(string id, out T value, T defaultValue);
        bool TryFindDataObject<T>(string id, Type type, out T value, T defaultValue) where T : class;
        bool TryFindDataObject(string id, Type type, out object value, object defaultValue);




        bool RemoveData(string id);
    }
}
