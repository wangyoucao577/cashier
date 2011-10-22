using System;
using System.Collections.Generic;
using System.Text;

namespace Cashier
{
    class ValueMarked
    {
        /// <summary>
        /// 验证输入字符串是否是数字（包括小数），最长为10个字符
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public static bool CheckMoney(string money)
        {
            bool flag = true;
            int count = 0;
            if (money.Length == 0)
            {
                flag = false;
            }
            else
            {
                char[] x = money.ToCharArray();
                for (int i = 0; i < money.Length; i++)
                {
                    if (!char.IsNumber(x[i]) && x[i] != '.')
                    {
                        flag = false;
                        break;
                    }
                    if (x[i] == '.')
                    {
                        count++;
                        //if (i == 0 || i == money.Length - 1)
                        //{
                        //    flag = false;
                        //}
                    }
                }
                if (count > 1)
                {
                    flag = false;
                }
            }
            return flag;
        }
    }
}
