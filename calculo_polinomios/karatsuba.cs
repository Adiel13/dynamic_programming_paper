using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Globalization;
using System.Runtime.Intrinsics.X86;

namespace calculo_polinomios
{
    internal class karatsuba
    {
        private int grado;
        public int[,] tableKaratsuba;

        public karatsuba(int grado) { 
            this.grado = grado;
            this.tableKaratsuba = new int[2, this.grado];
        }

        public void fillKaratsuba()
        {
            this.tableKaratsuba[0,0]= 2;
            this.tableKaratsuba[1, 0] = 3;

            this.tableKaratsuba[0, 1] = 3;
            this.tableKaratsuba[1, 1] = 9;

            this.tableKaratsuba[0, 2] = 4;
            this.tableKaratsuba[1, 2] = 9;
            int temp = 0;

            for (int i = 3; i < this.grado; i+=4) {
                if (i + 3 < grado)
                {
                    this.tableKaratsuba[0, i] = i + 2;
                    this.tableKaratsuba[1, i] = this.tableKaratsuba[1, temp] + this.tableKaratsuba[1, temp + 2] * 2;
                    this.tableKaratsuba[0, i + 1] = i + 3;
                    this.tableKaratsuba[1, i + 1] = this.tableKaratsuba[1, temp] + this.tableKaratsuba[1, temp + 2] * 2;

                    temp += 2;
                    this.tableKaratsuba[0, i + 2] = i + 4;
                    this.tableKaratsuba[1, i + 2] = this.tableKaratsuba[1, temp] * 3;
                    this.tableKaratsuba[0, i + 3] = i + 5;
                    this.tableKaratsuba[1, i + 3] = this.tableKaratsuba[1, temp] * 3;
                }

            }
        }

        public void printTableKaratsuba()
        {
            for (int i = 0; i < this.grado; i++) {
                Console.WriteLine(this.tableKaratsuba[0,i] + ", " + this.tableKaratsuba[1,i]);
            }
        }

        public int[,] TableKaratsuba()
        {
            return this.tableKaratsuba;
        }

        public int getGrado()
        {
            return grado;
        }

