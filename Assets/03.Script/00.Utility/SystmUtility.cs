
using System;
using System.Collections.Generic;

public static class SystmUtility 
{


    public static T RandomEnum<T>()
    {

        Array values = Enum.GetValues(typeof(T));

        return (T)values.GetValue(new Random().Next(0, values.Length));

    }

    /// <summary>
    /// 열거형 타입 중복안되게 랜덤으로 count수만큼 뽑는다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="count"></param>
    /// <returns></returns>
    public static T[] RandomEnumArray<T>(int count)
    {

        Array values = Enum.GetValues(typeof(T));

        if (values.Length < count) return null;     //만약 뽑으려는 수가  더 많다면 

        List<T> reulsts = new List<T>();

        T select;

        do
        {
            select = (T)values.GetValue(new Random().Next(0, values.Length));
            if (reulsts.Contains(select) == false)
            {
                reulsts.Add(select);
            }

        } while (reulsts.Count < count );
        
        

        return null;

    }

}
