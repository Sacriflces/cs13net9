using Packt.Shared;

partial class Program
{
    private static void OutputPeopleNames(IEnumerable<Person?> people, string title)
    {
        WriteLine(title);
        foreach (Person? p in people)
        {
            WriteLine("  {0}", p is null ? "<null> Person" : p.Name ?? "<null> Name");
            /* if p is null hten output: <null Person else output: p.Name unless p.Name is null the noutput: <null> Name  */
        }
    }
}