        public void generateCode() {
            String code = "void clean(uint16_t arr[], int limit){for(int i=0; i<limit; i++){arr[i] = 0;}}";
            for (int i = 2; i <= this.grado; i ++)
            {
                code += this.codeByGrade(i);
            }

            try
            {
                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter("karatsuba.c");
                //Write a line of text
                sw.WriteLine(code);
                //Close the file
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
            //Console.WriteLine(code);
        }
        public String switchcode(int grado)
        {
            String code = "switch(x){\n";
            for(int i =2; i<=grado; i++)
            {
                code += "case " + i + ": karatsuba_"+i+"(arr1, arr2, res); break;\n";
            }
            code  += "}\n";

            return code;
        }

        public String codeByGrade(int grado)
        {
            String code = "";
            switch (grado) {
                case 2:
                    code += "\n\nvoid karatsuba_2(uint16_t arr1[], uint16_t arr2[], uint16_t res[]){\n" +
                            "    res[0] = arr1[0]*arr2[0];\n" +
                            "    res[2] = arr1[1]*arr2[1];\n" +
                            "    res[1] = (arr1[0]+arr1[1])*(arr2[0]+arr2[1])-res[0]-res[2];\n";
                    break;
                case 3:
                    code += "void karatsuba_3(uint16_t arr1[], uint16_t arr2[], uint16_t res[]){\r\n    " +
                                "\r\tuint16_t a0[] = {arr1[0],arr1[1]};" +
                                "\r\n\tuint16_t a1[] = {arr1[2],0};" +
                                "\r\n\r\n    uint16_t b0[] = {arr2[0],arr2[1]};" +
                                "\r\n\tuint16_t b1[] = {arr2[2], 0};" +
                                "\r\n\tuint16_t a0b0[3];" +
                                "\r\n\tclean(a0b0, 3);" +
                                "\r\n\t\r\n\tkaratsuba_2(a0, b0, a0b0);" +
                                "\r\n\tuint16_t a1b1[3];" +
                                "\r\n\tclean(a1b1, 3);" +
                                "\r\n\tkaratsuba_2(a1, b1, a1b1);" +
                                "\r\n\r\n\tuint16_t a0_plus_a1[] ={arr1[0]+arr1[2], arr1[1]+arr1[3]};" +
                                "\r\n\tuint16_t b0_plus_b1[] ={arr2[0]+arr2[2], arr2[1]+arr2[3]};" +
                                "\r\n\tuint16_t aux[3];" +
                                "\r\n\tclean(aux, 3);" +
                                "\r\n\r\n\tkaratsuba_2(a0_plus_a1, b0_plus_b1, aux);" +
                                "\r\n\tuint16_t centro[] = {aux[0]-a1b1[0]-a0b0[0], aux[1]-a1b1[1]-a0b0[1], aux[2]-a1b1[2]-a0b0[2]};" +
                                "\r\n\r\n\tres[0]=a0b0[0];" +
                                "\r\n\tres[1]=a0b0[1];" +
                                "\r\n\tres[2]=a0b0[2]+centro[0];" +
                                "\r\n\tres[3]=centro[1];" +
                                "\r\n\tres[4]=centro[2] + a1b1[0];" +
                                "\r\n\tres[5]=a1b1[1];" +
                                "\r\n\tres[6]=a1b1[2];";
                    break;
                case 4:
                    code += "void karatsuba_4(uint16_t arr1[], uint16_t arr2[], uint16_t res[]){\r\n    " +
                                "\r\tuint16_t a0[] = {arr1[0],arr1[1]};" +
                                "\r\n\tuint16_t a1[] = {arr1[2],arr1[3]};" +
                                "\r\n\r\n    uint16_t b0[] = {arr2[0],arr2[1]};" +
                                "\r\n\tuint16_t b1[] = {arr2[2], arr2[3]};" +
                                "\r\n\tuint16_t a0b0[3];" +
                                "\r\n\tclean(a0b0, 3);" +
                                "\r\n\t\r\n\tkaratsuba_2(a0, b0, a0b0);" +
                                "\r\n\tuint16_t a1b1[3];" +
                                "\r\n\tclean(a1b1, 3);" +
                                "\r\n\tkaratsuba_2(a1, b1, a1b1);" +
                                "\r\n\r\n\tuint16_t a0_plus_a1[] ={arr1[0]+arr1[2], arr1[1]+arr1[3]};" +
                                "\r\n\tuint16_t b0_plus_b1[] ={arr2[0]+arr2[2], arr2[1]+arr2[3]};" +
                                "\r\n\tuint16_t aux[3];" +
                                "\r\n\tclean(aux, 3);" +
                                "\r\n\r\n\tkaratsuba_2(a0_plus_a1, b0_plus_b1, aux);" +
                                "\r\n\tuint16_t centro[] = {aux[0]-a1b1[0]-a0b0[0], aux[1]-a1b1[1]-a0b0[1], aux[2]-a1b1[2]-a0b0[2]};" +
                                "\r\n\r\n\tres[0]=a0b0[0];" +
                                "\r\n\tres[1]=a0b0[1];" +
                                "\r\n\tres[2]=a0b0[2]+centro[0];" +
                                "\r\n\tres[3]=centro[1];" +
                                "\r\n\tres[4]=centro[2] + a1b1[0];" +
                                "\r\n\tres[5]=a1b1[1];" +
                                "\r\n\tres[6]=a1b1[2];";
                    break;
                default:

                    int ktemp1 = 0;
                    int ktemp2 = 0;
                    if (grado % 4 == 0 || (grado + 1) % 4 == 0)
                    {
                        if (grado % 4 == 0)
                        {
                            ktemp1 = grado / 2;
                            ktemp2 = grado / 2;
                        }
                        else
                        {
                            ktemp1 = (grado + 1) / 2;
                            ktemp2 = (grado + 1) / 2;
                        }

                    }
                    else
                    {
                        int gradoaux = grado;
                        while (gradoaux % 4 != 0)
                        {
                            gradoaux++;
                        }
                        ktemp2 = gradoaux / 2;
                        ktemp1 = ktemp2 - 2;
                    }

                    code += "void karatsuba_" + grado + "(uint16_t arr1[], uint16_t arr2[], uint16_t res[]){\r\n    " +
                                "\r\tuint16_t a0[] ={";


                    for(int i =0; i < ktemp1; i++)
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
                        else if (i + 1 == ktemp2 && i + ktemp1 == grado) {
                            code += "0};";
                        }
                        else
                        {
                            code += "arr1[" + (i + ktemp1) + "],";
                        }
                    }

                    code += "\r\n\tuint16_t a0b0["+(ktemp1*2-1)+"];";
                    code += "\r\n\tuint16_t a1b1[" + (ktemp2 * 2 - 1) + "];";
                    code += "\r\n\tclean(a0b0, "+(ktemp1*2-1)+");";
                    code += "\r\n\tclean(a1b1, " + (ktemp2 * 2 - 1) + ");";

                    code += "\r\n\t\r\n\tkaratsuba_" + ktemp1 + "(a0, b0, a0b0);" +
                                "\r\n\tkaratsuba_" + ktemp2 + "(a1, b1, a1b1);";


                    code += "\r\n\r\n\tuint16_t a0_plus_a1[] ={";

                    int index = 0;
                    for (int i = 0; i <= ktemp2; i++) {
                        if (index < ktemp1)
                        {
                            if(i +1 == ktemp2)
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
                            if(i +1 == ktemp2)
                            {
                                fin = "};";               
                                if(grado < ktemp1 + ktemp2)
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

                            code +=  aux  + fin;

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

                    code += "\r\n\r\n\tkaratsuba_" + ktemp2 + "(a0_plus_a1, b0_plus_b1, aux);";

                    code += "\r\n\tuint16_t centro[] = {";

                    j = 0;
                    int totaux = (ktemp1 * 2 - 1);
                    for (int i = 0; i < (ktemp2 * 2 - 1); i++)
                    {
                        if(i >= totaux)
                        {
                            code += "aux[" + i + "]-a1b1[" + i + "]";
                        }
                        else
                        {
                            code += "aux[" + i + "]-a1b1[" + i + "]-a0b0["+j+"]";
                            j++;
                        }
                        if ((i + 1) == (ktemp2*2)-1)
                        {
                            code += "};\n";
                        }
                        else
                        {
                            code += ",";
                        }
                    }

                    int res =0;

                    if(grado % 2 == 0){
                        res = grado * 2 - 1;
                    }
                    else
                    {
                        res = (grado+1)* 2 - 1;

                    }

                    int p = 0;
                    int t1 = ktemp1*2 - 1;
                    int x = 0;
                    code += "\n\t";
                    for (int i = 0; i < t1; i++)
                    {
                        if (i > (ktemp1-1))
                        {
                            code += "res[" + p + "]=a0b0[" + i + "]+centro["+x+"];";
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

                    break;
            }
            code += "\n}";
            return code;
        }

    }
}
