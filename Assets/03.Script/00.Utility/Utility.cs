using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public static class Utility
{
    public static Vector3 GetRandomPointOnNavMesh(Vector3 center, float distance, int areaMask)
    {
        var randomPos = UnityEngine.Random.insideUnitSphere * distance + center;

        NavMeshHit hit;

        NavMesh.SamplePosition(randomPos, out hit, distance, areaMask);

        return hit.position;
    }

    public static float GetRandomNormalDistribution(float mean, float standard)
    {
        var x1 = UnityEngine.Random.Range(0f, 1f);
        var x2 = UnityEngine.Random.Range(0f, 1f);
        return mean + standard * (Mathf.Sqrt(-2.0f * Mathf.Log(x1)) * Mathf.Sin(2.0f * Mathf.PI * x2));
    }

    public static int GetMaxExp(int nextLevel)
    {
        int maxExp = 0;
        if(nextLevel <6)
        {
            maxExp = nextLevel * 20;
        }
        else if(nextLevel>= 6 && nextLevel < 11)
        {
            maxExp = nextLevel * 30;
        }
        else if(nextLevel>= 12 && nextLevel < 17)
        {
            maxExp = nextLevel * 40;
        }
        else
        {
            maxExp = nextLevel * 50;
        }

        return maxExp;
    }

    public static int GetPlayerCoinCalculate(int rank , int killCount )
    {
        int coin = 0;
        if(rank ==1)
        {
            //1등 
            coin += 30;
            coin += (killCount * 3);

        }

        else if(rank == 2)
        {
            coin += 15;
            coin += (killCount * 3);
        }

        else if(rank == 3)
        {
            coin += 5;
            coin += (killCount * 3);
        }

        else
        {
            coin += 0;
            coin += (killCount * 2);
        }

        return coin;
    }

    public static int GetPlayerExpCalculate(int rank, int killCount)
    {
        int exp = 0;
        if (rank == 1)
        {
            //1등 
            exp += 100;
            exp += (killCount * 10);

        }

        else if (rank == 2)
        {
            exp += 70;
            exp += (killCount * 10);
        }

        else if (rank == 3)
        {
            exp += 50;
            exp += (killCount * 10);
        }

        else
        {
            exp += 20;
            exp += (killCount * 10);
        }

        return exp;
    }
    /// <summary>
    /// 좀비로 선택될 유저의 수 
    /// </summary>
    /// <param name="playerCount"></param>
    /// <returns></returns>
    public static int GetPlayerZombieCount(int playerCount)
    {
        //8 => 2/6
        //7 => 2/5
        //6 => 2/4
        //5 => 1/4
        //4 => 1/3
        //3 => 1/2
        //2 => 1/1
        //1 => 0/1 카운틎증가x
        int result =0;
        switch (playerCount)
        {
            case 1:
                result = 0;
                break;
            case 2:
                result = 1;

                break;
            case 3:
                result = 1;
                break;
            case 4:
                result = 1;
                break;
            case 5:
                result = 2;
                break;
            case 6:
                result = 2;
                break;
            case 7:
                result = 2;
                break;
            case 8:
                result = 2;
                break;

        }
        return result;
    }

    public  static string GetRandomGameScene()
    {
        int ran = UnityEngine.Random.Range(0, 1);
        string result = "Main";

        return result;
    }

    private static IEnumerator CalculateEndGameData()
    {
        yield return new WaitForSeconds(1.0f);
    }


    public static List<T> GetRandomList<T>(List<T> listT, int count )
    {
        var temp = listT;
        
        List<T> result = new List<T>();   //결과값 생성
        for (int i = 0; i < count; i++)
        {
            int ran = UnityEngine.Random.Range(0, temp.Count); //랜덤뽑기 중복X
            result.Add(temp[ran]);
            temp.RemoveAt(ran); //뽑은 카드 리스트에 제거 => 다음 for문에서 cardList에서없게.
        }

        return result;
    }


    public static void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public static class RandomPID
    {
        public static string GetRandomPassword(int _totLen)
        {
            System.Random rand = new System.Random();
            string input = "abcdefghijklmnopqrstuvwxyz0123456789";

            var chars = Enumerable.Range(0, _totLen)
                                          .Select(x => input[rand.Next(0, input.Length)]);
            return new string(chars.ToArray());
        }
    }


    static string GetColorToCode(Color color)
    {
        //var result = "<"+_color.r
        string r = ((int)(color.r * 255)).ToString("X2");
        string g = ((int)(color.g * 255)).ToString("X2");
        string b = ((int)(color.b * 255)).ToString("X2");
        string a = ((int)(color.a * 255)).ToString("X2");
        string result = string.Format("{0}{1}{2}{3}", r, g, b, a);

        return result;
    }

    public static string GetColorContent(Color _color, string _content)
    {
        var colorHexCode = GetColorToCode(_color);

        var result = "<color=#" + colorHexCode + ">" + _content + "</color>\n";


        return result;

    }

}