using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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


    RootPrintingConfig<TOwner> IInternalPrintingConfig<TOwner>.GetRoot()
    {
        var reference = this;
        while (true)
        {
            if (reference!.parentConfig is null) return (RootPrintingConfig<TOwner>)reference;
            reference = reference.parentConfig;
        }
    }

    public string PrintToString(TOwner obj)
    {
        var stringBuilder = new StringBuilder();
        if (PrintingConfigExtensions.TryReturnNull(obj, out var stringValue))
        {
            stringBuilder.Append(stringValue);
        }
        else
        {
            var cyclicInheritanceIgnoredObjects = new HashSet<object>();
            ((IInternalPrintingConfig<TOwner>)this).GetRoot()
                .PrintToString(obj!, 0, stringBuilder, cyclicInheritanceIgnoredObjects);
        }

        return stringBuilder.ToString();
    }

    public PropertyPrintingConfig<TOwner, T> For<T>(
        Expression<Func<TOwner, T>> expression)
    {
        return new(expression, ((IInternalPrintingConfig<TOwner>)this).GetRoot());
    }


    public TypePrintingConfig<TOwner, T> ForType<T>()
    {
        return new(((IInternalPrintingConfig<TOwner>)this).GetRoot());
    }
}