namespace ObjectPrinting;

public class ObjectPrinter
{
    public static PrintingConfig<T> For<T>()
    {
        return new RootPrintingConfig<T>();
    }
}