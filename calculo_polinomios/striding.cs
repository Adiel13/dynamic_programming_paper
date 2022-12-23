using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace calculo_polinomios
{
    internal class striding
    {
        private int grado;
        public int[,] tableStriding;

        public striding(int grado)
        {
            this.grado = grado;
            this.tableStriding = new int[2, this.grado];
        }

        public void fillStriding()
        {
            this.tableStriding[0, 0] = 2;
            this.tableStriding[1, 0] = 6;

            this.tableStriding[0, 1] = 3;
            this.tableStriding[1, 1] = 6;

            this.tableStriding[0, 2] = 4;
            this.tableStriding[1, 2] = 21;

            this.tableStriding[0, 3] = 5;
            this.tableStriding[1, 3] = 36;

            int temp = 0;

            for (int i = 4; i < this.grado; i += 3)
            {
                if (i + 2 < grado)
                {
                    this.tableStriding[0, i] = i + 2;
                    this.tableStriding[1, i] = this.tableStriding[1, temp] * 6;

                    this.tableStriding[0, i + 1] = i + 3;
                    this.tableStriding[1, i + 1] = this.tableStriding[1, temp] * 3 + this.tableStriding[1, temp + 1] * 3;

                    this.tableStriding[0, i + 2] = i + 4;
                    this.tableStriding[1, i + 2] = this.tableStriding[1, temp] + this.tableStriding[1, temp + 1] * 5;

                    temp++;
                }

            }
        }

        public void printTableStriding()
        {
            for (int i = 0; i < this.grado; i++)
            {
                Console.WriteLine(this.tableStriding[1, i]);
            }
        }

        public int[,] TableStriding()
        {
            return this.tableStriding;
        }

        public int getGrado()
        {
            return this.grado;
        }

        public void generateCode()
        {
            String code = "#include <stdio.h>\r\n#include <stdlib.h>\r\n#include <arm_neon.h>" +
                "\n\nvoid clean(uint16_t arr[], int limit){for(int i=0; i<limit; i++){arr[i] = 0;}}";
            for (int i = 2; i <= this.grado; i++)
            {
                code += this.codeByGrade(i);
            }

            try
            {
                StreamWriter sw = new StreamWriter("striding.c");
                sw.WriteLine(code);
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }

        public String codeByGrade(int grado)
        {
            String code = "";
            switch (grado)
            {
                case 2:
                    code += "\nvoid striding_2(uint16_t arr1[], uint16_t arr2[], uint16_t res[]){\r\n    uint16_t a0b0 = arr1[0]*arr2[0];\r\n    uint16_t a1b1 = arr1[1]*arr2[1];\r\n    uint16_t a2b2 = 0;\r\n    uint16_t d01 = (arr1[0]+arr1[1])*(arr2[0]+arr2[1]);\r\n    uint16_t d02 = (arr1[0]+0)*(arr2[0]+0);\r\n    uint16_t d12 = (arr1[1]+0)*(arr2[1]+0);\r\n\r\n   \tres[0] = a0b0;\r\n    res[1] = d01-a1b1-a0b0;\r\n    res[2] = d02-a2b2-a0b0+a1b1;\r\n\tres[3] = d12-a1b1-a2b2;\r\n    res[4] = a2b2;\r\n}";
                    break;
                case 3:
                    code += "\nvoid striding_3(uint16_t arr1[], uint16_t arr2[], uint16_t res[]){\r\n    uint16_t a0b0 = arr1[0]*arr2[0];\r\n    uint16_t a1b1 = arr1[1]*arr2[1];\r\n    uint16_t a2b2 = arr1[2]*arr2[2];\r\n    uint16_t d01 = (arr1[0]+arr1[1])*(arr2[0]+arr2[1]);\r\n    uint16_t d02 = (arr1[0]+arr1[2])*(arr2[0]+arr2[2]);\r\n    uint16_t d12 = (arr1[1]+arr1[2])*(arr2[1]+arr2[2]);\r\n\r\n   \tres[0] = a0b0;\r\n    res[1] = d01-a1b1-a0b0;\r\n    res[2] = d02-a2b2-a0b0+a1b1;\r\n\tres[3] = d12-a1b1-a2b2;\r\n    res[4] = a2b2;\r\n}";
                    break;
                case 5:
                    break;
                default:
                    int x1=0, x2=0, x3 = 0;
                    int turno = 1;
                    for(int i =1; i <= grado; i++)
                    {
                        switch (turno)
                        {
                            case 1:
                                ++x1;
                                turno = 2;
                                break;
                            case 2:
                                ++x2;
                                turno = 3;
                                break;
                            case 3:
                                ++x3;
                                turno = 1;
                                break;
                        }

                    }
                    code += "\nvoid striding_" + grado + "(uint16_t arr1[], uint16_t arr2[], uint16_t res[]){";

                    code += "\n\tuint16_t a0 = {";
                    for(int i = 0; i< x1; i++)
                    {
                        code += "arr1["+i+"]";
                        if(i+1 != x1)
                        {
                            code += ",";
                        }
                    }
                    code += "};";

                    code += "\n\tuint16_t b0 = {";
                    for (int i = 0; i < x1; i++)
                    {
                        code += "arr2[" + i + "]";
                        if (i + 1 != x1)
                        {
                            code += ",";
                        }
                    }
                    code += "};";

                    code += "\n\tuint16_t a1 = {";
                    for (int i = 0; i < x2; i++)
                    {
                        code += "arr1[" + (i+x1) + "]";
                        if (i + 1 != x2)
                        {
                            code += ",";
                        }
                    }
                    code += "};";

                    code += "\n\tuint16_t b1 = {";
                    for (int i = 0; i < x2; i++)
                    {
                        code += "arr2[" + (i + x1) + "]";
                        if (i + 1 != x2)
                        {
                            code += ",";
                        }
                    }
                    code += "};";


                    code += "\n\tuint16_t a2 = {";
                    for (int i = 0; i < x3; i++)
                    {
                        code += "arr1[" + (i + x1+x2) + "]";
                        if (i + 1 != x3)
                        {
                            code += ",";
                        }
                    }
                    code += "};";

                    code += "\n\tuint16_t b2 = {";
                    for (int i = 0; i < x3; i++)
                    {
                        code += "arr2[" + (i + x1+x2) + "]";
                        if (i + 1 != x3)
                        {
                            code += ",";
                        }
                    }
                    code += "};";

                    code += "\n\tuint16_t d01_a = {";

                    for (int i = 0; i < x1; i++)
                    {
                        if ((i + 1) <= x2)
                        {
                            code += "a0[" + i + "]+a1[" + i+"]";
                        }
                        else
                        {
                            code += "a0[" + i + "]";
                        }
                        if(i + 1 < x1)
                        {
                            code += ",";
                        }
                    }
                    code += "};";


                    code += "\n\tuint16_t d01_b = {";

                    for (int i = 0; i < x1; i++)
                    {
                        if ((i + 1) <= x2)
                        {
                            code += "b0[" + i + "]+b1[" + i + "]";
                        }
                        else
                        {
                            code += "b0[" + i + "]";
                        }
                        if (i + 1 < x1)
                        {
                            code += ",";
                        }
                    }
                    code += "};";


                    code += "\n\tuint16_t d02_a = {";

                    for (int i = 0; i < x1; i++)
                    {
                        if ((i + 1) <= x3)
                        {
                            code += "a0[" + i + "]+a2[" + i + "]";
                        }
                        else
                        {
                            code += "a0[" + i + "]";
                        }
                        if (i + 1 < x1)
                        {
                            code += ",";
                        }
                    }
                    code += "};";


                    code += "\n\tuint16_t d02_b = {";

                    for (int i = 0; i < x1; i++)
                    {
                        if ((i + 1) <= x3)
                        {
                            code += "b0[" + i + "]+b2[" + i + "]";
                        }
                        else
                        {
                            code += "b0[" + i + "]";
                        }
                        if (i + 1 < x1)
                        {
                            code += ",";
                        }
                    }
                    code += "};";

                    code += "\n\tuint16_t d12_a = {";

                    for (int i = 0; i < x2; i++)
                    {
                        if ((i + 1) <= x3)
                        {
                            code += "a1[" + i + "]+a2[" + i + "]";
                        }
                        else
                        {
                            code += "a1[" + i + "]";
                        }
                        if (i + 1 < x2)
                        {
                            code += ",";
                        }
                    }
                    code += "};";


                    code += "\n\tuint16_t d12_b = {";

                    for (int i = 0; i < x2; i++)
                    {
                        if ((i + 1) <= x3)
                        {
                            code += "b1[" + i + "]+b2[" + i + "]";
                        }
                        else
                        {
                            code += "b1[" + i + "]";
                        }
                        if (i + 1 < x2)
                        {
                            code += ",";
                        }
                    }
                    code += "};";

                    code += "\n\n\tstriding_" + x1 + "(d01_a, d01_b, d01);";
                    code += "\n\tstriding_" + x1 + "(d02_a, d02_b, d02);";
                    code += "\n\tstriding_" + x2 + "(d12_a, d12_b, d12);";

                    code += "\n\n\tstriding_"+x1+ "(a0, b0, a0b0);";
                    code += "\n\tstriding_" + x2 + "(a1, b1, a1b1);";
                    code += "\n\tstriding_" + x3 + "(a2, b2, a2b2);";

                    code += "\n\n\tuint16_t centro1 = {";

                    for (int i = 0; i < x1*2 -1; i++)
                    {
                        if(i < x2 *2)
                        {
                            code += "d01[" + i + "]-a1b1[" + i + "]-a0b0[" + i + "]";
                        }
                        else
                        {
                            code += "d01[" + i + "]-a0b0[" + i + "]";
                        }
                        if (i+1 < x1 * 2-1)
                        {
                            code += ",";
                        }
                    }
                    code += "};";

                    code += "\n\tuint16_t centro2 = {";

                    for (int i = 0; i < x1 * 2 - 1; i++)
                    {
                        if (i < x3 * 2)
                        {
                            code += "d02[" + i + "]-a2b2[" + i + "]-a0b0[" + i + "]";
                            if(i < x2 * 2)
                            {
                                code += "+a1b1["+i+"]";
                            }
                        }
                        else
                        {
                            code += "d02[" + i + "]-a0b0[" + i + "]";
                            if (i  < x2 * 2)
                            {
                                code += "+a1b1[" + i+"]";
                            }
                        }
                        if (i + 1 < x1 * 2 - 1)
                        {
                            code += ",";
                        }
                    }
                    code += "};";

                    code += "\n\tuint16_t centro3 = {";

                    for (int i = 0; i < x2 * 2 - 1; i++)
                    {
                        if (i < x3 * 2)
                        {
                            code += "d12[" + i + "]-a1b1[" + i + "]-a2b2[" + i + "]";
                        }
                        else
                        {
                            code += "d12[" + i + "]-a1b1[" + i + "]";

                        }
                        if (i + 1 < x2 * 2 - 1)
                        {
                            code += ",";
                        }
                    }
                    code += "};";

                    int grado_tot = grado + grado - 1;
                    int c1 = x1, c2 = x1, c3 = x1, c4 = x2, c5 = x3;
                    for(int i = 0; i < grado_tot; i++)
                    {
                        if(i < c1 * 2 -1 && i < (c1*2) /2)
                        {
                            code += "\n\tres[" + i + "] = a0b0[" + i + "];";
                        }
                        else if (i < c1 * 2 - 1 && i >= (c1 * 2) / 2)
                        {
                            code += "\n\tres[" + i + "] = a0b0[" + i + "]+centro1["+(c1-i)*-1+"];";
                        }else if(i == c1*2 - 1)
                        {
                            code += "\n\tres[" + i + "] =centro1[" + (c1 - i) * -1 + "];";
                        }else if (i < (c1 * 2 - 1)+(c2*2-1) && i <= (c1 * 2 - 1) + (c2 * 2 - 1)/2)
                        {
                            code += "\n\tres[" + i + "] =centro1[" + (c1 - i) * -1 + "]+centro2[" + (c2+c1 - i) * -1 + "];";
                        }else if (i == (c1 * 2 - 1)+(c2 * 2)/2)
                        {
                            code += "\n\tres[" + i + "] =centro2[" + (c2 + c1 - i) * -1 + "];";
                        }else if (i < (c1 * 2 - 1) + (c2 * 2 - 1) + (c3 * 2 - 1) && i <= (c1 * 2 - 1) + (c2 * 2 - 1)/2 + (c3 * 2 - 1) / 2)
                        {
                            code += "\n\tres[" + i + "] =centro2[" + (c2 + c1 - i) * -1 + "]+centro3[" + (c3+c2 + c1 - i) * -1 + "];";
                        }
                        else if (c3 != c4 && i < (c1 * 2 - 1) + (c2 * 2 - 1) + (c3 * 2 - 1) && i <= (c1 * 2 - 1) + (c2 * 2 - 1) / 2 + (c3 * 2 - 1) / 2 + 1)
                        {
                            code += "\n\tres[" + i + "] =centro2[" + (c2 + c1 - i) * -1 + "]+centro3[" + (c3 + c2 + c1 - i) * -1 + "]+a2b2[0];";

                        }
                        else if(c3 == c4 && i < (c1 * 2 - 1) + (c2 * 2 - 1) + (c3 * 2 - 1) && i <= (c1 * 2 - 1) + (c2 * 2 - 1) / 2 + (c3 * 2 - 1) / 2  +1)
                        {
                            code += "\n\tres[" + i + "] =centro2[" + (c2 + c1 - i) * -1 + "]+centro3[" + (c3 + c2 + c1 - i) * -1 + "];";
                            ++i;
                            code += "\n\tres[" + i + "] =centro3[" + (c3 + c2 + c1 - i) * -1 + "];";
                        }
                        else if(i < (c1 * 2 - 1) + (c2 * 2 - 1) + (c3 * 2 - 1) + (c4 * 2 - 1) && i <= (c1 * 2 - 1) + (c2 * 2 - 1) / 2 + (c3 * 2 - 1) / 2 + (c4 * 2 - 1) / 2) 
                        {
                           code += "\n\tres[" + i + "] =centro3[" + (c3+c2 + c1 - i) * -1 + "]+a2b2[" + (c4+c3 + c2 + c1 - i) * -1 + "];";

                        }

                    }

                    code += "\n}";
                    break;
            }
            return code;
        }
    }
}
