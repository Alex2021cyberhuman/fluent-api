using System.Text;

namespace ObjectPrinting;

public abstract class PrintingConfig<TOwner> : IPrintingConfig<TOwner>, IInternalPrintingConfig<TOwner>
{
    private readonly PrintingConfig<TOwner>? parentConfig;

    protected PrintingConfig(PrintingConfig<TOwner>? parentConfig)
    {
        this.parentConfig = parentConfig;
    }

    PrintingConfig<TOwner>? IInternalPrintingConfig<TOwner>.ParentConfig => parentConfig;

    public string PrintToString(TOwner obj)
    {
        var sb = new StringBuilder();
        if (PrintingConfigExtensions.TryReturnNull(obj, out var stringValue))
            sb.Append(stringValue);
        else
            ((IInternalPrintingConfig<TOwner>)this).GetRoot().PrintToString(obj!, 0, sb);
        return sb.ToString();
    }


    public RootPrintingConfig<TOwner> GetRoot()
    {
        var reference = this;
        while (true)
        {
            if (reference!.parentConfig is null) return (RootPrintingConfig<TOwner>)reference;
            reference = reference.parentConfig;
        }
    }
}