using System;
using staafl.libcs;

class Program
{
    static void Main()
    {
        using (var fd = new JsonFile("test.txt"))
        {
            fd["Bulgaria"] = "Sof\n\n\a\bia";
            fd["Spain"] = "Madrid";
            fd["Switzerland"] = "Bern";
            Console.WriteLine(fd["Bulgaria"]);
        }
    }
}