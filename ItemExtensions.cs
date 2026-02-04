using ArtifactsMmoClient.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artifacts
{
    internal static class ItemExtensions
    {
        internal static void PrintCraftComponents(this ItemSchema item)
        {
            if (item == null || item.Craft == null) return;

            var builder = new StringBuilder("Craft components: \n");
            foreach (var component in item.Craft.Items)
            {
                builder.AppendLine($"{component.Code}: {component.Quantity}");
            }
            Console.WriteLine(builder);
        }
    }
}
