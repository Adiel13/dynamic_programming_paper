using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calculo_polinomios
{
    internal class hibrido
    {
        private int grado;
        private int[,] tableHibrido;
        private karatsuba k;
        private striding s;
        public hibrido(int grado)
        {
            this.grado = grado;
            this.tableHibrido = new int[2, this.grado];
            this.k = new karatsuba(this.grado);
            k.fillKaratsuba();
            this.s = new striding(this.grado);
            s.fillStriding();
        }

        public void fillHibrido()
        {
            this.tableHibrido[0, 0] = 2;
            this.tableHibrido[1, 0] = 3;

            this.tableHibrido[0, 1] = 3;
            this.tableHibrido[1, 1] = 6;

            this.tableHibrido[0, 2] = 4;
            this.tableHibrido[1, 2] = 9;

            this.tableHibrido[0, 3] = 5;
            this.tableHibrido[1, 3] = 15;

            this.tableHibrido[0, 4] = 6;
            this.tableHibrido[1, 4] = 18;

            this.tableHibrido[0, 5] = 7;
            this.tableHibrido[1, 5] = 22;

            this.tableHibrido[0, 6] = 8;
            this.tableHibrido[1, 6] = 27;

            this.tableHibrido[0, 7] = 9;
            this.tableHibrido[1, 7] = 36;

            int temp = 3;

            for (int i = 8; i < this.grado; i += 2)
            {
                if (i + 1 < grado)
                {
                    this.tableHibrido[0, i] = i + 2;
                    int peso = 0;
                    if (tableHibrido[1, temp] <= s.tableStriding[1, temp] && tableHibrido[1, temp] <= k.tableKaratsuba[1, temp])
                    {
                        peso = tableHibrido[1, temp];
                    }
                    else if (s.tableStriding[1, temp] <= k.tableKaratsuba[1, temp])
                    {
                        peso = s.tableStriding[1, temp];

                    }
                    else
                    {
                        peso = k.tableKaratsuba[1, temp];

                    }

                    this.tableHibrido[1, i] = peso * 3;

                    this.tableHibrido[0, i+1] = i + 3;

                    temp++;
                    int peso_aux = peso;
                    if (tableHibrido[1, temp] <= s.tableStriding[1, temp] && tableHibrido[1, temp] <= k.tableKaratsuba[1, temp])
                    {
                        peso = tableHibrido[1, temp];
                    }
                    else if (s.tableStriding[1, temp] <= k.tableKaratsuba[1, temp])
                    {
                        peso = s.tableStriding[1, temp];

                    }
                    else
                    {
                        peso = k.tableKaratsuba[1, temp];

                    }

                    this.tableHibrido[1, i+1] = peso_aux + peso * 2;
                }
            }            
        }

        public void printTableHibrido()
        {
            for (int i = 0; i < this.grado; i++)
            {
                Console.WriteLine(this.tableHibrido[1, i]);
            }
        }

        public int[,] TableHibrido()
        {
            return this.tableHibrido;
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
                StreamWriter sw = new StreamWriter("hibrido.c");
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
                case 4:
                case 8:
                case 16:
                case 32:
                case 64:
                case 128:
                case 256:
                case 512:
                case 1024:
                case 2048:
                    code += k.codeByGrade(grado);
                    break;
                case 3:
                    code += "void striding_3(uint16_t arr1[], uint16_t arr2[], uint16_t res[]){\r\n    uint16_t a0b0 = arr1[0]*arr2[0];\r\n    uint16_t a1b1 = arr1[1]*arr2[1];\r\n    uint16_t a2b2 = arr1[2]*arr2[2];\r\n    uint16_t d01 = (arr1[0]+arr1[1])*(arr2[0]+arr2[1]);\r\n    uint16_t d02 = (arr1[0]+arr1[2])*(arr2[0]+arr2[2]);\r\n    uint16_t d12 = (arr1[1]+arr1[2])*(arr2[1]+arr2[2]);\r\n\r\n   \tres[0] = a0b0;\r\n    res[1] = d01-a1b1-a0b0;\r\n    res[2] = d02-a2b2-a0b0+a1b1;\r\n\tres[3] = d12-a1b1-a2b2;\r\n    res[4] = a2b2;\r\n}";
                    break;
                case 9:
                    break;
                default:

                    int ktemp1 = 0;
                    int ktemp2 = 0;
                    if(grado % 2 == 0)
                    {
                        ktemp1 = grado / 2;
                        ktemp2 = grado / 2;
                    }
                    else
                    {
                        ktemp1 = ((grado + 1) / 2)-1;
                        ktemp2 = (grado + 1) / 2;
                    }

                    String name1 = "";
                    switch (ktemp1) {
                        case 3:
                        case 9:
                            name1 = "striding";
                            break;
                        case 2:
                        case 4:
                        case 8:
                        case 16:
                        case 32:
                        case 64:
                        case 128:
                        case 256:
                        case 512:
                        case 1024:
                        case 2048:
                            name1 = "karatsuba";
                            break;
                        default:
                            name1 = "hibrido";
                            break;
                    }

                    String name2 = "";
                    switch (ktemp2)
                    {
                        case 3:
                        case 9:
                            name2 = "striding";
                            break;
                        case 2:
                        case 4:
                        case 8:
                        case 16:
                        case 32:
                        case 64:
                        case 128:
                        case 256:
                        case 512:
                        case 1024:
                        case 2048:
                            name2 = "karatsuba";
                            break;
                        default:
                            name2 = "hibrido";
                            break;
                    }
                    code += "void hibrido_" + grado + "(uint16_t arr1[], uint16_t arr2[], uint16_t res[]){\r\n    " +
                                "\r\tuint16_t a0[] ={";


                    for (int i = 0; i < ktemp1; i++)
                    {
                        if (i + 1 == ktemp1)
                        {
                            code += "arr1[" + i + "]};";
                        }
                        else
                        {
                            code += "arr1[" + i + "],";
                        }
                    }
                    code += "\r\n    uint16_t a1[] ={";

                    for (int i = 0; i < ktemp2; i++)
                    {
                        if (i + 1 == ktemp2 && i + ktemp1 < grado)
                        {
                            code += "arr1[" + (i + ktemp1) + "]};";
                        }
                        else if (i + 1 == ktemp2 && i + ktemp1 == grado)
                        {
                            code += "0};";
                        }
                        else
                        {
    
                            code += "arr1[" + (i + ktemp1) + "],";
                        }
                    }


                    code += "\r\n\r\n    uint16_t b0[] ={";

                    for (int i = 0; i < ktemp1; i++)
                    {
                        if (i + 1 == ktemp1)
                        {
                            code += "arr1[" + i + "]};";
                        }
                        else
                        {
                            code += "arr1[" + i + "],";
                        }
                    }
                    code += "\r\n    uint16_t b1[] ={";

                    for (int i = 0; i < ktemp2; i++)
                    {
                        if (i + 1 == ktemp2 && i + ktemp1 < grado)
                        {
                            code += "arr1[" + (i + ktemp1) + "]};";
                        }
                        else if (i + 1 == ktemp2 && i + ktemp1 == grado)
                        {
                            code += "0};";
                        }
                        else
                        {
                            code += "arr1[" + (i + ktemp1) + "],";
                        }
                    }

                    code += "\r\n\tuint16_t a0b0[" + (ktemp1 * 2 - 1) + "];";
                    code += "\r\n\tuint16_t a1b1[" + (ktemp2 * 2 - 1) + "];";
                    code += "\r\n\tclean(a0b0, " + (ktemp1 * 2 - 1) + ");";
                    code += "\r\n\tclean(a1b1, " + (ktemp2 * 2 - 1) + ");";

                    code += "\r\n\t\r\n\t"+name1 + "_" + ktemp1 + "(a0, b0, a0b0);" +
                                "\r\n\t" +name2+"_"+ ktemp2 + "(a1, b1, a1b1);";


                    code += "\r\n\r\n\tuint16_t a0_plus_a1[] ={";

                    int index = 0;
                    for (int i = 0; i <= ktemp2; i++)
                    {
                        if (index < ktemp1)
                        {
                            if (i + 1 == ktemp2)
                            {
                                if (grado < ktemp1 + ktemp2)
                                {
                                    code += "arr1[" + index + "]+0};";

                                }
                                else
                                {
                                    code += "arr1[" + index + "]+arr1[" + (i + ktemp1) + "]};";
                                }
                            }
                            else
                            {
                                code += "arr1[" + index + "]+arr1[" + (i + ktemp1) + "],";
                            }
                        }
                        else if (i + 1 <= ktemp2 && index >= ktemp1)
                        {
                            String fin = "";
                            String aux = "";
                            if (i + 1 == ktemp2)
                            {
                                fin = "};";
                                if (grado < ktemp1 + ktemp2)
                                {
                                    aux = "0";
                                }
                                else
                                {
                                    aux = "arr1[" + (i + ktemp1) + "]";
                                }
                            }
                            else
                            {
                                aux = "arr1[" + (i + ktemp1) + "]";
                                fin = ",";
                            }

                            code += aux + fin;

                        }
                        index++;

                    }
                    int j = ktemp2;


                    code += "\r\n\tuint16_t b0_plus_b1[] ={";

                    index = 0;
                    for (int i = 0; i <= ktemp2; i++)
                    {
                        if (index < ktemp1)
                        {
                            if (i + 1 == ktemp2)
                            {
                                if (grado < ktemp1 + ktemp2)
                                {
                                    code += "arr2[" + index + "]+0};";

                                }
                                else
                                {
                                    code += "arr2[" + index + "]+arr2[" + (i + ktemp1) + "]};";
                                }
                            }
                            else
                            {
                                code += "arr2[" + index + "]+arr2[" + (i + ktemp1) + "],";
                            }
                        }
                        else if (i + 1 <= ktemp2 && index >= ktemp1)
                        {
                            String fin = "";
                            String aux = "";
                            if (i + 1 == ktemp2)
                            {
                                fin = "};";
                                if (grado < ktemp1 + ktemp2)
                                {
                                    aux = "0";
                                }
                                else
                                {
                                    aux = "arr2[" + (i + ktemp1) + "]";
                                }
                            }
                            else
                            {
                                aux = "arr2[" + (i + ktemp1) + "]";
                                fin = ",";
                            }

                            code += aux + fin;

                        }
                        index++;

                    }

                    code += "\r\n\tuint16_t aux[" + (ktemp2 * 2 - 1) + "];";
                    code += "\r\n\tclean(aux, " + (ktemp2 * 2 - 1) + ");";

                    code += "\r\n\r\n\t" + name2 + "_"+ktemp2 + "(a0_plus_a1, b0_plus_b1, aux);";

                    code += "\r\n\tuint16_t centro[] = {";

                    j = 0;
                    int totaux = (ktemp1 * 2 - 1);
                    for (int i = 0; i < (ktemp2 * 2 - 1); i++)
                    {
                        if (i >= totaux)
                        {
                            code += "aux[" + i + "]-a1b1[" + i + "]";
                        }
                        else
                        {
                            code += "aux[" + i + "]-a1b1[" + i + "]-a0b0[" + j + "]";
                            j++;
                        }
                        if ((i + 1) == (ktemp2 * 2) - 1)
                        {
                            code += "};\n";
                        }
                        else
                        {
                            code += ",";
                        }
                    }

                    int res = 0;

                    if (grado % 2 == 0)
                    {
                        res = grado * 2 - 1;
                    }
                    else
                    {
                        res = (grado + 1) * 2 - 1;

                    }

                    int p = 0;
                    int t1 = ktemp1 * 2 - 1;
                    int x = 0;
                    code += "\n\t";
                    for (int i = 0; i < t1; i++)
                    {
                        if (i > (ktemp1 - 1))
                        {
                            code += "res[" + p + "]=a0b0[" + i + "]+centro[" + x + "];";
                            x++;
                        }
                        else
                        {
                            code += "res[" + p + "]=a0b0[" + i + "];";
                        }
                        p++;
                    }
                    code += "res[" + p + "]=centro[" + x + "];";
                    p++;
                    x++;

                    int t3 = ktemp2 * 2 - 1;

                    for (int i = 0; i < t3; i++)
                    {
                        if (x >= t3)
                        {
                            code += "res[" + p + "]=a1b1[" + i + "];";
                        }
                        else
                        {
                            code += "res[" + p + "]=centro[" + x + "]+a1b1[" + i + "];";
                            x++;
                        }
                        p++;
                    }
                    code += "\n}";

                    break;
            }

            return code;
        }
    }
}
